IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityPolicyLineMapping')
BEGIN	

	CREATE TABLE HIBOPActivityPolicyLineMapping
	(
	UniqActivity			INT,
	UniqPolicy				INT,
	UniqLine				INT,
	UniqMarketingLine		INT,
	UniqCarrierSubmission	INT
	)
END
