# Phase 4 milestone sign-off (M4)

**Goal:** Production-grade modular monolith — observability, ops, namespace debt retirement, module boundaries.  
**Branch:** `Refactor` · **Date:** July 2026

---

## Exit criteria

| Criterion | Status | Evidence |
|-----------|--------|----------|
| Structured logging + production runbook | ✅ | [PRODUCTION_LOGGING.md](../ops/PRODUCTION_LOGGING.md); Serilog enrichers |
| Naming debt (`Infrastructure`, `Combined`) | ✅ | Phase 4.1.3–4.1.4; legacy route/JSON preserved |
| Module READMEs | ✅ | `Modules/*/README.md`, `ERP.Persistence/README.md` |
| `ITransactionCoordinator` cross-module ACID | ✅ | Inventory transactions + transfers |
| Idempotent inventory GL posting | ✅ | `Entry.SourceDocumentId` / `SourceDocumentType` |
| Namespace perimeter (Increments 6–7) | ✅ | `ERP.Host`; `Domain.Account.*` retired from production code |
| Inventory catalog → Accounting boundary | ✅ | `IInventoryCatalogValidationQueries`; arch test enabled |
| `AAA.ERP.OutputDtos` retired | ✅ | `Accounting.Domain.OutputDtos` + BuildingBlocks base DTOs |
| Optional SQL schemas | **Done** | [ADR-006](./adr/006-sql-schema-separation.md) Option B; `SeparateModuleSchemas` migration |
| Onboarding < 1 day | ✅ | Module READMEs + [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md) |

---

## Build / test metrics (2026-07-05)

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | 118 passed, 0 skipped |
| Arch tests (`ERP.Architecture.Tests`) | 54 guards, 0 skipped |
| `dotnet ef dbcontext info` | `ApplicationDbContext`, `MigrationsAssembly=ERP.Host` |

---

## Intentionally unchanged

| Item | Reason |
|------|--------|
| EF migration designer entity strings (`Domain.Account.Models.Entities.*`) | Historical snapshot identity — rewriting risks migration drift |
| API routes / JSON wire formats | Stability contract with frontend |
| SQL schema separation (`accounting.*`, `inventory.*`, `platform.*`, `identity.*`) | Implemented 4.2.2 — migrate in place via `RenameTable` |

---

## Ongoing (4.3.x)

| Cadence | Task |
|---------|------|
| Per PR | Arch test review — no new namespace or boundary violations |
| Per PR | Module owner review on cross-module changes |
| Monthly | Dependency graph check — no new cycles |

---

## Next

- **Post-M4 quality:** **Complete** — [POST_M4_QUALITY_SIGNOFF.md](./POST_M4_QUALITY_SIGNOFF.md) (D1–D6, L1)
- **Ongoing:** 4.3.x arch test review per PR
- **Phase 5:** Complete — see [PHASE5_SIGNOFF.md](./PHASE5_SIGNOFF.md) (ADR-007, schema runbook, Swagger fix, Arabic L1). **Future:** ERP.Persistence retirement; ops schema execution on staging/prod
- **Optional:** push `Refactor` and open PR when ready
