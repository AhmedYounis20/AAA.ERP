# ADR-007: Inventory.Domain EF navigation decoupling

## Status

**Approved** — July 2026 (Phase 5)

## Context

`Inventory.Domain` referenced `Accounting.Domain` entity types for EF navigation properties on junction and transaction aggregates. This violated module boundary intent while sharing one database (ADR-002) and schema-separated tables (ADR-006).

Acceptable interim pattern: shared DB with cross-schema FKs configured in **Infrastructure** only, domain entities holding **Guid FKs** without cross-module navigations.

## Decision

1. Remove Accounting navigation properties from Inventory domain entities; retain Guid FK columns.
2. Configure cross-module FK relationships in `Inventory.Infrastructure` DbConfigs using `HasOne<AccountingEntity>().WithMany().HasForeignKey(...)`.
3. Remove `.Include()` on Accounting navigations in Inventory repositories; order/filter by FK where needed.
4. Move shared wire DTO `AttachmentDto` to `Shared.DTOs` for catalog attachment projections.
5. Remove `Accounting.Domain` project reference from `Inventory.Domain`.
6. Guard with arch test `Inventory_Domain_should_not_reference_Accounting_Domain`.

## Consequences

- **Positive:** Inventory.Domain no longer depends on Accounting.Domain; clearer module boundary.
- **Positive:** No SQL or API wire change — FK columns and JSON shapes unchanged.
- **Neutral:** Read APIs return FK ids without nested branch/party objects unless enriched via future query ports/DTOs.
- **Negative:** Branch/party names on reads require Accounting query ports if UI needs them (future enhancement).

## Related

- [ADR-002](./002-single-database.md)
- [ADR-006](./006-sql-schema-separation.md)
- [POST_M4_QUALITY_SIGNOFF.md](../POST_M4_QUALITY_SIGNOFF.md)
