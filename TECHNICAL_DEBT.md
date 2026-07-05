# Technical Debt Tracker

This document tracks known technical debt and code quality issues that need to be addressed.

## Phase 4 completion markers (July 2026 — `Refactor` branch) — **M4 COMPLETE**

See [M4_SIGNOFF.md](docs/architecture/M4_SIGNOFF.md).

| ID | Item | Status |
|----|------|--------|
| 4.1.1 | Production structured logging + [runbook](docs/ops/PRODUCTION_LOGGING.md) | **Done** |
| 4.1.3 | `Infrastracture` → `Infrastructure` namespaces in `ERP.Persistence` | **Done** |
| 4.1.4 | `Compined` → `Combined` types; legacy route/JSON preserved | **Done** |
| 4.1.5 | `ITransactionCoordinator` for cross-module ACID | **Done** (transactions + transfers) |
| 4.2.1 | ADR-006 SQL schema evaluation | **Done** — Option B approved |
| 4.2.2 | Module SQL schemas + `SeparateModuleSchemas` migration | **Done** — `f256151`; dev DB applied |
| 4.2.3 | `Entry.SourceDocumentId` idempotent GL | **Done** |
| 4.2.4 | Module READMEs | **Done** |
| 4.2.5 | Products/Variants N+1 query fixes | **Done** |
| 6a–6c | Namespace perimeter + `ERP.Host` rename + solution diagram | **Done** |
| 7a–7e | `Domain.Account.*` namespace retirement | **Done** |
| 8a | `AAA.ERP.OutputDtos` namespace retirement | **Done** |
| 8b | Inventory catalog → Accounting via Contracts query port | **Done** |
| C1 | Accounting validators → `ValidationMessages` constants | **Done** — `9ec86a9` / `371e4c3` |
| C2 | BuildingBlocks Account base validators → `ValidationMessages` | **Done** — `371e4c3` |
| C3 | Inventory validators → `ValidationMessages` | **Done** (uncommitted batch) |
| C4 | Service/controller error keys → `ErrorMessages` / `SuccessMessages` | **Done** (uncommitted batch) |
| C5 | XML docs on Accounting service/repository interfaces | **Done** (uncommitted batch) |

**Ongoing:** 4.3.x arch test review per PR — see [MODULAR_MONOLITH.md §16](docs/architecture/MODULAR_MONOLITH.md#16-phase-status).

## High Priority

### 1. Typo: "Compined" → "Combined"
**Status:** **Done** (Phase 4 — task 4.1.4). Types renamed to `Combined*`; legacy API route `/api/CompinedEntries` and JSON wire value `"Compined"` preserved for `EntryType.Combined` (intentional — not filenames).

### 2. Folder Name Typo: "Infrastracture" → "Infrastructure"
**Status:** **Done** (Phase 4 — task 4.1.3). Project folder is `ERP.Persistence`; namespaces are `ERP.Infrastructure.*`; `InfrastructureExtensions.cs` replaces `InfrasctructureExtensions.cs`.

## Medium Priority

### 3. Inconsistent Error Message Keys
**Impact:** Localization difficulties
**Status:** **Partial — C4 done** for Accounting/Inventory services and Host controllers (`Shared.Constants.ErrorMessages` / `SuccessMessages`). Remaining ad-hoc strings may exist in Identity/Platform modules.

### 4. Missing XML Documentation
**Impact:** Developer experience, API documentation
**Status:** **Partial — C5 done** for Accounting `Application` service/repository interfaces and `IAccountingIntegrationService`. Inventory interfaces and controller endpoints remain.

### 5. Magic Strings in Validators
**Impact:** Maintainability, localization
**Status:** **Done** (C1–C3). All Accounting and Inventory FluentValidation keys use `ValidationMessages` in `ERP.BuildingBlocks.Application.Constants`.

### 6. Inventory catalog → Account entity coupling
**Status:** **Done** (Increment 8b). Supplier existence checks use `IInventoryCatalogValidationQueries` (Contracts); arch test `Inventory_catalog_services_should_not_reference_Account_domain_entities` enabled. Junction entities in `Inventory.Domain` still reference Accounting navigation types for EF — acceptable shared-database pattern.

## Low Priority

### 7. Inconsistent Naming in DTOs
- Some use `Id` suffix, some don't
- Mix of `Dto` and `DTO` suffixes

### 8. Unused Using Statements
- Run code cleanup to remove unused imports

### 9. Consider Adding:
- FluentValidation rule sets for different scenarios
- More specific exception types
- Request/Response logging middleware

## Completed Items

- [x] CreatedBy/ModifiedBy not being set - **Fixed in Phase 1**
- [x] Missing Global Exception Handler - **Fixed in Phase 1**
- [x] Missing Pagination - **Fixed in Phase 2**
- [x] No Soft Delete - **Fixed in Phase 2**
- [x] Missing Filter DTOs - **Fixed in Phase 7**
- [x] No Audit Trail - **Fixed in Phase 8**
- [x] No Bulk Operations - **Fixed in Phase 9**
- [x] No Caching - **Fixed in Phase 10**
- [x] Commented out code in BaseRepository - **Removed in Phase 13**
- [x] God `IUnitOfWork` repository properties - **Removed in Wave 3E**
- [x] Horizontal `ERP.Domain` / `ERP.Application` / `ERP.Infrastracture` projects - **Retired in Wave 3E**
- [x] `AAA.ERP.OutputDtos` legacy namespace - **Retired in Increment 8a**
- [x] Skipped inventory catalog arch test - **Enabled in Increment 8b**
