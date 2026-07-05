# Phase 6 API migration sign-off — Documentation & closure

> **Date:** July 2026  
> **Milestone:** Module-owned API migration closed

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 6.1 | `MODULAR_MONOLITH.md` — target layout, §9 API surface, ADR-011 increment | ✅ |
| 6.2 | `SOLUTION_DIAGRAM.md` — `*.Api` projects and dependency flow | ✅ |
| 6.3 | `CONTRIBUTING.md` — current structure (Api projects no longer "planned") | ✅ |
| 6.4 | `ADR-011` — marked implemented | ✅ |

---

## Migration summary (Phases 0–6)

| Phase | Outcome |
|-------|---------|
| 0 | Foundation: `ERP.BuildingBlocks.Api`, arch tests, templates |
| 1 | Internal cleanup: DTOs, feature folders, `Identity.Contracts` |
| 2 | Pilot: `Identity.Api`, `Platform.Audit.Api` |
| 3 | `Accounting.Api` — 21 controllers |
| 4 | `Inventory.Api` — 14 controllers |
| 5 | Host slim-down — `ERP.Host.Dev` only |
| 6 | Architecture documentation sync |

**Total business controllers in modules:** 37 (+ 1 dev-only `DbWork` in Host)

---

## Deferred (not blocking)

- `Inventory.Api` → `Accounting.Application` for `IBaseQueryService<,>` — move to Contracts/BuildingBlocks
- `DbWorkController` Persistence coupling — optional dev abstraction
- Account validators in `BuildingBlocks/Validators/Account/`
- `InventoryFilterDtos` in BuildingBlocks
