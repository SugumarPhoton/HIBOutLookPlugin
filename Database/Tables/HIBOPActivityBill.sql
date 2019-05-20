IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityBill')
BEGIN
	CREATE TABLE HIBOPActivityBill
	(
	BillId				INT NOT NULL IDENTITY(1,1),
	UniqTranshead		INT,
	DescriptionOf		VARCHAR(125),
	UniqEntity			INT,
	BillNumber			INT,
	UniqAgency			INT,
	AgencyName			VARCHAR(125),
	Amount				NUMERIC(19,4),
	Balance				NUMERIC(19,4),
	InsertedDate		DATETIME,
	UpdatedDate			DATETIME
	)
END
