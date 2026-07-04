# ADR-001: Adopt modular monolith

## Status

Accepted — July 2026

## Context

AAA.ERP is a layered monolith with growing coupling between Accounting and Inventory (God `IUnitOfWork`, cross-module entity access). The system requires ACID transactions across inventory and GL posting.

## Decision

Evolve in place to a **modular monolith**: one deployable, one database, explicit module boundaries enforced by architecture tests and contracts projects.

Do **not** adopt microservices at this stage.

## Consequences

- Incremental migration over ~9 months (0.5 FTE) without API breaking changes
- Architecture tests in CI from Phase 0
- Future option to extract modules if business scale requires it
