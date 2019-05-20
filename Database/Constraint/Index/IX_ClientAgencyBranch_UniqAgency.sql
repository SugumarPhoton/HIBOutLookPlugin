IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_ClientAgencyBranch_UniqAgency')   
    DROP INDEX IX_ClientAgencyBranch_UniqAgency ON HIBOPClientAgencyBranch;   
GO  
CREATE NONCLUSTERED INDEX IX_ClientAgencyBranch_UniqAgency  
    ON HIBOPClientAgencyBranch (UniqAgency);   
GO  
