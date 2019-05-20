IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityBill_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityBill_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityBill_SP]
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
		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityBill_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

