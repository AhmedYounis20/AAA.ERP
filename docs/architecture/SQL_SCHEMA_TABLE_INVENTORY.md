# SQL schema table inventory (4.2.2)

**Status:** Draft for review — July 2026  
**Source of truth:** `ERP.Host/Migrations/ApplicationDbContextModelSnapshot.cs` (HEAD `d65001f`)  
**Related:** [ADR-006](./adr/006-sql-schema-separation.md) (revised — Option B approved)

---

## Schema names

| Schema | Module | Notes |
|--------|--------|-------|
| `accounting` | Accounting | GL, sub-ledgers, shared `Attachments` |
| `inventory` | Inventory | Catalog, stock, transactions; junction tables stay here even when FK → `accounting` |
| `platform` | Platform.Audit | Cross-cutting audit log |
| `identity` | Identity | ASP.NET Identity (`AspNet*` tables) |
| `dbo` | EF / ops | `__EFMigrationsHistory` only — **unchanged** |

EF pattern (next slice): `builder.ToTable("Entries", "accounting");`

---

## Summary

| Schema | Tables |
|--------|--------|
| `accounting` | 21 |
| `inventory` | 45 |
| `platform` | 1 |
| `identity` | 7 |
| **Total mapped** | **74** |

---

## `accounting` (21)

| Table | DbConfig / configuration |
|-------|--------------------------|
| AccountGuides | `Modules/Accounting/Accounting.Infrastructure/Persistence/AccountGuideDbConfig.cs` |
| Attachments | `ERP.Persistence/DBConfiguration/Config/Account/Attachments/AttachmentDbConfig.cs` |
| Banks | `Modules/Accounting/Accounting.Infrastructure/Persistence/BankDbConfig.cs` |
| Branches | `Modules/Accounting/Accounting.Infrastructure/Persistence/BranchDbConfig.cs` |
| CashInBoxes | `Modules/Accounting/Accounting.Infrastructure/Persistence/CashInBoxDbConfig.cs` |
| ChartOfAccounts | `Modules/Accounting/Accounting.Infrastructure/Persistence/ChartOfAccountDbConfig.cs` |
| CollectionBooks | `Modules/Accounting/Accounting.Infrastructure/Persistence/CollectionBookDbConfig.cs` |
| CostCenterChartOfAccounts | `Modules/Accounting/Accounting.Infrastructure/Persistence/CostCenterChartOfAccountDbConfig.cs` |
| CostCenters | `Modules/Accounting/Accounting.Infrastructure/Persistence/CostCenterDbConfig.cs` |
| Currencies | `Modules/Accounting/Accounting.Infrastructure/Persistence/CurrencyDbConfig.cs` |
| Customers | `Modules/Accounting/Accounting.Infrastructure/Persistence/CustomerDbConfig.cs` |
| Entries | `Modules/Accounting/Accounting.Infrastructure/Persistence/EntryDbConfig.cs` |
| EntryAttachments | `Modules/Accounting/Accounting.Infrastructure/Persistence/EntryAttachmentDbConfig.cs` |
| EntryCostCenters | `Modules/Accounting/Accounting.Infrastructure/Persistence/EntryCostCenterDbConfig.cs` |
| FinancialPeriods | `Modules/Accounting/Accounting.Infrastructure/Persistence/FinancialPeriodDbConfig.cs` |
| FinancialTransactions | `Modules/Accounting/Accounting.Infrastructure/Persistence/FinancialTransactionDbConfig.cs` |
| FixedAssets | `Modules/Accounting/Accounting.Infrastructure/Persistence/FixedAssetDbConfig.cs` |
| GLSettings | `Modules/Accounting/Accounting.Infrastructure/Persistence/GLSettingDbConfig.cs` |
| Roles | `Modules/Accounting/Accounting.Infrastructure/Persistence/RoleDbConfig.cs` |
| Suppliers | `Modules/Accounting/Accounting.Infrastructure/Persistence/SupplierDbConfig.cs` |
| Taxes | `Modules/Accounting/Accounting.Infrastructure/Persistence/TaxDbConfig.cs` |

> **Note:** `Roles` is the accounting business-role aggregate, not ASP.NET Identity (`identity.AspNetRoles`).

