IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Services_UniqEntity')   
    DROP INDEX IX_Services_UniqEntity ON HIBOPActivityServices;   
GO  
CREATE NONCLUSTERED INDEX IX_Services_UniqEntity  
    ON HIBOPActivityServices (UniqEntity);   
GO  
