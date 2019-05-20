/*
-- =============================================
-- Module :		OUTLOOK PULGIN
-- Author:      Balachandar
-- Create Date: 26-OCT-2017
-- Description: This Procedure is used to display details of Folders and Subfolders.
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------
-- Unit Testing
---------------

EXEC HIBOPGetFolders_SP '','2017-01-25'
EXEC HIBOPGetFolders_SP '65602',''

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/

IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetFolders_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetFolders_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetFolders_SP]
(
--@ParentFolderid INT,
@LastSyncDate DATETIME
)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
		
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
	END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetFolders_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END