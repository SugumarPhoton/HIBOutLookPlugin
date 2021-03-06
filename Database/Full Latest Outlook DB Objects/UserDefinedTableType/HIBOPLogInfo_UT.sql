/****** Object:  UserDefinedTableType [dbo].[HIBOPLogInfo_UT]    Script Date: 2/7/2019 6:42:13 AM ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'HIBOPLogInfo_UT' AND ss.name = N'dbo')
CREATE TYPE [dbo].[HIBOPLogInfo_UT] AS TABLE(
	[UniqId] [varchar](125) NULL,
	[UniqEmployee] [varchar](10) NULL,
	[UniqEntity] [int] NULL,
	[UniqActivity] [int] NULL,
	[PolicyYear] [varchar](10) NULL,
	[PolicyType] [varchar](50) NULL,
	[DescriptionType] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[FolderId] [int] NULL,
	[SubFolder1Id] [int] NULL,
	[SubFolder2Id] [int] NULL,
	[ClientAccessibleDate] [datetime] NULL,
	[EmailAction] [varchar](100) NULL,
	[Version] [int] NOT NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[ClientLookupCode] [varchar](10) NULL
)
GO
