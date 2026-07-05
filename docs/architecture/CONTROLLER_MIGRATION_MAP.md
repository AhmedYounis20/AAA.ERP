# Controller migration map

Maps every business controller from `ERP.Host` to its target `{Module}.Api` location.  
**Status:** Phase 0 complete — map defined; migration starts Phase 2.  
**ADR:** [011-module-owned-api.md](./adr/011-module-owned-api.md)

---

## Summary

| Module | Controllers | Target project |
|--------|-------------|----------------|
| Accounting | 22 | `Accounting.Api` |
| Inventory | 14 | `Inventory.Api` |
| Identity | 1 | `Identity.Api` |
| Platform (Audit) | 1 | `Platform.Audit.Api` |
| **Total** | **38** | |

**Moved in Phase 0:** Base CRUD controllers → `ERP.BuildingBlocks.Api/Controllers/` (not counted above).

---

## Accounting → `Accounting.Api`

| # | Host path | Target path | Feature |
|---|-----------|-------------|---------|
| 1 | `Controllers/Accounting/AccountGuidesController.cs` | `Controllers/ChartOfAccounts/AccountGuidesController.cs` | ChartOfAccounts |
| 2 | `Controllers/Accounting/ChartOfAccountsController.cs` | `Controllers/ChartOfAccounts/ChartOfAccountsController.cs` | ChartOfAccounts |
| 3 | `Controllers/Accounting/CollectionBooksController.cs` | `Controllers/Settings/CollectionBooksController.cs` | Settings |
| 4 | `Controllers/Accounting/CostCentersController.cs` | `Controllers/Settings/CostCentersController.cs` | Settings |
| 5 | `Controllers/Accounting/CurrenciesController.cs` | `Controllers/Settings/CurrenciesController.cs` | Settings |
| 6 | `Controllers/Accounting/FinancialPeriodsController.cs` | `Controllers/Settings/FinancialPeriodsController.cs` | Settings |
| 7 | `Controllers/Accounting/GLSettingsController.cs` | `Controllers/Settings/GLSettingsController.cs` | Settings |
| 8 | `Controllers/Accounting/RolesController.cs` | `Controllers/Settings/RolesController.cs` | Settings |
| 9 | `Controllers/Accounting/SubLeadgers/BanksController.cs` | `Controllers/SubLedger/BanksController.cs` | SubLedger |
| 10 | `Controllers/Accounting/SubLeadgers/BranchesController.cs` | `Controllers/SubLedger/BranchesController.cs` | SubLedger |
| 11 | `Controllers/Accounting/SubLeadgers/CashInBoxesController.cs` | `Controllers/SubLedger/CashInBoxesController.cs` | SubLedger |
| 12 | `Controllers/Accounting/SubLeadgers/CustomersController.cs` | `Controllers/SubLedger/CustomersController.cs` | SubLedger |
| 13 | `Controllers/Accounting/SubLeadgers/FixedAssetsController.cs` | `Controllers/SubLedger/FixedAssetsController.cs` | SubLedger |
| 14 | `Controllers/Accounting/SubLeadgers/SuppliersController.cs` | `Controllers/SubLedger/SuppliersController.cs` | SubLedger |
| 15 | `Controllers/Accounting/SubLeadgers/TaxesController.cs` | `Controllers/SubLedger/TaxesController.cs` | SubLedger |
| 16 | `Controllers/Accounting/Entries/CombinedEntriesController.cs` | `Controllers/Entries/CombinedEntriesController.cs` | Entries |
| 17 | `Controllers/Accounting/Entries/EntriesController.cs` | `Controllers/Entries/EntriesController.cs` | Entries |
| 18 | `Controllers/Accounting/Entries/JournalEntriesController.cs` | `Controllers/Entries/JournalEntriesController.cs` | Entries |
| 19 | `Controllers/Accounting/Entries/OpeningEntriesController.cs` | `Controllers/Entries/OpeningEntriesController.cs` | Entries |
| 20 | `Controllers/Accounting/Entries/PaymentEntriesController.cs` | `Controllers/Entries/PaymentEntriesController.cs` | Entries |
| 21 | `Controllers/Accounting/Entries/ReceiptEntriesController.cs` | `Controllers/Entries/ReceiptEntriesController.cs` | Entries |
| 22 | `Controllers/Accounting/DbWorkController.cs` | `Controllers/Dev/DbWorkController.cs` | Dev (keep `[DevelopmentOnly]`) |

---

## Inventory → `Inventory.Api`

| # | Host path | Target path | Feature |
|---|-----------|-------------|---------|
| 1 | `Controllers/Inventory/AttributeDefinitionsController.cs` | `Controllers/Catalog/AttributeDefinitionsController.cs` | Catalog |
| 2 | `Controllers/Inventory/AttributeValuesController.cs` | `Controllers/Catalog/AttributeValuesController.cs` | Catalog |
| 3 | `Controllers/Inventory/ColorsController.cs` | `Controllers/Catalog/ColorsController.cs` | Catalog |
| 4 | `Controllers/Inventory/ItemsController.cs` | `Controllers/Catalog/ItemsController.cs` | Catalog |
| 5 | `Controllers/Inventory/ManufacturerCompaniesController.cs` | `Controllers/Catalog/ManufacturerCompaniesController.cs` | Catalog |
| 6 | `Controllers/Inventory/PackingUnitsController.cs` | `Controllers/Catalog/PackingUnitsController.cs` | Catalog |
| 7 | `Controllers/Inventory/ProductsController.cs` | `Controllers/Catalog/ProductsController.cs` | Catalog |
| 8 | `Controllers/Inventory/SizesController.cs` | `Controllers/Catalog/SizesController.cs` | Catalog |
| 9 | `Controllers/Inventory/VariantsController.cs` | `Controllers/Catalog/VariantsController.cs` | Catalog |
| 10 | `Controllers/Inventory/SellingPricesController.cs` | `Controllers/Pricing/SellingPricesController.cs` | Pricing |
| 11 | `Controllers/Inventory/StockBalancesController.cs` | `Controllers/Stock/StockBalancesController.cs` | Stock |
| 12 | `Controllers/Inventory/ExportTransactionsController.cs` | `Controllers/Transactions/ExportTransactionsController.cs` | Transactions |
| 13 | `Controllers/Inventory/ImportTransactionsController.cs` | `Controllers/Transactions/ImportTransactionsController.cs` | Transactions |
| 14 | `Controllers/Inventory/InventoryTransferController.cs` | `Controllers/Transactions/InventoryTransferController.cs` | Transactions |

---

## Identity → `Identity.Api`

| # | Host path | Target path | Feature |
|---|-----------|-------------|---------|
| 1 | `Controllers/Identity/AuthController.cs` | `Controllers/AuthController.cs` | Auth |

---

## Platform → `Platform.Audit.Api`

| # | Host path | Target path | Feature |
|---|-----------|-------------|---------|
| 1 | `Controllers/Audit/AuditLogsController.cs` | `Controllers/AuditLogsController.cs` | Audit |

---

## Migration waves (planned)

| Phase | Scope | Expected Host count after |
|-------|-------|---------------------------|
| Phase 0 | Base controllers → BuildingBlocks.Api | 38 |
| Phase 2 | Identity + Platform (2 controllers) | 36 |
| Phase 3 | Accounting (22 controllers) | 14 |
| Phase 4 | Inventory (14 controllers) | 0 |
| Phase 5 | Remove empty `ERP.Host/Controllers/` | 0 |

Update `ModuleApiLayerTests.Host_business_controller_count_baseline_for_ratchet` after each wave.
