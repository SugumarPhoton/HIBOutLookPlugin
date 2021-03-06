IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityCode')
BEGIN	
	CREATE TABLE HIBOPActivityCode
	(
	UniqActivityCode		INT NOT NULL,
	ActivityCode			CHAR(4),
	ActivityName			VARCHAR(125),
	UniqActivityEvent		INT,
	OwnerTypeCode			CHAR(2),
	UniqEmployee			INT,
	ClosedStatus			CHAR(1),
	Flags					INT,
	InsertedDate			DATETIME,
	UpdatedDate				DATETIME
)
END
