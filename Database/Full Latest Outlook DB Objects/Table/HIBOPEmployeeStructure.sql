/****** Object:  Table [dbo].[HIBOPEmployeeStructure]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployeeStructure]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPEmployeeStructure](
	[UniqEntity] [int] NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[UniqDepartment] [int] NULL,
	[UniqProfitCenter] [int] NULL
) ON [PRIMARY]
END
GO
/****** Object:  Index [IX_HIBOPEmployeeStructure_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployeeStructure]') AND name = N'IX_HIBOPEmployeeStructure_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_HIBOPEmployeeStructure_UniqEntity] ON [dbo].[HIBOPEmployeeStructure]
(
	[UniqEntity] ASC
)
INCLUDE ( 	[UniqAgency],
	[UniqBranch]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
