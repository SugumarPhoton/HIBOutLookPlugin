/****** Object:  Table [dbo].[HIBOPEntityEmployee]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEntityEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPEntityEmployee](
	[UniqEntity] [int] NOT NULL,
	[UniqEmployee] [int] NOT NULL,
	[Lookupcode] [char](6) NOT NULL,
	[EmployeeName] [varchar](125) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IDX_HIBOPEntityEmployee_UniqEmployee]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEntityEmployee]') AND name = N'IDX_HIBOPEntityEmployee_UniqEmployee')
CREATE NONCLUSTERED INDEX [IDX_HIBOPEntityEmployee_UniqEmployee] ON [dbo].[HIBOPEntityEmployee]
(
	[UniqEmployee] ASC
)
INCLUDE ( 	[UniqEntity]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EntityEmployee_UniqEmployee]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEntityEmployee]') AND name = N'IX_EntityEmployee_UniqEmployee')
CREATE NONCLUSTERED INDEX [IX_EntityEmployee_UniqEmployee] ON [dbo].[HIBOPEntityEmployee]
(
	[UniqEmployee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_EntityEmployee_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEntityEmployee]') AND name = N'IX_EntityEmployee_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_EntityEmployee_UniqEntity] ON [dbo].[HIBOPEntityEmployee]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
