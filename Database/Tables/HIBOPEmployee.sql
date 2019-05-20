IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPEmployee')
BEGIN	

	CREATE TABLE HIBOPEmployee
	(
	UniqEntity		INT NOT NULL,
	LookupCode		CHAR(6),
	EmployeeName	VARCHAR(125),
	Department		VARCHAR(60),
	JobTitle		VARCHAR(60),
	InactiveDate	DATETIME,
	RoleFlags		INT,
	Flags			INT,
	InsertedDate	DATETIME,
	UpdatedDate		DATETIME
	)
END