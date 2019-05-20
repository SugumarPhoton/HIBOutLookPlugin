IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPOutlookPluginLog')
BEGIN
CREATE TABLE [dbo].[HIBOPOutlookPluginLog](
[LogId]							[Int] IDENTITY(100001,1), 
[UniqId]						[VARCHAR] (125)  NULL, 
[UniqEmployee]					INT NOT NULL,
[UniqEntity]      				INT NOT NULL,
[UniqActivity]					INT NOT NULL,
[PolicyYear]					[VARCHAR] (10),
[PolicyType]					[VARCHAR] (50),
[DescriptionType]				[VARCHAR] (50),
[Description]					[VARCHAR] (500),
[FolderId]						INT,
[SubFolder1Id]					INT,
[SubFolder2Id]					INT,
[ClientAccessibleDate]			[DATETIME] ,
[EmailAction]					VARCHAR(100),
[Version]						INT NOT NULL,
[InsertedDate]					[DATETIME] NOT NULL,
[UpdatedDate]					[DATETIME] ,
[ClientLookupCode]				VARCHAR(10)
)
END