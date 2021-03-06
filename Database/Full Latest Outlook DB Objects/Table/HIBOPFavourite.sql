/****** Object:  Table [dbo].[HIBOPFavourite]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPFavourite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPFavourite](
	[FavId] [int] IDENTITY(1,1) NOT NULL,
	[FavouriteName] [varchar](500) NOT NULL,
	[UniqEmployee] [int] NOT NULL,
	[UniqEntity] [int] NULL,
	[UniqActivity] [int] NULL,
	[PolicyYear] [varchar](10) NULL,
	[PolicyType] [varchar](10) NULL,
	[DescriptionType] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[FolderId] [int] NULL,
	[SubFolder1Id] [int] NULL,
	[SubFolder2Id] [int] NULL,
	[Status] [int] NULL,
	[ClientAccessibleDate] [datetime] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[IPAddress] [varchar](512) NULL,
	[UserLookupCode] [varchar](512) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
