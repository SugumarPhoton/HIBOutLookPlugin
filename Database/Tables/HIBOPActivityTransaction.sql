IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityTransaction')
BEGIN
	CREATE TABLE HIBOPActivityTransaction
	(
	TransactionId		INT NOT NULL IDENTITY(1,1),
	UniqTranshead		INT,
	Code				VARCHAR(6),
	DescriptionOf		VARCHAR(125),
	UniqEntity			INT,
	InvoiceNumber		INT,
	ItemNumber			INT,
	Amount				NUMERIC(19,4),
	Balance				NUMERIC(19,4),
	InsertedDate		DATETIME,
	UpdatedDate			DATETIME
	)
END
