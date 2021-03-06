/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityCarrierSubmission_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityCarrierSubmission_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityCarrierSubmission_SP] AS' 
END
GO

/*
EXEC [HIBOPGetActivityCarrierSubmission_SP] 'SHACH1',null,'1.1.1.1'
EXEC [HIBOPGetActivityCarrierSubmission_SP] 'SHACH1','2016-06-01 16:06:44.813','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityCarrierSubmission_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME
, @IPAddress Varchar(100))
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

		DECLARE @UniqEmployee INT

		Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	   
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
			CS.SubmissionStatus,
			E.UniqEmployee,CS.UpdatedDate, CS.InsertedDate
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
		
    Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityCarrierSubmission_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityCarrierSubmission_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
