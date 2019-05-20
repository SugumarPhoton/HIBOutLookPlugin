IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityAccount_SP')
  EXEC('CREATE PROCEDURE [HIBOPGetActivityAccount_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER  PROCEDURE [dbo].[HIBOPGetActivityAccount_SP]
(@User VARCHAR(10)
,@LastSyncDate DATETIME
,@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT OUTPUT
)
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY

	   
	DECLARE @securityuserid AS int
	DECLARE @UniqEmployee INT
	DECLARE @iCheckStructure AS TINYINT
	DECLARE @iCheckEmployeeAccess AS TINYINT
	DECLARE @UploadGuid AS VARCHAR(128)
	SET @UploadGuid=NEWID()
	
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

	CREATE TABLE #HIBOPEmployeeStructure
	(
	UniqEntity			int			Null,
	UniqAgency			int			Null,
	UniqBranch			int			Null
	)

	INSERT INTO #HIBOPEmployeeStructure
	SELECT UniqEntity,UniqAgency,UniqBranch FROM HIBOPEmployeeStructure (NOLOCK) Where UniqEntity = @UniqEmployee
	Group By UniqEntity,UniqAgency,UniqBranch

	Create Table #SecurityUserClient (uniqentity Int)
	CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #SecurityUserClient (uniqentity);
		
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
		
		
		MERGE HIBOPGetActivityAccountTemp BT
		USING  (
		SELECT 
		UPPER(@User) as Lookupcode,
		--ROW_NUMBER() OVER(ORDER BY c.UniqEntity,a.UniqAgency,b.UniqBranch ASC) AS RowNumber,
		C.UniqEntity,
		A.UniqAgency,
		A.AgencyCode,
		A.AgencyName,
		B.UniqBranch,
		B.BranchCode,
		B.BranchName,
		A.InsertedDate,
		A.UpdatedDate
		FROM HIBOPClient c WITH (NOLOCK)
		INNER JOIN HIBOPClientAgencyBranch AS cl WITH (NOLOCK) ON C.Uniqentity=cl.uniqentity
		INNER JOIN #HIBOPEmployeeStructure ES WITH (NOLOCK) ON ES.UniqAgency = cl.UniqAgency AND ES.UniqBranch = cl.UniqBranch
		INNER JOIN HIBOPAgency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
		INNER JOIN HIBOPBRANCH AS b WITH (NOLOCK) ON cl.Uniqbranch=b.Uniqbranch
		WHERE c.uniqentity <> -1 AND ISNULL(C.UpdatedDate,C.InsertedDate) > @LastSyncDate  
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  
 ) B
	ON B.Lookupcode=BT.Lookupcode AND B.UniqEntity=BT.UniqEntity AND B.UniqAgency=BT.UniqAgency AND B.UniqBranch =  Bt.UniqBranch

	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Lookupcode,UniqEntity,UniqAgency,AgencyCode,AgencyName,UniqBranch,BranchCode,BranchName,InsertedDate,UpdatedDate)
		VALUES (B.Lookupcode,B.UniqEntity,B.UniqAgency,B.AgencyCode,B.AgencyName,B.UniqBranch,B.BranchCode,B.BranchName,B.InsertedDate,B.UpdatedDate);
	

	SELECT AccountId,UniqEntity ,UniqAgency ,AgencyCode ,AgencyName ,UniqBranch ,BranchCode ,BranchName ,
		InsertedDate ,UpdatedDate from HIBOPGetActivityAccountTemp WITH (NOLOCK)
		 WHERE Lookupcode=@User
		ORDER BY AccountId
	OFFSET (@PageNumber-1)*@RowsPerPage ROWS
	FETCH NEXT @RowsPerPage ROWS ONLY
	
	SELECT @RowCount = COUNT(UniqEntity)
	FROM HIBOPGetActivityAccountTemp  WITH (NOLOCK)
	 WHERE Lookupcode=@User
	
	--DELETE FROM  HIBOPGetActivityAccountTemp  WHERE uploadguid=@uploadguid
	DROP TABLE #SecurityUserClient
	DROP TABLE #HIBOPEmployeeStructure
	
END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityAccount_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END