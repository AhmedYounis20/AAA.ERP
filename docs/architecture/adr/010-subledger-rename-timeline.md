# ADR-010: SubLeadger → SubLedger rename timeline

**Status:** Accepted  
**Date:** July 2026  
**Context:** Phase 8 naming debt — `SubLeadger` is a longstanding typo across domain types, folders, validators, API routes, and seed data.

## Decision

Retain `SubLeadger` spellings for **API-stable** surfaces (routes, JSON property names, database table/column names) through **v1.x**.

Introduce correct `SubLedger` naming only for **new** types and internal documentation. Full mechanical rename is scheduled for **v2.0** with:

1. Database migration (table renames only if columns change — table names may stay for zero-downtime).
2. `[Obsolete]` type aliases in `Accounting.Domain` for one release cycle.
3. Client notification via API deprecation headers on `/api/SubLeadgers/*` routes.

## Rationale

- ~150+ references including migrations, localization `.resx`, and `ChartOfAccounts.json` seed data.
- Breaking rename without shims risks production client regressions.
- `ComandValidators` → `CommandValidators` and `DbworkController` → `DbWorkController` are completed (no wire impact or dev-only).

## Completed in Phase 8

| Item | Status |
|------|--------|
| `ComandValidators` → `CommandValidators` | Done — folders and namespaces |
| `DbworkController` → `DbWorkController` | Done — dev-only route now `api/DbWork` |
| `SubLeadger` → `SubLedger` | Tracked here; v2.0 |

## Enforcement

`NamespaceDebtTests` continues to guard against new `Compined` typos. No new `SubLeadger`-pattern typos in **new** public type names.
