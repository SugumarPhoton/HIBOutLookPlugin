IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityBinder')
BEGIN	

	CREATE TABLE HIBOPActivityBinder
	(
		[UniqBinder] [int] NOT NULL,
		[UniqEntity] [int] NOT NULL,
		[UniqLine] [int] NOT NULL,
		[BinderNumber] [int] NOT NULL,
		[DescriptionOf] [varchar](125) NOT NULL,
		[EffectiveDate] [datetime] NOT NULL,
		[ExpirationDate] [datetime] NULL,
		[IssuedDate] [datetime] NULL,
		[InsertedDate] [datetime] NOT NULL,
		[UpdatedDate] [datetime] NULL
	)
END 