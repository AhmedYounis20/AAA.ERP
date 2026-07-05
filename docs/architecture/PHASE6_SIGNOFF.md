# Phase 6 sign-off — Namespace normalization

**Goal:** One namespace per physical project; retire `Shared.*` drift.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 6  
**Date:** July 2026

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 6.1 | ADR-009 namespace policy | ✅ | [adr/009-namespace-policy.md](./adr/009-namespace-policy.md) |
| 6.2–6.3 | Migrate `Shared.*` → `ERP.BuildingBlocks.*` | ✅ | Bulk migration; arch guard `BuildingBlocks_and_modules_should_not_declare_new_Shared_namespaces` |
| 6.4 | `Controllers/Account/` → `Controllers/Accounting/` | ✅ | 22 controllers; routes unchanged |
| 6.5 | `ERP.Infrastructure.*` → `ERP.Persistence.*` | ⏭️ | Cosmetic; deferred (low impact) |

---

## Notes

- EF migration snapshots retain legacy type strings until next schema migration (expected).
- `ERP.Shared.Resources` (Platform.Localization) unchanged — not kernel `Shared.*` debt.
