# ADR-007: Database-driven sub-ledger chart-of-account parent resolution

## Status

Accepted — August 2026

## Context

Sub-ledgers (banks, customers, suppliers, branches, fixed assets, etc.) create child nodes under predefined chart-of-account (COA) headers when a domain entity is created.

Previously, parent COA IDs were hardcoded in `Accounting.Domain.Utility.Sd` and injected into create handlers. That approach:

- Coupled application code to one database seed snapshot
- Duplicated COA-creation logic across handlers and workflow services
- Silently fell back to default account properties when a parent row was missing
- Could not support per-tenant or reconfigured COA trees without redeploying code

The domain model already exposed `IsSubLeadgerBaseAccount` on `ChartOfAccount`, but creation never queried it.

## Decision

1. **Configure base accounts in data**, not code:
   - `IsSubLeadgerBaseAccount = true`
   - `SubLeadgerType` (Bank, Customer, Supplier, …)
   - `SubLedgerAccountRole` (Default, Asset, AccumulatedDepreciation, DepreciationExpense, StockUnderTransfer)
   - `FixedAssetType?` for fixed-asset mappings

2. **Resolve parents at runtime** through `ISubLedgerCoaParentResolver` keyed by `(SubLeadgerType, FixedAssetType?, SubLedgerAccountRole)`.

3. **Create child COA nodes** only through `ISubLedgerCoaFactory` (single implementation, fail-fast if parent is null).

4. **Validate configuration on startup** via `ISubLedgerCoaConfigurationValidator` (hosted service). Disable in automated tests with:
   ```json
   "Accounting:SubLedger:ValidateConfigurationOnStartup": false
   ```

5. **Tax remains user-selected**: callers supply `ChartOfAccountId`; `ISubLedgerCoaParentValidator` enforces eligibility.

6. **Inventory stock-under-transfer** resolves through `IAccountingIntegrationService.GetStockUnderTransferBranchAsync()` using `SubLedgerAccountRole.StockUnderTransfer`.

7. **Remove all COA GUID constants** from `Sd.cs`. IDs exist only in seed JSON and one-time migration SQL.

## Consequences

### Positive

- Adding a sub-ledger type requires enum + seed/migration metadata, not new GUID constants
- Missing configuration fails loudly at startup (production) or at resolve time
- Resolver cache invalidated when base COA rows change
- Architecture test prevents reintroduction of `Sd.*` COA constants

### Negative / trade-offs

- Requires migration + seed metadata for every environment
- Integration/smoke test hosts must disable startup validation or seed the full mapping set
- Fixed assets still need 22 base rows (8 asset types × roles, minus lands depreciation)

### StockUnderTransfer identity cutover

Old code used hardcoded `Sd.StockUnderTransferBranch = 00000000-0000-0000-0000-000000000001`.
Seed data (and the binding table) use Branch/COA `6776b6a3-019c-480b-855b-08dc3dfcb069` ("Stock Under Transfer").

Runtime now resolves via `SubLedgerAccountRole.StockUnderTransfer` → that seeded ID.

**Current status (pre-production):** no environments have open in-transit stock after deploy, so **no data remap is required**. Ensure seed/import creates Branch + COA `6776b6a3-…069` and the `StockUnderTransfer` binding.

**If a later environment already wrote balances/transfers against the sentinel GUID**, use `scripts/ops/remap-stock-under-transfer-sentinel.sql` (pre-check, then optional REMAP).

Integration tests may still invent the sentinel GUID locally; seeded environments must not rely on it.

## References

- `Modules/Accounting/Accounting.Infrastructure/Services/SubLedger/`
- Migration `20260806151042_AddSubLedgerBaseAccountMetadata`
- Migration `20261111065337_MoveSubLedgerBaseAccountBinding`
- `scripts/patch-chart-of-accounts-seed-metadata.py`
- `scripts/ops/remap-stock-under-transfer-sentinel.sql`
