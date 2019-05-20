IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPFolderAttachment')
BEGIN	
	CREATE TABLE HIBOPFolderAttachment
	(
		FolderId				INT				NOT NULL,
		ParentFolderId			INT				NOT NULL,
		FolderType				VARCHAR(20)		NOT NULL,
		FolderName				VARCHAR(100)	NOT NULL,
		Status					VARCHAR(10)		NULL,
		[InsertedDate]			DATETIME 		NOT NULL,
		[UpdatedDate]			DATETIME		
	)
END
GO
