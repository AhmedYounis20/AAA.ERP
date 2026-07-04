# Phase 0 retrospective

**Date:** 4 July 2026  
**Milestone:** M0 — Foundation and governance  
**Decision:** **Go** for Phase 1 (Accounting integration facade)

## Completed tasks

| ID | Task | Deliverable |
|----|------|-------------|
| 0.1.1 | Module owners assigned | [MODULE_OWNERS.md](./MODULE_OWNERS.md) |
| 0.1.2 | Architecture test project | `Tests/ERP.Architecture.Tests` |
| 0.1.3 | Domain → no Infrastructure | `LayerDependencyTests` |
| 0.1.4 | Application → no Infrastructure | `LayerDependencyTests` |
| 0.1.5 | API controllers → no Infrastructure (except legacy seed) | `ApiControllerDependencyTests` |
| 0.1.6 | Inventory→Account ratchet (skipped) | `ModuleBoundaryTests` |
| 0.1.7 | CI pipeline | `.github/workflows/ci.yml` |
| 0.2.1 | MediatR audit | [MEDIATR_REGISTRATION.md](./MEDIATR_REGISTRATION.md) |
| 0.2.2–0.2.3 | Single MediatR registration | `WebBuilderExtensions.cs` |
| 0.2.4 | Smoke tests | `Tests/ERP.SmokeTests` + [SMOKE_TEST_CHECKLIST.md](./SMOKE_TEST_CHECKLIST.md) |
| 0.2.5 | Duplicate DI removed | `InfrasctructureExtensions.cs` |
| 0.2.6 | PR template freeze rule | `.github/pull_request_template.md` |
| 0.3.1 | Contributing guide | [CONTRIBUTING.md](./CONTRIBUTING.md) |
| 0.3.2 | Rename tracking issues | [RENAME_TRACKING.md](./RENAME_TRACKING.md) |
| 0.3.3 | Retrospective + go/no-go | This document |

## Exit criteria (M0)

- [x] Architecture tests in CI
- [x] MediatR single registration
- [x] No duplicate DI registrations
- [x] ADR-001 to ADR-004 committed
- [x] Module owners assigned
- [x] Smoke tests automated
- [x] PR template with `IUnitOfWork` freeze rule

## Checkpoint CP-1: Go/no-go

| Question | Answer |
|----------|--------|
| Do arch tests run reliably? | Yes — `dotnet test Tests/ERP.Architecture.Tests` |
| Is MediatR consolidated? | Yes — verified by audit doc + tests |
| Are smoke tests green? | Yes — in-memory host tests in CI |

**Result:** Proceed to **Phase 1** starting with `IAccountingIntegrationService` (§10.3).

## Known deferred items

| Item | Target phase |
|------|--------------|
| `Infrastracture` → `Infrastructure` rename | Phase 4 / dedicated PR |
| `Compined` → `Combined` rename | Phase 4 / dedicated PR |
| Enable Inventory→Account arch test | Phase 1 completion |
| Refactor `DbworkController` seed tooling | Phase 3 |
| Restore or remove `IItemRepository` from `UnitOfWork` (implementation commented out) | Phase 3 / Inventory cleanup |

## Phase 1 start

**Recommended start:** Week of 7 July 2026  
**First tasks:** 1.1.1–1.1.4 (`Accounting.Contracts` + `IAccountingIntegrationService`)
