/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityServices_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityServices_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityServices_SP] AS' 
END
GO
/*
EXEC HIBOPGetActivityServices_SP 'LEUNA1',NULL,'1.1.1.1'
EXEC HIBOPGetActivityServices_SP 'MARJA1','2018-09-21 13:37:13.620','172.16.108.92'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityServices_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME,@IPAddress Varchar(100))
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	    Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

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

		CREATE TABLE #HIBOPEmployeeStructure
		(
		UniqEntity			int			Null,
		UniqAgency			int			Null,
		UniqBranch			int			Null,
		UniqDepartment		int			Null
		)

		INSERT INTO #HIBOPEmployeeStructure
		SELECT UniqEntity,UniqAgency,UniqBranch,UniqDepartment FROM HIBOPEmployeeStructure WITH (NOLOCK) Where UniqEntity = @UniqEmployee
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
			UniqServiceHead,
			c.UniqEntity,
			ServiceNumber,
			UniqCdServiceCode,
			[Description],
			ContractNumber,
			InceptionDate,
			s.ExpirationDate,
			CASE WHEN S.Flags& 2=2 THEN 'Active' ELSE  'InActive' END [Status],
			S.InsertedDate,
			S.UpdatedDate,
			S.Flags
		FROM HIBOPActivityServices S WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on s.UniqEntity=C.UniqEntity
		WHERE c.uniqentity <> -1 
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))
		AND ISNULL(S.UpdatedDate, S.InsertedDate)>@LastSyncDate
		AND s.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = S.UniqAgency AND ES.UniqBranch = S.UniqBranch
				AND ES.UniqDepartment = CASE WHEN S.UniqDepartment = -1 THEN ES.UniqDepartment ELSE S.UniqDepartment END)

		DROP TABLE #EntityEmployee

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityServices_SP',@DeltaSyncdate

	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityServices_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END


GO
