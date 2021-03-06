/****** Object:  StoredProcedure [dbo].[HIBOPUserDeltaSyncUpdate_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSyncUpdate_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPUserDeltaSyncUpdate_SP] AS' 
END
GO
--[dbo].[HIBOPUserDeltaSyncUpdate_SP] 'LEUNA1','HIBOPGetActivityAccount_SP','2018-09-17 05:29:37.230'
ALTER  PROCEDURE [dbo].[HIBOPUserDeltaSyncUpdate_SP] --'LEUNA1',Null,23,1,23
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


GO
