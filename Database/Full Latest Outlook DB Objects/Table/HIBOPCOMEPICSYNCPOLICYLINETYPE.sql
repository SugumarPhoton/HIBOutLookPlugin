/****** Object:  Table [dbo].[HIBOPCOMEPICSYNCPOLICYLINETYPE]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPCOMEPICSYNCPOLICYLINETYPE]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPCOMEPICSYNCPOLICYLINETYPE](
	[EPICLASTSYNCDATETIME] [datetime] NULL
) ON [PRIMARY]
END
GO
