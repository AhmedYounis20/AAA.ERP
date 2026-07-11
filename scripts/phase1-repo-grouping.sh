#!/usr/bin/env bash
# Phase 1 — group flat Repositories/Services into feature subfolders (paths only; namespaces unchanged).
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

move_file() {
  local src="$1" dest_dir="$2"
  [ -f "$src" ] || return 0
  mkdir -p "$dest_dir"
  git mv "$src" "$dest_dir/" 2>/dev/null || mv "$src" "$dest_dir/"
}

echo "=== Accounting.Application Repositories ==="
ACC_APP_REPO=Modules/Accounting/Accounting.Application/Repositories
for f in IEntryRepository.cs IJournalEntryService.cs; do :; done
for base in Entry Journal Payment Receipt Opening Combined Complex; do
  move_file "$ACC_APP_REPO/I${base}EntryRepository.cs" "$ACC_APP_REPO/Entries" 2>/dev/null || true
done
move_file "$ACC_APP_REPO/IEntryRepository.cs" "$ACC_APP_REPO/Entries"
for f in IChartOfAccountRepository.cs IAccountGuideRepository.cs; do
  move_file "$ACC_APP_REPO/$f" "$ACC_APP_REPO/ChartOfAccounts"
done
for f in ICurrencyRepository.cs ICostCenterRepository.cs IFinancialPeriodRepository.cs \
  IGLSettingRepository.cs ICollectionBookRepository.cs IRoleRepository.cs; do
  move_file "$ACC_APP_REPO/$f" "$ACC_APP_REPO/Settings"
done
for f in IBankRepository.cs IBranchRepository.cs ICashInBoxRepository.cs ICustomerRepository.cs \
  IFixedAssetRepository.cs ITaxRepository.cs; do
  move_file "$ACC_APP_REPO/$f" "$ACC_APP_REPO/SubLedger"
done

echo "=== Accounting.Application Services ==="
ACC_APP_SVC=Modules/Accounting/Accounting.Application/Services
for base in Entry Journal Payment Receipt Opening Combined Complex; do
  move_file "$ACC_APP_SVC/I${base}EntryService.cs" "$ACC_APP_SVC/Entries" 2>/dev/null || true
done
move_file "$ACC_APP_SVC/IEntryService.cs" "$ACC_APP_SVC/Entries"
for f in IChartOfAccountService.cs IAccountGuideService.cs; do
  move_file "$ACC_APP_SVC/$f" "$ACC_APP_SVC/ChartOfAccounts"
done
for f in ICurrencyService.cs ICostCenterService.cs IFinancialPeriodService.cs \
  IGLSettingService.cs ICollectionBookService.cs IRoleService.cs; do
  move_file "$ACC_APP_SVC/$f" "$ACC_APP_SVC/Settings"
done
for f in IBankService.cs IBranchService.cs ICashInBoxService.cs ICustomerService.cs \
  IFixedAssetService.cs ISupplierService.cs ITaxService.cs; do
  move_file "$ACC_APP_SVC/$f" "$ACC_APP_SVC/SubLedger"
done

echo "=== Accounting.Infrastructure Repositories ==="
ACC_INFRA_REPO=Modules/Accounting/Accounting.Infrastructure/Repositories
for base in Entry Journal Payment Receipt Opening Combined Complex; do
  move_file "$ACC_INFRA_REPO/${base}EntryRepository.cs" "$ACC_INFRA_REPO/Entries" 2>/dev/null || true
done
move_file "$ACC_INFRA_REPO/EntryRepository.cs" "$ACC_INFRA_REPO/Entries"
for f in ChartOfAccountRepository.cs AccountGuideRepository.cs; do
  move_file "$ACC_INFRA_REPO/$f" "$ACC_INFRA_REPO/ChartOfAccounts"
done
for f in CurrencyRepository.cs CostCenterRepository.cs FinancialPeriodRepository.cs \
  GLSettingRepository.cs CollectionBookRepository.cs RoleRepository.cs; do
  move_file "$ACC_INFRA_REPO/$f" "$ACC_INFRA_REPO/Settings"
done
for f in BankRepository.cs BranchRepository.cs CashInBoxRepository.cs CustomerRepository.cs \
  FixedAssetRepository.cs SupplierRepository.cs TaxRepository.cs BaseSubLeadgerRepository.cs; do
  move_file "$ACC_INFRA_REPO/$f" "$ACC_INFRA_REPO/SubLedger"
done

echo "=== Accounting.Infrastructure Services ==="
ACC_INFRA_SVC=Modules/Accounting/Accounting.Infrastructure/Services
for base in Entry Journal Payment Receipt Opening Combined Complex; do
  move_file "$ACC_INFRA_SVC/${base}EntryService.cs" "$ACC_INFRA_SVC/Entries" 2>/dev/null || true
done
move_file "$ACC_INFRA_SVC/EntryService.cs" "$ACC_INFRA_SVC/Entries"
for f in ChartOfAccountService.cs AccountGuideService.cs; do
  move_file "$ACC_INFRA_SVC/$f" "$ACC_INFRA_SVC/ChartOfAccounts"
