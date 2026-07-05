# ADR-012: Hybrid CQRS — MediatR for actions, read services for GET

## Status

Accepted — July 2026

## Context

Controllers inject both `ISender` (MediatR) and `I*Service`, with inconsistent write paths (Create/Update via MediatR, Delete via service). Pass-through command handlers delegate to services without owning business logic.

ADR-004 established MediatR as the write path but did not define the read side or controller dependency rules.

## Decision

Adopt **hybrid CQRS** as the official application-layer pattern:

| HTTP / intent | Application path | Controller may inject |
|---------------|------------------|------------------------|
| POST, PUT, PATCH, DELETE | `ISender.Send(command)` → command handler | `ISender` |
| GET (list, detail, paginate, lookups, custom reads) | `I{Aggregate}ReadService` or `I{Aggregate}Queries` | Read service / queries only |

**Rules:**

- Controllers must **not** inject write services (`I*Service` with Create/Update/Delete).
- Command handlers own write orchestration and business rules (especially P0/P1 aggregates).
- Read services return DTOs or entities via `ApiResponse<T>`; no MediatR for simple GET lists.
- Cross-module writes use `*.Contracts` integration ports only.

**Registration:**

- MediatR: single point in `ERP.Host/Utilities/WebBuilderExtensions.cs` (per ADR-004).
- Read services: `{Module}.Infrastructure` via `Add{Module}Module()`.

## Consequences

- New features follow [HYBRID_CQRS_PLAN.md](../HYBRID_CQRS_PLAN.md).
- Pilots: Tax (Accounting), Color (Inventory) in Sprint 1.
- `BaseController` dual DI is transitional; retirement tracked in Sprint 11.
- Optional MediatR queries for hot reads remain backlog only.

## Related

- [ADR-004](./004-mediatr-single-write-path.md)
- [HYBRID_CQRS_PLAN.md](../HYBRID_CQRS_PLAN.md)
