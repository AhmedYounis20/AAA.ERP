# ADR-003: Synchronous integration for inventory GL posting

## Status

Accepted — July 2026

## Context

`InventoryTransactionService` previously built Accounting commands directly and called Accounting internals, coupling modules. Replaced by `IAccountingIntegrationService` (Phase 1); idempotent posting via `SourceDocumentId` (Phase 4.2.3).

## Decision

Introduce `IAccountingIntegrationService` in `Accounting.Contracts`. Inventory calls this facade **synchronously** within the same DB transaction for receipt, export, and transfer flows.

Use in-process domain events only for non-critical side effects (audit, cache invalidation).

## Consequences

- Phase 1 implementation required before enabling module boundary arch test
- GL posting logic consolidated in Accounting module
- Idempotency via `SourceDocumentId` on GL entries — **implemented** (Phase 4.2.3)
