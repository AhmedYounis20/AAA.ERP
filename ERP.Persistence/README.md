# ERP.Persistence — persistence kernel

Shared data access layer for the modular monolith: **EF Core `ApplicationDbContext`**, base repositories/services, slim `IUnitOfWork`, and **`ITransactionCoordinator`**. Migrations remain in **`ERP.Host/Migrations/`**.

> **Namespace note:** Project folder is `ERP.Persistence`; CLR namespaces remain `ERP.Infrastructure.*` (incremental alignment). New kernel code should prefer `ERP.Persistence` when substantially touching files.

## Purpose

- Single SQL Server database, single `ApplicationDbContext`, module EF configs via `ModuleModelConfigurationRegistry`.
- Generic base repository/service patterns used by modules during the strangler migration.
- Cross-module **ACID coordination** through `TransactionCoordinator`.
- Dev seed import/export utilities (`DbWorkController`).

**Module-specific horizontal code** (sub-ledgers, attachments, lookup queries, financial-period usage) lives in **`Modules/Accounting/Accounting.Infrastructure`** as of Phase 7.

## Project map (kernel only)

```
ERP.Persistence/
├── DBConfiguration/
│   ├── DbContext/ApplicationDbContext.cs
│   └── Config/BaseConfig/              ← shared EF base configs
│   └── Config/Identity/                ← identity schema
├── Repositories/
│   ├── BaseRepositories/               ← BaseRepository, tree/setting variants
│   ├── UnitOfWork.cs
│   └── TransactionCoordinator.cs
├── Services/BaseServices/                ← BaseService, BaseTreeService, …
└── Utilities/
    ├── InfrastructureExtensions.cs
    └── ImportDataToSeed.cs / ExportDataToSeed.cs
```

## Architecture rules

| Rule | Detail |
|------|--------|
| No API reference | Persistence must not reference `ERP.Host`. |
| No module-specific services | Accounting/Infrastructure/Inventory horizontal code belongs in module Infrastructure projects. |
| Module configs | Modules register via `ModuleModelConfigurationRegistry`. |
| Migrations | `dotnet ef ... --project ERP.Host --startup-project ERP.Host`. |
| Coordinator scope | `ITransactionCoordinator` is **scoped** per HTTP request. |

## Related

- [FINAL_SIGNOFF.md](../docs/architecture/FINAL_SIGNOFF.md)
- [MODULE_TEMPLATE.md](../docs/architecture/MODULE_TEMPLATE.md)
