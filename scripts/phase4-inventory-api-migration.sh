#!/usr/bin/env bash
# Phase 4 — move Inventory controllers from Host to Inventory.Api.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

API=Modules/Inventory/Inventory.Api/Controllers
HOST=ERP.Host/Controllers/Inventory

move_ctrl() {
  local src="$1" dest_dir="$2" ns="$3"
  mkdir -p "$dest_dir"
  git mv "$src" "$dest_dir/" 2>/dev/null || mv "$src" "$dest_dir/"
  local file="$dest_dir/$(basename "$src")"
  sed -i -E "s|^namespace ERP\.Host\.Controllers\.Inventory;|namespace $ns;|" "$file"
}

echo "=== Catalog ==="
for f in AttributeDefinitionsController.cs AttributeValuesController.cs ColorsController.cs \
  ItemsController.cs ManufacturerCompaniesController.cs PackingUnitsController.cs \
  ProductsController.cs SizesController.cs VariantsController.cs; do
  move_ctrl "$HOST/$f" "$API/Catalog" "Inventory.Api.Controllers.Catalog"
done

echo "=== Pricing ==="
move_ctrl "$HOST/SellingPricesController.cs" "$API/Pricing" "Inventory.Api.Controllers.Pricing"

echo "=== Stock ==="
move_ctrl "$HOST/StockBalancesController.cs" "$API/Stock" "Inventory.Api.Controllers.Stock"

echo "=== Transactions ==="
for f in ExportTransactionsController.cs ImportTransactionsController.cs InventoryTransferController.cs; do
  move_ctrl "$HOST/$f" "$API/Transactions" "Inventory.Api.Controllers.Transactions"
done

echo "=== Done ==="
