IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityLookupDetails_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityLookupDetails_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].HIBOPGetActivityLookupDetails_SP
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

