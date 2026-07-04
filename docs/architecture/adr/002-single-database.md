# ADR-002: Single database with composed DbContext

## Status

Accepted — July 2026

## Context

Inventory transactions must post GL entries atomically. Split databases would require distributed transactions or eventual consistency.

## Decision

Keep **one SQL Server database**. Each future module owns EF `IEntityTypeConfiguration` classes; Host/Persistence owns migrations.

Optional SQL schemas (`accounting.*`, `inventory.*`) may be introduced in Phase 4 after modules are stable in code.

## Consequences

- Simple operations and reporting
- Cross-module transactions via shared scoped `DbContext`
- Module extraction to separate DB requires explicit ADR and saga/outbox design
