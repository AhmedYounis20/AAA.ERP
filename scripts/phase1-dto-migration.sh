#!/usr/bin/env bash
# Phase 1 — move DTOs from Domain to Application and update namespaces.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "=== Moving Accounting DTOs ==="
mkdir -p Modules/Accounting/Accounting.Application/Dtos/{FinancialPeriods,Entries,Lookups}

git mv Modules/Accounting/Accounting.Domain/Models/Dtos/FinancialPeriods/FinancialPeriodDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/FinancialPeriods/ 2>/dev/null || \
  mv Modules/Accounting/Accounting.Domain/Models/Dtos/FinancialPeriods/FinancialPeriodDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/FinancialPeriods/

git mv Modules/Accounting/Accounting.Domain/Models/Dtos/Entry/FinancialTransactionInputModel.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/ 2>/dev/null || \
  mv Modules/Accounting/Accounting.Domain/Models/Dtos/Entry/FinancialTransactionInputModel.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/

git mv Modules/Accounting/Accounting.Domain/OutputDtos/EntryDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/ 2>/dev/null || \
  mv Modules/Accounting/Accounting.Domain/OutputDtos/EntryDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/

git mv Modules/Accounting/Accounting.Domain/OutputDtos/ComplexEntryDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/ 2>/dev/null || \
  mv Modules/Accounting/Accounting.Domain/OutputDtos/ComplexEntryDto.cs \
  Modules/Accounting/Accounting.Application/Dtos/Entries/

