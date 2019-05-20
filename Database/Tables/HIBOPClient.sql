IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPClient')
BEGIN	
	CREATE TABLE HIBOPClient
	(
	[OPCId]					INT IDENTITY(1,1) NOT NULL, 
	[UniqEntity]			INT NOT NULL,
	[LookupCode]			VARCHAR(10) NOT NULL,
	[NameOf]				VARCHAR(100) NOT NULL,
	[PrimaryContactName]	VARCHAR(100) NOT NULL,
	[Address]				VARCHAR(500) ,	
	[City]					VARCHAR(50) ,
	StateCode				VARCHAR(4),
	StateName				VARCHAR(50),
	[PostalCode]			VARCHAR(10),
	[CountryCode]			VARCHAR(4),
	[Country]				VARCHAR(125),
	[Status]				INT,
	[InsertedDate]			DATETIME NOT NULL,
	[UpdatedDate]			DATETIME,
	[InactiveDate]			DATETIME
	)
END
