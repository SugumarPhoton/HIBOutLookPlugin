--/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityClaim_SP]    Script Date: 9/17/2018 4:35:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
EXEC HIBOPGetActivityClaim_SP 'LEUNA1',NULL,'1.1.1.1'
EXEC HIBOPGetActivityClaim_SP 'LEUNA1','2017-08-01 08:08:12.610','1.1.1.1'
*/
alter PROCEDURE [dbo].[HIBOPGetActivityClaim_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME, @IPAddress Varchar(100))
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY

		DECLARE @securityuserid AS int
		DECLARE @UniqEmployee INT
		DECLARE @iCheckStructure AS TINYINT
		DECLARE @iCheckEmployeeAccess AS TINYINT
		DECLARE @UploadGuid varchar(128)
		set @UploadGuid=newid()

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

		CREATE TABLE #EntityEmployee (UniqEntity Int)
		CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #EntityEmployee (UniqEntity);

		INSERT INTO #EntityEmployee
		SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		INNER JOIN EpicDMZSub.DBO.securityuser su (NOLOCK) On su.uniqsecurityuser = sus.UniqSecurityUser And su.uniqemployee = @UniqEmployee
		Group By cl.uniqentity
		
	IF (@LastSyncDate >'1900-01-01' OR @LastSyncDate IS NOT NULL)
	BEGIN
		SELECT DISTINCT
				C.UniqEntity,
				UniqClaim,
				ClaimCode,
				ClaimName,
				LossDate,
				ReportedDate,
				ClaimNumber as AgencyClaimNumber,
				CompanyClaimNumber,
				ClosedDate,
				CL.InsertedDate,
				CL.UpdatedDate,
				CL.Flags
			FROM HIBOPClient c with (nolock)
			INNER JOIN HIBOPClaim CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[STATUS] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee))) AND 
			ISNULL(CL.UpdatedDate,CL.InsertedDate)>@LastSyncDate
	END
	ELSE
	BEGIN
		SELECT DISTINCT
				C.UniqEntity,
				UniqClaim,
				ClaimCode,
				ClaimName,
				LossDate,
				ReportedDate,
				ClaimNumber as AgencyClaimNumber,
				CompanyClaimNumber,
				ClosedDate,
				CL.InsertedDate,
				CL.UpdatedDate,
				CL.Flags
			FROM HIBOPClient c with (nolock)
			INNER JOIN HIBOPClaim CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[STATUS] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))
	END

	DROP TABLE #EntityEmployee

	Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityClaim_SP',@DeltaSyncdate
END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityClaim_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
