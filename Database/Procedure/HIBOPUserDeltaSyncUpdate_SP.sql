--[dbo].[HIBOPUserDeltaSyncUpdate_SP] 'LEUNA1','HIBOPGetActivityAccount_SP','2018-09-17 05:29:37.230'
alter  PROCEDURE [dbo].[HIBOPUserDeltaSyncUpdate_SP] --'LEUNA1',Null,23,1,23
(@User VARCHAR(10),
 @IPAddress Varchar(100),
 @SpName varchar(255),
 @LastSyncDate datetime
)
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY

	IF @LastSyncDate IS NULL
	Begin
	select @LastSyncDate='1900-01-01'
	end 

  Update HIBOPUserDeltaSyncInfo
  set IsDeltaFlag  = 0,
      LastSyncDate = @LastSyncDate,
	  UpdatedDate  = getdate()
  where UserLookupCode = @User
  and   IPAddress	   = @IPAddress
  and   SpName		   = @SpName

		
END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For Error MSG : '+ERROR_MESSAGE()

     END CATCH 

SET NOCOUNT OFF
END

