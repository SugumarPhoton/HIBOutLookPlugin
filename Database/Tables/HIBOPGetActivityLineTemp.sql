IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPGetActivityLineTemp')
BEGIN
CREATE TABLE [dbo].[HIBOPGetActivityLineTemp](
	[UploadGuid] [varchar](128) NULL,
	[UniqLine] [int] NULL,
	[UniqPolicy] [int] NULL,
	[UniqEntity] [int] NULL,
	[PolicyType] [varchar](4) NULL,
	[PolicyDesc] [varchar](125) NULL,
	[LineCode] [varchar](4) NULL,
	[LineOfBusiness] [varchar](500) NULL,
	[LineStatus] [varchar](500) NULL,
	[PolicyNumber] [varchar](25) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[UniqCdLineStatus] [int] NULL,
	[IOC] [varchar](6) NULL,
	[BillModeCode] [varchar](1) NULL,
	[ExpirationDate] [datetime] NULL,
	[EffectiveDate] [datetime] NULL,
	[Status] [varchar](20) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) 
END