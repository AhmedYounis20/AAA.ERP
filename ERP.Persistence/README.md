# ERP.Persistence — persistence kernel

Shared data access layer for the modular monolith: **EF Core `ApplicationDbContext`**, base repositories, the god `IUnitOfWork`, horizontal (unmigrated) handlers/repos/services, and **`ITransactionCoordinator`**. Migrations remain in **`ERP.API/Migrations/`** until a dedicated migrations project is introduced.

> **Namespace note (Phase 4.1.3):** The project folder is `ERP.Persistence` but most types still use the `ERP.Infrastructure.*` namespace and `RootNamespace` migration is incremental. New code in this project should prefer `ERP.Persistence` namespaces when touching files substantially.

## Purpose

- Single SQL Server database, single `ApplicationDbContext`, module EF configs applied via `ModuleModelConfigurationRegistry`.
- Base repository patterns (`BaseRepository<T>`, tree/setting variants) used by modules during the strangler migration.
- Cross-module **ACID coordination** through `TransactionCoordinator` implementing `ITransactionCoordinator`.
- Residual horizontal slices not yet moved to `Modules/*` (attachments, sub-ledger base services, seed import/export).

## Project map

```
ERP.Persistence/
├── DBConfiguration/
│   ├── DbContext/ApplicationDbContext.cs    ← Identity + module registry
│   └── Config/                            ← legacy horizontal DbConfigs
├── Repositories/
│   ├── BaseRepositories/                  ← BaseRepository, UnitOfWork
│   ├── Account/                           ← Attachment, FinancialPeriodUsageQueries
│   └── TransactionCoordinator.cs          ← ITransactionCoordinator impl
├── Services/
│   ├── BaseServices/                      ← BaseService, BaseTreeService, …
│   └── Account/                           ← SubLeadgerService, queries
└── Utilities/
    ├── InfrastructureExtensions.cs          ← AddInfrastructureServices()
    ├── DbConfigExtensions.cs
    └── ImportDataToSeed.cs / ExportDataToSeed.cs
```

**Related building blocks:**

| Project | Role |
|---------|------|
| `BuildingBlocks/ERP.BuildingBlocks.Persistence` | `IApplicationDbContext`, `ModuleModelConfigurationRegistry`, `ITransactionCoordinator` interface |
| `BuildingBlocks/ERP.BuildingBlocks.Application` | `ValidationBehavior`, `LoggingBehavior`, `ApiResponse` |
| `BuildingBlocks/ERP.BuildingBlocks.Domain` | `BaseEntity`, tree bases, `Attachment` |

**Host references:** `ERP.API` → `ERP.Persistence` + all `*.Infrastructure` module projects.

## How to add a feature

**Prefer module extraction** ([MODULE_TEMPLATE.md](../docs/architecture/MODULE_TEMPLATE.md)) for new aggregates. Only add to `ERP.Persistence` when:

- Extending **shared** infrastructure (base repo, coordinator, context behavior), or
- Maintaining **unmigrated** horizontal code until its module slice lands.

For shared persistence changes:

1. Interface in `ERP.Application` or `BuildingBlocks` as appropriate.
2. Implementation in `ERP.Persistence/Repositories` or `Services`.
3. Register in `InfrastructureExtensions.AddRepositories()` / `AddServices()`.
4. If EF config: legacy configs under `DBConfiguration/Config/`; module configs belong in `Modules/*/Infrastructure/Persistence/`.

**Do not** add new properties to `IUnitOfWork` — inject dedicated repositories (Phase 0 freeze).

## Architecture rules

| Rule | Detail |
|------|--------|
| No API reference | Persistence must not reference `ERP.API`. |
| Module configs | Modules register via `ModuleModelConfigurationRegistry`; context applies them in `OnModelCreating`. |
| Migrations | Owned by host: `dotnet ef ... --project ERP.API --startup-project ERP.API`. |
| Design-time factory | `ERP.API/Persistence/ApplicationDbContextFactory.cs` bootstraps module configs without full DI. |
| Coordinator scope | `ITransactionCoordinator` is **scoped**; one coordinator per HTTP request. |

## Key integration points

### `ITransactionCoordinator`

```csharp
// BuildingBlocks — ERP.Application.Repositories.ITransactionCoordinator
Task<ApiResponse<T>> ExecuteAsync<T>(
    Func<CancellationToken, Task<ApiResponse<T>>> action,
    CancellationToken cancellationToken = default);
```

- **Implementation:** `Repositories/TransactionCoordinator.cs` — wraps `IUnitOfWork.BeginTransactionAsync` / `CommitAsync` / `RollbackAsync`.
- **Registration:** `InfrastructureExtensions.AddRepositories()` → `services.AddScoped<ITransactionCoordinator, TransactionCoordinator>()`.
- **Consumers:** `InventoryTransactionService.Create` (and `InventoryTransferService` after 4.2.x migration). Callback should call `IAccountingIntegrationService` for GL side effects.

On failure or `IsSuccess == false`, the coordinator rolls back the ambient transaction.

### `IApplicationDbContext` / `ApplicationDbContext`

- Implements EF `DbContext` + identity stores.
- Injects `ICurrentUser` for audit columns on `BaseEntity`.
- Applies `ModuleModelConfigurationRegistry.Configurators` after assembly configs.

### `IUnitOfWork`

Legacy unit-of-work still used by horizontal services and `TransactionCoordinator`. New module code should prefer explicit repository injection.

## Tests

```bash
dotnet build AAA.ERP.sln -c Release
dotnet test AAA.ERP.sln -c Release

dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj -c Release
dotnet test Tests/ERP.IntegrationTests/ERP.IntegrationTests.csproj -c Release

dotnet ef dbcontext info --project ERP.API/ERP.API.csproj --startup-project ERP.API/ERP.API.csproj
```

Architecture tests in `LayerDependencyTests` and `ModuleBoundaryTests` guard persistence layering.

## Related docs

- [MODULAR_MONOLITH.md §6.4 — Transaction coordinator](../docs/architecture/MODULAR_MONOLITH.md)
- [CONTRIBUTING.md — Module EF and migrations](../docs/architecture/CONTRIBUTING.md#module-ef-and-migrations-phase-2)
- [MEDIATR_REGISTRATION.md](../docs/architecture/MEDIATR_REGISTRATION.md)
