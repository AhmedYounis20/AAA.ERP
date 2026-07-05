# Phase 2 sign-off — Host & security hardening

**Goal:** Close production security gaps in the composition root.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 2  
**Date:** July 2026

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 2.1 | Guard `DbworkController` | ✅ | `[DevelopmentOnly]` → 404 outside Development; `ERP.Host/Security/DevelopmentOnlyAttribute.cs` |
| 2.2 | Remove Host → `Identity.Domain` direct reference | ✅ | `AddIdentity<ApplicationUser>` moved to `Identity.Infrastructure.AddIdentityModule()`; removed from `ERP.Host.csproj` |
| 2.3 | Host arch tests | ✅ | `HostDependencyTests.cs` — Identity.Domain boundary + controller security attributes |
| 2.4 | Controller auth audit | ✅ | `InventoryTransferController` + `[Authorize]`; `AuthController` + `[AllowAnonymous]`; base controllers already `[Authorize]` |

---

## Controller authorization summary (2.4)

| Controller | Authorization |
|------------|---------------|
| `AuthController` | `[AllowAnonymous]` — login/register/refresh |
| `DbworkController` | `[DevelopmentOnly]` — 404 in Staging/Production |
| `InventoryTransferController` | `[Authorize]` — **fixed** (was missing) |
| `BaseController` / descendants | `[Authorize]` on base class |
| `GLSettingsController`, `FinancialPeriodsController`, `AuditLogsController` | `[Authorize]` explicit |

**Note:** Host controllers still use module **Domain** entity types in generics (transitional). Full Host→Domain decoupling is tracked in Enhancement Plan Phase 7.

---

## Build / test

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | 124 passed (60 arch + 15 smoke + 11 integration + 38 unit) |
| Arch tests | **60** guards (+4 from Phase 2) |

---

## Related

- [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) — Phase 3 next
- [PHASE1_SIGNOFF.md](./PHASE1_SIGNOFF.md)
