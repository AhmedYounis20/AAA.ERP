# Phase 1.5 Migration Report — Category C Pilot (FinancialPeriod)

**Status:** In progress (3 of 5 planned items)  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** FinancialPeriod, CostCenter, Branch workflow-service pilots

---

## Summary

Phase 1.5 migrates Category C entities from `BaseService` / `BaseTreeSettingService` passthrough handlers to dedicated `*WorkflowService` types.

| # | Entity | Category | Commit | Outcome |
|---|--------|----------|--------|---------|
| 1 | FinancialPeriod | C | `a2b8cd5` | `FinancialPeriodWorkflowService`; `IFinancialPeriodService` removed |
| 2 | CostCenter | C | `b0cdb8b` | `CostCenterWorkflowService`; `ICostCenterService` removed |
| 3 | Branch | C | *(this commit)* | `BranchWorkflowService`; `IBranchService` slimmed to `GetNextSubLeadgers` |

**Remaining in Phase 1.5 plan:** FixedAsset, Entry group.

---

## Pattern Applied — Category C Workflow

### Before

```csharp
public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service)
    : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(...)
        => service.Create(request);
}
```

### After

```csharp
public sealed class FinancialPeriodCreateCommandHandler(IFinancialPeriodWorkflowService workflow)
    : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(...)
        => workflow.Create(request);
}
```

Write orchestration lives in `Accounting.Infrastructure/Workflows/FinancialPeriodWorkflowService.cs`:

| Operation | Logic preserved |
|-----------|-----------------|
| **Create** | Year-number uniqueness; auto `StartDate` from last period; `EndDate` from `PeriodTypeByMonth` |
| **Update** | Year rename guards (entries + inventory); period shrink guards (last-period, entries/inventory after new end date) |
| **Delete** | Last-period only; not used in entries or inventory transactions |

Cross-module dependencies: `IEntryRepository`, `IFinancialPeriodInventoryUsageQueries`.

### Queries

`FinancialPeriodQueries` no longer depends on a write service:

| Method | Source |
|--------|--------|
| `GetAll` | `repository.GetDtos()` |
| `GetPaginated` | `BaseEntityRepositoryQueries<FinancialPeriod>` |
| `GetById` | `repository.GetDto(id)` |
| `GetNextDefaultdata` | `repository.GetNextDefaultdata()` |
| `GetCurrentFinancialPeriod` | `repository.GetCurrentFinancialPeroid()` |

---

## Files Added / Removed

| Added | Removed |
|-------|---------|
| `IFinancialPeriodWorkflowService.cs` | `IFinancialPeriodService.cs` |
| `Workflows/FinancialPeriodWorkflowService.cs` | `FinancialPeriodService.cs` |

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| CQRS unit tests | **6/6 passed** |
| Architecture tests (`GenericCqrsHandlerArchitectureTests`) | **2/2 passed** |
| API contracts | **Unchanged** |

Note: Category C handlers intentionally inject `IFinancialPeriodWorkflowService` — excluded from generic-handler architecture test (handlers do not inherit `CrudHandlerExecutionBase`).

---

## Architectural Observations

1. **Workflow folder established** — `Accounting.Infrastructure/Workflows/` is the home for Category C write orchestration per ADR-013.
2. **Validation inlined in workflow** — private `ValidateCreate/Update/Delete` methods replace `BaseService` protected overrides; behavior parity preserved without inheriting `BaseService`.
3. **Unused `IUnitOfWork` dropped** — was injected in old service but never used; not carried forward.
4. **Template for CostCenter/Branch/FixedAsset** — extract CRUD + validation to `*WorkflowService`, slim queries to repository.

---

## CostCenter — tree + COA linking (`b0cdb8b`)

`CostCenterWorkflowService` preserves:

| Operation | Logic |
|-----------|--------|
| **Create** | Duplicate-name validation; domain + `RelatedToAccount` nodes link `CostCenterChartOfAccount` rows |
| **Update** | Transactional COA add/remove via `RemoveChartOfAccounts`; domain node field updates |
| **Delete** | Parent-with-children guard; standard entity delete |

`CostCenterQueries` → `BaseTreeSettingRepositoryQueries<CostCenter>`.

---

## Branch — sub-ledger + attachments (`this commit`)

`BranchWorkflowService` preserves:

| Operation | Logic |
|-----------|--------|
| **Create** | Transactional; domain nodes get COA + optional logo attachment; category nodes name/parent only |
| **Update** | Category → standard sub-ledger update; domain → transactional COA name sync + attachment CRUD |
| **Delete** | Transactional; domain nodes delete COA + attachment |

Handlers consolidated into `BranchCreateCommandHandler.cs` (create/update/delete) → `IBranchWorkflowService`.

`IBranchService` retained for `GetNextSubLeadgers` only (`SubLedgerNextDefaultsHelper` + `SD.BranchChartOfAccountId`).

`BranchQueries` → `BaseTreeSettingRepositoryQueries<Branch>`.

| Added | Removed / slimmed |
|-------|-------------------|
| `IBranchWorkflowService.cs` | `BranchUpdateCommandHandler.cs`, `BranchDeleteCommandHandler.cs` |
| `Workflows/BranchWorkflowService.cs` | `IBranchService` write methods |

---

## Recommended Next Steps (Phase 1.5 continuation)

| Priority | Item | Category | Notes |
|----------|------|----------|-------|
| 4 | FixedAsset | C | Asset-type COA rules |
| 5 | Entry group | C | `EntryWorkflowService` first in entry delegation chain |
