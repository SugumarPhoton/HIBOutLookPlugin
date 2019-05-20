/*
-- =============================================
-- Module :		OUTLOOK PULGIN
-- Author:      Vairavan
-- Create Date: 05-Sep-2018
-- Description: This Procedure is used to display details of Sync interval in minutes.
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------
-- Unit Testing
---------------
Declare @IntervalMinutes int ,
		@ClientActIntervalMinutes int 
exec [dbo].[HIBOPSyncIntervalMinutes]  @IntervalMinutes  output,@ClientActIntervalMinutes  output
select @IntervalMinutes,@ClientActIntervalMinutes

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/

IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPSyncIntervalMinutes')
   EXEC('CREATE PROCEDURE [HIBOPSyncIntervalMinutes] AS BEGIN SET NOCOUNT ON; END')
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

