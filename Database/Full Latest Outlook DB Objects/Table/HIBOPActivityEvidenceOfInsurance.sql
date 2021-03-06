/****** Object:  Table [dbo].[HIBOPActivityEvidenceOfInsurance]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityEvidenceOfInsurance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityEvidenceOfInsurance](
	[UniqEvidence] [int] NOT NULL,
	[UniqEntityClient] [int] NOT NULL,
	[Title] [varchar](125) NULL,
	[FormEditionDate] [char](6) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Evidence_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityEvidenceOfInsurance]') AND name = N'IX_Evidence_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Evidence_UniqEntity] ON [dbo].[HIBOPActivityEvidenceOfInsurance]
(
	[UniqEntityClient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
