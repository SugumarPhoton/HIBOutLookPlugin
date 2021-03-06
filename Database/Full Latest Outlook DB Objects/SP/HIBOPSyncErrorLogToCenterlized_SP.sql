/****** Object:  StoredProcedure [dbo].[HIBOPSyncErrorLogToCenterlized_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncErrorLogToCenterlized_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncErrorLogToCenterlized_SP] AS' 
END
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

GO
