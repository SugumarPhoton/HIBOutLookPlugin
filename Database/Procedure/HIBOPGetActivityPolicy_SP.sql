IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityPolicy_SP')
  EXEC('CREATE PROCEDURE [HIBOPGetActivityPolicy_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityPolicy_SP]
(@User VARCHAR(10)
,@LastSyncDate DATETIME
,@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT=0 OUTPUT
)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 

	 IF @LastSyncDate IS NULL
	 BEGIN
		SET @LastSyncDate='1900-01-01'
	 END

	 
		DECLARE @securityuserid AS int
		DECLARE @UniqEmployee INT
		DECLARE @iCheckStructure AS TINYINT
		DECLARE @iCheckEmployeeAccess AS TINYINT
		DECLARE @UploadGuid varchar(128)
		set @UploadGuid=newid()
		
	
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
		UniqBranch			int			Null,
		UniqDepartment		int			Null
		)

		INSERT INTO #HIBOPEmployeeStructure
		SELECT UniqEntity,UniqAgency,UniqBranch,UniqDepartment FROM HIBOPEmployeeStructure Where UniqEntity = @UniqEmployee
		Group By UniqEntity,UniqAgency,UniqBranch,UniqDepartment

		Create Table #SecurityUserClient (uniqentity Int)
		create unique nonclustered index IX_SecurityUserClient on #SecurityUserClient (uniqentity);
		
		INSERT INTO #SecurityUserClient
		SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		WHERE sus.uniqsecurityuser = @securityuserid
		Group By cl.uniqentity

		CREATE TABLE #HIBOPActivityPolicyTemp
		(
		EmployeeLookUpCode VARCHAR(6),
		UniqEntity	int
		,CdPolicyLineTypeCode VARCHAR(128)
		,UniqPolicy	INT
		,PolicyNumber	VARCHAR(25)
		,DescriptionOf	VARCHAR(125)
		,EffectiveDate	DATETIME
		,ExpirationDate	DATETIME
		,PolicyStatus	VARCHAR(20)
		,[Status]	VARCHAR(20)
		,InsertedDate	DATETIME
		,UpdatedDate DATETIME
		)

		INSERT INTO #HIBOPActivityPolicyTemp
		(EmployeeLookUpCode,UniqEntity ,CdPolicyLineTypeCode ,UniqPolicy ,PolicyNumber 
		,DescriptionOf ,EffectiveDate ,ExpirationDate ,PolicyStatus ,[Status] ,
		InsertedDate ,UpdatedDate)

		SELECT 
			@User,
			C.UniqEntity,
			PL.CdPolicyLineTypeCode,
			P.UniqPolicy,
			P.PolicyNumber,
			RTRIM(LTRIM(P.DescriptionOf)) AS DescriptionOf,
			P.EffectiveDate,
			P.ExpirationDate,	
			P.PolicyStatus,
			--CASE WHEN P.Flags& 2=2 THEN 'Active' ELSE 'InActive' END [Status],
			CASE WHEN P.ExpirationDate> DATEADD(Day,-1,CAST(GETDATE() AS DATE)) THEN 'Active' ELSE 'InActive' END [Status],
			P.InsertedDate,
			P.UpdatedDate
		FROM HIBOPPolicy p WITH(NOLOCK)
		INNER JOIN HIBOPPolicyLineType PL WITH(NOLOCK) ON P.UniqCdPolicyLineType=PL.UniqCdPolicyLineType
		INNER JOIN HIBOPClient c WITH (NOLOCK) on P.UniqEntity=C.UniqEntity
		WHERE c.uniqentity <> -1 
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee))) 
		AND ISNULL(P.UpdatedDate, P.InsertedDate)>@LastSyncDate
		AND P.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END)
		ORDER BY p.UniqPolicy
		
		SELECT  EmployeeLookUpCode,UniqEntity ,CdPolicyLineTypeCode ,UniqPolicy ,PolicyNumber 
		,DescriptionOf ,EffectiveDate ,ExpirationDate ,PolicyStatus ,[Status] ,
		InsertedDate ,UpdatedDate FROM #HIBOPActivityPolicyTemp WITH (NOLOCK)
		order by UniqPolicy
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY


		SELECT @RowCount = COUNT(UniqPolicy)
		FROM #HIBOPActivityPolicyTemp p WITH(NOLOCK)
		
		DROP table #HIBOPActivityPolicyTemp
		DROP TABLE #SecurityUserClient
		DROP TABLE #HIBOPEmployeeStructure
END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityPolicy_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END