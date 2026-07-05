# Phase 0 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M0 — Foundation & governance for module-owned APIs

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 0.1 | [ADR-011](./adr/011-module-owned-api.md) — module-owned API strategy | ✅ |
| 0.2 | `ERP.BuildingBlocks.Api` — base CRUD controllers moved from Host | ✅ |
| 0.3 | `Identity.Api` scaffold + `AddApplicationPart` in Host | ✅ |
| 0.4 | `ModuleApiLayerTests` — Api layer isolation + Host ratchet baseline (38) | ✅ |
| 0.5 | [CONTROLLER_MIGRATION_MAP.md](./CONTROLLER_MIGRATION_MAP.md) — 38 controllers mapped | ✅ |
| 0.6 | [MODULE_API_TEMPLATE.md](./MODULE_API_TEMPLATE.md) | ✅ |
| 0.7 | PR template — no new controllers in Host | ✅ |
| 0.8 | [CONTRIBUTING.md](./CONTRIBUTING.md) — folder layout + Api placement | ✅ |
| 0.9 | Empty `Inventory.Infrastructure/Modules/Shared/Shared.API` scaffold removed | ✅ |

---

## Exit criteria

- [x] Architecture tests run on every PR (`ModuleApiLayerTests` added)
- [x] `ERP.BuildingBlocks.Api` builds and Host references it
- [x] `Identity.Api` template builds and is wired via `AddApplicationPart`
- [x] ADR-011 committed
- [x] Controller migration map complete
- [x] PR freeze: no new business controllers in `ERP.Host`

---

## Baseline metrics

| Metric | Value |
|--------|-------|
| Business controllers in Host | 38 |
| Base controllers in BuildingBlocks.Api | 4 |
| New projects | `ERP.BuildingBlocks.Api`, `Identity.Api` |
| New arch tests | 5 (4 active + 1 skipped ratchet) |

---

## Next phase

**Phase 1** — Internal layer cleanup (DTOs → Application, feature-grouped Repos/Services).  
**Phase 2** — Pilot API extraction: move `AuthController` and `AuditLogsController`.
