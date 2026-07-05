# Hybrid CQRS — Final sign-off (Sprint 11 / M4)

> **Date:** July 2026  
> **Branch:** `AY/HybridCqrsRefactor`  
> **ADR:** [012-hybrid-cqrs-mediatr-actions-read-services.md](./adr/012-hybrid-cqrs-mediatr-actions-read-services.md)  
> **Plan:** [HYBRID_CQRS_PLAN.md](./HYBRID_CQRS_PLAN.md)

## Summary

The hybrid CQRS program is **complete** for Accounting and Inventory business APIs. All 35 CRUD controllers follow:

| HTTP | Path | Controller injects |
|------|------|-------------------|
| POST/PUT/PATCH/DELETE | `ISender.Send(command)` → handler | `ISender` |
| GET | `I*ReadService` / `I*Queries` | Read contract only |

Legacy `BaseController` dual DI (`IBaseService` + `ISender`) is **retired** — base classes marked `[Obsolete]`; no production controller inherits them.

## Milestones

| Milestone | Sprint | Status |
|-----------|--------|--------|
| M0 Standards | 0 | ✅ ADR-012, plan, tracker |
| M1 P0 inventory | 3 | ✅ Import, export, transfer |
| M2 P1 entries | 6 | ✅ All 6 entry types |
| M3 Master data | 10 | ✅ Accounting + Inventory catalog |
| M4 Program complete | 11 | ✅ This sign-off |

## Controller coverage (35 enforced)

- **Accounting:** 21 controllers (entries, sub-ledgers, settings, COA)
- **Inventory:** 14 controllers (catalog, pricing, stock, transactions)

### Out of scope (by design)

| Controller | Reason |
|------------|--------|
| `AuthController` | Identity auth service exception |
| `AuditLogsController` | Read-only audit API |
| `DbWorkController` | Development-only tooling |

## Architecture tests (strict mode)

`HybridCqrsArchitectureTests` auto-discovers Accounting + Inventory controllers and enforces:

1. No legacy write `I*Service` injection in controllers
2. `ISender` required when controller exposes write HTTP actions
3. No inheritance from obsolete `BaseController` hierarchy
4. Read contracts (`I*ReadService`, `I*Queries`) do not extend `IBaseService`
5. Minimum controller count guard (≥ 35)

## Test evidence

| Suite | Result |
|-------|--------|
| `dotnet test AAA.ERP.sln -c Release` | ✅ 183 tests (incl. 88 arch, 5 hybrid CQRS rules) |
| Smoke tests (`ERP.SmokeTests`) | ✅ 18/18 |
| Integration tests | ✅ 25/25 |

## Transitional backlog (post-M4)

These are **intentionally deferred** — controllers are compliant; handlers still delegate to application services:

- Inline create/update/delete business logic from `I*Service` into command handlers (P0/P1 priority aggregates first)
- Remove or narrow `I*Service` write surface after handlers own orchestration
- Optional: unify `*Queries` vs `*ReadService` naming for entry types

## Sprint 11 checklist

- [x] S11-1 — `BaseController` hierarchy marked obsolete; zero controller inheritance
- [x] S11-2 — Write services retained for handler delegation (documented backlog; not removed)
- [x] S11-3 — Arch tests strict (assembly discovery, 5 rules)
- [x] S11-4 — Full regression green
- [x] S11-5 — Automated smoke suite green (see [SMOKE_TEST_CHECKLIST.md](./SMOKE_TEST_CHECKLIST.md) for manual CRUD)
- [x] S11-6 — This sign-off document

**Signed off:** Hybrid CQRS program — M4 complete.
