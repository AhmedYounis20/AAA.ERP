# Phase 3 sign-off — Operational readiness

**Goal:** Staging/production parity tooling, CI quality gates, and operator checklists.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 3  
**Date:** July 2026

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 3.1 | `SeparateModuleSchemas` on **staging** | **Operator** | [STAGING_DEPLOYMENT_CHECKLIST.md](../ops/STAGING_DEPLOYMENT_CHECKLIST.md) + [SCHEMA_MIGRATION_RUNBOOK.md](../ops/SCHEMA_MIGRATION_RUNBOOK.md) |
| 3.2 | Migration on **production** | **Operator** | Same checklist — after staging sign-off |
| 3.3 | Smoke checklist on staging | **Operator** | [SMOKE_TEST_CHECKLIST.md](./SMOKE_TEST_CHECKLIST.md) (manual section) |
| 3.4 | Nullable warnings gate on changed files (CI) | ✅ | `scripts/check-nullable-on-changed-files.sh`; `.github/workflows/ci.yml`; PR template |
| 3.5 | Serilog production config verified | ✅ | [PRODUCTION_LOGGING.md](../ops/PRODUCTION_LOGGING.md); `ProductionLoggingConfigurationTests` |

### Engineering deliverables (this phase)

| Deliverable | Location |
|-------------|----------|
| Staging/production deployment checklist | `docs/ops/STAGING_DEPLOYMENT_CHECKLIST.md` |
| Schema verification SQL | `scripts/verify-module-schemas.sql` |
| Production Dbwork guard smoke test | `ProductionApiSmokeTests` in `ERP.SmokeTests` |
| CI nullable gate on PRs | `.github/workflows/ci.yml` |

---

## Build / test metrics

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | **127 passed** (60 arch + 18 smoke + 11 integration + 38 unit) |
| Smoke tests | **18** (+3: Dbwork Production guard ×2, InventoryTransfer auth) |

---

## Operator sign-off (complete before go-live)

| Environment | Schema migrated | Smoke checklist | Signed |
|-------------|-----------------|-----------------|--------|
| Staging | [ ] | [ ] | _________ |
| Production | [ ] | [ ] | _________ |

---

## Related

- [PHASE2_SIGNOFF.md](./PHASE2_SIGNOFF.md)
- [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) — Phase 4 next
