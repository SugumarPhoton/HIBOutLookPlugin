/****** Object:  Table [dbo].[HIBOPActivityCode]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityCode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityCode](
	[UniqActivityCode] [int] NOT NULL,
	[ActivityCode] [char](4) NULL,
	[ActivityName] [varchar](125) NULL,
	[UniqActivityEvent] [int] NULL,
	[OwnerTypeCode] [char](2) NULL,
	[UniqEmployee] [int] NULL,
	[ClosedStatus] [char](1) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [HIBOPActivityCode_IDX1]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityCode]') AND name = N'HIBOPActivityCode_IDX1')
CREATE UNIQUE CLUSTERED INDEX [HIBOPActivityCode_IDX1] ON [dbo].[HIBOPActivityCode]
(
	[UniqActivityCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
