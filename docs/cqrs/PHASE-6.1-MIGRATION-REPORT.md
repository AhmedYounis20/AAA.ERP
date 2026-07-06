# Phase 6.1 Migration Report — Sub-Ledger Query Cleanup

**Status:** Complete  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Remove slim sub-ledger helper services; migrate queries to repository

---

## Summary

Phase 6.1 begins ADR-013 cleanup by removing seven transitional `I*Service` types that only existed to supply default-data helpers (`GetNextSubLedgers` / `GetNextFixedAsset`). Read queries now call `IChartOfAccountRepository` directly.

| Entity | Service removed | Query change |
|--------|-----------------|--------------|
| Bank | `IBankService`, `BankService` | `SubLedgerNextDefaultsHelper` via repository |
| Customer | `ICustomerService`, `CustomerService` | Same |
| Supplier | `ISupplierService`, `SupplierService` | Same |
| CashInBox | `ICashInBoxService`, `CashInBoxService` | Same |
| Tax | `ITaxService`, `TaxService` | Inline parent-required code generation |
| Branch | `IBranchService`, `BranchService` | `SubLedgerNextDefaultsHelper` via repository |
| FixedAsset | `IFixedAssetService`, `FixedAssetService` | Inline type-specific code generation |

---

## Changes

- Updated seven `*Queries` classes to inject `IChartOfAccountRepository` instead of slim services
- Removed 14 service interface/implementation files from `SubLedger` folders
- Removed seven `I*Service` DI registrations from `AccountingModuleExtensions`

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |

---

## Next Steps (Phase 6.2+)

- Migrate entry-group `*Queries` off slim read services — see `PHASE-6.2-MIGRATION-REPORT.md`
- Migrate inventory transaction `*Queries` off `IImportTransactionService` / `IExportTransactionService`
- Migrate catalog `*Queries` (Item, Product, Variant) off write-capable services once workflow extraction completes
- Update `MediatRRegistrationTests` baselines if handler consolidation changes counts
