IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPActvityOwnerList')
BEGIN
	CREATE TABLE HIBOPActvityOwnerList
	(
		[UniqEntity] [int] NOT NULL,
		[LookupCode] [char](6) NULL,
		[OwnerName] [varchar](125) NULL,
		[InactiveDate] [datetime] NULL,
		[Flags] [int] NULL,
		[InsertedDate] [datetime] NULL,
		[UpdatedDate] [datetime] NULL
	)
END

