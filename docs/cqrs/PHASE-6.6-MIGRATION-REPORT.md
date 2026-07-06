# Phase 6.6 Migration Report — Slim Read Service Removal

**Status:** Complete  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Remove last thin read facades; queries use repositories directly

---

## Summary

Phase 6.6 inlines the remaining read-only logic from three slim services into query classes (or a shared helper), then deletes the services and their interfaces.

| Removed | Query now uses | Notes |
|---------|----------------|-------|
| `IChartOfAccountService` | `IChartOfAccountRepository` via `ChartOfAccountQueries` | `GenerateNewChildCode`, `GetNextAccountDefaultData` inlined |
| `IAttributeDefinitionService` | `IAttributeDefinitionRepository` via `AttributeDefinitionQueries` | `GetActiveAttributeDefinitions`, `GetWithPredefinedValues` inlined |
| `IInventoryTransactionService` | `IInventoryTransactionRepository` + `IAccountingIntegrationService` | Shared helpers in `InventoryTransactionQuerySupport` |

**Retained (intentional):** `IEntryService`, `IComplexEntryService`, `IStockBalanceService`, `IGLSettingService` — see Phase 6.5 report for rationale.

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |
| `dotnet test Tests/Inventory.UnitTests/Inventory.UnitTests.csproj` | **6/6 passed** |

---

## Optional Next Steps (Phase 6.7+)

- Inline `IEntryService` / `IComplexEntryService` into entry-group queries
- Remove `BaseService` / `BaseTreeSettingService` inheritance from catalog workflows
- Update `MediatRRegistrationTests` baselines if handler counts change
