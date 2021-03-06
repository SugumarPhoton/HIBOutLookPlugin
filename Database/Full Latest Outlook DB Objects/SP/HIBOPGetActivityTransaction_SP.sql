/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityTransaction_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityTransaction_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityTransaction_SP] AS' 
END
GO
/*
EXEC [HIBOPGetActivityTransaction_SP] 'MARJA1',NUll,'172.16.108.92'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityTransaction_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME,@IPAddress Varchar(100))
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
		Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	   
		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode =@User
		--SELECT LookUpCode  FROM HIBOPEmployee WITH (NOLOCK) WHERE UniqEntity in (select UniqEntity from #tmp)
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
		
   Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityTransaction_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityTransaction_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
