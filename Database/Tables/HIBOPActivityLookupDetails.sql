IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPActivityLookupDetails')
BEGIN
	CREATE TABLE [dbo].[HIBOPActivityLookupDetails]
	(
		[ALDId]	INT IDENTITY(1,1),
		[UniqLine] [int] NOT NULL,
		[UniqPolicy] [int] NOT NULL,
		[UniqEntity] [int] NOT NULL,
		[UniqClaim] [int] NULL,
		[LineCode] [char](4) NOT NULL,
		[PolicyType] [char](4) NOT NULL,
		[Linedescription] [nvarchar](max) NOT NULL,
		[PolicyNumber] [varchar](25) NOT NULL,
		[PolicyDesc] [varchar](125) NOT NULL,
		[UniqCdPolicyLineType] [int] NOT NULL,
		[UniqCdLineStatus] [int] NOT NULL,
		[LineExpDate] [datetime] NOT NULL,
		[LineEffDate] [datetime] NOT NULL,
		[PolicyExpDate] [datetime] NOT NULL,
		[PolicyEffDate] [datetime] NOT NULL,
		[ClaimNumber] [int] NULL,
		[CompanyClaimNumber] [varchar](25) NULL,
		[DateLoss] [datetime] NULL,
		[ClosedDate] [datetime] NULL,
		[LookupCode] [char](10) NOT NULL,
		[AccountName] [varchar](100) NOT NULL,
		[InsertedDate] [datetime] NOT NULL,
		[UpdatedDate] [datetime]  NULL,
		UniqClaimAssociation INT,
		IOC VARCHAR(125),
		IOCCode VARCHAR(20),
		AttachmentDesc	VARCHAR(125)
	) 
END
