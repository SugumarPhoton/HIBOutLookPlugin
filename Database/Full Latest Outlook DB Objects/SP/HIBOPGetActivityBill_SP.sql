/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityBill_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityBill_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityBill_SP] AS' 
END
GO


--EXEC HIBOPGetActivityBill_SP 'VIDJA1',null,'1.1.1.1'

ALTER PROCEDURE [dbo].[HIBOPGetActivityBill_SP]
(@User VARCHAR(10),
@LastSyncDate DATETIME
, @IPAddress Varchar(100)
)
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT
	    Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

		SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode =@User
	

	IF @LastSyncDate >'1900-01-01'
	BEGIN
		SELECT DISTINCT E.UniqEmployee,
			B.BillId,
			B.UniqTranshead,
			B.UniqEntity,
			B.BillNumber,
			B.UniqAgency,
			A.AgencyName,
			B.Amount,
			B.Balance,
			B.InsertedDate,
			B.UpdatedDate
		FROM HIBOPActivityBill B WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON B.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		INNER JOIN HIBOPAgency A WITH(NOLOCK) ON A.UniqAgency=B.UniqAgency
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(B.UpdatedDate, B.InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN

		SELECT DISTINCT
			B.BillId,
			B.UniqTranshead,
			B.UniqEntity,
			B.BillNumber,
			B.UniqAgency,
			A.AgencyName,
			B.Amount,
			B.Balance,
			B.InsertedDate,
			B.UpdatedDate
		FROM HIBOPActivityBill B WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON B.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPAgency A WITH(NOLOCK) ON A.UniqAgency=B.UniqAgency
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee
	END
		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityBill_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityBill_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END


GO
