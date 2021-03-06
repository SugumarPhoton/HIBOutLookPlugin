/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityEvidence_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityEvidence_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityEvidence_SP] AS' 
END
GO
/*
EXEC [HIBOPGetActivityEvidence_SP] 'SHACH1',null,'1.1.1.1'
EXEC [HIBOPGetActivityEvidence_SP] 'SHACH1','2017-04-17 15:51:56.020','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityEvidence_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME, @IPAddress Varchar(100))
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY

	 
		DECLARE @UniqEmployee INT

		Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	   
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

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityEvidence_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityEvidence_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
