/*
-- =============================================
-- Module :		PullFromLocalDB
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to Pull data from local DB to centralized DB and list the details
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPGetActivtiyDetails_SP 

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncFavouriteToCentralized_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncFavouriteToCentralized_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[HIBOPSyncFavouriteToCentralized_SP]
	(
		@HIBOPFavorite HIBOPFavourite_UT READONLY
	
	)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
	 
		DECLARE @FavName VARCHAR(200)
		
	   
	    SELECT @FavName = FavName FROM @HIBOPFavorite 
		
		
		IF NOT EXISTS (SELECT 1 FROM HIBOPFavourite WHERE FavouriteName=@FavName)
		BEGIN
			INSERT INTO HIBOPFavourite
			(
				FavouriteName,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,[Description],FolderId,SubFolder1Id,SubFolder2Id,
				ClientAccessibleDate,[Status],InsertedDate
			) 
			SELECT 	T.FavName,E.UniqEmployee,T.UniqEntity,T.UniqActivity,T.PolicyYear,T.PolicyType,T.DescriptionType,T.[Description],T.FolderId,T.SubFolder1Id,T.SubFolder2Id,
				T.ClientAccessibleDate,1,T.InsertedDate FROM @HIBOPFavorite T
				INNER JOIN (SELECT DISTINCT UniqEmployee,Lookupcode FROM HIBOPEntityEmployee WITH(NOLOCK)) E ON T.UniqEmployee=E.Lookupcode
		END
		END TRY

	 BEGIN CATCH
        
		SELECT 'Insert Failed For HIBOPSyncFavouriteToCentralized_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END
