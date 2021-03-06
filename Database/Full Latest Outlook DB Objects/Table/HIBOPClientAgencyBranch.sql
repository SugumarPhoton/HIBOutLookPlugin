/****** Object:  Table [dbo].[HIBOPClientAgencyBranch]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClientAgencyBranch]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPClientAgencyBranch](
	[UniqEntity] [int] NOT NULL,
	[UniqAgency] [int] NOT NULL,
	[UniqBranch] [int] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Index [HIBOPClientAgencyBranch_NDX1]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClientAgencyBranch]') AND name = N'HIBOPClientAgencyBranch_NDX1')
CREATE NONCLUSTERED INDEX [HIBOPClientAgencyBranch_NDX1] ON [dbo].[HIBOPClientAgencyBranch]
(
	[UniqAgency] ASC,
	[UniqBranch] ASC
)
INCLUDE ( 	[UniqEntity]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientAgencyBranch_UniqAgency]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClientAgencyBranch]') AND name = N'IX_ClientAgencyBranch_UniqAgency')
CREATE NONCLUSTERED INDEX [IX_ClientAgencyBranch_UniqAgency] ON [dbo].[HIBOPClientAgencyBranch]
(
	[UniqAgency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientAgencyBranch_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClientAgencyBranch]') AND name = N'IX_ClientAgencyBranch_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_ClientAgencyBranch_UniqEntity] ON [dbo].[HIBOPClientAgencyBranch]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
