IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityLine_SP')
  EXEC('CREATE PROCEDURE [HIBOPGetActivityLine_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityLine_SP]
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

		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN

			INSERT INTO HIBOPGetActivityLineTemp
			(UploadGuid,UniqLine,UniqPolicy,UniqEntity,PolicyType,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber
			,UniqCdPolicyLineType,UniqCdLineStatus,IOC,BillModeCode,ExpirationDate,EffectiveDate,[Status],InsertedDate,UpdatedDate)
		
			SELECT 
				@UploadGuid,
				UniqLine,
				a.UniqPolicy,
				A.UniqEntity,
				pt.CdPolicyLineTypeCode AS PolicyType,
				LTRIM(RTRIM(PolicyDesc)) AS PolicyDesc,
				LineCode,
				LineOfBusiness,
				LineStatus,
				a.PolicyNumber,
				a.UniqCdPolicyLineType,
				UniqCdLineStatus,
				IOC,
				BillModeCode,
				a.ExpirationDate,
				a.EffectiveDate,
				--CASE WHEN A.Flags &2 = 2 THEN 'Active' ELSE 'Inactive' END [Status],
				CASE WHEN a.ExpirationDate>GETDATE() THEN 'Active' ELSE 'Inactive' END [Status],
				A.InsertedDate,
				A.UpdatedDate
			FROM HIBOPActivityLine A WITH(NOLOCK)	
			INNER JOIN  HIBOPPolicy p WITH(NOLOCK) ON a.UniqPolicy=p.UniqPolicy
			INNER JOIN HIBOPPolicyLineType AS pt ON a.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[status] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  
			AND 
			ISNULL(A.UpdatedDate, A.InsertedDate)>@LastSyncDate 
			and  a.ExpirationDate> DATEADD(mm,-18,getdate()) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END)
			

			SELECT UniqLine,UniqPolicy,UniqEntity,PolicyType,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber
			,UniqCdPolicyLineType,UniqCdLineStatus,IOC,BillModeCode,ExpirationDate,EffectiveDate,[Status],InsertedDate,UpdatedDate
			FROM HIBOPGetActivityLineTemp WITH (NOLOCK) WHERE UploadGuid=@UploadGuid
			ORDER BY UniqLine
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY

			SELECT @RowCount = COUNT(UniqLine)
			FROM HIBOPGetActivityLineTemp WITH (NOLOCK) WHERE UploadGuid=@UploadGuid

			DELETE FROM HIBOPGetActivityLineTemp  WHERE UploadGuid=@UploadGuid
		END
		ELSE 
		BEGIN
			INSERT INTO HIBOPGetActivityLineTemp
			(UploadGuid,UniqLine,UniqPolicy,UniqEntity,PolicyType,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber
			,UniqCdPolicyLineType,UniqCdLineStatus,IOC,BillModeCode,ExpirationDate,EffectiveDate,[Status],InsertedDate,UpdatedDate)
		

			SELECT 
				@UploadGuid,
				UniqLine,
				a.UniqPolicy,
				A.UniqEntity,
				pt.CdPolicyLineTypeCode AS PolicyType,
				LTRIM(RTRIM(PolicyDesc)) AS PolicyDesc,
				LineCode,
				LineOfBusiness,
				LineStatus,
				a.PolicyNumber,
				a.UniqCdPolicyLineType,
				UniqCdLineStatus,
				IOC,
				BillModeCode,
				a.ExpirationDate,
				a.EffectiveDate,
				--CASE WHEN A.Flags &2 = 2 THEN 'Active' ELSE 'Inactive' END [Status],
				CASE WHEN a.ExpirationDate>GETDATE() THEN 'Active' ELSE 'Inactive' END [Status],
				A.InsertedDate,
				A.UpdatedDate
			FROM HIBOPActivityLine A WITH(NOLOCK)	
			INNER JOIN  HIBOPPolicy p WITH(NOLOCK) ON a.UniqPolicy=p.UniqPolicy
			INNER JOIN HIBOPPolicyLineType AS pt ON a.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[status] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))
			and   a.ExpirationDate> DATEADD(mm,-18,getdate()) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END)

			SELECT UniqLine,UniqPolicy,UniqEntity,PolicyType,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber
			,UniqCdPolicyLineType,UniqCdLineStatus,IOC,BillModeCode,ExpirationDate,EffectiveDate,[Status],InsertedDate,UpdatedDate
			FROM HIBOPGetActivityLineTemp WITH (NOLOCK) WHERE UploadGuid=@UploadGuid
			ORDER BY UniqLine
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY

			SELECT @RowCount = COUNT(UniqLine)
			FROM HIBOPGetActivityLineTemp WITH (NOLOCK) WHERE UploadGuid=@UploadGuid

			DELETE FROM HIBOPGetActivityLineTemp  WHERE UploadGuid=@UploadGuid
		
		END		

		DROP TABLE #SecurityUserClient
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityLine_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END