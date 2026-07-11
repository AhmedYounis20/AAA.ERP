# Accounting module

General ledger, chart of accounts, sub-ledgers, financial periods, taxes, and journal entries. This module is the **system of record for posted GL entries** and exposes cross-module integration via `Accounting.Contracts`.

## Purpose

- Own accounting aggregates: currencies, COA, cost centers, sub-ledgers (banks, customers, suppliers, …), entries (journal, payment, receipt, opening, combined), taxes, and GL settings.
- Post inventory-driven GL entries through `IAccountingIntegrationService` with idempotent `SourceDocumentId` / `SourceDocumentType` (Phase 4.2.3).
- Register EF configurations and DI via `AddAccountingModule()`; host owns HTTP controllers under `ERP.Host/Controllers/Account/`.

## Project map

| Project | Role |
|---------|------|
| `Accounting.Domain` | Module entities (`Tax`, `Entry`, …). Some aggregates still reference types in `ERP.Domain` during transition. |
| `Accounting.Application` | Commands, validators, repository/service interfaces. No EF or Infrastructure references. |
| `Accounting.Contracts` | **Public integration surface** for other modules — DTOs + `IAccountingIntegrationService`. No domain entities. |
| `Accounting.Infrastructure` | Handlers, repositories, services, `*DbConfig`, `AccountingIntegrationService`, `AccountingModuleExtensions`. |

```
Modules/Accounting/
├── Accounting.Domain/Entities/
├── Accounting.Application/
│   ├── Commands/{Aggregate}/
│   ├── Validators/{Aggregate}/
│   ├── Repositories/
│   └── Services/
├── Accounting.Contracts/Integration/     ← cross-module API
└── Accounting.Infrastructure/
    ├── Handlers/{Aggregate}/
    ├── Repositories/
    ├── Services/
    ├── Persistence/*DbConfig.cs
    └── AccountingModuleExtensions.cs
```

**Host wiring:** `ERP.Host/Utilities/WebBuilderExtensions.cs` calls `AddAccountingModule()` after `AddInfrastructureServices()`.

**Swagger group:** `accounting` (see `ERP.Host/Utilities/SwaggerModuleGroups.cs`).

## How to add a feature

Follow [MODULE_TEMPLATE.md](../../docs/architecture/MODULE_TEMPLATE.md). Minimal slice for a new aggregate (e.g. Tax pattern):

1. **Entity** — `Accounting.Domain/Entities/{Entity}.cs` (or transitional base in `ERP.Domain`).
2. **Application** — `*CreateCommand` / `*UpdateCommand`, validators, `I*Repository`, `I*Service` in `Accounting.Application`.
3. **Infrastructure** — `*CommandHandler`, `*Repository`, `*Service`, `{Entity}DbConfig` in `Accounting.Infrastructure`.
4. **Composition** — Register repo/service in `AccountingModuleExtensions.AddAccountingModule()`; register `*DbConfig` in `ApplyAccountingModuleConfigurations`.
5. **Host** — Add controller in `ERP.Host/Controllers/Account/`; assign Swagger module group.
6. **MediatR** — Handler assembly is already registered in `WebBuilderExtensions.AddProjectUtilities()`; add validator assembly if you introduce a new Application project (not needed for aggregates in this module).
7. **Migration** — `dotnet ef migrations add <Name> --project ERP.Host --startup-project ERP.Host`; confirm no unintended table drops.

**Writes:** Controller → `ISender.Send(command)` → Handler → Service (ADR-004). Do not add `AddMediatR` inside the module.

## Architecture rules

Enforced by `Tests/ERP.Architecture.Tests/ModuleBoundaryTests.cs`:

| Rule | Detail |
|------|--------|
| Domain purity | `Accounting.Domain` must not reference Infrastructure, EF, or Inventory entities. |
| Application isolation | `Accounting.Application` must not reference `Accounting.Infrastructure` or `ERP.Persistence`. |
| No API reference | `Accounting.Infrastructure` must not reference `ERP.Host`. |
| No Inventory in Accounting | Accounting Infrastructure must not reference `ERP.Domain.Models.Entities.Inventory`. |
| Contracts stay thin | `Accounting.Contracts` references only shared response types — safe for Inventory to reference. |
| God `IUnitOfWork` | Do not add new repository properties to `IUnitOfWork`; inject dedicated repositories. |

## Key integration points

### `IAccountingIntegrationService` (`Accounting.Contracts`)

Inventory (and future modules) call this interface — never Accounting domain types directly.

| Method | Use |
|--------|-----|
| `GetOpenPeriodForDateAsync` | Resolve open financial period for posting date |
| `GetBranchAccountAsync` | Branch GL account for inventory posting |
| `ValidatePartyAccountAsync` | Validate party COA before transaction |
| `PostInventoryTransactionEntryAsync` | Receipt/issue GL entry (idempotent via `SourceDocumentId`) |
| `PostInventoryTransferEntryAsync` | Transfer stage/complete/reject GL (see `EntrySourceDocumentTypes`) |

Implementation: `Accounting.Infrastructure/Services/AccountingIntegrationService.cs`.

**Source document types** (`Accounting.Contracts/Integration/EntrySourceDocumentTypes.cs`):

- `InventoryTransaction`
- `InventoryTransferStage` / `InventoryTransferComplete` / `InventoryTransferReject`

### `ITransactionCoordinator`

Defined in `BuildingBlocks/ERP.BuildingBlocks.Persistence` (namespace `ERP.Application.Repositories`). Implemented in `ERP.Persistence/Repositories/TransactionCoordinator.cs`. Accounting services that participate in cross-module ACID flows should accept the coordinator from DI when coordinating with Inventory; inventory posting itself is invoked *inside* the coordinator callback via `IAccountingIntegrationService`.

## Tests

```bash
# Full solution
dotnet build AAA.ERP.sln -c Release
dotnet test AAA.ERP.sln -c Release

# Module boundary rules
dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj -c Release

# Accounting-related integration (inventory posting, transfers)
dotnet test Tests/ERP.IntegrationTests/ERP.IntegrationTests.csproj -c Release --filter "FullyQualifiedName~Inventory"

# EF / migrations
dotnet ef dbcontext info --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
```

## Related docs

- [MODULAR_MONOLITH.md §10.6](../../docs/architecture/MODULAR_MONOLITH.md#106-phase-4--hardening) — Phase 4 / M4 exit criteria
- [MODULE_TEMPLATE.md](../../docs/architecture/MODULE_TEMPLATE.md)
- [MODULE_OWNERS.md](../../docs/architecture/MODULE_OWNERS.md)
- [ADR-006 — SQL schema separation (deferred)](../../docs/architecture/adr/006-sql-schema-separation.md)
