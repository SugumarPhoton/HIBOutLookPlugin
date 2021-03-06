/****** Object:  StoredProcedure [dbo].[HIBOPSyncAuditLogToCentralized_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPSyncAuditLogToCentralized_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPSyncAuditLogToCentralized_SP] AS' 
END
GO
ALTER PROCEDURE [dbo].[HIBOPSyncAuditLogToCentralized_SP]
	(
		@HIBOPLogInfo HIBOPLogInfo_UT READONLY
	
	)
AS
BEGIN
	 SET NOCOUNT ON
     BEGIN TRY
	 
		--DECLARE @UserLookUpCode VARCHAR(10)
		--DECLARE @UniqEmployee INT
	   
	 --   SELECT @UserLookUpCode = (SELECT DISTINCT UniqEmployee FROM @HIBOPLogInfo )
		--SELECT @UniqEmployee = UniqEmployee FROM HIBOPEntityEmployee WITH(NOLOCK) WHERE Lookupcode=@UserLookUpCode
		
			INSERT INTO HIBOPOutlookPluginLog
			(
			UniqId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,[Description],FolderId,SubFolder1Id,
			SubFolder2Id,ClientAccessibleDate,EmailAction,[Version],InsertedDate,UpdatedDate,ClientLookupCode
			)
			SELECT T.UniqId,E.UniqEmployee,T.UniqEntity,T.UniqActivity,T.PolicyYear,T.PolicyType,T.DescriptionType,T.[Description],T.FolderId,T.SubFolder1Id,
			T.SubFolder2Id,T.ClientAccessibleDate,T.EmailAction,T.[Version],T.InsertedDate,T.UpdatedDate,T.ClientLookupCode FROM @HIBOPLogInfo T
			INNER JOIN (SELECT DISTINCT UniqEmployee,Lookupcode FROM HIBOPEntityEmployee WITH(NOLOCK)) E ON T.UniqEmployee=E.Lookupcode

	 END TRY

	 BEGIN CATCH
        
		SELECT 'Insert Failed For HIBOPSyncAuditLogToCentralized_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END

GO
