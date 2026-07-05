-- Phase 3 — verify SeparateModuleSchemas migration (operator / DBA).
-- Expect tables under accounting, inventory, platform, identity schemas.

SELECT s.name AS schema_name, COUNT(*) AS table_count
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE s.name IN (N'accounting', N'inventory', N'platform', N'identity')
GROUP BY s.name
ORDER BY s.name;

SELECT s.name AS schema_name, t.name AS table_name
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE s.name IN (N'accounting', N'inventory', N'platform', N'identity')
ORDER BY s.name, t.name;
