IF NOT EXISTS ( SELECT 'X' FROM SYS.table_types WHERE NAME = 'HIBOPFavourite_UT')
BEGIN
CREATE TYPE [dbo].[HIBOPFavourite_UT] AS TABLE(
	[UniqEmployee] VARCHAR(10) NOT NULL,
	[FavName]	VARCHAR(200) NOT NULL,
	[UniqEntity] INT ,
	[UniqActivity] INT ,
	PolicyYear VARCHAR(10),
	PolicyType VARCHAR(10),
	DescriptionType VARCHAR(50),
	[Description] VARCHAR(200),
	FolderId INT ,
	SubFolder1Id INT ,
	SubFolder2Id INT ,
	ClientAccessibleDate DATETIME,
	InsertedDate	DATETIME	
)
END
GO
