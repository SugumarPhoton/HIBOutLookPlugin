IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityCarrierSubmission_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityCarrierSubmission_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].HIBOPGetActivityCarrierSubmission_SP
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
	   
		SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH (NOLOCK) WHERE LookUpCode =@User

	SELECT DISTINCT
		CS.UniqCarrierSubmission,
		CS.Carrier,
		CS.CarrierSubmission,
		CS.UniqMarketingSubmission,
		CS.UniqActivity,
		CS.UniqEntity,
		CS.LastSubmittedDate,
		CS.RequestedPremium,
		CS.SubmissionStatus
	FROM HIBOPCarrierSubmission CS WITH(NOLOCK)
	INNER JOIN HIBOPActivityMasterMarketing MS WITH(NOLOCK) ON CS.CarrierSubmission=MS.UniqMarketingSubmission
	INNER JOIN HIBOPClient C WITH(NOLOCK) ON CS.UniqEntity=C.UniqEntity
	INNER JOIN HIBOPEntityEmployee EE WITH(NOLOCK) ON C.UniqEntity=EE.UniqEntity
	WHERE EE.UniqEmployee=@UniqEmployee AND ISNULL(CS.UpdatedDate, CS.InsertedDate)>@LastSyncDate


END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityCarrierSubmission_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

