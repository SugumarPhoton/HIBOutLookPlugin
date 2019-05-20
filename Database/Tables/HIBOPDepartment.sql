IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPDepartment')
BEGIN
	CREATE TABLE HIBOPDepartment
	(
	UniqDepartment	INT,
	DepartmentCode	CHAR(3),
	NameOf			VARCHAR(100),	
	InsertedDate	DATETIME,		
	UpdatedDate		DATETIME,
	Flags			INT
	)
END

