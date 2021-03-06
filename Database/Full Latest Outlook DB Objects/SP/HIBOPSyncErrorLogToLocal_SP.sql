/****** Object:  StoredProcedure [dbo].[HIBOPSyncErrorLogToLocal_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncErrorLogToLocal_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncErrorLogToLocal_SP] AS' 
END
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

GO
