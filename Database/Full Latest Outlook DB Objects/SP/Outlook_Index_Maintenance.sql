/****** Object:  StoredProcedure [dbo].[Outlook_Index_Maintenance]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Outlook_Index_Maintenance]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Outlook_Index_Maintenance] AS' 
END
GO
ALTER Procedure [dbo].[Outlook_Index_Maintenance]
As 
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON 
CREATE TABLE #FragmentedIndexes
(
 DatabaseName SYSNAME NULL
 , SchemaName SYSNAME NULL
 , TableName SYSNAME  NULL
 , IndexName SYSNAME  NULL
 , [Fragmentation%] FLOAT NULL
 ,Index_Type_Desc nvarchar(120) NULL
)

INSERT INTO #FragmentedIndexes
SELECT
 DB_NAME(DB_ID()) AS DatabaseName
 , ss.name AS SchemaName
 , OBJECT_NAME (s.object_id) AS TableName
 , i.name AS IndexName
 , s.avg_fragmentation_in_percent AS [Fragmentation%]
 , s.index_type_desc as [Index_Type_Desc]
FROM sys.dm_db_index_physical_stats(db_id(),NULL, NULL, NULL, 'SAMPLED') s
INNER JOIN sys.indexes i ON s.[object_id] = i.[object_id]
AND s.index_id = i.index_id
INNER JOIN sys.objects o ON s.object_id = o.object_id
INNER JOIN sys.schemas ss ON ss.[schema_id] = o.[schema_id]
WHERE s.database_id = DB_ID()
--AND i.index_id != 0
--AND s.record_count > 0
--AND o.is_ms_shipped = 0

/*
select Index_Type_Desc,count(0)
from #FragmentedIndexes
group by Index_Type_Desc
with rollup 
*/

DECLARE @RebuildIndexesSQL NVARCHAR(MAX)
SET @RebuildIndexesSQL = ''

SELECT
 @RebuildIndexesSQL = isnull(@RebuildIndexesSQL,'') + Alter_Scripts
from
(
SELECT
CASE
 WHEN ([Fragmentation%] > 30 and Index_Type_Desc NOT like '%HEAP%')
   THEN CHAR(10) + 'ALTER INDEX ' + QUOTENAME(IndexName) + ' ON '
      + QUOTENAME(SchemaName) + '.'
      + QUOTENAME(TableName) + ' REBUILD;'
 WHEN (([Fragmentation%] > 5 and [Fragmentation%] < 30) and Index_Type_Desc NOT like '%HEAP%')
    THEN CHAR(10) + 'ALTER INDEX ' + QUOTENAME(IndexName) + ' ON '
    + QUOTENAME(SchemaName) + '.'
    + QUOTENAME(TableName) + ' REORGANIZE;'
 WHEN ([Fragmentation%] > 5 and Index_Type_Desc like '%HEAP%')
	 THEN CHAR(10) + 'ALTER TABLE ' +  QUOTENAME(SchemaName) + '.' + QUOTENAME(TableName)  + 'REBUILD;'
END Alter_Scripts,*
FROM #FragmentedIndexes
)a
where Alter_Scripts is not null 


--select @RebuildIndexesSQL,LEN(@RebuildIndexesSQL)

DECLARE @StartOffset INT
DECLARE @Length INT
SET @StartOffset = 0
SET @Length = 4000

WHILE (@StartOffset < LEN(@RebuildIndexesSQL))
BEGIN
 PRINT SUBSTRING(@RebuildIndexesSQL, @StartOffset, @Length)
 SET @StartOffset = @StartOffset + @Length
END
PRINT SUBSTRING(@RebuildIndexesSQL, @StartOffset, @Length)
EXECUTE sp_executesql @RebuildIndexesSQL
DROP TABLE #FragmentedIndexes

SET NOCOUNT OFF
END


GO
