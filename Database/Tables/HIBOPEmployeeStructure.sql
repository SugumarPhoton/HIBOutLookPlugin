IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPEmployeeStructure')
BEGIN	
	CREATE TABLE HIBOPEmployeeStructure
	(
		UniqEntity			int			Null,
		UniqAgency			int			Null,
		UniqBranch			int			Null,
		UniqDepartment		int			Null,
		UniqProfitCenter	int			Null
	)
END
GO