---

## `inventory` (45)

| Table | DbConfig / configuration |
|-------|--------------------------|
| AttributeDefinitions | `Modules/Inventory/Inventory.Infrastructure/Persistence/AttributeDefinitionDbConfig.cs` |
| AttributeValues | `Modules/Inventory/Inventory.Infrastructure/Persistence/AttributeValueDbConfig.cs` |
| Colors | `Modules/Inventory/Inventory.Infrastructure/Persistence/ColorDbConfig.cs` |
| ExpiryLevels | `Modules/Inventory/Inventory.Infrastructure/Persistence/ExpiryLevels/ExpiryLevelDbConfig.cs` |
| InventoryThresholds | `Modules/Inventory/Inventory.Infrastructure/Persistence/InventoryThresholds/InventoryThresholdDbConfig.cs` |
| InventoryTransactionItems | `Modules/Inventory/Inventory.Infrastructure/Persistence/InventoryTransactions/InventoryTransactionItemDbConfig.cs` |
| InventoryTransactions | `Modules/Inventory/Inventory.Infrastructure/Persistence/InventoryTransactions/InventoryTransactionDbConfig.cs` |
| InventoryTransferItems | `Modules/Inventory/Inventory.Infrastructure/Persistence/InventoryTransfers/InventoryTransferItemDbConfig.cs` |
| InventoryTransfers | `Modules/Inventory/Inventory.Infrastructure/Persistence/InventoryTransfers/InventoryTransferDbConfig.cs` |
| ItemCodes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemCodeDbConfig.cs` |
| ItemManufacturerCompanies | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemManufacturerCompanyDbConfig.cs` |
| ItemPackingUnits | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemPackingUnitDbConfig.cs` |
| ItemPackingUnitSellingPrices | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemPackingUnitSellingPriceDbConfig.cs` |
| ItemSellingPriceDiscounts | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemSellingPriceDiscountDbConfig.cs` |
| Items | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemDbConfig.cs` |
| ItemSuppliers | `Modules/Inventory/Inventory.Infrastructure/Persistence/Items/ItemSupplierDbConfig.cs` |
| ManufacturerCompanies | `Modules/Inventory/Inventory.Infrastructure/Persistence/ManufacturerCompanyDbConfig.cs` |
| PackingUnits | `Modules/Inventory/Inventory.Infrastructure/Persistence/PackingUnitDbConfig.cs` |
| ProductAttachments | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductAttachmentDbConfig.cs` |
| ProductAttributeDefinitions | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductAttributeDefinitionDbConfig.cs` |
| ProductCodes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductCodeDbConfig.cs` |
| ProductCostCenters | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductCostCenterDbConfig.cs` |
| ProductManufacturerCompanies | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductManufacturerCompanyDbConfig.cs` |
| ProductPackingUnits | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductPackingUnitDbConfig.cs` — **add explicit `ToTable` in 4.2.2 slice** |
| ProductPackingUnitSellingPrices | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductPackingUnitSellingPriceDbConfig.cs` |
| ProductPurchaseTaxes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductPurchaseTaxDbConfig.cs` |
| Products | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductDbConfig.cs` |
| ProductSalesTaxes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductSalesTaxDbConfig.cs` |
| ProductSellingPriceDiscounts | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductSellingPriceDiscountDbConfig.cs` — **add explicit `ToTable` in 4.2.2 slice** |
| ProductSuppliers | `Modules/Inventory/Inventory.Infrastructure/Persistence/Products/ProductSupplierDbConfig.cs` |
| SellingPrices | `Modules/Inventory/Inventory.Infrastructure/Persistence/SellingPriceDbConfig.cs` |
| Sizes | `Modules/Inventory/Inventory.Infrastructure/Persistence/SizeDbConfig.cs` |
| StockBalances | `Modules/Inventory/Inventory.Infrastructure/Persistence/StockBalances/StockBalanceDbConfig.cs` |
| VariantAttachments | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantAttachmentDbConfig.cs` |
| VariantAttributeValues | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantAttributeValueDbConfig.cs` |
| VariantCodes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantCodeDbConfig.cs` |
| VariantCostCenters | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantCostCenterDbConfig.cs` |
| VariantManufacturerCompanies | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantManufacturerCompanyDbConfig.cs` |
| VariantPackingUnits | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantPackingUnitDbConfig.cs` |
| VariantPackingUnitSellingPrices | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantPackingUnitSellingPriceDbConfig.cs` |
| VariantPurchaseTaxes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantPurchaseTaxDbConfig.cs` |
| Variants | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantDbConfig.cs` |
| VariantSalesTaxes | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantSalesTaxDbConfig.cs` |
| VariantSellingPriceDiscounts | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantSellingPriceDiscountDbConfig.cs` |
| VariantSuppliers | `Modules/Inventory/Inventory.Infrastructure/Persistence/Variants/VariantSupplierDbConfig.cs` |

