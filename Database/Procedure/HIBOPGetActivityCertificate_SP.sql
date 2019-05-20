IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityCertificate_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityCertificate_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE HIBOPGetActivityCertificate_SP
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
			C.UniqCertificate,
			C.UniqEntity,
			C.Title,
			C.InsertedDate,
			C.UpdatedDate
		FROM HIBOPActivityCertificate C WITH(NOLOCK)
		INNER JOIN HIBOPClient CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON CL.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(C.UpdatedDate, C.InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN

		SELECT DISTINCT
			C.UniqCertificate,
			C.UniqEntity,
			C.Title,
			C.InsertedDate,
			C.UpdatedDate
		FROM HIBOPActivityCertificate C WITH(NOLOCK)
		INNER JOIN HIBOPClient CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON CL.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee
	END
		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityCertificate_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

