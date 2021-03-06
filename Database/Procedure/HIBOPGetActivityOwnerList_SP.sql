IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetActivityOwnerList_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetActivityOwnerList_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetActivityOwnerList_SP]
(@LastSynDate DATETIME,
@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT OUTPUT
) 
AS 
BEGIN
     SET NOCOUNT ON

     BEGIN TRY		


		SELECT
			DISTINCT E.Lookupcode,E.EmployeeName
		FROM HIBOPEmployee E WITH(NOLOCK)
		order by Lookupcode
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY

		SELECT @RowCount = COUNT(*)
		FROM HIBOPEmployee E WITH(NOLOCK)
		
		
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityOwnerList_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END