---

## `platform` (1)

| Table | DbConfig / configuration |
|-------|--------------------------|
| AuditLogs | `Modules/Platform/Platform.Audit/Platform.Audit.Infrastructure/Persistence/AuditLogDbConfig.cs` |

---

## `identity` (7)

Configured by ASP.NET Identity via `IdentityDbContext<ApplicationUser>` in `ApplicationDbContext` — no module `*DbConfig` files. Next slice: override in `OnModelCreating` or a dedicated `IdentityDbConfig` in `Identity.Infrastructure` / `ERP.Persistence`.

| Table | Entity |
|-------|--------|
| AspNetRoles | Identity role |
| AspNetRoleClaims | Identity role claim |
| AspNetUserClaims | Identity user claim |
| AspNetUserLogins | Identity external login |
| AspNetUserRoles | Identity user–role join |
| AspNetUsers | `Shared.BaseEntities.Identity.ApplicationUser` |
| AspNetUserTokens | Identity token |

---

## Cross-schema foreign keys (inventory → accounting)

SQL Server allows these within one database. Junction tables remain in `inventory`.

| Inventory table | FK column(s) | Accounting target |
|-----------------|--------------|---------------------|
| ItemSuppliers | SupplierId | Suppliers |
| ProductSuppliers | SupplierId | Suppliers |
| VariantSuppliers | SupplierId | Suppliers |
| ProductCostCenters | CostCenterId | CostCenters |
| VariantCostCenters | CostCenterId | CostCenters |
| ProductPurchaseTaxes | TaxId | Taxes |
| ProductSalesTaxes | TaxId | Taxes |
| VariantPurchaseTaxes | TaxId | Taxes |
| VariantSalesTaxes | TaxId | Taxes |
| EntryAttachments (accounting) | AttachmentId | Attachments — same schema, listed for completeness |

Inventory transaction / transfer headers reference `Branches` (accounting) by `BranchId` — FK spans schemas.

---

## Out of scope (this migration)

| Item | Reason |
|------|--------|
| Historical migration `.Designer.cs` entity type strings | Explicitly deferred per M4 sign-off |
| API routes / JSON wire formats | Stability contract |
| Namespace refactors | Separate slices |
| `__EFMigrationsHistory` schema | Stays in `dbo` |

---

## Next slice checklist (4.2.2 implementation)

- [x] Add shared schema name constants — `DbSchemaNames` in `ERP.BuildingBlocks.Persistence`
- [x] Update all `*DbConfig` `ToTable(name)` → `ToTable(name, schema)`
- [x] Add explicit `ToTable` for `ProductPackingUnits`, `ProductSellingPriceDiscounts`
- [x] Configure Identity `AspNet*` tables → `identity` schema
- [x] EF migration: `20260705173310_SeparateModuleSchemas` (`EnsureSchema` + `RenameTable`; no `DropTable`)
- [ ] Apply migration on dev DB: `dotnet ef database update --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj`
- [x] Update [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md) §10.6 and [M4_SIGNOFF.md](./M4_SIGNOFF.md)

---

## Rollback plan (implementation slice)

- **Applied migration:** run `dotnet ef database update <PreviousMigration>` — Down() uses `RenameTable` back to `dbo`.
- **Not yet applied:** `dotnet ef migrations remove` and revert DbConfig changes.
- Test rollback on a DB copy before applying to shared dev/staging.
