# Phase 5 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M5 — Host slim-down (composition root only)

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 5.1 | Move `DbWorkController` → `ERP.Host/Dev/` (`ERP.Host.Dev`) | ✅ |
| 5.2 | Remove empty `ERP.Host/Controllers/` tree | ✅ |
| 5.3 | Slim `ERP.Host/GlobalUsing.cs` (remove module controller globals) | ✅ |
| 5.4 | Enable `Host_should_not_contain_business_controllers` arch test | ✅ |
| 5.5 | Ratchet baseline: **0** controllers under `ERP.Host.Controllers` | ✅ |
| 5.6 | Update logging unit-test stubs to `*.Api.Controllers` namespaces | ✅ |
| 5.7 | Full test suite green | ✅ |

---

## Host layout after Phase 5

```
ERP.Host/
  Dev/DbWorkController.cs     # [DevelopmentOnly] seed import/export
  Security/DevelopmentOnlyAttribute.cs
  Utilities/                  # composition root wiring (AddApplicationPart, Swagger, etc.)
```

**Business HTTP APIs:** `Accounting.Api`, `Inventory.Api`, `Identity.Api`, `Platform.Audit.Api`

---

## Known exceptions

- `ERP.Host.Dev.DbWorkController` references `ERP.Persistence` / `ApplicationDbContext` — exempt; guarded by `[DevelopmentOnly]`
- `Inventory.Api` → `Accounting.Application` for `IBaseQueryService<,>` (unchanged from Phase 4)

---

## Module API migration complete

Phases 0–5 deliver module-owned APIs with Host as composition root only. Optional follow-ups: Phase 6 hardening, `MODULAR_MONOLITH.md` diagram refresh, retire `ERP.Persistence` coupling in DbWork.
