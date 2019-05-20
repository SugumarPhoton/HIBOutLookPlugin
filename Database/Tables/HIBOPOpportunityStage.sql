IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPOpportunityStage')
BEGIN	

	CREATE TABLE HIBOPOpportunityStage
	(
	UniqOpportunityStage	INT,
	OpportunityStage		VARCHAR(100)
	)
END
GO