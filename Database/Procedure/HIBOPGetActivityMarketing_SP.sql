IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityMarketing_SP')
  EXEC('CREATE PROCEDURE [HIBOPGetActivityMarketing_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityMarketing_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

		IF @LastSyncDate IS NULL
		BEGIN
			SET @LastSyncDate ='1900-01-01'
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

			
		CREATE TABLE #Market (UniqMarketingSubmission INT, LineCode VARCHAR(200))

		INSERT INTO #Market
		SELECT DISTINCT m.UniqMarketingSubmission,
				STUFF((SELECT DISTINCT ',' + p.CdPolicyLineTypeCode
				FROM HIBOPActivityMasterMarketing m1 WITH(NOLOCK)
				INNER JOIN HIBOPPolicyLineType p WITH(NOLOCK)on m1.uniqcdpolicylinetype=p.uniqcdpolicylinetype
				WHERE m.UniqMarketingSubmission = m1.UniqMarketingSubmission
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)')
				,1,1,'') LineCode
		FROM HIBOPActivityMasterMarketing m WITH(NOLOCK);

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

		CREATE TABLE #EntityEmployee (UniqEntity Int)
		CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #EntityEmployee (UniqEntity);

		INSERT INTO #EntityEmployee
		SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		INNER JOIN EpicDMZSub.DBO.securityuser su (NOLOCK) On su.uniqsecurityuser = sus.UniqSecurityUser And su.uniqemployee = @UniqEmployee
		Group By cl.uniqentity

		SELECT DISTINCT
			p.UniqMarketingSubmission,
			M.UniqEntity,
			M.UniqAgency,
			M.UniqBranch,
			DescriptionOf MarketingSubbmission,
			LineCode as LineOfBusiness,
			EffectiveDate,
			m.ExpirationDate,
			LastSubmittedDate,
			CASE WHEN m.Flags & 2 =2 THEN 'Active' ELSE 'InActive' END [Status],
			m.InsertedDate,
			m.UpdatedDate
		FROM HIBOPActivityMasterMarketing m WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on m.UniqEntity=C.UniqEntity
		LEFT OUTER JOIN #Market p on m.UniqMarketingSubmission=p.UniqMarketingSubmission
		WHERE c.uniqentity <> -1 
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee))) and
		ISNULL(M.UpdatedDate,M.InsertedDate)>@LastSyncDate
		AND m.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = M.UniqAgency AND ES.UniqBranch = M.UniqBranch
				AND ES.UniqDepartment = CASE WHEN M.UniqDepartment = -1 THEN ES.UniqDepartment ELSE M.UniqDepartment END)

		DROP TABLE #EntityEmployee
		DROP TABLE #HIBOPEmployeeStructure
		DROP TABLE #Market
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityMarketing_SP Error MSG : '+ERROR_MESSAGE()
 END CATCH 

	 SET NOCOUNT OFF
END