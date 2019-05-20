/*
-- =============================================
-- Module :		PullErrorLogFromCentralizedDB
-- Author:      BALACHANDAR.C
-- Create Date: 26-OCT-17
-- Description: This Procedure is used to Pull data from local DB to Centralized DB about application errors
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPSyncErrorLogToLocal_SP 65537,'2017-10-25'
EXEC HIBOPSyncErrorLogToLocal_SP 65537,''

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncErrorLogToLocal_SP')
   EXEC('CREATE PROCEDURE [HIBOPSyncErrorLogToLocal_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[HIBOPSyncErrorLogToLocal_SP]
(
@User		 VARCHAR(10),
@LastSyncDate DATETIME
)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN
			SELECT 
				[Source],
				Thread,
				[Level],
				Logger,
				[Message],
				Exception,
				LoggedBy,
				LogDate				
				FROM HIBOPErrorLog E WITH(NOLOCK)
			WHERE E.LoggedBy=@User AND E.LogDate > @LastSyncDate
		END	
		ELSE BEGIN
			SELECT 
				[Source],
				Thread,
				[Level],
				Logger,
				[Message],
				Exception,
				LoggedBy,
				LogDate
				FROM HIBOPErrorLog E WITH(NOLOCK)
			WHERE E.LoggedBy=@User

		END
	 END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPSynzClientToLocal_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	 SET NOCOUNT OFF

END
