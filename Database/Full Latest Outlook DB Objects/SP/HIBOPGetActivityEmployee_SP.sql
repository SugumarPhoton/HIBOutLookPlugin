/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityEmployee_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityEmployee_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityEmployee_SP] AS' 
END
GO

/*
declare @p6 bigint
set @p6=1
exec [dbo].[HIBOPGetActivityEmployee_SP_test] @EmployeeLookupCode='LEUNA1',@LastSyncDate=null,
@IPAddress='10.200.6.7',@RowsPerPage=30000,@PageNumber=1,@RowCount=@p6 output
select @p6
*/
/*
declare @p6 bigint
set @p6=1
exec [dbo].[HIBOPGetActivityEmployee_SP_test] @EmployeeLookupCode='MARJA1',@LastSyncDate=null,
@IPAddress='10.200.6.7',@RowsPerPage=30000,@PageNumber=1,@RowCount=@p6 output
select @p6
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityEmployee_SP]
	(
				@EmployeeLookupCode		CHAR(6),
				@LastSyncDate			DATETIME,
				@IPAddress				Varchar(100),
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

		--Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

		Declare @DeltaSyncdate datetime
		
	
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @EmployeeLookupCode

		/*
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
		*/
		
		select @securityuserid  = uniqsecurityuser,
			   @iCheckStructure = iCheckStructure,
			   @iCheckEmployeeAccess = iCheckEmployeeAccess
		from Vw_SecurityUser(nolock)
		where uniqemployee = @UniqEmployee
		and   LookUpCode   = @EmployeeLookupCode

		--CREATE TABLE #HIBOPEmployeeStructure
		--(
		--UniqEntity			int			Null,
		--UniqAgency			int			Null,
		--UniqBranch			int			Null,
		--UniqDepartment		int			Null
		--)

		--INSERT INTO #HIBOPEmployeeStructure
		SELECT UniqEntity,UniqAgency,UniqBranch,UniqDepartment into #HIBOPEmployeeStructure FROM HIBOPEmployeeStructure(nolock) Where UniqEntity = @UniqEmployee
		Group By UniqEntity,UniqAgency,UniqBranch,UniqDepartment

		--Create Table #SecurityUserClient (uniqentity Int)
		
		
		--INSERT INTO #SecurityUserClient
		SELECT cl.uniqentity into #SecurityUserClient FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		WHERE sus.uniqsecurityuser = @securityuserid
		Group By cl.uniqentity

		--CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #SecurityUserClient (uniqentity);

		--CREATE TABLE #HIBOPGetActivityDetailsTemp(
		--UniqEmployee [int] NOT NULL,
		--EmployeeLookupcode VARCHAR(6) NOT NULL,
		--[UniqEntity] [int] NOT NULL,
		--[UniqActivity] [int] NOT NULL,
		
		--) 

		
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN
		 --   INSERT INTO #HIBOPGetActivityDetailsTemp
			--(UniqEmployee,EmployeeLookupcode,UniqEntity,UniqActivity)

			SELECT DISTINCT
				@UniqEmployee AS UniqEmployee,
				@EmployeeLookupCode as EmployeeLookupcode,
				a.UniqEntity,
				A.UniqActivity,
				A.InsertedDate,
			    A.UpdatedDate,
				A.ClosedDate
				into #HIBOPGetActivityDetailsTemp
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
			--AND  ISNULL(A.UpdatedDate, A.InsertedDate)>@LastSyncDate 
			AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > @LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > @LastSyncDate)
			and a.UniqAgency<>-1 and a.UniqBranch <>-1 
			and (ClosedDate is null or  ClosedDate> DATEADD(mm,-18,getdate())) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)

			--CREATE UNIQUE NONCLUSTERED INDEX IX_HIBOPGetActivityDetailsTemp on #HIBOPGetActivityDetailsTemp (UniqEmployee,uniqentity,UniqActivity)

			SELECT @UniqEmployee AS UniqEmployee,@EmployeeLookupCode as EmployeeLookupcode,UniqEntity,UniqActivity
			FROM #HIBOPGetActivityDetailsTemp 
			ORDER BY UniqActivity
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY

			SELECT @RowCount = count (UniqActivity)
			FROM #HIBOPGetActivityDetailsTemp    
		

			IF @PageNumber = 1 
			BEGIN

				SELECT @DeltaSyncdate=MAX(ColumnValue)
				from (
					select max(InsertedDate)Max_inst,max(UpdatedDate)Max_upd,max(ClosedDate)Max_cls
					from #HIBOPGetActivityDetailsTemp
					)a
				Unpivot(ColumnValue For ColumnName IN (Max_inst,Max_upd,Max_cls)) AS H

				Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @EmployeeLookupCode,@IPAddress,'HIBOPGetActivityEmployee_SP',@DeltaSyncdate
			END

			--DELETE FROM HIBOPGetActivityDetailsTemp WHERE UploadGuid=@UploadGuid
			DROP TABLE #HIBOPGetActivityDetailsTemp
		
			
		END
		ELSE 
		BEGIN
			--INSERT INTO #HIBOPGetActivityDetailsTemp
			--(UniqEmployee,EmployeeLookupcode,UniqEntity,UniqActivity)
			
			SELECT DISTINCT
				@UniqEmployee AS UniqEmployee,
				@EmployeeLookupCode as EmployeeLookupcode,
				a.UniqEntity,
				A.UniqActivity,
				A.InsertedDate,
			    A. UpdatedDate,
				A.ClosedDate
				into #HIBOPGetActivityDetailsTemp1
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
			FROM #HIBOPGetActivityDetailsTemp1  
			ORDER BY UniqActivity
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY
			
			SELECT @RowCount = count (UniqActivity)
			FROM #HIBOPGetActivityDetailsTemp1    (nolock)

			IF @PageNumber = 1 
			BEGIN

				SELECT @DeltaSyncdate=MAX(ColumnValue)
				from (
					select max(InsertedDate)Max_inst,max(UpdatedDate)Max_upd,max(ClosedDate)Max_cls
					from #HIBOPGetActivityDetailsTemp1
					)a
				Unpivot(ColumnValue For ColumnName IN (Max_inst,Max_upd,Max_cls)) AS H

				Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @EmployeeLookupCode,@IPAddress,'HIBOPGetActivityEmployee_SP',@DeltaSyncdate
			END

			DROP TABLE #HIBOPGetActivityDetailsTemp1
		END
		
		

	

		DROP TABLE #SecurityUserClient
		--DROP TABLE #HIBOPGetActivityDetailsTemp
		DROP TABLE #HIBOPEmployeeStructure

		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetActivtiyDetails_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
