/****** Object:  Table [dbo].[HIBOPEmployeeAgency]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployeeAgency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPEmployeeAgency](
	[UniqEntity] [int] NOT NULL,
	[UniqAgency] [int] NOT NULL,
	[UniqBranch] [int] NOT NULL,
	[UniqDepartment] [int] NOT NULL,
	[UniqProfitCenter] [int] NOT NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
/****** Object:  Index [HIBOPEmployeeAgency_NDX1]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPEmployeeAgency]') AND name = N'HIBOPEmployeeAgency_NDX1')
CREATE NONCLUSTERED INDEX [HIBOPEmployeeAgency_NDX1] ON [dbo].[HIBOPEmployeeAgency]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
