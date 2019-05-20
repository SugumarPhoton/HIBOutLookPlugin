IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPActivityOpportunity')
BEGIN
CREATE TABLE HIBOPActivityOpportunity
(
			UniqOpportunity			INT NOT NULL,
			UniqEntity				INT,
			OppDesc					VARCHAR(125),
			TargetedDate			DATETIME,
			ActualDate				DATETIME,
			UniqSalesTeam			INT,
			UniqEmployeeOwner		INT,
			UniqOpportunityStage	INT,
			OwnerName				VARCHAR(125),
			SalesTeam				VARCHAR(125),
			SalesManager			VARCHAR(125),
			Stage					VARCHAR(125),
			Flags					INT,
			InsertedDate			DATETIME,
			UpdatedDate				DATETIME
)


END

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HIBOPActivityOpportunity' AND COLUMN_NAME = 'UniqAgency')
BEGIN
ALTER TABLE HIBOPActivityOpportunity ADD UniqAgency INT NULL
END

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HIBOPActivityOpportunity' AND COLUMN_NAME = 'UniqBranch')
BEGIN
ALTER TABLE HIBOPActivityOpportunity ADD UniqBranch INT NULL
END

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HIBOPActivityOpportunity' AND COLUMN_NAME = 'UniqDepartment')
BEGIN
ALTER TABLE HIBOPActivityOpportunity ADD UniqDepartment INT NULL
END
