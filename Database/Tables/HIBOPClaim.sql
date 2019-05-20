IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPClaim')
BEGIN	
	CREATE TABLE HIBOPClaim
	(
	UniqEntity			INT NOT NULL,
	UniqClaim			INT NOT NULL,
	ClaimCode			VARCHAR(5),
	ClaimName			VARCHAR(125),	
	LossDate			DATETIME,
	ReportedDate		DATETIME,
	ClaimNumber			INT,
	CompanyClaimNumber	VARCHAR(25),
	ClosedDate			DATETIME,
	Flags				INT,
	InsertedDate		DATETIME,
	UpdatedDate			DATETIME
	)
END
