/****** Object:  StoredProcedure [dbo].[HIBOPSyncIntervalMinutes]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncIntervalMinutes]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncIntervalMinutes] AS' 
END
GO
ALTER PROCEDURE [dbo].[HIBOPSyncIntervalMinutes]
(
@IntervalMinutes int output,
@ClientActIntervalMinutes int output
)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
        SELECT @IntervalMinutes = cast(CommonLkpName as int) --Minutes
		FROM HIBOPCommonLookup(NOLOCK)
		WHERE CommonLkpTypeCode = 'SyncIntervalMinutes'
		and   CommonLkpCode     = 'SIM'
		
		SELECT @ClientActIntervalMinutes = cast(CommonLkpName as int) --Minutes
		FROM HIBOPCommonLookup(NOLOCK)
		WHERE CommonLkpTypeCode = 'ClientActivitySyncIntervalMinutes'
		and   CommonLkpCode     = 'CASIM'

		SELECT
		@IntervalMinutes AS IntervalMinutes--,@ClientActIntervalMinutes AS ClientActIntervalMinutes
   	 END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPSyncIntervalMin Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
