# API smoke test checklist (Phase 0 — task 0.2.4)

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

## Manual (with live database)

Use Swagger UI (`/` when API is running) or Postman after `dotnet run --project ERP.API`.

### Auth

- [ ] `POST /api/Auth/login` — valid credentials return token
- [ ] `GET /api/Currencies` without token — 401

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

**Sign-off:** Automated smoke tests pass in CI. Manual checklist optional before production deploy.

**Last automated run:** Phase 0 completion — see CI workflow.
