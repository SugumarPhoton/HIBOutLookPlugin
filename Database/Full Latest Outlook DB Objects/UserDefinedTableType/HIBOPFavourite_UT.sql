/****** Object:  UserDefinedTableType [dbo].[HIBOPFavourite_UT]    Script Date: 2/7/2019 6:42:13 AM ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'HIBOPFavourite_UT' AND ss.name = N'dbo')
CREATE TYPE [dbo].[HIBOPFavourite_UT] AS TABLE(
	[UniqEmployee] [varchar](10) NOT NULL,
	[FavName] [varchar](200) NOT NULL,
	[UniqEntity] [int] NULL,
	[UniqActivity] [int] NULL,
	[PolicyYear] [varchar](10) NULL,
	[PolicyType] [varchar](10) NULL,
	[DescriptionType] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[FolderId] [int] NULL,
	[SubFolder1Id] [int] NULL,
	[SubFolder2Id] [int] NULL,
	[ClientAccessibleDate] [datetime] NULL,
	[InsertedDate] [datetime] NULL,
	[IPAddress] [varchar](512) NULL,
	[UserLookupCode] [varchar](512) NULL
)
GO
