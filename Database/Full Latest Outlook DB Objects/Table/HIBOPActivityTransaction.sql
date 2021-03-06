/****** Object:  Table [dbo].[HIBOPActivityTransaction]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[UniqTranshead] [int] NULL,
	[Code] [varchar](6) NULL,
	[DescriptionOf] [varchar](125) NULL,
	[UniqEntity] [int] NULL,
	[InvoiceNumber] [int] NULL,
	[ItemNumber] [int] NULL,
	[Amount] [numeric](19, 4) NULL,
	[Balance] [numeric](19, 4) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Transaction_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityTransaction]') AND name = N'IX_Transaction_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Transaction_UniqEntity] ON [dbo].[HIBOPActivityTransaction]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
