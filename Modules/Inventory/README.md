# Inventory module

Product catalog (products, variants, attributes), stock balances, inventory transactions (receipt/issue), and inter-branch transfers. Inventory **never** references Accounting domain entities; GL posting goes through `Accounting.Contracts`.

## Purpose

- Own catalog and stock aggregates: colors, sizes, packing units, products, variants, items, stock balances, thresholds, expiry levels, transactions, and transfers.
- Coordinate stock mutations with GL posting via `IAccountingIntegrationService` inside `ITransactionCoordinator` for atomic cross-module operations.
- Register EF configurations and DI via `AddInventoryModule()`.

## Project map

| Project | Role |
|---------|------|
| `Inventory.Domain` | DTOs and module-local models. **Entities remain in `ERP.Domain`** for most aggregates (transitional). |
| `Inventory.Application` | Commands, validators, repository/service interfaces. |
| `Inventory.Infrastructure` | Handlers, repositories, services, `*DbConfig`, `InventoryModuleExtensions`. |

```
Modules/Inventory/
├── Inventory.Domain/Models/          ← DTOs; entities still in ERP.Domain
├── Inventory.Application/
│   ├── Commands/{Aggregate}/
│   ├── Validators/{Aggregate}/
│   ├── Repositories/
│   └── Services/
└── Inventory.Infrastructure/
    ├── Handlers/{Aggregate}/
    ├── Repositories/
    ├── Services/                     ← ProductService, InventoryTransactionService, …
    ├── Persistence/{Area}/*DbConfig.cs
    └── InventoryModuleExtensions.cs
```

**Host wiring:** `AddInventoryModule()` in `WebBuilderExtensions.BuildApplication`.

**Swagger group:** `inventory`.

**Horizontal handlers:** Unmigrated inventory handlers may still live under `ERP.Persistence` (`ERP.Infrastructure` namespace) until fully retired.

## How to add a feature

1. **Command + validator** — `Inventory.Application/Commands/{Aggregate}/`, `Validators/{Aggregate}/`.
2. **Ports** — `I*Repository`, `I*Service` in `Inventory.Application`.
3. **Implementation** — Handler in `Inventory.Infrastructure/Handlers/`, repo/service in `Repositories/` and `Services/`.
4. **EF config** — `{Entity}DbConfig` under `Inventory.Infrastructure/Persistence/`; register in `ApplyInventoryModuleConfigurations`.
5. **DI** — Register in `InventoryModuleExtensions.AddInventoryModule()`.
6. **API** — Controller under `ERP.Host/Controllers/Inventory/`; Swagger group `inventory`.
7. **Cross-module GL** — If the feature posts entries, build an `Inventory*PostingRequest` and call `IAccountingIntegrationService` inside `_transactionCoordinator.ExecuteAsync` (see below).

Example handler registration is already in `WebBuilderExtensions` via `typeof(InventoryModuleExtensions).Assembly`.

## Architecture rules

| Rule | Detail |
|------|--------|
| No Accounting domain | `InventoryTransactionService` and `InventoryTransferService` must not reference `ERP.Domain.Models.Entities.Account` (arch test). |
| Catalog decoupling | `ProductService` / `VariantService` → Account entity reference test is **skipped** until cost-center/tax decoupling (Phase 3 debt). |
| Application isolation | `Inventory.Application` must not reference `Inventory.Infrastructure` or `ERP.Persistence`. |
| No API reference | `Inventory.Infrastructure` must not reference `ERP.Host`. |
| Integration boundary | Reference **`Accounting.Contracts` only** — never `Accounting.Domain` or `Accounting.Infrastructure`. |
| Writes via MediatR | Controller → `ISender.Send` → Handler → Service. |

## Key integration points

### `IAccountingIntegrationService`

Inject from `Accounting.Contracts.Integration`. Used by:

- `InventoryTransactionService` — `PostInventoryTransactionEntryAsync` after stock update.
- `InventoryTransferService` — `PostInventoryTransferEntryAsync` with phase-specific `SourceDocumentType` (`InventoryTransferStage`, `InventoryTransferComplete`, `InventoryTransferReject`).

Posting requests live in `Accounting.Contracts/Integration/` (`InventoryTransactionPostingRequest`, `InventoryTransferPostingRequest`).

### `ITransactionCoordinator`

Inject `ITransactionCoordinator` for any flow that must commit **stock + GL** atomically.

**Current usage (Phase 4.1.5 / 4.2.3):**

| Service | Method | Coordinator |
|---------|--------|-------------|
| `InventoryTransactionService` | `Create` | ✅ `_transactionCoordinator.ExecuteAsync` |
| `InventoryTransferService` | `Create`, `ApproveTransfer`, `RejectTransfer` | ✅ `_transactionCoordinator.ExecuteAsync` |

Pattern (from `InventoryTransactionService.Create`):

```csharp
return await _transactionCoordinator.ExecuteAsync(async ct =>
{
  // 1. Persist inventory state
  // 2. await _accountingIntegration.PostInventoryTransactionEntryAsync(request, ct);
  // 3. Return ApiResponse<T>
}, cancellationToken);
```

Coordinator implementation: `ERP.Persistence/Repositories/TransactionCoordinator.cs`. Registration: `InfrastructureExtensions.AddRepositories()`.

## Tests

```bash
dotnet build AAA.ERP.sln -c Release
dotnet test AAA.ERP.sln -c Release

dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj -c Release

# Inventory integration (transactions, transfers, staged GL)
dotnet test Tests/ERP.IntegrationTests/ERP.IntegrationTests.csproj -c Release --filter "FullyQualifiedName~Inventory"
```

**Transfer staging:** `InventoryTransferIntegrationTests` expects **two GL entries** for a staged transfer (stage + complete).

## Performance note (4.2.5)

Products and Variants list endpoints were reviewed for N+1 queries (Phase 4.2.5):

| Endpoint | Repository method | Strategy |
|----------|-------------------|----------|
| `GET /api/Products` | `ProductRepository.GetDtos` | `AsSplitQuery` + includes; variant counts via single `GroupBy` query |
| `GET /api/Products/{id}` | `GetDtoById` | **Intentionally heavy** — full product graph for edit/detail |
| `GET /api/Variants` | `GetDtos` → `GetPaginatedDtos` | **Intentionally heavy** full list; prefer paginated endpoint |
| `GET /api/Variants/paginated` | `GetPaginatedDtos` | `AsSplitQuery` + includes per page |
| `GET /api/Variants/byProduct/{id}` | `GetVariantsByProductId` | Product lookup + `Include(VariantCodes)` batch |
| `GET /api/Variants/search-variants` | `SearchVariants` | `Include(Product, VariantCodes)` batch |

Integration tests: `Tests/ERP.IntegrationTests/Inventory/CatalogListIntegrationTests.cs`.

## Related docs

- [MODULAR_MONOLITH.md §10.6](../../docs/architecture/MODULAR_MONOLITH.md#106-phase-4--hardening)
- [MODULE_TEMPLATE.md](../../docs/architecture/MODULE_TEMPLATE.md)
- [MODULE_OWNERS.md](../../docs/architecture/MODULE_OWNERS.md)
