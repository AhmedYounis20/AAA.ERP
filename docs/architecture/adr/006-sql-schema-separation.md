# ADR-006: SQL schema separation evaluation

## Status

Accepted — July 2026 (Phase 4.2.1)

## Context

Phase 4 Month 2 task 4.2.1 asked whether to introduce SQL Server schemas (`accounting.*`, `inventory.*`, `platform.*`) on the existing single database (ADR-002). M3 is complete: modules are extracted in code, `ERP.Persistence` owns one `ApplicationDbContext`, and cross-module flows use `ITransactionCoordinator` + `IAccountingIntegrationService`.

### Options considered

| Option | Description |
|--------|-------------|
| **A — Defer** | Keep `dbo` default schema; module ownership enforced in code only |
| **B — Schemas now** | `accounting.Entries`, `inventory.Products`, etc.; EF `ToTable(name, schema)` |
| **C — Separate databases** | Out of scope; contradicts ADR-002 without new ADR |

### Evaluation criteria

| Criterion | Defer (A) | Schemas now (B) |
|-----------|-----------|-----------------|
| Cross-module FK joins | Unchanged | Still possible within one DB |
| Migration risk | None | High — 60+ tables, hosted SQL Server |
| Module boundary clarity | Code + arch tests | Code + physical naming |
| Ops / reporting | Current scripts work | Views or schema-qualified queries needed |
| Team velocity | No migration sprint | 1–2 week EF + DBA effort |
| Extraction path | ADR-002 already documents | Marginal improvement over code modules |

## Decision

**Defer SQL schema separation (Option A).** Revisit when **any** trigger is met:

1. DBA or reporting requires physical separation for permissions or backup policies
2. A module extraction ADR is initiated (separate DB)
3. Table count or naming collisions make `dbo` unmaintainable

Task **4.2.2** (schema migration) is **not approved** at this time.

## Consequences

- No EF `ToTable(..., schema)` churn in Phase 4
- Module boundaries remain compile-time (projects, arch tests, Contracts)
- `SourceDocumentId` on GL entries (4.2.3) proceeds on `dbo.Entries` — sufficient for idempotent inventory posting
- Future schema work should be a dedicated sprint with: full table inventory, rollback plan, and staging validation on a DB copy

## References

- ADR-002: Single database with composed DbContext
- MODULAR_MONOLITH.md §10.6 tasks 4.2.1–4.2.2
