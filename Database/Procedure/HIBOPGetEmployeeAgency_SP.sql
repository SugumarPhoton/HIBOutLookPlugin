IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetEmployeeAgency_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetEmployeeAgency_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetEmployeeAgency_SP]
( @User VARCHAR(6),@LastSyncDate DATETIME )
AS
BEGIN
     SET NOCOUNT ON

     BEGIN TRY		

	 
	IF @LastSyncDate IS NULL
	BEGIN
		SET @LastSyncDate='1900-01-01'
	END
	 
		SELECT
			EA.UniqEntity,
			E.EmployeeName,
			E.LookupCode,
			EA.UniqAgency,
			A.AgencyCode,
			A.AgencyName,
			EA.UniqBranch,
			B.BranchCode,
			B.BranchName,
			EA.UniqDepartment,
			D.DepartmentCode,
			D.NameOf as DepartmentName,
			EA.UniqProfitCenter,
			P.ProfitCenterCode,
			P.NameOf as ProfitCenterName
		FROM HIBOPEmployeeStructure EA WITH(NOLOCK)
		INNER JOIN HIBOPEmployee E WITH(NOLOCK) ON EA.UniqEntity=E.UniqEntity
		INNER JOIN HIBOPAgency A WITH(NOLOCK) ON EA.UniqAgency=A.UniqAgency
		INNER JOIN HIBOPBranch B WITH(NOLOCK) ON EA.UniqBranch=B.UniqBranch
		INNER JOIN HIBOPDepartment D WITH(NOLOCK) ON EA.UniqDepartment=D.UniqDepartment
		INNER JOIN HIBOPProfitCenter P WITH(NOLOCK) ON EA.UniqProfitCenter=P.UniqProfitCenter
		WHERE E.LookupCode=@User --AND ISNULL(EA.UpdatedDate,EA.InsertedDate)>@LastSyncDate
		
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetEmployeeAgency_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END