/*
-- =============================================
-- Module :		SyncLogDetailsToCenralizedDB
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to sync the data from local to centrazlized database
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPSyncLogToCentralized_SP 78625,'2013-01-25'
EXEC HIBOPSyncLogToCentralized_SP 78625,''

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncAuditLogToCentralized_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncAuditLogToCentralized_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPSyncAuditLogToCentralized_SP]
	(
		@HIBOPLogInfo HIBOPLogInfo_UT READONLY
	
	)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
	 
		--DECLARE @UserLookUpCode VARCHAR(10)
		--DECLARE @UniqEmployee INT
	   
	 --   SELECT @UserLookUpCode = (SELECT DISTINCT UniqEmployee FROM @HIBOPLogInfo )
		--SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH(NOLOCK) WHERE Lookupcode=@UserLookUpCode
		
			INSERT INTO HIBOPOutlookPluginLog
			(
			UniqId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,[Description],FolderId,SubFolder1Id,
			SubFolder2Id,ClientAccessibleDate,EmailAction,[Version],InsertedDate,UpdatedDate,ClientLookupCode
			)
			SELECT T.UniqId,E.UniqEmployee,T.UniqEntity,T.UniqActivity,T.PolicyYear,T.PolicyType,T.DescriptionType,T.[Description],T.FolderId,T.SubFolder1Id,
			T.SubFolder2Id,T.ClientAccessibleDate,T.EmailAction,T.[Version],T.InsertedDate,T.UpdatedDate,T.ClientLookupCode FROM @HIBOPLogInfo T
			INNER JOIN (SELECT DISTINCT UniqEmployee,Lookupcode FROM HIBOPEntityEmployee WITH(NOLOCK)) E ON T.UniqEmployee=E.Lookupcode

	 END TRY

	 BEGIN CATCH
        
		SELECT 'Insert Failed For HIBOPSyncAuditLogToCentralized_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END
