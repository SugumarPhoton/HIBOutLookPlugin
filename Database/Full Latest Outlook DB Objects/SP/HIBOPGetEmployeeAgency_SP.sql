/****** Object:  StoredProcedure [dbo].[HIBOPGetEmployeeAgency_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetEmployeeAgency_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetEmployeeAgency_SP] AS' 
END
GO
/*
EXEC [HIBOPGetEmployeeAgency_SP] 'SHACH1',null,'1.1.1.1'
EXEC [HIBOPGetEmployeeAgency_SP] 'SHACH1','2017-04-17 15:51:56.020','1.1.1.1'
*/
ALTER PROCEDURE [dbo].[HIBOPGetEmployeeAgency_SP]
( @User VARCHAR(6),@LastSyncDate DATETIME,@IPAddress Varchar(100) )
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

		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetEmployeeAgency_SP',@DeltaSyncdate
		
	 END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetEmployeeAgency_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

	 SET NOCOUNT OFF
END

GO
