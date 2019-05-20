IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_EntityEmployee_UniqEmployee')   
    DROP INDEX IX_EntityEmployee_UniqEmployee ON HIBOPEntityEmployee;   
GO  
CREATE NONCLUSTERED INDEX IX_EntityEmployee_UniqEmployee  
    ON HIBOPEntityEmployee (UniqEmployee);   
GO  
