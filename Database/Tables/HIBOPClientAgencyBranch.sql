IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPClientAgencyBranch')
BEGIN
CREATE TABLE [HIBOPClientAgencyBranch]
(
	[UniqEntity] INT NOT NULL,
	[UniqAgency] INT NOT NULL,
	[UniqBranch] INT NOT NULL
	
)

END