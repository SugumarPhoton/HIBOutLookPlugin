IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_ClientAgencyBranch_UniqEntity')   
    DROP INDEX IX_ClientAgencyBranch_UniqEntity ON HIBOPClientAgencyBranch;   
GO  
CREATE NONCLUSTERED INDEX IX_ClientAgencyBranch_UniqEntity  
    ON HIBOPClientAgencyBranch (UniqEntity);   
GO  
