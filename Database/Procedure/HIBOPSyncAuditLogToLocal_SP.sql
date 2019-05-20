IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncAuditLogToLocal_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncAuditLogToLocal_SP] AS BEGIN SET NOCOUNT ON; END')
GO
Alter PROCEDURE [dbo].[HIBOPSyncAuditLogToLocal_SP]
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
			L.UniqId,
			L.UniqEntity,
			C.LookupCode AS ClientLookupCode,
			C.NameOf as ClientName,
			C.PrimaryContactName ,
			L.UniqActivity,
			A.ActivityCode,
			A.DescriptionOf,
			A.PolicyNumber,
			L.PolicyYear,
			L.PolicyType,
			L.DescriptionType,
			L.[Description],
			L.FolderId,
			F.FolderName AS FolderNameF,
			L.SubFolder1Id,
			F1.FolderName AS FolderNameF1,
			L.SubFolder2Id,
			F2.FolderName AS FolderNameF2,
			L.ClientAccessibleDate,
			L.EmailAction,
			L.[Version],
			L.InsertedDate,
			L.UpdatedDate
		FROM HIBOPOutlookPluginLog L WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON L.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPActivity A WITH(NOLOCK) ON L.UniqActivity=A.UniqActivity
		INNER JOIN HIBOPFolderAttachment F WITH(NOLOCK) ON L.FolderId=F.ParentFolderId
		INNER JOIN HIBOPFolderAttachment F1 WITH(NOLOCK) ON L.SubFolder1Id=F1.ParentFolderId
		INNER JOIN HIBOPFolderAttachment F2 WITH(NOLOCK) ON L.SubFolder2Id=F1.ParentFolderId
		WHERE L.UniqEmployee=@UniqEmployee AND ISNULL(L.UpdatedDate,L.InsertedDate)>@LastSyncDate
		
		END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPSyncLogToLocal_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END