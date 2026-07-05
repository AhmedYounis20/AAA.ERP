# Phase 4 API migration sign-off

> **Date:** July 2026  
> **Milestone:** M4 — Inventory API extraction

---

## Deliverables

| ID | Deliverable | Status |
|----|-------------|--------|
| 4.1 | Create `Inventory.Api` project | ✅ |
| 4.2 | Move 14 Inventory controllers from Host | ✅ |
| 4.3 | Wire `AddApplicationPart(InventoryApiAssemblyMarker)` | ✅ |
| 4.4 | Update arch test ratchet: Host controllers **1** (DbWork only) | ✅ |
| 4.5 | Full test suite green | ✅ |

---

## Controller mapping

| Feature folder | Controllers moved |
|----------------|-------------------|
| `Catalog/` | AttributeDefinitions, AttributeValues, Colors, Items, ManufacturerCompanies, PackingUnits, Products, Sizes, Variants |
| `Pricing/` | SellingPrices |
| `Stock/` | StockBalances |
| `Transactions/` | ExportTransactions, ImportTransactions, InventoryTransfer |

**Remains in Host:** `DbWorkController`

---

## Known exceptions

- `Inventory.Api` references `Inventory.Domain` for CRUD generic type parameters
- `Inventory.Api` references `Accounting.Application` for `IBaseQueryService<,>` (lookup queries) — target: move to Contracts or BuildingBlocks later

---

## Next phase

**Phase 5** — Host slim-down (optional DbWork relocation, remove empty `Controllers/` folder)
