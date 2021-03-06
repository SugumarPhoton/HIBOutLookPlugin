/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityCertificate_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityCertificate_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityCertificate_SP] AS' 
END
GO
/*
EXEC [HIBOPGetActivityCertificate_SP] 'SHACH1',null,'1.1.1.1'
EXEC [HIBOPGetActivityCertificate_SP] 'SHACH1','2016-06-01 16:06:44.813','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityCertificate_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME
, @IPAddress Varchar(100))
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
		
		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityCertificate_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityCertificate_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
