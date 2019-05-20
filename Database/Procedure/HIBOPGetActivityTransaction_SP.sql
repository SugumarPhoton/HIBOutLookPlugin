IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityTransaction_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityTransaction_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].HIBOPGetActivityTransaction_SP
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
	   
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode =@User

	IF @LastSyncDate >'1900-01-01'
	BEGIN
		SELECT DISTINCT
			T.TransactionId,
			T.UniqTranshead,
			T.UniqEntity,
			T.InvoiceNumber,
			T.Code,
			T.DescriptionOf,
			T.Amount,
			T.Balance,
			T.InsertedDate,
			T.UpdatedDate
		FROM HIBOPActivityTransaction T WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON T.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(T.UpdatedDate, T.InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN

		SELECT DISTINCT
			T.TransactionId,
			T.UniqTranshead,
			T.UniqEntity,
			T.InvoiceNumber,
			T.Code,
			T.DescriptionOf,
			T.Amount,
			T.Balance,
			T.InsertedDate,
			T.UpdatedDate
		FROM HIBOPActivityTransaction T WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON T.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee
	END
		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityTransaction_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


