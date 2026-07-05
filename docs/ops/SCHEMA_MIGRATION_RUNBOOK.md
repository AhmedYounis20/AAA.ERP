# Schema migration runbook — staging / production

Apply the `SeparateModuleSchemas` migration (commit `f256151`) on non-dev environments.

## Prerequisites

- SQL Server backup of target database
- Connection string with DDL permissions
- Maintenance window (migration renames schemas; brief lock on metadata)

## Steps

1. **Backup**
   ```bash
   # Example — adjust server/database
   sqlcmd -S <server> -Q "BACKUP DATABASE [ERP] TO DISK = N'/backup/ERP_pre_schema.bak' WITH INIT"
   ```

2. **Deploy application build** that includes migration `SeparateModuleSchemas` (ERP.Host assembly).

3. **Apply migration**
   ```bash
   dotnet ef database update \
     --project ERP.Host/ERP.Host.csproj \
     --startup-project ERP.Host/ERP.Host.csproj \
     --connection "<staging-or-prod-connection-string>"
   ```

4. **Verify schemas**
   ```sql
   SELECT s.name AS schema_name, t.name AS table_name
   FROM sys.tables t
   JOIN sys.schemas s ON t.schema_id = s.schema_id
   WHERE s.name IN ('accounting','inventory','platform','identity')
   ORDER BY s.name, t.name;
   ```

5. **Update reporting / ad-hoc SQL** to schema-qualified names:
   - `accounting.Entries`, `accounting.ChartOfAccounts`, …
   - `inventory.Products`, `inventory.StockBalances`, …
   - See [SQL_SCHEMA_TABLE_INVENTORY.md](../architecture/SQL_SCHEMA_TABLE_INVENTORY.md)

6. **Smoke test** — run integration/smoke suite against the environment or hit critical API paths (login, journal entry, stock balance read).

## Rollback

- Restore from backup taken in step 1 if migration fails mid-flight.
- Do **not** manually drop schemas without DBA review.

## Related

- [ADR-006](../architecture/adr/006-sql-schema-separation.md)
- [MODULAR_MONOLITH.md §4.2.2](../architecture/MODULAR_MONOLITH.md)
