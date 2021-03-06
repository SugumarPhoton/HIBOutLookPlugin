/****** Object:  StoredProcedure [dbo].[HIBOPGetCommonLookUp_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetCommonLookUp_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetCommonLookUp_SP] AS' 
END
GO
/*
EXEC HIBOPGetCommonLookUp_SP 'MARJA1',NULL,'172.16.108.92'
EXEC HIBOPGetCommonLookUp_SP 'LEUNA1','2018-08-05 05:58:12.163','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetCommonLookUp_SP] 
(
@User VARCHAR(10),
@LastSyncDate DATETIME,
@IPAddress Varchar(100))
AS 
BEGIN

    SET NOCOUNT ON

     BEGIN TRY		

	  Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	 
		IF @LastSyncDate IS NULL
		BEGIN
			SET @LastSyncDate='1900-01-01'
		END

	SELECT 
		CommonLkpId,
		CommonLkpTypeCode,
		CommonLkpCode,
		CommonLkpName,
		CommonLkpDescription,
		SortOrder,
		IsDeleted,
		CreatedDate,
		ModifiedDate
	FROM HIBOPCommonLookup WITH(NOLOCK)
	WHERE IsDeleted=1 AND ISNULL(ModifiedDate, CreatedDate)>@LastSyncDate

	Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetCommonLookUp_SP',@DeltaSyncdate

END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetCommonLookUp_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END

GO
