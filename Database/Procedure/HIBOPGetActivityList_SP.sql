IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityList_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityList_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityList_SP]
(@User VARCHAR(10),@LastSyncDate DATETIME)
AS 
BEGIN
     SET NOCOUNT ON

     BEGIN TRY	
	 
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
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityList_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END
GO