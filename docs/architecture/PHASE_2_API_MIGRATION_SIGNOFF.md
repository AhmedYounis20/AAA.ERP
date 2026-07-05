# Phase 2 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M2 — Pilot API extraction (Identity + Platform)

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 2.1 | Move `AuthController` → `Identity.Api/Controllers/` | ✅ |
| 2.2 | Create `Platform.Audit.Api` + move `AuditLogsController` | ✅ |
| 2.3 | Wire `AddApplicationPart` for both Api assemblies in Host | ✅ |
| 2.4 | Delete controllers from `ERP.Host/Controllers/Identity` and `Audit` | ✅ |
| 2.5 | Update arch test ratchet baseline: Host controllers **36** (was 38) | ✅ |
| 2.6 | Smoke + integration tests pass | ✅ |

---

## Exit criteria

- [x] Identity + Platform audit APIs served from module Api projects
- [x] API routes unchanged (`/api/Auth`, `/api/AuditLogs`)
- [x] Host business controller count = 36
- [x] Full test suite green

---

## Next phase

**Phase 3** — Accounting API extraction (22 controllers → `Accounting.Api`)
