/****** Object:  Table [dbo].[HIBOPUserDeltaSyncInfo]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSyncInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPUserDeltaSyncInfo](
	[UserDeltaSyncInfo] [int] IDENTITY(1,1) NOT NULL,
	[IPAddress] [varchar](100) NULL,
	[EmployeeId] [int] NULL,
	[UserLookupCode] [varchar](10) NULL,
	[SpName] [varchar](256) NULL,
	[LastSyncDate] [datetime] NULL,
	[IsDeltaFlag] [bit] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_HIBOPUserDeltaSyncInfo_IPadd_Uselookupcode]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSyncInfo]') AND name = N'IX_HIBOPUserDeltaSyncInfo_IPadd_Uselookupcode')
CREATE NONCLUSTERED INDEX [IX_HIBOPUserDeltaSyncInfo_IPadd_Uselookupcode] ON [dbo].[HIBOPUserDeltaSyncInfo]
(
	[IPAddress] ASC,
	[EmployeeId] ASC,
	[UserLookupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_HIBOPUserDeltaSyncInfo_UserLookupCode_IsDeltaFlag_SpName]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSyncInfo]') AND name = N'IX_HIBOPUserDeltaSyncInfo_UserLookupCode_IsDeltaFlag_SpName')
CREATE NONCLUSTERED INDEX [IX_HIBOPUserDeltaSyncInfo_UserLookupCode_IsDeltaFlag_SpName] ON [dbo].[HIBOPUserDeltaSyncInfo]
(
	[UserLookupCode] ASC,
	[IsDeltaFlag] ASC,
	[SpName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
