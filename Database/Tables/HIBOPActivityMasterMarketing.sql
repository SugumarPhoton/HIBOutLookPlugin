IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPActivityMasterMarketing')
BEGIN
CREATE TABLE HIBOPActivityMasterMarketing
(
	[UniqMarketingSubmission] [int] NOT NULL,
	[UniqEntity] [int] NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[DescriptionOf] [varchar](125) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[LastSubmittedDate] [datetime] NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
)
END

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HIBOPActivityMasterMarketing' AND COLUMN_NAME = 'UniqDepartment')
BEGIN
	ALTER TABLE HIBOPActivityMasterMarketing ADD UniqDepartment Int  NULL ;
END	
GO
