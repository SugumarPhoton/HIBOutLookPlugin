IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPActivityLine')
BEGIN
CREATE TABLE HIBOPActivityLine
(
UniqLine				INT NOT NULL,
UniqPolicy				INT,
UniqEntity				INT,
PolicyDesc				VARCHAR(125),
LineCode				char(4),
LineOfBusiness			VARCHAR(500),		
LineStatus				VARCHAR(500),
PolicyNumber			VARCHAR(25),
UniqCdPolicyLineType	INT,
UniqCdLineStatus		INT,
IOC						CHAR(6),
BillModeCode			CHAR(1),
ExpirationDate			DATETIME,
EffectiveDate			DATETIME,	
Flags					INT,
InsertedDate			DATETIME,
UpdatedDate				DATETIME
)
END
