/*
-- =============================================
-- Module :		PullActivityDetailsFromEpic
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to Pull data from EPIC to centralized database
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPGetActivityDetails_SP 'caram1',null,10,1,1
EXEC HIBOPGetActivityDetails_SP 'A0001',''

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityDetails_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityDetails_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityDetails_SP]
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
		DECLARE @GetDate Datetime = GetDate()
		DECLARE @securityuserid AS int
		DECLARE @UniqEmployee INT
		DECLARE @iCheckStructure AS TINYINT
		DECLARE @iCheckEmployeeAccess AS TINYINT
		DECLARE @UploadGuid varchar(128)
		set @UploadGuid=newid()

		SELECT @EmployeeLookupCode = LTRIM(RTRIM(@EmployeeLookupCode))
	
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @EmployeeLookupCode

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
			CASE WHEN A.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END 'Status',
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
			INNER JOIN EpicDMZSub.dbo.company ic ON a.uniqentitycompanyissuing = ic.uniqentity 
			INNER JOIN EpicDMZSub.dbo.company pc ON a.uniqentitycompanybilling = pc.uniqentity 
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON O.UniqEntity = A.UniqEmployee
			INNER JOIN #EntityEmployee EE WITH(NOLOCK) ON EE.UniqEntity = A.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType = p.UniqCdPolicyLineType
			--INNER JOIN broker  pb ON a.uniqentitybrokerbilling = pb.uniqentity 
			WHERE a.status & 16 = 0 
			and (A.ClosedDate is null or A.ClosedDate> DATEADD(mm,-18,@GetDate))
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END)

			AND  ISNULL(A.UpdatedDate, A.InsertedDate) > @LastSyncDate
						
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
			CASE WHEN A.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END 'Status',
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
			INNER JOIN EpicDMZSub.dbo.company ic ON a.uniqentitycompanyissuing = ic.uniqentity 
			INNER JOIN EpicDMZSub.dbo.company pc ON a.uniqentitycompanybilling = pc.uniqentity 
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
		
		Drop Table #HIBOPEmployeeStructure
		Drop Table #EntityEmployee
		DROP TABLE #HIBOPGetActivityDetailsTemp

		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetActivtiyDetails_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END