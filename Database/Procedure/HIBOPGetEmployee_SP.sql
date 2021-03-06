IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPGetEmployee_SP')
   EXEC('CREATE PROCEDURE [HIBOPGetEmployee_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPGetEmployee_SP]
(@User Varchar(2000) )
AS
BEGIN
     SET NOCOUNT ON

     BEGIN TRY	
	 
	 DECLARE @EmployeeList TABLE(EmpCode VARCHAR(6))

	 INSERT INTO @EmployeeList
	 SELECT Item from dbo.HIBOPSplitString(@User,',')	

	
	SELECT 
		UniqEntity,
		LookupCode,
		EmployeeName,
		Department,
		JobTitle,
		InactiveDate,
		RoleFlags,
		e.Flags,
		InsertedDate,
		UpdatedDate,
		CASE WHEN SU.Flags & 2=2 AND su.TypeCode='E' THEN 1 ELSE 0 END AS IsAdmin
	FROM HIBOPEmployee E
	INNER JOIN @EmployeeList L ON E.LookupCode=l.EmpCode
	LEFT JOIN epicdmzsub.dbo.SECURITYUSER AS su WITH(NOLOCK) ON E.UniqEntity=su.UniqEmployee AND l.EmpCode=su.UserCode COLLATE Latin1_General_CI_AS
	--WHERE LookupCode=@User 

	END TRY

	 BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetEmployee_SP Error MSG : '+ERROR_MESSAGE()
     END CATCH 

END