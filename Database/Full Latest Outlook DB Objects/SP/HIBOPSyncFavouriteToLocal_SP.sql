/****** Object:  StoredProcedure [dbo].[HIBOPSyncFavouriteToLocal_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncFavouriteToLocal_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncFavouriteToLocal_SP] AS' 
END
GO

ALTER PROCEDURE [dbo].[HIBOPSyncFavouriteToLocal_SP]
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

GO
