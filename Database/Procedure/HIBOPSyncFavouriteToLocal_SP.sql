/*
-- =============================================
-- Module :		PullFromLocalDB
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to Pull data from centralized to LOCAL DB and list the details
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPSyncFavouriteToLocal_SP 

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncFavouriteToLocal_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncFavouriteToLocal_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [HIBOPSyncFavouriteToLocal_SP]
	(
				@User					VARCHAR(10),
				@LastSyncDate			DATETIME
	
	)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY

		DECLARE @UniqEmployee INT
	   
	   SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH (NOLOCK) WHERE LookUpCode =@User
		
		SELECT 
			F.FavouriteName,
			E.LookUpCode,
			F.UniqEntity,
			F.UniqActivity,
			F.PolicyYear,
			F.PolicyType,
			F.DescriptionType,
			F.Description,
			F.FolderId,
			F.SubFolder1Id,
			F.SubFolder2Id,
			F.Status,
			F.ClientAccessibleDate,
			F.InsertedDate
		FROM HIBOPFavourites F WITH(NOLOCK)
		LEFT OUTER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON F.UniqEmployee=E.UniqEmployee
		WHERE UniqEmployee=@UniqEmployee AND L.InsertedDate=@LastSyncDate
		
		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPSyncFavouriteToLocal_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END
