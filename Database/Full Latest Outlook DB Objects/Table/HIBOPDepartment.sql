/****** Object:  Table [dbo].[HIBOPDepartment]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPDepartment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPDepartment](
	[UniqDepartment] [int] NULL,
	[DepartmentCode] [char](3) NULL,
	[NameOf] [varchar](100) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Flags] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
