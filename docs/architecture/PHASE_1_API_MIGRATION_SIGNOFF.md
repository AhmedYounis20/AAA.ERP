# Phase 1 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M1 — Internal layer cleanup

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 1.1 | Move Accounting DTOs (16 files) → `Accounting.Application/Dtos/` | ✅ |
| 1.2 | Move Inventory DTOs (37 files) → `Inventory.Application/Dtos/` | ✅ |
| 1.3 | Move `AccountFilterDtos.cs` → `Accounting.Application/Dtos/Filters/` | ✅ |
| 1.4 | Group Repos/Services by feature folder (Accounting + Inventory) | ✅ |
| 1.5 | Create `Identity.Contracts` with `ICurrentUser` | ✅ |
| 1.6 | Normalize `ApplicationUser` → `Identity.Domain` namespace | ✅ |
| 1.7 | Arch tests: Domain must not contain `*Dto` types | ✅ |

---

## Exit criteria

- [x] No DTOs in `*.Domain` (arch tests enforce)
- [x] Account filter DTOs in `Accounting.Application.Dtos.Filters`
- [x] Repos/Services grouped under `{Feature}/` subfolders
- [x] `Identity.Contracts` created; `ICurrentUser` moved from Application
- [x] `dotnet build` + full test suite green

---

## Deferred to backlog

- Move account base validators from `BuildingBlocks/Validators/Account/` → `Accounting.Application` (large consumer surface; non-blocking)
- Move `InventoryFilterDtos.cs` from BuildingBlocks → `Inventory.Application/Dtos/Filters/`

---

## Next phase

**Phase 2** — complete (see [PHASE_2_API_MIGRATION_SIGNOFF.md](./PHASE_2_API_MIGRATION_SIGNOFF.md))
