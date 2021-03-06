/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityList_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityList_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityList_SP] AS' 
END
GO
/*
EXEC [HIBOPGetActivityList_SP] 'LEUNA1','2017-11-30 23:14:22.950','1.1.1.1'
EXEC [HIBOPGetActivityList_SP] 'LEUNA1',null,'1.1.1.1'

*/
ALTER PROCEDURE [dbo].[HIBOPGetActivityList_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME,@IPAddress Varchar(100))
AS 
BEGIN
     SET NOCOUNT ON

     BEGIN TRY	

	 Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	 
	 IF @LastSyncDate IS NULL
	 BEGIN
		SET @LastSyncDate ='1900-01-01'
	 END
   
		SELECT
			DISTINCT UniqActivityCode,ActivityCode,ActivityName,
			A.InsertedDate,A.UpdatedDate,E.lookupCode,EmployeeName,
			case when a.ClosedStatus='S' THEN 1 else 0 END AS ISClosedStatus
		FROM HIBOPActivityCode A WITH(NOLOCK)
		LEFT OUTER JOIN HIBOPEmployee E WITH(NOLOCK) ON A.UniqEmployee=E.UniqEntity
		WHERE A.flags & 2 = 2 and A.UniqActivityEvent=-1
		AND ISNULL(A.UpdatedDate,A.InsertedDate)>@LastSyncDate 

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityList_SP',@DeltaSyncdate
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityList_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END

GO
