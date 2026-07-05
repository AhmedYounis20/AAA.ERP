# API smoke test checklist (Phase 0 + Phase 3 ops)

## Automated (CI)

Run without SQL Server using in-memory EF:

```bash
dotnet test Tests/ERP.SmokeTests/ERP.SmokeTests.csproj
```

| Check | Test |
|-------|------|
| OpenAPI document loads | `OpenApi_document_is_available` |
| Protected routes return 401 | `Protected_endpoints_require_authentication` |
| Auth login route responds | `Auth_login_endpoint_is_reachable` |
| Correlation ID header | `Responses_include_correlation_id_header` |
| Module Swagger filtering | `Module_swagger_documents_filter_endpoints_by_module` |
| Dbwork blocked in Production | `Dbwork_endpoints_return_not_found_outside_development` |

Full solution (CI):

```bash
dotnet test AAA.ERP.sln -c Release
```

## Staging / production (with live database)

Use after schema migration — see [STAGING_DEPLOYMENT_CHECKLIST.md](../ops/STAGING_DEPLOYMENT_CHECKLIST.md).

Use Swagger UI (`/swagger` when API is running) or Postman after deploy.

### Auth

- [ ] `POST /api/Auth/login` — valid credentials return token
- [ ] `GET /api/Currencies` without token — 401

### Security (Phase 2–3)

- [ ] `GET /api/Dbwork/export/account` on **staging/production** — **404** (not Development)
- [ ] `GET /api/InventoryTransfer` without token — 401

### Accounting CRUD (sample)

- [ ] `GET /api/Currencies` — list
- [ ] `GET /api/ChartOfAccounts` — tree/list
- [ ] `GET /api/FinancialPeriods` — list
- [ ] `GET /api/Taxes` — list

### Inventory CRUD (sample)

- [ ] `GET /api/Products` — paginated list
- [ ] `GET /api/Variants` — list
- [ ] `GET /api/Colors` — list
- [ ] `GET /api/StockBalances` — query

### Regression after MediatR consolidation

- [ ] `POST /api/Taxes` — create via MediatR handler
- [ ] `POST /api/Colors` — create via MediatR handler
- [ ] `PUT /api/Colors/{id}` — update via MediatR handler

### Logging (Production)

- [ ] Log lines are JSON (`CompactJsonFormatter`) — see [PRODUCTION_LOGGING.md](../ops/PRODUCTION_LOGGING.md)
- [ ] `X-Correlation-Id` appears in response and log context

**Sign-off:** Automated smoke tests pass in CI. Complete manual checklist on **staging** before production ([STAGING_DEPLOYMENT_CHECKLIST.md](../ops/STAGING_DEPLOYMENT_CHECKLIST.md)).
