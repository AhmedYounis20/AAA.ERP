# ADR-008: EF DbConfig cross-module foreign keys

## Status

**Approved** — July 2026 (Enhancement Plan Phase 4)

## Context

Inventory tables reference Accounting entities (branches, suppliers, financial periods) via foreign keys. ADR-007 removed cross-module **navigation properties** from `Inventory.Domain`, but EF still needs relationship configuration in Infrastructure.

Application layers must integrate through `*.Contracts` ports, not foreign module Domain or Application assemblies.

## Decision

1. **`*.Infrastructure.Persistence.*` DbConfig classes** may reference foreign module **Domain entity types** only for `HasOne<T>().WithMany().HasForeignKey(...)` configuration.
2. **Application layers** (`*.Application`) must not reference other modules' Domain assemblies; use `*.Contracts` DTOs and query/integration ports instead.
3. **Infrastructure services** outside `Persistence` must not reference foreign module Domain; use Contracts query ports (e.g. `IFinancialPeriodInventoryUsageQueries`).
4. Architecture tests enforce Application boundaries; DbConfig exception is documented here, not blanket-approved for all Infrastructure code.

## Consequences

- **Positive:** Clear rule for the only remaining intentional Domain cross-reference (EF mapping).
- **Positive:** Financial period usage queries moved to `Inventory.Contracts` + `Inventory.Infrastructure`.
- **Neutral:** Inventory.Infrastructure may still reference `Accounting.Domain` in `*DbConfig` only (existing ADR-007 pattern).

## Related

- [ADR-007](./007-inventory-domain-ef-decoupling.md)
- [ARCHITECTURE_ENHANCEMENT_PLAN.md](../ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 4
