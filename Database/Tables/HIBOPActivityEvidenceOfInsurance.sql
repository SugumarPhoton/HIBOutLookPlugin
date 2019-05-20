IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityEvidenceOfInsurance')
BEGIN	

	CREATE TABLE HIBOPActivityEvidenceOfInsurance
	(
	[UniqEvidence] [int] NOT NULL,
	[UniqEntityClient] [int] NOT NULL,
	[Title] [varchar](125) NULL,
	[FormEditionDate] [char](6) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL
)
END 