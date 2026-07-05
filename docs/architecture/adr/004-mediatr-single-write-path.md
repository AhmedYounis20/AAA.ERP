# ADR-004: MediatR as sole write path

## Status

Accepted — July 2026

## Context

The codebase uses both MediatR handlers and direct service methods for writes. MediatR was registered in two places (`WebBuilderExtensions` and `InfrasctructureExtensions`).

## Decision

- **Commands (writes):** Controller → `ISender.Send(command)` → Handler
- **MediatR registration:** Single point in `ERP.Host/Utilities/WebBuilderExtensions.cs`
- **Validators:** Registered from `ERP.Application` assembly
- **Handlers:** Registered from `ERP.Infrastracture` assembly

New features should use handlers; existing read services may remain until migrated to queries.

## Consequences

- Phase 0 removes duplicate MediatR and duplicate `ValidationBehavior` registration
- Duplicate DI service registrations cleaned in `InfrasctructureExtensions`
