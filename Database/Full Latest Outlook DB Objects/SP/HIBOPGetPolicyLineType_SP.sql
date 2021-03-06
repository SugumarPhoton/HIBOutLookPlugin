/****** Object:  StoredProcedure [dbo].[HIBOPGetPolicyLineType_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetPolicyLineType_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetPolicyLineType_SP] AS' 
END
GO
/*
EXEC [HIBOPGetPolicyLineType_SP] 'VIDJA1',NULL,'1.1.1.1'
EXEC [HIBOPGetPolicyLineType_SP] 'VIDJA1','2017-12-12 08:04:53.360','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetPolicyLineType_SP]
(   @User VARCHAR(10),
	@LastSyncDate DATETIME,
	@IPAddress Varchar(100)
)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY

	    Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
		
		IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
		BEGIN
		
			SELECT 
				P.CdPolicyLineTypeCode,
				P.PolicyLineTypeDesc,
				P.UniqCdPolicyLineType,
				P.InsertedDate,
				P.UpdatedDate
			FROM HIBOPPolicyLineType P WITH(NOLOCK)
			WHERE ISNULL(P.UpdatedDate,P.InsertedDate)>@LastSyncDate
		END
		ELSE BEGIN
			SELECT 
				P.CdPolicyLineTypeCode,
				P.PolicyLineTypeDesc,
				P.UniqCdPolicyLineType,
				P.InsertedDate,
				P.UpdatedDate
			FROM HIBOPPolicyLineType P WITH(NOLOCK)

		END

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetPolicyLineType_SP',@DeltaSyncdate

	END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPGetPolicyLineType_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
