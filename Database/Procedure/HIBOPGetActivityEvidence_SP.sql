IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityEvidence_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityEvidence_SP] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE HIBOPGetActivityEvidence_SP
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
			EV.UniqEvidence,
			EV.UniqEntityClient,
			EV.Title,
			FormEditionDate,
			EV.InsertedDate,
			EV.UpdatedDate
		FROM HIBOPActivityEvidenceOfInsurance EV WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON C.UniqEntity=EV.UniqEntityClient
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(EV.UpdatedDate, EV.InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN

		SELECT DISTINCT
			EV.UniqEvidence,
			EV.UniqEntityClient,
			EV.Title,
			FormEditionDate,
			EV.InsertedDate,
			EV.UpdatedDate
		FROM HIBOPActivityEvidenceOfInsurance EV WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON C.UniqEntity=EV.UniqEntityClient
		INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee
	END
		

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityEvidence_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

