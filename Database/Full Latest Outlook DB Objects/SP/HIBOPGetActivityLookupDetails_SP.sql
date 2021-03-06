/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityLookupDetails_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityLookupDetails_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityLookupDetails_SP] AS' 
END
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityLookupDetails_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
	   
		SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH (NOLOCK) WHERE LookUpCode =@User

	IF @LastSyncDate >'1900-01-01'
	BEGIN
		SELECT DISTINCT

			ALDId,
			UniqLine,
			UniqPolicy,
			a.UniqEntity,
			UniqClaim,
			LineCode,
			PolicyType,
			Linedescription,
			PolicyNumber,
			PolicyDesc,
			UniqCdPolicyLineType,
			UniqCdLineStatus,
			LineExpDate,
			LineEffDate,
			PolicyExpDate,
			PolicyEffDate,
			ClaimNumber,
			CompanyClaimNumber,
			DateLoss,
			ClosedDate,
			A.LookupCode ,
			AccountName,
			InsertedDate,
			UpdatedDate,
			UniqClaimAssociation,
			IOC,
			IOCCode,
			AttachmentDesc
		FROM HIBOPActivityLookupDetails A WITH(NOLOCK)
		INNER JOIN(SELECT DISTINCT UniqEntity,uniqemployee from HIBOPEntityEmployee WITH(NOLOCK)) EE  ON A.UniqEntity=ee.UniqEntity
		WHERE EE.UniqEmployee=@UniqEmployee AND ISNULL(A.UpdatedDate, A.InsertedDate)>@LastSyncDate
	END
	ELSE
	BEGIN
		SELECT DISTINCT

			ALDId,
			UniqLine,
			UniqPolicy,
			a.UniqEntity,
			UniqClaim,
			LineCode,
			PolicyType,
			Linedescription,
			PolicyNumber,
			PolicyDesc,
			UniqCdPolicyLineType,
			UniqCdLineStatus,
			LineExpDate,
			LineEffDate,
			PolicyExpDate,
			PolicyEffDate,
			ClaimNumber,
			CompanyClaimNumber,
			DateLoss,
			ClosedDate,
			A.LookupCode ,
			AccountName,
			InsertedDate,
			UpdatedDate,
			UniqClaimAssociation,
			IOC,
			IOCCode,
			AttachmentDesc
		FROM HIBOPActivityLookupDetails A WITH(NOLOCK)
		INNER JOIN(SELECT DISTINCT UniqEntity,uniqemployee from HIBOPEntityEmployee WITH(NOLOCK)) EE  ON A.UniqEntity=ee.UniqEntity
		WHERE EE.UniqEmployee=@UniqEmployee

	END

		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityLookupDetails_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
