IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityCertificate')
BEGIN	

	CREATE TABLE HIBOPActivityCertificate
	(
	[UniqCertificate] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[Title] [varchar](125) NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL
	) 

END 