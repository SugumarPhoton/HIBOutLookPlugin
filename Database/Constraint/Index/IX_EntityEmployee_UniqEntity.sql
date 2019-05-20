IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_EntityEmployee_UniqEntity')   
    DROP INDEX IX_EntityEmployee_UniqEntity ON HIBOPEntityEmployee;   
GO  
CREATE NONCLUSTERED INDEX IX_EntityEmployee_UniqEntity   
    ON HIBOPEntityEmployee (UniqEntity);   
GO  
