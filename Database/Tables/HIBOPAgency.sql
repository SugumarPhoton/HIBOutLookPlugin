IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPAgency')
BEGIN	
	CREATE TABLE [dbo].[HIBOPAgency](
	[UniqAgency] [int] NOT NULL,
	[AgencyCode] [char](3) NULL,
	[AgencyName] [varchar](100) NULL,
	[LicenseNumber] [varchar](15) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
	) 
END
