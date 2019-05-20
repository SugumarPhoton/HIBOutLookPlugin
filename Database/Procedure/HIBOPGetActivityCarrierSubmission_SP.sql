IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityCarrierSubmission_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityCarrierSubmission_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[HIBOPGetActivityCarrierSubmission_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
	   
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode =@User

	IF @LastSyncDate >'1900-01-01'
	BEGIN
		SELECT DISTINCT
			CS.CarrierSubmissionId,
			CS.UniqCarrierSubmission,
			CS.Carrier,
			CS.CarrierSubmission,
			CS.UniqMarketingSubmission,
			MS.DescriptionOf as MarkettingSubmission,
			--A.UniqActivity,
			CS.UniqEntity,
			CS.LastSubmittedDate,
			CS.RequestedPremium,
			CS.SubmissionStatus
		FROM HIBOPCarrierSubmission CS WITH(NOLOCK)
		INNER JOIN HIBOPActivityMasterMarketing MS WITH(NOLOCK) ON CS.UniqMarketingSubmission=MS.UniqMarketingSubmission
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON CS.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		--INNER JOIN HIBOPActivity A WITH(NOLOCK) ON CS.UniqCarrierSubmission=A.UniqAssociatedItem-- AND A.AssociationType='Carrier'
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(CS.UpdatedDate, CS.InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN

		SELECT DISTINCT
			CS.CarrierSubmissionId,
			CS.UniqCarrierSubmission,
			CS.Carrier,
			CS.CarrierSubmission,
			CS.UniqMarketingSubmission,
			MS.DescriptionOf as MarkettingSubmission,
			--A.UniqActivity,
			CS.UniqEntity,
			CS.LastSubmittedDate,
			CS.RequestedPremium,
			CS.SubmissionStatus
		FROM HIBOPCarrierSubmission CS WITH(NOLOCK)
		INNER JOIN HIBOPActivityMasterMarketing MS WITH(NOLOCK) ON CS.UniqMarketingSubmission=MS.UniqMarketingSubmission
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON CS.UniqEntity=C.UniqEntity
		--INNER JOIN HIBOPActivity A WITH(NOLOCK) ON CS.UniqCarrierSubmission=A.UniqAssociatedItem --AND A.AssociationType='Carrier'
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee
	END
		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityCarrierSubmission_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

