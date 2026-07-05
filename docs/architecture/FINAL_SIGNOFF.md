# Final sign-off — zero deferred work (pre-production)

**Goal:** Close every remaining deferred item before go-live.  
**Branch:** `Refactor` · **Date:** July 2026

---

## Exit criteria — all complete

| Criterion | Status | Evidence |
|-----------|--------|----------|
| ERP.Persistence horizontal Accounting code retired | ✅ | SubLedger, Attachment, BaseQuery, FinancialPeriodUsage → `Accounting.Infrastructure` |
| Persistence kernel guard | ✅ | Arch test `Persistence_kernel_should_not_contain_Accounting_horizontal_services` |
| Inventory branch/party read enrichment (ADR-007) | ✅ | `IInventoryAccountingDisplayQueries` + `*ReadDto` on transactions/transfers/stock |
| BuildingBlocks `DTOs` → `Dtos` namespace | ✅ | `ERP.BuildingBlocks.Application.Dtos.BaseDtos` |
| Entry header FluentValidation DRY | ✅ | Phase 6 — `EntryHeaderRulesValidator` |
| Shared `AttachmentDto` | ✅ | Phase 6 |
| Identity `*Dto` naming | ✅ | Phase 6 |
| `ConflictException` / `ForbiddenException` | ✅ | Phase 6 |
| Arabic localization L1 | ✅ | Phase 5 |
| ADR-007 Inventory.Domain decoupling | ✅ | Phase 5 |

---

## Build / test metrics

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | **120 passed** (56 arch + 15 smoke + 11 integration + 38 unit) |

---

## Intentionally out of scope (not product debt)

| Item | Notes |
|------|-------|
| Microservices split | Architecture decision — monolith retained |
| API route breaking changes | Legacy routes preserved by design |
| EF migration `.Designer.cs` entity strings | Historical snapshots — no runtime impact |
| Generic base repos in ERP.Persistence kernel | Shared kernel by design until optional BuildingBlocks.Persistence move |
| `ERP.Infrastructure.*` namespace vs `ERP.Persistence` folder | Cosmetic; folder is persistence kernel |

---

## Related

- [PHASE5_SIGNOFF.md](./PHASE5_SIGNOFF.md)
- [PHASE6_SIGNOFF.md](./PHASE6_SIGNOFF.md)
- [POST_M4_QUALITY_SIGNOFF.md](./POST_M4_QUALITY_SIGNOFF.md)
- [M4_SIGNOFF.md](./M4_SIGNOFF.md)
