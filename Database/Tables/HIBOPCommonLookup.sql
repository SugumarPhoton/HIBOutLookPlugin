IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPCommonLookup')
BEGIN	
	CREATE TABLE HIBOPCommonLookup
	(
		CommonLkpId				INT				NOT NULL,
		CommonLkpTypeCode		VARCHAR(50)		NOT NULL,
		CommonLkpCode			VARCHAR(10)		NOT NULL,
		CommonLkpName			VARCHAR(500)	NOT NULL,
		CommonLkpDescription	VARCHAR(1000)	NULL,		
		SortOrder				INT				NULL,
		IsDeleted				Bit				NOT NULL,		
		CreatedBy				VARCHAR(50)		NOT NULL,
		CreatedDate				DATETIME		NOT NULL,
		ModifiedBy				VARCHAR(50)		NOT NULL,
		ModifiedDate			DATETIME		NOT NULL
	)
END
GO
