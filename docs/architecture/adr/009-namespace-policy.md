# ADR-009: Namespace mapping policy

## Status

**Approved** — July 2026 (Enhancement Plan Phase 6)

## Context

Physical projects were migrated to `ERP.BuildingBlocks.*` and `{Module}.*` layouts, but many types still declared legacy `Shared.*` namespaces. This drift made assembly boundaries harder to read and allowed new code to copy obsolete patterns.

Host controllers under `Controllers/Account/` used a folder name that did not match the accounting module vocabulary.

## Decision

Retire `Shared.*` namespaces. Use the mapping below for all namespace declarations and `using` statements.

| Legacy namespace | Target namespace |
|------------------|------------------|
| `Shared.BaseEntities` | `ERP.BuildingBlocks.Domain.Entities` |
| `Shared.DTOs` | `ERP.BuildingBlocks.Application.DTOs` |
| `Shared.Responses` | `ERP.BuildingBlocks.Application.Responses` |
| `Shared.Behaviors` | `ERP.BuildingBlocks.Application.Behaviors` |
| `Shared.Exceptions` | `ERP.BuildingBlocks.Application.Exceptions` |
| `Shared.Constants` | `ERP.BuildingBlocks.Application.Constants` |
| `Shared.Extensions` | `ERP.BuildingBlocks.Application.Extensions` |
| `ERP.Shared.BaseEntities.Attachments` | `ERP.BuildingBlocks.Domain.Entities.Attachments` |

Additional rules:

1. **BuildingBlocks kernel** — shared primitives live in `ERP.BuildingBlocks.Domain` or `ERP.BuildingBlocks.Application` only.
2. **Modules** — module code uses `{Module}.{Layer}.*` (e.g. `Accounting.Application`, `Inventory.Domain`).
3. **Host** — `ERP.Host.Controllers.Accounting.*` replaces `ERP.Host.Controllers.Account.*`; `[Route]` attributes stay unchanged for API compatibility.
4. **ERP.Shared project** — source merged into BuildingBlocks; project references updated accordingly.
5. **Migrations** — existing EF migration snapshots are excluded from bulk renames; new migrations pick up updated entity namespaces.

## Consequences

- **Positive:** One namespace per physical project; architecture tests can block new `Shared.*` declarations.
- **Positive:** Attachment DTOs, constants, extensions, and exception handler colocated with other kernel types.
- **Neutral:** `Shared.BaseEntities.Identity` becomes `ERP.BuildingBlocks.Domain.Entities.Identity` until Identity domain namespace is refined in a later phase.
- **Negative:** Large diff touching most modules; requires full-solution build verification.

## Related

- [ARCHITECTURE_ENHANCEMENT_PLAN.md](../ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 6
- [ADR-001](./001-adopt-modular-monolith.md)
