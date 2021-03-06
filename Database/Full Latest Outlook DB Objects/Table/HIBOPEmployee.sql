/****** Object:  Table [dbo].[HIBOPEmployee]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPEmployee](
	[UniqEntity] [int] NOT NULL,
	[LookupCode] [char](6) NULL,
	[EmployeeName] [varchar](125) NULL,
	[Department] [varchar](60) NULL,
	[JobTitle] [varchar](60) NULL,
	[InactiveDate] [datetime] NULL,
	[RoleFlags] [int] NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [Emp_UniqEntity_PK] PRIMARY KEY CLUSTERED 
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [CI_HIBOPEmployee_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployee]') AND name = N'CI_HIBOPEmployee_UniqEntity')
CREATE NONCLUSTERED INDEX [CI_HIBOPEmployee_UniqEntity] ON [dbo].[HIBOPEmployee]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_HIBOPEmployee_LookupCode]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployee]') AND name = N'IX_HIBOPEmployee_LookupCode')
CREATE NONCLUSTERED INDEX [IX_HIBOPEmployee_LookupCode] ON [dbo].[HIBOPEmployee]
(
	[LookupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
