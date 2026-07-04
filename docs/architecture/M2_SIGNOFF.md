# Phase 2 milestone sign-off (M2)

**Goal:** Prove module project structure end-to-end with the **Taxes** vertical slice.  
**Pilot module:** Accounting · **Date:** July 2026 · **Branch:** `Refactor`

---

## Exit criteria

| Criterion | Status | Evidence |
|-----------|--------|----------|
| Taxes fully in `Accounting.*` projects | ✅ | Entity, commands, handlers, repo, service, `TaxDbConfig` under `Modules/Accounting/` |
| `AddAccountingModule()` composition pattern | ✅ | `AccountingModuleExtensions.cs`; called from `WebBuilderExtensions` |
| `MODULE_TEMPLATE.md` published | ✅ | [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md) |
| Arch tests cover Accounting dependency rules | ✅ | 6 rules in `ModuleBoundaryTests.cs` |
| No API breaking changes | ✅ | `/api/Taxes` unchanged; smoke test 401 gate |
| EF design-time works with module configs | ✅ | `ApplicationDbContextFactory` + drift check |
| Integration test for Tax write path | ✅ | `TaxIntegrationTests.Create_tax_via_MediatR_persists_and_can_be_read_back` |

---

## Build / test metrics (2026-07-04)

Measured on developer machine (`dotnet` 8, Release):

| Metric | Incremental (warm) | Cold (full rebuild) |
|--------|-------------------|---------------------|
| `dotnet build AAA.ERP.sln -c Release` | ~10–28 s | ~105 s |
| `dotnet test AAA.ERP.sln -c Release` | ~14 s | ~14 s |
| Test count | 53 passed, 1 skipped | — |

**Assessment:** Module projects add negligible overhead vs pre-M2 horizontal layout. No split into separate deployables required at M2.

**Baseline note:** Pre-M2 commit `e912130` (Phase 0) had ~48 tests; +5 tests from Accounting arch rules and Tax integration test.

---

## Deferred to Phase 3

| Item | Reason |
|------|--------|
| ExpiryLevels micro-pilot (2.4.2) | Child entity on Product/Variant; no standalone API — see MODULE_TEMPLATE.md |
| Item→Variant FK migration drift | Pre-existing model/snapshot mismatch; fix before next production migration |
| Retire horizontal `ERP.Domain` / `ERP.Infrastracture` | Phase 3 Waves 3A–3D |
| Second module scaffold (Inventory) | Wave 3D; use MODULE_TEMPLATE.md |

---

## Team review checklist (2.4.3)

- [ ] Module folder layout matches [MODULAR_MONOLITH.md §4.2](./MODULAR_MONOLITH.md#42-target-solution-layout)
- [ ] Naming: `{Module}.Domain|Application|Infrastructure|Contracts`
- [ ] Single MediatR registration point; module assemblies added in host only
- [ ] EF: module owns DbConfig, host owns migrations
- [ ] New aggregates follow [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md)
- [ ] CI runs `ERP.Architecture.Tests` on every PR

**Reviewers:** Module owners per [MODULE_OWNERS.md](./MODULE_OWNERS.md)

---

## Next milestone

**Phase 3 — Module extraction** ([§10.5](./MODULAR_MONOLITH.md#105-phase-3--module-extraction)): BuildingBlocks + Platform, then Accounting aggregates using this template (~1–2 pd each).
