IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPPolicyLineType')
BEGIN	
	CREATE TABLE HIBOPPolicyLineType
	(
	UniqCdPolicyLineType		INT NOT NULL,
	CdPolicyLineTypeCode		VARCHAR(4) NOT NULL,
	PolicyLineTypeDesc			VARCHAR(125),	
	[Status]					INT,
	[InsertedDate]				DATETIME NOT NULL,
	[UpdatedDate]				DATETIME,
	[UniqDepartment]			INT,
	[UniqProfitCenter]			INT	
	)
END

