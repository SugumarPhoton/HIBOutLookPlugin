/*
-- =============================================
-- Module :		OUTLOOK PULGIN
-- Author:      Balachandar
-- Create Date: 26-OCT-2017
-- Description: This Procedure is used to display details of PolicyLineType.
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------
-- Unit Testing
---------------

EXEC HIBOPGetPolicyLineType_SP '2017-01-25'

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetPolicyLineType_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetPolicyLineType_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetPolicyLineType_SP]
(
	@LastSyncDate DATETIME
)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN

			SELECT 
				P.CdPolicyLineTypeCode,
				P.PolicyLineTypeDesc,
				P.UniqCdPolicyLineType,
				P.InsertedDate,
				P.UpdatedDate
			FROM HIBOPPolicyLineType P WITH(NOLOCK)
			WHERE ISNULL(P.UpdatedDate,P.InsertedDate)>@LastSyncDate
		END
		ELSE BEGIN
			SELECT 
				P.CdPolicyLineTypeCode,
				P.PolicyLineTypeDesc,
				P.UniqCdPolicyLineType,
				P.InsertedDate,
				P.UpdatedDate
			FROM HIBOPPolicyLineType P WITH(NOLOCK)

		END
	END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetPolicyLineType_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END