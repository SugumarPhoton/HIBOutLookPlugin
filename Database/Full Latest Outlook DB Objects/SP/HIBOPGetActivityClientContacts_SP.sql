/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityClientContacts_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityClientContacts_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityClientContacts_SP] AS' 
END
GO
/*
EXEC [HIBOPGetActivityClientContacts_SP] 'SHEJO1',null,'1.1.1.1'
EXEC [HIBOPGetActivityClientContacts_SP] 'SHEJO1','2017-06-17 20:07:16.053','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityClientContacts_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME, @IPAddress Varchar(100))
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY
		
	DECLARE @UniqEmployee INT

	Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	   
	SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH (NOLOCK) WHERE LookUpCode =@User
	
	IF (@LastSyncDate >'1900-01-01' OR @LastSyncDate IS NOT NULL)
	BEGIN
		SELECT 
			ClientContactId,
			C.UniqEntity,
			UniqContactName,
			UniqContactNumber,
			ContactName,
			ContactType,
			ContactValue,
			InsertedDate,
			UpdatedDate
		FROM HIBOPActivityClientContacts C WITH(NOLOCK)
		INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee AND ISNULL(UpdatedDate,InsertedDate)>@LastSyncDate
	END
	ELSE BEGIN
		SELECT 
			ClientContactId,
			C.UniqEntity,
			UniqContactName,
			UniqContactNumber,
			ContactName,
			ContactType,
			ContactValue,
			InsertedDate,
			UpdatedDate
		FROM HIBOPActivityClientContacts C WITH(NOLOCK)
		INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		WHERE E.UniqEmployee=@UniqEmployee

	END

	Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityClientContacts_SP',@DeltaSyncdate

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityClientContacts_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
