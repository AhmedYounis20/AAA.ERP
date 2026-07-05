#!/usr/bin/env bash
# Phase 3 — move Accounting controllers from Host to Accounting.Api (DbWork stays in Host).
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

API=Modules/Accounting/Accounting.Api/Controllers
HOST=ERP.Host/Controllers/Accounting

move_ctrl() {
  local src="$1" dest_dir="$2" ns="$3"
  mkdir -p "$dest_dir"
  git mv "$src" "$dest_dir/" 2>/dev/null || mv "$src" "$dest_dir/"
  local file="$dest_dir/$(basename "$src")"
  sed -i -E "s|^namespace ERP\.Host\.Controllers\.Accounting(\.[A-Za-z]+)?;|namespace $ns;|" "$file"
}

echo "=== ChartOfAccounts ==="
move_ctrl "$HOST/AccountGuidesController.cs" "$API/ChartOfAccounts" "Accounting.Api.Controllers.ChartOfAccounts"
move_ctrl "$HOST/ChartOfAccountsController.cs" "$API/ChartOfAccounts" "Accounting.Api.Controllers.ChartOfAccounts"

echo "=== Settings ==="
for f in CollectionBooksController.cs CostCentersController.cs CurrenciesController.cs \
  FinancialPeriodsController.cs GLSettingsController.cs RolesController.cs; do
  move_ctrl "$HOST/$f" "$API/Settings" "Accounting.Api.Controllers.Settings"
done

echo "=== SubLedger ==="
for f in BanksController.cs BranchesController.cs CashInBoxesController.cs CustomersController.cs \
  FixedAssetsController.cs SuppliersController.cs TaxesController.cs; do
  move_ctrl "$HOST/SubLeadgers/$f" "$API/SubLedger" "Accounting.Api.Controllers.SubLedger"
done
rmdir "$HOST/SubLeadgers" 2>/dev/null || true

echo "=== Entries ==="
for f in CombinedEntriesController.cs EntriesController.cs JournalEntriesController.cs \
  OpeningEntriesController.cs PaymentEntriesController.cs ReceiptEntriesController.cs; do
  move_ctrl "$HOST/Entries/$f" "$API/Entries" "Accounting.Api.Controllers.Entries"
done
rmdir "$HOST/Entries" 2>/dev/null || true

echo "=== Done (DbWorkController remains in Host) ==="
