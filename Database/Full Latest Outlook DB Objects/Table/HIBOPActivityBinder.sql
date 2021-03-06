/****** Object:  Table [dbo].[HIBOPActivityBinder]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityBinder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityBinder](
	[UniqBinder] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqLine] [int] NOT NULL,
	[BinderNumber] [int] NOT NULL,
	[DescriptionOf] [varchar](125) NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NULL,
	[IssuedDate] [datetime] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
