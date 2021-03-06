IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetClientEmployee_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetClientEmployee_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetClientEmployee_SP] 
(@User VARCHAR(6),
@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT OUTPUT
)
AS
BEGIN 
	SET NOCOUNT ON

	DECLARE @securityuserid AS int
	DECLARE @UniqEmployee INT
	DECLARE @iCheckStructure AS TINYINT
	DECLARE @iCheckEmployeeAccess AS TINYINT
	DECLARE @UploadGuid AS VARCHAR(128)
	SET @UploadGuid=NEWID()
	SET @User = UPPER(@User)

	SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @User

	SELECT 
	@securityuserid	=	su.uniqsecurityuser,
	@iCheckStructure = CASE WHEN su.typecode IN ('E','I') THEN 0   -- enterprise user, system user
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 1 THEN 0  -- SecurityUser.Grant
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 3 THEN 1  -- SecurityUser.Deny
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.securitygroupsecurityuserjt jt
	INNER JOIN EpicDMZSub.DBO.securitygroup sg ON jt.uniqsecuritygroup = sg.uniqsecuritygroup -- SecurityGroup.Grant
	WHERE jt.uniqsecurityuser = su.uniqsecurityuser
	AND CAST(SUBSTRING(sg.programaccess, 431, 1) AS TINYINT) = 1) THEN 0
	ELSE 1 END,
	@iCheckEmployeeAccess = CASE WHEN su.typecode IN ('E','I') THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 1 THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 3 THEN 1
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.SecurityGroupSecurityUserJT jt
	INNER JOIN EpicDMZSub.DBO.securitygroup sg ON jt.uniqsecuritygroup = sg.uniqsecuritygroup 
	WHERE jt.uniqsecurityuser = su.uniqsecurityuser
	AND CAST(SUBSTRING(sg.programaccess, 1467, 1) AS TINYINT) = 1) THEN 0
	ELSE 1 END
	FROM EpicDMZSub.DBO.securityuser su (NOLOCK)
	WHERE su.uniqemployee = @UniqEmployee

	Create Table #SecurityUserClient (uniqentity Int)
	create unique nonclustered index IX_SecurityUserClient on #SecurityUserClient (uniqentity);
		
	INSERT INTO #SecurityUserClient
	SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
	INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
	INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
	WHERE sus.uniqsecurityuser = @securityuserid
	Group By cl.uniqentity

	INSERT INTO HIBOPGetClientEmployeeTemp(UploadGuid,ClientId)

	SELECT 
	@UploadGuid,
	c.uniqentity 
	FROM HIBOPClient c
	WHERE c.uniqentity <> -1
	AND (@iCheckStructure = 0 
	OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
	AND (@iCheckEmployeeAccess = 0 
	OR (c.[STATUS] & 8192 = 8192 
	OR EXISTS (SELECT 1
	FROM EpicDMZSub.dbo.EntityEmployeeJT eejt
	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  

	select DISTINCT 
	ClientId AS ClientId
	,@UniqEmployee as UserId
	,@User AS EmployeeLookupcode
	from HIBOPGetClientEmployeeTemp  c WITH (NOLOCK)
	WHERE UploadGuid=@UploadGuid 
	order by ClientId
	OFFSET (@PageNumber-1)*@RowsPerPage ROWS
	FETCH NEXT @RowsPerPage ROWS ONLY
	
	SELECT @RowCount = count (DISTINCT cab.ClientId)
	FROM HIBOPGetClientEmployeeTemp    as cab with (nolock)
	WHERE UploadGuid=@UploadGuid

	DELETE FROM HIBOPGetClientEmployeeTemp   WHERE UploadGuid=@UploadGuid
	DROP TABLE #SecurityUserClient
	

	SET NOCOUNT OFF
END





