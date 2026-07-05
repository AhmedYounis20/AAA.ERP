# Post-M4 quality sign-off (D1–D6 + localization)

**Goal:** Close quality-debt slices after M4 — error keys, namespaces, XML docs, Swagger, localization resource parity.  
**Branch:** `Refactor` · **Date:** July 2026

---

## Exit criteria

| Criterion | Status | Evidence |
|-----------|--------|----------|
| D6 doc sync (C3–C8 commit refs) | ✅ | `TECHNICAL_DEBT.md`, `MODULAR_MONOLITH.md` §16 |
| D1 persistence + exception handler error keys | ✅ | `BaseTreeSettingService`, `SubLeadgerService`, `BaseService`, `Shared/Exceptions/Handler/CustomExceptionHandler` |
| D2 Identity namespace cleanup | ✅ | `Identity.Infrastructure.Services`; arch test |
| D3 Platform `ICacheService` XML docs | ✅ | `Platform.Caching.Application/ICacheService.cs` |
| D4 Controller Swagger XML docs | ✅ | All `ERP.Host/Controllers/**`; `IncludeXmlComments`; Identity auth DTOs |
| D5 hygiene | ✅ | Identity namespace arch test; `dotnet format` on touched projects |
| Localization resource parity | ✅ | `ErrorMessages`, `SuccessMessages`, `ValidationMessages` wire keys in `Resource.resx` + `Resource.ar-EG.resx` |

---

## Build / test metrics (2026-07-05)

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | 118 passed, 0 skipped |
| Arch tests (`ERP.Architecture.Tests`) | 54 guards, 0 skipped |
| Integration tests | 11 passed |
| `dotnet ef dbcontext info` | `ApplicationDbContext`, `MigrationsAssembly=ERP.Host` |

---

## Intentionally deferred (Phase 5 — not blockers)

| Item | Reason |
|------|--------|
| Inventory.Domain EF navigation decoupling | Requires ADR/plan first; multi-session; shared DB acceptable today |
| ERP.Persistence base-service retirement | Wave 3E follow-up; one cluster per slice |
| Staging/prod `SeparateModuleSchemas` | Operator task on other environments |
| DTO `Dto`/`DTO` naming normalization | Low priority; no wire impact |
| FluentValidation rule sets / specific exception types | Enhancement backlog |
| Arabic copy review for new localization keys | `ar-EG` uses English fallback for newly added keys until translated |

---

## Ongoing (4.3.x)

| Cadence | Task |
|---------|------|
| Per PR | Arch test review — no new namespace or boundary violations |
| Per PR | Module owner review on cross-module changes |

---

## Related

- [M4_SIGNOFF.md](./M4_SIGNOFF.md)
- [TECHNICAL_DEBT.md](../../TECHNICAL_DEBT.md)
- [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md) §16
