/****** Object:  StoredProcedure [dbo].[HIBOPSyncFavouriteToCentralized_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncFavouriteToCentralized_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncFavouriteToCentralized_SP] AS' 
END
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
				ClientAccessibleDate,[Status],InsertedDate,UserLookupCode,IPAddress
			) 
			SELECT 	T.FavName,E.UniqEmployee,T.UniqEntity,T.UniqActivity,T.PolicyYear,T.PolicyType,T.DescriptionType,T.[Description],T.FolderId,T.SubFolder1Id,T.SubFolder2Id,
				T.ClientAccessibleDate,1,T.InsertedDate,T.UserLookupCode,t.IPAddress FROM @HIBOPFavorite T
				INNER JOIN (SELECT DISTINCT UniqEmployee,Lookupcode FROM HIBOPEntityEmployee WITH(NOLOCK)) E ON T.UniqEmployee=E.Lookupcode
		END
		END TRY

	 BEGIN CATCH
        
		SELECT 'Insert Failed For HIBOPSyncFavouriteToCentralized_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
