IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetCommonLookUp_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetCommonLookUp_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetCommonLookUp_SP] 
(@LastSyncDate DATETIME)
AS 
BEGIN

    SET NOCOUNT ON

     BEGIN TRY		

	 
		IF @LastSyncDate IS NULL
		BEGIN
			SET @LastSyncDate='1900-01-01'
		END

	SELECT 
		CommonLkpId,
		CommonLkpTypeCode,
		CommonLkpCode,
		CommonLkpName,
		CommonLkpDescription,
		SortOrder,
		IsDeleted,
		CreatedDate,
		ModifiedDate
	FROM HIBOPCommonLookup WITH(NOLOCK)
	WHERE IsDeleted=1 AND ISNULL(ModifiedDate, CreatedDate)>@LastSyncDate

END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetCommonLookUp_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END