for f in Modules/Accounting/Accounting.Domain/OutputDtos/Lookups/*.cs; do
  [ -f "$f" ] || continue
  base=$(basename "$f")
  git mv "$f" "Modules/Accounting/Accounting.Application/Dtos/Lookups/$base" 2>/dev/null || \
    mv "$f" "Modules/Accounting/Accounting.Application/Dtos/Lookups/$base"
done

echo "=== Moving Inventory DTOs ==="
mkdir -p Modules/Inventory/Inventory.Application/Dtos/{Catalog,Lookups,Pricing}

git mv Modules/Inventory/Inventory.Domain/Models/Dtos/SellingPriceDto.cs \
  Modules/Inventory/Inventory.Application/Dtos/Pricing/ 2>/dev/null || \
  mv Modules/Inventory/Inventory.Domain/Models/Dtos/SellingPriceDto.cs \
  Modules/Inventory/Inventory.Application/Dtos/Pricing/

for f in Modules/Inventory/Inventory.Domain/Models/Dtos/Inventory/*.cs; do
  [ -f "$f" ] || continue
  base=$(basename "$f")
  git mv "$f" "Modules/Inventory/Inventory.Application/Dtos/Catalog/$base" 2>/dev/null || \
    mv "$f" "Modules/Inventory/Inventory.Application/Dtos/Catalog/$base"
done

for f in Modules/Inventory/Inventory.Domain/OutputDtos/Lookups/*.cs; do
  [ -f "$f" ] || continue
  base=$(basename "$f")
  git mv "$f" "Modules/Inventory/Inventory.Application/Dtos/Lookups/$base" 2>/dev/null || \
    mv "$f" "Modules/Inventory/Inventory.Application/Dtos/Lookups/$base"
done

echo "=== Moving Account filter DTOs ==="
mkdir -p Modules/Accounting/Accounting.Application/Dtos/Filters
git mv BuildingBlocks/ERP.BuildingBlocks.Application/DTOs/Filters/AccountFilterDtos.cs \
  Modules/Accounting/Accounting.Application/Dtos/Filters/ 2>/dev/null || \
  mv BuildingBlocks/ERP.BuildingBlocks.Application/DTOs/Filters/AccountFilterDtos.cs \
  Modules/Accounting/Accounting.Application/Dtos/Filters/

echo "=== Updating namespaces in moved files ==="
find Modules/Accounting/Accounting.Application/Dtos -name '*.cs' -exec sed -i \
  -e 's/namespace Accounting\.Domain\.Models\.Dtos\.FinancialPeriods/namespace Accounting.Application.Dtos.FinancialPeriods/g' \
  -e 's/namespace Accounting\.Domain\.Models\.Dtos\.Entry/namespace Accounting.Application.Dtos.Entries/g' \
  -e 's/namespace Accounting\.Domain\.OutputDtos\.Lookups/namespace Accounting.Application.Dtos.Lookups/g' \
  -e 's/namespace Accounting\.Domain\.OutputDtos/namespace Accounting.Application.Dtos.Entries/g' \
  {} +

find Modules/Inventory/Inventory.Application/Dtos -name '*.cs' -exec sed -i \
  -e 's/namespace Inventory\.Domain\.Models\.Dtos\.Inventory/namespace Inventory.Application.Dtos.Catalog/g' \
  -e 's/namespace Inventory\.Domain\.Models\.Dtos/namespace Inventory.Application.Dtos/g' \
  -e 's/namespace Inventory\.Domain\.OutputDtos\.Lookups/namespace Inventory.Application.Dtos.Lookups/g' \
  {} +

sed -i 's/namespace ERP\.BuildingBlocks\.Application\.DTOs\.Filters/namespace Accounting.Application.Dtos.Filters/g' \
  Modules/Accounting/Accounting.Application/Dtos/Filters/AccountFilterDtos.cs

echo "=== Updating references across solution ==="
find . -name '*.cs' -not -path '*/obj/*' -not -path '*/bin/*' -exec sed -i \
  -e 's/Accounting\.Domain\.Models\.Dtos\.FinancialPeriods/Accounting.Application.Dtos.FinancialPeriods/g' \
  -e 's/Accounting\.Domain\.Models\.Dtos\.Entry/Accounting.Application.Dtos.Entries/g' \
  -e 's/Accounting\.Domain\.OutputDtos\.Lookups/Accounting.Application.Dtos.Lookups/g' \
  -e 's/Accounting\.Domain\.OutputDtos/Accounting.Application.Dtos.Entries/g' \
  -e 's/Inventory\.Domain\.Models\.Dtos\.Inventory/Inventory.Application.Dtos.Catalog/g' \
  -e 's/Inventory\.Domain\.Models\.Dtos/Inventory.Application.Dtos/g' \
  -e 's/Inventory\.Domain\.OutputDtos\.Lookups/Inventory.Application.Dtos.Lookups/g' \
  {} +

# Account filter DTOs — update type references (keep InventoryFilterDtos in BuildingBlocks)
# Controllers use ERP.BuildingBlocks.Application.DTOs.Filters for EntryFilterDto etc.
find . -name '*.cs' -not -path '*/obj/*' -not -path '*/bin/*' \
  -not -path './BuildingBlocks/ERP.BuildingBlocks.Application/DTOs/Filters/InventoryFilterDtos.cs' \
  -exec sed -i \
  -e 's/using ERP\.BuildingBlocks\.Application\.DTOs\.Filters;/using Accounting.Application.Dtos.Filters;/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.ChartOfAccountFilterDto/Accounting.Application.Dtos.Filters.ChartOfAccountFilterDto/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.EntryFilterDto/Accounting.Application.Dtos.Filters.EntryFilterDto/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.FinancialPeriodFilterDto/Accounting.Application.Dtos.Filters.FinancialPeriodFilterDto/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.CostCenterFilterDto/Accounting.Application.Dtos.Filters.CostCenterFilterDto/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.SubLeadgerFilterDto/Accounting.Application.Dtos.Filters.SubLeadgerFilterDto/g' \
  -e 's/ERP\.BuildingBlocks\.Application\.DTOs\.Filters\.CurrencyFilterDto/Accounting.Application.Dtos.Filters.CurrencyFilterDto/g' \
  {} +

echo "=== Cleaning empty Domain DTO folders ==="
find Modules/Accounting/Accounting.Domain/Models/Dtos Modules/Accounting/Accounting.Domain/OutputDtos \
  Modules/Inventory/Inventory.Domain/Models/Dtos Modules/Inventory/Inventory.Domain/OutputDtos \
  -type d -empty -delete 2>/dev/null || true

echo "=== Done ==="
