/*
-- =============================================
-- Module :		PullErrorLogFromLocalDB
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to Pull data from local DB to Centralized DB about application errors
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPSyncErrorLogToCenteralized_SP 65537,'2017-10-25'
EXEC HIBOPSyncErrorLogToCenteralized_SP 65537,''

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncErrorLogToCenterlized_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncErrorLogToCenterlized_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPSyncErrorLogToCenterlized_SP]
(
@ErrorLogInfo HIBOPErrorLog_UT Readonly
)
AS 
BEGIN
     SET NOCOUNT ON

     BEGIN TRY
			INSERT INTO HIBOPErrorLog ([Source],Thread,[Level],Logger,[Message],Exception,LoggedBy,LogDate)
			SELECT [Source],Thread,[Level],Logger,[Message],Exception,LoggedBy,LogDate FROM @ErrorLogInfo
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPSyncErrorLogToCenterlized_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END
