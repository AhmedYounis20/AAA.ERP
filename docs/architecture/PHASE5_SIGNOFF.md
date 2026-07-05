# Phase 5 sign-off — Inventory boundary + ops readiness

**Goal:** Close deferred post-M4 items that do not require production access.  
**Branch:** `Refactor` · **Date:** July 2026

---

## Exit criteria

| Criterion | Status | Evidence |
|-----------|--------|----------|
| ADR-007 Inventory.Domain EF decoupling | ✅ | [adr/007-inventory-domain-ef-decoupling.md](./adr/007-inventory-domain-ef-decoupling.md) |
| Remove `Accounting.Domain` ref from `Inventory.Domain` | ✅ | `Inventory.Domain.csproj`; arch test `Inventory_Domain_should_not_reference_Accounting_Domain` |
| Shared `AttachmentDto` for catalog wire shape | ✅ | `Shared/DTOs/AttachmentDto.cs`; inventory DTOs/repos updated |
| Infrastructure FK wiring without domain navigations | ✅ | `Inventory.Infrastructure/Persistence/**` DbConfigs |
| Staging/prod schema migration runbook | ✅ | [docs/ops/SCHEMA_MIGRATION_RUNBOOK.md](../ops/SCHEMA_MIGRATION_RUNBOOK.md) |
| Swagger v1 smoke (schema ID conflict) | ✅ | `CustomSchemaIds` in `SwaggerExtensions.cs`; 15/15 smoke tests pass |
| Arabic localization for L1 keys | ✅ | `Resource.ar-EG.resx` — 154 keys translated (July 2026) |

---

## Build / test metrics (2026-07-05)

| Metric | Result |
|--------|--------|
| `dotnet build AAA.ERP.sln -c Release` | Green |
| `dotnet test AAA.ERP.sln -c Release` | **119 passed**, 0 skipped |
| Arch tests (`ERP.Architecture.Tests`) | 55 guards (includes Inventory Domain boundary) |
| Smoke tests | 15 passed (incl. `/swagger/v1/swagger.json`) |
| Integration tests | 11 passed |

---

## Intentionally deferred (future slices — not Phase 5 blockers)

| Item | Reason |
|------|--------|
| ERP.Persistence base-service retirement | Wave 3E follow-up; kernel retained by design; one cluster per slice |
| Staging/prod `SeparateModuleSchemas` execution | Operator task — runbook ready; requires target environment |
| DTO `Dto`/`DTO` naming normalization | Low priority; no wire impact |
| FluentValidation rule sets / specific exception types | Enhancement backlog |
| Branch/party name enrichment on inventory reads | Future Accounting query ports if UI needs nested objects |

---

## Related

- [POST_M4_QUALITY_SIGNOFF.md](./POST_M4_QUALITY_SIGNOFF.md)
- [M4_SIGNOFF.md](./M4_SIGNOFF.md)
- [TECHNICAL_DEBT.md](../../TECHNICAL_DEBT.md)
- [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md) §16
