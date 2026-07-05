# Module API migration plan

> **Status:** Phases 0–4 **complete** — Phase 5 (Host slim-down) is next  
> **ADR:** [011-module-owned-api.md](./adr/011-module-owned-api.md)  
> **Sign-off:** [PHASE_0_API_MIGRATION_SIGNOFF.md](./PHASE_0_API_MIGRATION_SIGNOFF.md)

Moves HTTP controllers from `ERP.Host` into per-module `*.Api` projects while preserving API routes.

---

## Phase 0 — Foundation (complete)

| ID | Task | Status |
|----|------|--------|
| 0.1 | ADR-011 module-owned API | ✅ |
| 0.2 | `ERP.BuildingBlocks.Api` + base controllers | ✅ |
| 0.3 | `Identity.Api` template + `AddApplicationPart` | ✅ |
| 0.4 | `ModuleApiLayerTests` arch rules | ✅ |
| 0.5 | `CONTROLLER_MIGRATION_MAP.md` | ✅ |
| 0.6 | `MODULE_API_TEMPLATE.md` | ✅ |
| 0.7 | PR template freeze rule | ✅ |
| 0.8 | `CONTRIBUTING.md` updated | ✅ |
| 0.9 | Delete empty scaffolds | ✅ |

---

## Phase 1 — Internal layer cleanup (complete)

See [PHASE_1_API_MIGRATION_SIGNOFF.md](./PHASE_1_API_MIGRATION_SIGNOFF.md).

---

## Phase 2 — Pilot API extraction (complete)

See [PHASE_2_API_MIGRATION_SIGNOFF.md](./PHASE_2_API_MIGRATION_SIGNOFF.md).

---

## Phase 3 — Accounting API extraction (complete)

See [PHASE_3_API_MIGRATION_SIGNOFF.md](./PHASE_3_API_MIGRATION_SIGNOFF.md).

---

## Phase 4 — Inventory API extraction (complete)

See [PHASE_4_API_MIGRATION_SIGNOFF.md](./PHASE_4_API_MIGRATION_SIGNOFF.md).

---

## Phase 5 — Host slim-down

See [CONTROLLER_MIGRATION_MAP.md](./CONTROLLER_MIGRATION_MAP.md) for per-controller targets and wave schedule.

| Phase | Scope | Host controllers after |
|-------|-------|------------------------|
| Phase 4 | Inventory (14) | 1 (DbWork only) |
| Phase 5 | Remove empty `ERP.Host/Controllers/` | 0 |