done
for f in CurrencyService.cs CostCenterService.cs FinancialPeriodService.cs \
  GLSettingService.cs CollectionBookService.cs RoleService.cs; do
  move_file "$ACC_INFRA_SVC/$f" "$ACC_INFRA_SVC/Settings"
done
for f in BankService.cs BranchService.cs CashInBoxService.cs CustomerService.cs \
  FixedAssetService.cs SupplierService.cs TaxService.cs; do
  move_file "$ACC_INFRA_SVC/$f" "$ACC_INFRA_SVC/SubLedger"
done
mkdir -p "$ACC_INFRA_SVC/Integration"
for f in AccountingIntegrationService.cs AttachmentCrossModulePort.cs \
  InventoryAccountingDisplayQueries.cs InventoryCatalogValidationQueries.cs; do
  move_file "$ACC_INFRA_SVC/$f" "$ACC_INFRA_SVC/Integration"
done

echo "=== Inventory.Application Repositories ==="
INV_APP_REPO=Modules/Inventory/Inventory.Application/Repositories
for f in IAttributeDefinitionRepository.cs IAttributeValueRepository.cs IColorRepository.cs \
  IItemRepository.cs IManufacturerCompanyRepository.cs IPackingUnitRepository.cs \
  IProductAttributeDefinitionRepository.cs IProductRepository.cs ISizeRepository.cs \
  IVariantAttributeValueRepository.cs IVariantRepository.cs; do
  move_file "$INV_APP_REPO/$f" "$INV_APP_REPO/Catalog"
done
move_file "$INV_APP_REPO/ISellingPriceRepository.cs" "$INV_APP_REPO/Pricing"
move_file "$INV_APP_REPO/IStockBalanceRepository.cs" "$INV_APP_REPO/Stock"
for f in IInventoryTransactionRepository.cs IInventoryTransferRepository.cs; do
  move_file "$INV_APP_REPO/$f" "$INV_APP_REPO/Transactions"
done

echo "=== Inventory.Application Services ==="
INV_APP_SVC=Modules/Inventory/Inventory.Application/Services
for f in IAttributeDefinitionService.cs IAttributeValueService.cs IColorService.cs \
  IItemService.cs IManufacturerCompanyService.cs IPackingUnitService.cs \
  IProductService.cs ISizeService.cs IVariantService.cs; do
  move_file "$INV_APP_SVC/$f" "$INV_APP_SVC/Catalog"
done
move_file "$INV_APP_SVC/ISellingPriceService.cs" "$INV_APP_SVC/Pricing"
move_file "$INV_APP_SVC/IStockBalanceService.cs" "$INV_APP_SVC/Stock"
for f in IInventoryTransactionService.cs IInventoryTransferService.cs \
  IImportTransactionService.cs IExportTransactionService.cs; do
  move_file "$INV_APP_SVC/$f" "$INV_APP_SVC/Transactions"
done

echo "=== Inventory.Infrastructure Repositories ==="
INV_INFRA_REPO=Modules/Inventory/Inventory.Infrastructure/Repositories
for f in AttributeDefinitionRepository.cs AttributeValueRepository.cs ColorRepository.cs \
  ItemRepository.cs ManufacturerCompanyRepository.cs PackingUnitRepository.cs \
  ProductAttributeDefinitionRepository.cs ProductRepository.cs SizeRepository.cs \
  VariantAttributeValueRepository.cs VariantRepository.cs; do
  move_file "$INV_INFRA_REPO/$f" "$INV_INFRA_REPO/Catalog"
done
move_file "$INV_INFRA_REPO/SellingPriceRepository.cs" "$INV_INFRA_REPO/Pricing"
move_file "$INV_INFRA_REPO/StockBalanceRepository.cs" "$INV_INFRA_REPO/Stock"
for f in InventoryTransactionRepository.cs InventoryTransferRepository.cs \
  FinancialPeriodInventoryUsageQueries.cs; do
  move_file "$INV_INFRA_REPO/$f" "$INV_INFRA_REPO/Transactions"
done

echo "=== Inventory.Infrastructure Services ==="
INV_INFRA_SVC=Modules/Inventory/Inventory.Infrastructure/Services
for f in AttributeDefinitionService.cs AttributeValueService.cs ColorService.cs \
  ItemService.cs ManufacturerCompanyService.cs PackingUnitService.cs \
  ProductService.cs SizeService.cs VariantService.cs; do
  move_file "$INV_INFRA_SVC/$f" "$INV_INFRA_SVC/Catalog"
done
move_file "$INV_INFRA_SVC/SellingPriceService.cs" "$INV_INFRA_SVC/Pricing"
move_file "$INV_INFRA_SVC/StockBalanceService.cs" "$INV_INFRA_SVC/Stock"
for f in InventoryTransactionService.cs InventoryTransferService.cs \
  ImportTransactionService.cs ExportTransactionService.cs; do
  move_file "$INV_INFRA_SVC/$f" "$INV_INFRA_SVC/Transactions"
done

echo "=== Done ==="
