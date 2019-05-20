IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Opportunity_UniqEntity')   
    DROP INDEX IX_Opportunity_UniqEntity ON HIBOPActivityOpportunity;   
GO  
CREATE NONCLUSTERED INDEX IX_Opportunity_UniqEntity   
    ON HIBOPActivityOpportunity (UniqEntity);   
GO  
