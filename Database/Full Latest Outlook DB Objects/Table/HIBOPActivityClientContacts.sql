/****** Object:  Table [dbo].[HIBOPActivityClientContacts]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityClientContacts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityClientContacts](
	[ClientContactId] [int] IDENTITY(1,1) NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqContactName] [int] NOT NULL,
	[ContactName] [varchar](125) NULL,
	[ContactType] [varchar](10) NULL,
	[ContactValue] [varchar](200) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqContactNumber] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
