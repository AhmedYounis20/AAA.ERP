# Phase 5 sign-off — Testing expansion

**Goal:** Module-owned tests around critical ERP workflows.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 5  
**Date:** July 2026

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 5.1 | Split unit tests per module | ✅ | `Tests/Accounting.UnitTests/`, `Tests/Inventory.UnitTests/` |
| 5.2 | Handler tests: receipt/issue → GL | ✅ | `InventoryGlPostingHandlerTests.cs` |
| 5.3 | Transfer GL + coordinator mocks | ✅ | Covered in integration + contract tests |
| 5.4 | Integration tests ≥20 | ✅ | **24** in `ERP.IntegrationTests` |
| 5.5 | `IAccountingIntegrationService` contract tests | ✅ | `AccountingIntegrationServiceContractTests.cs` |
| 5.6 | CI Coverlet coverage artifact | ✅ | `.github/workflows/ci.yml` publishes `coverage.cobertura.xml` |

---

## Test counts (Release)

| Project | Tests |
|---------|------:|
| ERP.Architecture.Tests | 72 |
| ERP.UnitTests | 36 |
| ERP.IntegrationTests | 24 |
| ERP.SmokeTests | 18 |
| Accounting.UnitTests | 9 |
| Inventory.UnitTests | 4 |
| **Total** | **163** |

_Note: 200-test target deferred to ongoing growth; critical GL/inventory paths are covered._
