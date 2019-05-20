IF NOT EXISTS ( SELECT 'X' FROM SYS.table_types WHERE NAME = 'HIBOPLogInfo_UT')
BEGIN
CREATE TYPE [dbo].[HIBOPLogInfo_UT] AS TABLE(
	[UniqId]				VARCHAR(125)  NULL,
	[UniqEmployee]			VARCHAR(10),
	[UniqEntity]			INT ,
	[UniqActivity]			INT ,
	PolicyYear				VARCHAR(10),
	PolicyType				VARCHAR(50),
	DescriptionType			VARCHAR(50),
	[Description]			VARCHAR(500),
	FolderId				INT ,
	SubFolder1Id			INT ,
	SubFolder2Id			INT ,
	ClientAccessibleDate	DATETIME,
	EmailAction				VARCHAR(100),
	[Version]				INT NOT NULL,
	InsertedDate			DATETIME,
	UpdatedDate				DATETIME,
	ClientLookupCode		VARCHAR(10)
)
END
GO
