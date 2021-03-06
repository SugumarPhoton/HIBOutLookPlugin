IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityClientContacts_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityClientContacts_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityClientContacts_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY
		
	DECLARE @UniqEmployee INT
	   
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

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For HIBOPGetActivityClientContacts_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END