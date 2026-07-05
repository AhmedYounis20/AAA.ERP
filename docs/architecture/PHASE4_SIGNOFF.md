# Phase 4 sign-off — Cross-module boundary hardening

**Goal:** Enforce Contracts-only integration; eliminate bidirectional Application-layer coupling.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 4  
**Date:** July 2026  
**Status:** **Complete** (including deferred slice)

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 4.1 | Create `Inventory.Contracts` | ✅ | `Modules/Inventory/Inventory.Contracts/` |
| 4.2 | `IInventoryQueryService` + financial-period ports | ✅ | `IInventoryQueryService`, `IFinancialPeriodInventoryUsageQueries` |
| 4.3 | Move `TransactionNumberDto`, `EntryNumberDto` to Contracts | ✅ | `Accounting.Contracts/Dtos/` |
| 4.4 | Remove `Accounting.Application` → `Inventory.Application` | ✅ | Project reference removed |
| 4.5 | ADR-008 EF DbConfig cross-module FK | ✅ | [adr/008-ef-dbconfig-cross-module-fk.md](./adr/008-ef-dbconfig-cross-module-fk.md) |
| 4.6 | Arch tests | ✅ | `CrossModuleContractsTests.cs` — **9** cross-module guards |
| 4.7 | `Inventory.Infrastructure` → `Accounting.Application` removed | ✅ | `IAttachmentCrossModulePort`; no `Accounting.Application` project ref |

---

## Deferred items — closed

| Item | Resolution |
|------|------------|
| Attachment repo in catalog services | `IAttachmentCrossModulePort` in `Accounting.Contracts` |
| `IInventoryQueryService` | Added on `Inventory.Contracts`; implemented by `FinancialPeriodInventoryUsageQueries` |
| `Inventory.Infrastructure` → `Accounting.Domain` in `*DbConfig` | Allowed per ADR-008 (unchanged) |

---

## Build / test metrics

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | **163 passed** |
