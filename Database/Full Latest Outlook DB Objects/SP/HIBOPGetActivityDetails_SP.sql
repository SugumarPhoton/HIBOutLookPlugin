/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityDetails_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityDetails_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityDetails_SP] AS' 
END
GO
/*
declare @p6 bigint
set @p6=1
exec [dbo].[HIBOPGetActivityDetails_SP_test] @EmployeeLookupCode='LEUNA1',@LastSyncDate=null,
@IPAddress='10.200.6.7',@RowsPerPage=30000,@PageNumber=1,@RowCount=@p6 output
select @p6
*/
/*
declare @p6 bigint
set @p6=1
exec [dbo].[HIBOPGetActivityDetails_SP_test] @EmployeeLookupCode='MARJA1',@LastSyncDate=null,
@IPAddress='10.200.6.7',@RowsPerPage=30000,@PageNumber=1,@RowCount=@p6 output
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityDetails_SP]
	(
				@EmployeeLookupCode		CHAR(6),
				@LastSyncDate			DATETIME,
				@IPAddress Varchar(100),
				@RowsPerPage			INT , 
				@PageNumber				INT ,
				@RowCount				BIGINT OUTPUT
	
	)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
		DECLARE @GetDate Datetime = GetDate()
		DECLARE @securityuserid AS int
		DECLARE @UniqEmployee INT
		DECLARE @iCheckStructure AS TINYINT
		DECLARE @iCheckEmployeeAccess AS TINYINT
		DECLARE @UploadGuid varchar(128)
		set @UploadGuid=newid()

		--Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())--code commeted for issue5 by grant data miss because of time mismatch

		Declare @DeltaSyncdate datetime

		SELECT @EmployeeLookupCode = LTRIM(RTRIM(@EmployeeLookupCode))
	
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @EmployeeLookupCode

		--CREATE TABLE #HIBOPEmployeeStructure
		--(
		--UniqEntity			int			Null,
		--UniqAgency			int			Null,
		--UniqBranch			int			Null,
		--UniqDepartment		int			Null
		--)

		--INSERT INTO #HIBOPEmployeeStructure
		SELECT UniqEntity,UniqAgency,UniqBranch,UniqDepartment into #HIBOPEmployeeStructure FROM HIBOPEmployeeStructure WITH (NOLOCK) Where UniqEntity = @UniqEmployee
		Group By UniqEntity,UniqAgency,UniqBranch,UniqDepartment

		--CREATE TABLE #EntityEmployee (UniqEntity Int)
		--CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #EntityEmployee (UniqEntity);

		--INSERT INTO #EntityEmployee
		SELECT cl.uniqentity into #EntityEmployee FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
		INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		INNER JOIN EpicDMZSub.DBO.securityuser su (NOLOCK) On su.uniqsecurityuser = sus.UniqSecurityUser And su.uniqemployee = @UniqEmployee
		Group By cl.uniqentity

		CREATE TABLE #HIBOPGetActivityDetailsTemp(
		[UniqEntity] [int] NOT NULL,
		[UniqActivity] [int] NOT NULL,
		[UniqActivityCode] [int] NULL,
		[ActivityCode] [varchar](4) NULL,
		[DescriptionOf] [varchar](125) NULL,
		[UniqCdPolicyLineType] [int] NULL,
		[CdPolicyLineTypeCode] [varchar](4) NULL,
		[PolicyNumber] [varchar](50) NULL,
		[EffectiveDate] [datetime] NULL,
		[ExpirationDate] [datetime] NULL,
		[InsertedDate] [datetime] NULL,
		[UpdatedDate] [datetime] NULL,
		[Status] [varchar](10) NULL,
		[UniqAgency] [int] NULL,
		[UniqBranch] [int] NULL,
		[UniqDepartment] [int] NULL,
		[UniqProfitCenter] [int] NULL,
		[UniqAssociatedItem] [int] NULL,
		[AssociationType] [varchar](20) NULL,
		[OwnerName] [varchar](125) NULL,
		[ClosedDate] [datetime] NULL,
		[UniqPolicy] [int] NULL,
		[UniqLine] [int] NULL,
		[UniqClaim] [int] NULL,
		[LossDate] [datetime] NULL,
		[Policydescription] [varchar](125) NULL,
		[LineCode] [varchar](4) NULL,
		[LineDescription] [varchar](max) NULL,
		[ICO] [varchar](6) NULL,
		[LineEffectiveDate] [datetime] NULL,
		[LineExpirationDate] [datetime] NULL
		) 

		CREATE UNIQUE NONCLUSTERED INDEX IX_HIBOPGetActivityDetailsTemp on #HIBOPGetActivityDetailsTemp (UniqActivityCode,uniqentity,UniqActivity)
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN
		    INSERT INTO #HIBOPGetActivityDetailsTemp
			(UniqEntity,UniqActivity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,CdPolicyLineTypeCode
			,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,[Status],UniqAgency,UniqBranch,UniqDepartment
			,UniqProfitCenter,UniqAssociatedItem,AssociationType,OwnerName,ClosedDate,UniqPolicy,UniqLine,UniqClaim
			,LossDate,Policydescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate)

			SELECT 
			a.UniqEntity,
			A.UniqActivity,
			A.UniqActivityCode,
			A.ActivityCode,
			A.DescriptionOf,
			A.UniqCdPolicyLineType,
			P.CdPolicyLineTypeCode,
			A.PolicyNumber,
			A.EffectiveDate,
			A.ExpirationDate,
			A.InsertedDate,
			A. UpdatedDate,
			CASE WHEN A.Status & 2 = 2 THEN 'Open' ELSE 'Closed' END 'Status',
			A.UniqAgency,
			A.UniqBranch,
			A.UniqDepartment,
			A.UniqProfitCenter,
			A.UniqAssociatedItem,
			A.AssociationType,
			O.OwnerName,
			A.ClosedDate,
			a.UniqPolicy,
			a.UniqLine,
			a.UniqClaim,
			a.LossDate,
			LTRIM(RTRIM(a.Policydescription)) AS Policydescription,
			a.LineCode,
			a.LineDescription,
			a.ICO,
			A.LineEffectiveDate,
			a.LineExpirationDate
			FROM HIBOPActivity A WITH(NOLOCK) 
			INNER JOIN HIBOPActivityCode c WITH (NOLOCK) ON a.uniqactivitycode = c.uniqactivitycode 
			INNER JOIN EpicDMZSub.dbo.company ic WITH (NOLOCK) ON a.uniqentitycompanyissuing = ic.uniqentity 
			INNER JOIN EpicDMZSub.dbo.company pc WITH (NOLOCK) ON a.uniqentitycompanybilling = pc.uniqentity 
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON O.UniqEntity = A.UniqEmployee
			INNER JOIN #EntityEmployee EE WITH(NOLOCK) ON EE.UniqEntity = A.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType = p.UniqCdPolicyLineType
			--INNER JOIN broker  pb ON a.uniqentitybrokerbilling = pb.uniqentity 
			WHERE a.status & 16 = 0 
			and (A.ClosedDate is null or A.ClosedDate> DATEADD(mm,-18,@GetDate))
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)
			AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > @LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > @LastSyncDate)
			--AND  (ISNULL(isnull(A.UpdatedDate,A.ClosedDate), A.InsertedDate) > @LastSyncDate )
			 -- AND	COALESCE(A.UpdatedDate,A.ClosedDate,A.InsertedDate)	 > @LastSyncDate 

			SELECT UniqEntity,UniqActivity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,CdPolicyLineTypeCode
			,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,[Status],UniqAgency,UniqBranch,UniqDepartment
			,UniqProfitCenter,UniqAssociatedItem,AssociationType,OwnerName,ClosedDate,UniqPolicy,UniqLine,UniqClaim
			,LossDate,Policydescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate
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
			(UniqEntity,UniqActivity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,CdPolicyLineTypeCode
			,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,[Status],UniqAgency,UniqBranch,UniqDepartment
			,UniqProfitCenter,UniqAssociatedItem,AssociationType,OwnerName,ClosedDate,UniqPolicy,UniqLine,UniqClaim
			,LossDate,Policydescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate)
			
			SELECT 
			A.UniqEntity,
			A.UniqActivity,
			A.UniqActivityCode,
			A.ActivityCode,
			A.DescriptionOf,
			A.UniqCdPolicyLineType,
			P.CdPolicyLineTypeCode,
			A.PolicyNumber,
			A.EffectiveDate,
			A.ExpirationDate,
			A.InsertedDate,
			A. UpdatedDate,
			CASE WHEN A.Status & 2 = 2 THEN 'Open' ELSE 'Closed' END 'Status',
			A.UniqAgency,
			A.UniqBranch,
			A.UniqDepartment,
			A.UniqProfitCenter,
			A.UniqAssociatedItem,
			A.AssociationType,
			O.OwnerName,
			A.ClosedDate,
			a.UniqPolicy,
			a.UniqLine,
			a.UniqClaim,
			a.LossDate,
			LTRIM(RTRIM(a.Policydescription)) AS Policydescription,
			a.LineCode,
			a.LineDescription,
			a.ICO,
			A.LineEffectiveDate,
			a.LineExpirationDate
			FROM HIBOPActivity A WITH(NOLOCK) 
			INNER JOIN HIBOPActivityCode c ON a.uniqactivitycode = c.uniqactivitycode 
			INNER JOIN EpicDMZSub.dbo.company ic WITH (NOLOCK) ON a.uniqentitycompanyissuing = ic.uniqentity 
			INNER JOIN EpicDMZSub.dbo.company pc WITH (NOLOCK) ON a.uniqentitycompanybilling = pc.uniqentity 
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON O.UniqEntity = A.UniqEmployee
			INNER JOIN #EntityEmployee EE WITH(NOLOCK) ON EE.UniqEntity = A.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType = p.UniqCdPolicyLineType
			--INNER JOIN broker  pb ON a.uniqentitybrokerbilling = pb.uniqentity 
			WHERE a.status & 16 = 0 
			and (A.ClosedDate is null or A.ClosedDate> DATEADD(mm,-18,@GetDate))
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)
			
			SELECT UniqEntity,UniqActivity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,CdPolicyLineTypeCode
			,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,[Status],UniqAgency,UniqBranch,UniqDepartment
			,UniqProfitCenter,UniqAssociatedItem,AssociationType,OwnerName,ClosedDate,UniqPolicy,UniqLine,UniqClaim
			,LossDate,Policydescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate
			FROM #HIBOPGetActivityDetailsTemp 
			ORDER BY UniqActivity
			OFFSET (@PageNumber-1)*@RowsPerPage ROWS
			FETCH NEXT @RowsPerPage ROWS ONLY
			
			SELECT @RowCount = count (UniqActivity)
			FROM #HIBOPGetActivityDetailsTemp    (nolock)
			--WHERE UploadGuid=@UploadGuid

			--DELETE FROM HIBOPGetActivityDetailsTemp WHERE UploadGuid=@UploadGuid
		END
		

		IF @PageNumber = 1 
		BEGIN

		 SELECT @DeltaSyncdate=MAX(ColumnValue)
		 from (
				select max(InsertedDate)Max_inst,max(UpdatedDate)Max_upd,max(ClosedDate)Max_cls
				from #HIBOPGetActivityDetailsTemp
			  )a
		 Unpivot(ColumnValue For ColumnName IN (Max_inst,Max_upd,Max_cls)) AS H

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @EmployeeLookupCode,@IPAddress,'HIBOPGetActivityDetails_SP',@DeltaSyncdate
		END

		Drop Table #HIBOPEmployeeStructure
		Drop Table #EntityEmployee
		DROP TABLE #HIBOPGetActivityDetailsTemp

		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetActivtiyDetails_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
