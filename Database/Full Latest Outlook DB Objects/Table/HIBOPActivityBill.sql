/****** Object:  Table [dbo].[HIBOPActivityBill]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityBill]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityBill](
	[BillId] [int] IDENTITY(1,1) NOT NULL,
	[UniqTranshead] [int] NULL,
	[DescriptionOf] [varchar](125) NULL,
	[UniqEntity] [int] NULL,
	[BillNumber] [int] NULL,
	[UniqAgency] [int] NULL,
	[AgencyName] [varchar](125) NULL,
	[Amount] [numeric](19, 4) NULL,
	[Balance] [numeric](19, 4) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Bill_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityBill]') AND name = N'IX_Bill_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Bill_UniqEntity] ON [dbo].[HIBOPActivityBill]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
