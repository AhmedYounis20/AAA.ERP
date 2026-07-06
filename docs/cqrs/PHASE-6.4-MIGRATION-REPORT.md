# Phase 6.4 Migration Report — Catalog Query Cleanup

**Status:** Complete  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Migrate Item, Product, Variant queries off `I*Service` to repositories

---

## Summary

Phase 6.4 decouples catalog read paths from write-capable services. Queries now use `I*Repository` (and code repositories for next-code helpers) directly.

| Query | Was delegating to | Now uses |
|-------|-------------------|----------|
| `VariantQueries` | `IVariantService` | `IVariantRepository` |
| `ProductQueries` | `IProductService` | `IProductRepository` + `IBaseRepository<ProductCode>` + `BaseTreeSettingRepositoryQueries` |
| `ItemQueries` | `IItemService` | `IItemRepository` + `IBaseRepository<ItemCode>` + `BaseTreeSettingRepositoryQueries` |

**Retained:** `IItemService`, `IProductService`, `IVariantService` — still used by workflow services for CRUD writes. Read methods remain on services for internal use (e.g. `ProductService` → `IVariantService`) until a future workflow extraction phase.

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |
| `dotnet test Tests/Inventory.UnitTests/Inventory.UnitTests.csproj` | **6/6 passed** |

---

## Next Steps (Phase 6.5+)

- Extract CRUD logic from `ItemService` / `ProductService` / `VariantService` into workflow services (remove `BaseService` inheritance)
- Slim `I*Service` interfaces to write-only or remove entirely
- Optional: inline `IEntryService` / `IComplexEntryService` / `IInventoryTransactionService` into queries
