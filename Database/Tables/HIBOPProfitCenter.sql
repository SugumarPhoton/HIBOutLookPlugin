IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPProfitCenter')
BEGIN
	CREATE TABLE HIBOPProfitCenter
	(
	UniqProfitCenter	INT NOT NULL,
	ProfitCenterCode	CHAR(4),
	NameOf				VARCHAR(100),	
	InsertedDate		DATETIME,		
	UpdatedDate			DATETIME,
	Flags				INT
	)
END