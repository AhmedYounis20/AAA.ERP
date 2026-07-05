# ADR-006: SQL schema separation evaluation

## Status

**Revised — Option B approved** — July 2026 (pre-production, 4.2.2)

> **Revision history:** Original decision (July 2026, Phase 4.2.1) deferred schemas (Option A). Reopened after M4 sign-off because the system is **not in production** — migration risk is lower than originally scored. Task **4.2.2** is now **approved**. Table inventory: [SQL_SCHEMA_TABLE_INVENTORY.md](../SQL_SCHEMA_TABLE_INVENTORY.md).

## Context

Phase 4 Month 2 task 4.2.1 asked whether to introduce SQL Server schemas (`accounting.*`, `inventory.*`, `platform.*`, `identity.*`) on the existing single database (ADR-002). M3 is complete: modules are extracted in code, `ERP.Persistence` owns one `ApplicationDbContext`, and cross-module flows use `ITransactionCoordinator` + `IAccountingIntegrationService`.

M4 is complete. The defer decision assumed hosted production SQL Server with live data. Current state: **pre-production** — drop/recreate or schema-transfer migration on dev/staging is acceptable with team confirmation.

### Options considered

| Option | Description |
|--------|-------------|
| **A — Defer** | Keep `dbo` default schema; module ownership enforced in code only |
| **B — Schemas now** | `accounting.Entries`, `inventory.Products`, etc.; EF `ToTable(name, schema)` |
| **C — Separate databases** | Out of scope; contradicts ADR-002 without new ADR |

### Evaluation criteria (original 4.2.1)

| Criterion | Defer (A) | Schemas now (B) |
|-----------|-----------|-----------------|
| Cross-module FK joins | Unchanged | Still possible within one DB |
| Migration risk | None | High — 60+ tables, hosted SQL Server |
| Module boundary clarity | Code + arch tests | Code + physical naming |
| Ops / reporting | Current scripts work | Views or schema-qualified queries needed |
| Team velocity | No migration sprint | 1–2 week EF + DBA effort |
| Extraction path | ADR-002 already documents | Marginal improvement over code modules |

### Re-evaluation (pre-production, July 2026)

| Criterion | Updated assessment |
|-----------|-------------------|
| Migration risk | **Medium** — 74 tables; in-place `ALTER SCHEMA TRANSFER` via EF migration; no drop/recreate |
| Module boundary clarity | Physical schemas align with module READMEs and arch tests |
| Ops / reporting | Pre-production — update scripts before go-live |
| Team velocity | One dedicated slice: inventory doc → DbConfig → single migration |

## Decision (revised)

**Approve SQL schema separation (Option B)** for task **4.2.2**:

1. Four schemas: `accounting`, `inventory`, `platform`, `identity`.
2. `dbo` retains `__EFMigrationsHistory` only.
3. Full table mapping in [SQL_SCHEMA_TABLE_INVENTORY.md](../SQL_SCHEMA_TABLE_INVENTORY.md) (74 tables).
4. Implementation: update module `*DbConfig`, one EF migration, validate on dev DB.
5. Cross-schema FKs (inventory junction → accounting masters) remain valid in single DB per ADR-002.

Original defer triggers (DBA permissions, module extraction ADR, naming collisions) no longer block pre-production hardening.

## Consequences

- EF `ToTable(..., schema)` on all mapped entities; Identity tables configured explicitly.
- Reporting / ad-hoc SQL must use schema-qualified names (`accounting.Entries`, etc.).
- Historical migration designer entity strings unchanged (snapshot identity).
- Rollback plan documented in table inventory; test on DB copy before hosted apply.

## Implementation slices

| Slice | Deliverable |
|-------|-------------|
| **9a** | ADR revision + [SQL_SCHEMA_TABLE_INVENTORY.md](../SQL_SCHEMA_TABLE_INVENTORY.md) — **Done** (`f256151`) |
| **9b** | DbConfig + Identity schema + `SeparateModuleSchemas` migration + doc updates — **Done** (`f256151`) |
| **9c** | Dev DB migration applied — **Done** (July 2026) |

Migration strategy: **in-place** — `SeparateModuleSchemas` uses `EnsureSchema` + `RenameTable` (`ALTER SCHEMA TRANSFER` on SQL Server); no drop/recreate.

## References

- ADR-002: Single database with composed DbContext
- MODULAR_MONOLITH.md §10.6 tasks 4.2.1–4.2.2
- M4_SIGNOFF.md
