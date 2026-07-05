# Staging / production deployment checklist (Phase 3)

Operator checklist for go-live after engineering Phases 0–7 and enhancement Phases 1–2.

**Related runbooks:**

- [SCHEMA_MIGRATION_RUNBOOK.md](./SCHEMA_MIGRATION_RUNBOOK.md) — `SeparateModuleSchemas`
- [PRODUCTION_LOGGING.md](./PRODUCTION_LOGGING.md) — Serilog verification
- [SMOKE_TEST_CHECKLIST.md](../architecture/SMOKE_TEST_CHECKLIST.md) — API smoke paths

---

## Pre-deploy

| Step | Action | Owner | Done |
|------|--------|-------|------|
| P1 | Full backup of target SQL Server database | DBA / Ops | [ ] |
| P2 | Deploy build that includes migration `20260705173310_SeparateModuleSchemas` | DevOps | [ ] |
| P3 | Set `ASPNETCORE_ENVIRONMENT=Production` (or Staging) on host | DevOps | [ ] |
| P4 | Connection string points to correct database | DevOps | [ ] |
| P5 | Secrets (`ApiSettings:Secret`, DB password) from vault — not committed | DevOps | [ ] |

---

## Staging — schema migration (task 3.1)

Follow [SCHEMA_MIGRATION_RUNBOOK.md](./SCHEMA_MIGRATION_RUNBOOK.md).

| Step | Action | Done |
|------|--------|------|
| S1 | Backup staging database | [ ] |
| S2 | `dotnet ef database update` against staging connection | [ ] |
| S3 | Verify schemas (`accounting`, `inventory`, `platform`, `identity`) — see runbook SQL | [ ] |
| S4 | Update ad-hoc SQL / reports to schema-qualified names ([SQL_SCHEMA_TABLE_INVENTORY.md](../architecture/SQL_SCHEMA_TABLE_INVENTORY.md)) | [ ] |

**Sign-off:** _________________ **Date:** _________

---

## Staging — smoke verification (task 3.3)

### Automated (from build agent or jump box)

```bash
dotnet test AAA.ERP.sln -c Release
```

| Suite | Expected |
|-------|----------|
| `ERP.Architecture.Tests` | All pass |
| `ERP.SmokeTests` | All pass (incl. Production `Dbwork` guard) |
| `ERP.IntegrationTests` | All pass (if SQL available) |

### Manual (against staging URL)

Use [SMOKE_TEST_CHECKLIST.md](../architecture/SMOKE_TEST_CHECKLIST.md) — Auth, Accounting sample CRUD, Inventory sample CRUD, MediatR create/update.

| Check | Done |
|-------|------|
| Login returns JWT | [ ] |
| Protected route without token → 401 | [ ] |
| `GET /api/Dbwork/export/account` → **404** (not Development) | [ ] |
| Inventory transfer requires auth | [ ] |
| Import transaction + GL posting (if test data exists) | [ ] |

**Sign-off:** _________________ **Date:** _________

---

## Production — schema migration (task 3.2)

Only after **staging sign-off**.

| Step | Action | Done |
|------|--------|------|
| R1 | Maintenance window communicated | [ ] |
| R2 | Production backup verified restorable | [ ] |
| R3 | Apply migration per runbook | [ ] |
| R4 | Repeat smoke checklist on production URL | [ ] |
| R5 | Verify production logs — JSON stdout, `CorrelationId` present ([PRODUCTION_LOGGING.md](./PRODUCTION_LOGGING.md)) | [ ] |

**Sign-off:** _________________ **Date:** _________

---

## Rollback

- Restore database from pre-migration backup ([SCHEMA_MIGRATION_RUNBOOK.md](./SCHEMA_MIGRATION_RUNBOOK.md)).
- Redeploy previous application version if needed.

---

## CI gates (engineering — no operator action)

| Gate | Location |
|------|----------|
| Build + full test suite | `.github/workflows/ci.yml` |
| Nullable warnings on changed `.cs` files (PR) | `scripts/check-nullable-on-changed-files.sh` |
