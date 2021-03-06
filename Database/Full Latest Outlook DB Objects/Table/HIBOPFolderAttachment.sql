/****** Object:  Table [dbo].[HIBOPFolderAttachment]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPFolderAttachment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPFolderAttachment](
	[FolderId] [int] NOT NULL,
	[ParentFolderId] [int] NOT NULL,
	[FolderType] [varchar](20) NOT NULL,
	[FolderName] [varchar](100) NOT NULL,
	[Status] [varchar](10) NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [HIBOPFolderAttachment_PK] PRIMARY KEY CLUSTERED 
(
	[FolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
