/****** Object:  StoredProcedure [dbo].[HIBOPGetFolders_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetFolders_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetFolders_SP] AS' 
END
GO
/*
EXEC [HIBOPGetFolders_SP] 'LEUNA1',NULL,'1.1.1.1'
EXEC [HIBOPGetFolders_SP] 'VIDJA1','2017-12-12 08:04:53.373','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetFolders_SP]
(@User VARCHAR(10),
--@ParentFolderid INT,
@LastSyncDate DATETIME,
@IPAddress Varchar(100)
)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
		
		Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

		IF (@LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01')-- AND (@ParentFolderid IS NULL OR @ParentFolderid='') 
		BEGIN
		
			SELECT 
				F.FolderId,
				F.ParentFolderId,
				F.FolderName,
				F.FolderType,
				F.InsertedDate,
				F.UpdatedDate
			FROM HIBOPFolderAttachment F WITH(NOLOCK)
			WHERE F.InsertedDate>@LastSyncDate
			
	END
	ELSE BEGIN
			SELECT 
				F.FolderId,
				F.ParentFolderId,
				F.FolderName,
				F.FolderType,
				F.InsertedDate,
				F.UpdatedDate
			FROM HIBOPFolderAttachment F WITH(NOLOCK)
			--WHERE ParentFolderId=@ParentFolderid 
			ORDER BY FolderId
	END

	Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetFolders_SP',@DeltaSyncdate

	END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetFolders_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
