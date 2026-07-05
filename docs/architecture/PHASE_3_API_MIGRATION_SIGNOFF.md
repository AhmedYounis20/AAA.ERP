# Phase 3 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M3 — Accounting API extraction

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 3.1 | Create `Accounting.Api` project | ✅ |
| 3.2 | Move 21 Accounting controllers from Host | ✅ |
| 3.3 | Wire `AddApplicationPart(AccountingApiAssemblyMarker)` | ✅ |
| 3.4 | Keep `DbWorkController` in Host (Persistence dependency) | ✅ |
| 3.5 | Update arch test ratchet: Host controllers **15** | ✅ |
| 3.6 | Full test suite green | ✅ |

---

## Controller mapping

| Feature folder | Controllers moved |
|----------------|-------------------|
| `ChartOfAccounts/` | AccountGuides, ChartOfAccounts |
| `Settings/` | CollectionBooks, CostCenters, Currencies, FinancialPeriods, GLSettings, Roles |
| `SubLedger/` | Banks, Branches, CashInBoxes, Customers, FixedAssets, Suppliers, Taxes |
| `Entries/` | Combined, Entries, Journal, Opening, Payment, Receipt |

**Remains in Host:** `DbWorkController` (dev seed tooling → `ERP.Persistence`)

---

## Known exception

`Accounting.Api` references `Accounting.Domain` for generic CRUD base controller type parameters (`BaseSettingController<TEntity, ...>`). Arch test `Accounting_Api_may_reference_Accounting_Domain_for_CRUD_generics_only` ensures only controllers depend on Domain.

---

## Exit criteria

- [x] Zero accounting business controllers under `ERP.Host/Controllers/Accounting/` (except DbWork)
- [x] API routes unchanged
- [x] Host business controller count = 15 (14 Inventory + 1 DbWork)
- [x] Architecture + smoke + integration tests pass

---

## Next phase

**Phase 4** — Inventory API extraction (14 controllers → `Inventory.Api`)
