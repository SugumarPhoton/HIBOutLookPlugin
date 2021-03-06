/****** Object:  StoredProcedure [dbo].[HIBOPGetClientEmployee_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientEmployee_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetClientEmployee_SP] AS' 
END
GO
/*
Declare @rowcount int 
EXEC [HIBOPGetClientEmployee_SP] 'LEUNA1',NULL,'10.200.6.7',30000,1,@rowcount= @rowcount
select @rowcount
*/
/*
Declare @rowcount int 
EXEC [HIBOPGetClientEmployee_SP] 'LEUNA1','2018-04-01 06:05:06.650','1.1.1.1',30000,1,@rowcount= @rowcount
select @rowcount
*/
ALTER PROCEDURE [dbo].[HIBOPGetClientEmployee_SP]  --'LEUNA1',300,1,300
(@User VARCHAR(6),
 @LastSyncDate DATETIME,
 @IPAddress Varchar(100),
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

	Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

	SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @User

	SELECT 
	@securityuserid	=	su.uniqsecurityuser,
	@iCheckStructure = CASE WHEN su.typecode IN ('E','I') THEN 0   -- enterprise user, system user
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 1 THEN 0  -- SecurityUser.Grant
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 3 THEN 1  -- SecurityUser.Deny
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.securitygroupsecurityuserjt jt WITH (NOLOCK)
	INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup -- SecurityGroup.Grant
	WHERE jt.uniqsecurityuser = su.uniqsecurityuser
	AND CAST(SUBSTRING(sg.programaccess, 431, 1) AS TINYINT) = 1) THEN 0
	ELSE 1 END,
	@iCheckEmployeeAccess = CASE WHEN su.typecode IN ('E','I') THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 1 THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 3 THEN 1
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.SecurityGroupSecurityUserJT jt WITH (NOLOCK)
	INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup 
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

	IF @LastSyncDate IS NULL
	BEGIN
	SET @LastSyncDate='1900-01-01'
	END

	INSERT INTO HIBOPGetClientEmployeeTemp(UploadGuid,ClientId,Client_InsertedDate,Client_UpdatedDate)

	SELECT 
	 DISTINCT --code added for diskio issue
	@UploadGuid,
	c.uniqentity ,
	C.InsertedDate ,
	C.UpdatedDate  
	FROM HIBOPClient c(Nolock)
	WHERE c.uniqentity <> -1
	and  ISNULL(C.UpdatedDate,C.InsertedDate) > @LastSyncDate  
	AND (@iCheckStructure = 0 
	OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
	AND (@iCheckEmployeeAccess = 0 
	OR (c.[STATUS] & 8192 = 8192 
	OR EXISTS (SELECT 1
	FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  
	--select * from HIBOPGetClientEmployeeTemp
	select   
	--DISTINCT --code commented for diskio issue
	ClientId AS ClientId
	,@UniqEmployee as UserId
	,@User AS EmployeeLookupcode
	from HIBOPGetClientEmployeeTemp  c WITH (NOLOCK)
	WHERE UploadGuid=@UploadGuid 
	and   ISNULL(Client_UpdatedDate,Client_InsertedDate) > @LastSyncDate
	order by ClientId
	OFFSET (@PageNumber-1)*@RowsPerPage ROWS
	FETCH NEXT @RowsPerPage ROWS ONLY
	


	SELECT @RowCount = 
	--count (Distinct cab.ClientId)--code commented for diskio issue
	count ( cab.ClientId) 
	FROM HIBOPGetClientEmployeeTemp    as cab with (nolock)
	WHERE UploadGuid=@UploadGuid
	and   ISNULL(Client_UpdatedDate,Client_InsertedDate) > @LastSyncDate

	DELETE FROM HIBOPGetClientEmployeeTemp   WHERE UploadGuid=@UploadGuid
	DROP TABLE #SecurityUserClient

	IF @PageNumber=1 
	Begin
		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetClientEmployee_SP',@DeltaSyncdate
	End

	SET NOCOUNT OFF
END







GO
