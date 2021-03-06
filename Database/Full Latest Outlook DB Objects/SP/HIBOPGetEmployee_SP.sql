/****** Object:  StoredProcedure [dbo].[HIBOPGetEmployee_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetEmployee_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetEmployee_SP] AS' 
END
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
GO
