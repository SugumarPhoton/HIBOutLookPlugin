IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPFavourite')
BEGIN
CREATE TABLE [dbo].[HIBOPFavourite]
(
[FavId]							[INT] IDENTITY(1,1)	NOT NULL, 
[FavouriteName]					VARCHAR(500) NOT NULL,
[UniqEmployee]					INT NOT NULL,
[UniqEntity]      				INT ,
[UniqActivity]					INT ,
[PolicyYear]					[VARCHAR] (10),
[PolicyType]					[VARCHAR] (10),
[DescriptionType]				[VARCHAR] (50),
[Description]					[VARCHAR] (500),
[FolderId]						INT,
[SubFolder1Id]					INT,
[SubFolder2Id]					INT,
[Status]						INT,
[ClientAccessibleDate]			[DATETIME]	,
[InsertedDate]					[DATETIME] NOT NULL
)
END