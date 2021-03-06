IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityEmployee_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityEmployee_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityEmployee_SP]
	(
				@EmployeeLookupCode		CHAR(6),
				@LastSyncDate			DATETIME,
				@RowsPerPage			INT , 
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
		
	
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @EmployeeLookupCode

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
		CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #SecurityUserClient (uniqentity);
		
		INSERT INTO #SecurityUserClient
		SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		WHERE sus.uniqsecurityuser = @securityuserid
		Group By cl.uniqentity

		CREATE TABLE #HIBOPGetActivityDetailsTemp(
		UniqEmployee [int] NOT NULL,
		EmployeeLookupcode VARCHAR(6) NOT NULL,
		[UniqEntity] [int] NOT NULL,
		[UniqActivity] [int] NOT NULL,
		
		) 

		CREATE UNIQUE NONCLUSTERED INDEX IX_HIBOPGetActivityDetailsTemp on #HIBOPGetActivityDetailsTemp (UniqEmployee,uniqentity,UniqActivity)
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN
		    INSERT INTO #HIBOPGetActivityDetailsTemp
			(UniqEmployee,EmployeeLookupcode,UniqEntity,UniqActivity)

			SELECT DISTINCT
				@UniqEmployee AS UniqEmployee,
				@EmployeeLookupCode as EmployeeLookupcode,
				a.UniqEntity,
				A.UniqActivity
			FROM HIBOPActivity A WITH(NOLOCK)
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON A.UniqEmployee =O.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType=p.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[Status] & 8192 = 8192 
			OR EXISTS (SELECT 1	FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  
			AND  ISNULL(A.UpdatedDate, A.InsertedDate)>@LastSyncDate 
			and a.UniqAgency<>-1 and a.UniqBranch <>-1 
			and (ClosedDate is null or  ClosedDate> DATEADD(mm,-18,getdate())) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)

			SELECT @UniqEmployee AS UniqEmployee,@EmployeeLookupCode as EmployeeLookupcode,UniqEntity,UniqActivity
			FROM #HIBOPGetActivityDetailsTemp 
			ORDER BY UniqActivity
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY

			SELECT @RowCount = count (UniqActivity)
			FROM #HIBOPGetActivityDetailsTemp    
			

			--DELETE FROM HIBOPGetActivityDetailsTemp WHERE UploadGuid=@UploadGuid

		
			
		END
		ELSE 
		BEGIN
			INSERT INTO #HIBOPGetActivityDetailsTemp
			(UniqEmployee,EmployeeLookupcode,UniqEntity,UniqActivity)
			
			SELECT DISTINCT
				@UniqEmployee AS UniqEmployee,
				@EmployeeLookupCode as EmployeeLookupcode,
				a.UniqEntity,
				A.UniqActivity
				
			FROM HIBOPActivity A WITH(NOLOCK)
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON A.UniqEmployee =O.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK) ON A.UniqCdPolicyLineType=p.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			WHERE c.uniqentity <> -1 
			AND (@iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
			AND (@iCheckEmployeeAccess = 0 
			OR (c.[Status] & 8192 = 8192 
			OR EXISTS (SELECT 1 FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  
			and  a.UniqAgency<>-1 and a.UniqBranch <>-1 
			and (ClosedDate is null or  ClosedDate> DATEADD(mm,-18,getdate())) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)

			SELECT @UniqEmployee AS UniqEmployee,@EmployeeLookupCode as EmployeeLookupcode,UniqEntity,UniqActivity
			FROM #HIBOPGetActivityDetailsTemp  
			ORDER BY UniqActivity
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY
			
			SELECT @RowCount = count (UniqActivity)
			FROM #HIBOPGetActivityDetailsTemp    (nolock)
			
		END
		
		DROP TABLE #SecurityUserClient
		DROP TABLE #HIBOPGetActivityDetailsTemp
		DROP TABLE #HIBOPEmployeeStructure

		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetActivtiyDetails_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END