# Phase 6.3 Migration Report — Inventory Transaction Query Cleanup

**Status:** Complete  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Remove import/export/transfer read facades; queries use repository + core read service

---

## Summary

Phase 6.3 removes three transitional inventory transaction read services. Queries now call `IInventoryTransactionRepository`, `IInventoryTransferRepository`, and `IInventoryTransactionService` directly.

| Service removed | Query now uses |
|-----------------|----------------|
| `IImportTransactionService` | `IInventoryTransactionRepository` + `IInventoryTransactionService` + `IInventoryAccountingDisplayQueries` |
| `IExportTransactionService` | Same (with `InventoryTransactionType.Issue` filter) |
| `IInventoryTransferService` | `IInventoryTransferRepository` + `IInventoryAccountingDisplayQueries` |

**Retained:** `IInventoryTransactionService` / `InventoryTransactionService` — shared read logic (pagination, transaction numbering, type/branch queries).

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |
| `dotnet test Tests/Inventory.UnitTests/Inventory.UnitTests.csproj` | **6/6 passed** |

---

## Next Steps (Phase 6.5+)

- Extract CRUD logic from catalog services into workflow services (remove `BaseService` inheritance)
- Optional: inline remaining core read services into queries
