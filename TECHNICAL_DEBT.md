# Technical Debt Tracker

This document tracks known technical debt and code quality issues that need to be addressed.

## Phase 4 completion markers (July 2026 — `Refactor` branch)

| ID | Item | Status |
|----|------|--------|
| 4.1.1 | Production structured logging + [runbook](docs/ops/PRODUCTION_LOGGING.md) | **Done** |
| 4.1.3 | `Infrastracture` → `Infrastructure` namespaces in `ERP.Persistence` | **Done** |
| 4.1.4 | `Compined` → `Combined` types; legacy route/JSON preserved | **Done** |
| 4.1.5 | `ITransactionCoordinator` for cross-module ACID | **Done** (transactions + transfers) |
| 4.2.1 | ADR-006 SQL schemas | **Deferred** |
| 4.2.3 | `Entry.SourceDocumentId` idempotent GL | **Done** |
| 4.2.4 | Module READMEs | **Done** |
| 4.2.5 | Products/Variants N+1 query fixes | **Done** |
| 6a–6c | Namespace perimeter + `ERP.Host` rename + solution diagram | **Done** |
| 7a | `ERP.Domain.Commands.Account.*` → `Accounting.Domain.Commands.*` | **Done** (`c51317f`) |
| 7b | `ERP.Application.Validators.Account.*` → BuildingBlocks / Accounting.Application | **Done** (`1e27afe`) |
| 7c | `Domain.Account.Commands` / `InputModels` → `ERP.BuildingBlocks.Application.Commands.BaseInputModels` | **Done** (`5374da1`) |
| 7d | `Domain.Account.Models.Dtos` → Accounting.Domain / Identity.Application | **Done** (`2e6a5c6`) |
| 7e | `Domain.Account.DBConfiguration` / `Utility` → `ERP.Infrastructure` / `Accounting.Domain.Utility` | **Done** (uncommitted) |

**Remaining Phase 4 / namespace follow-ups:** Increment **7** wrap-up; migration designer entity strings (`Domain.Account.Models.Entities.*` in historical snapshots) intentionally unchanged; ongoing 4.3.x — see [MODULAR_MONOLITH.md §16](docs/architecture/MODULAR_MONOLITH.md#16-phase-status).

## High Priority

### 1. Typo: "Compined" → "Combined"
**Status:** **Done** (Phase 4 — task 4.1.4). Types renamed to `Combined*`; legacy API route `/api/CompinedEntries` and JSON wire value `"Compined"` preserved for `EntryType.Combined` (intentional — not filenames).

### 2. Folder Name Typo: "Infrastracture" → "Infrastructure"
**Status:** **Done** (Phase 4 — task 4.1.3). Project folder is `ERP.Persistence`; namespaces are `ERP.Infrastructure.*`; `InfrastructureExtensions.cs` replaces `InfrasctructureExtensions.cs`.

## Medium Priority

### 3. Inconsistent Error Message Keys
**Impact:** Localization difficulties
**Current State:** Mix of PascalCase, SCREAMING_CASE, and camelCase
**Recommendation:** Standardize to PascalCase, use `ErrorMessages` constants class.

### 4. Missing XML Documentation
**Impact:** Developer experience, API documentation
**Files needing documentation:**
- All service interfaces
- All repository interfaces
- Public DTOs
- Controller endpoints

### 5. Magic Strings in Validators
**Impact:** Maintainability, localization
**Recommendation:** Replace with `ErrorMessages` constants.

### 6. Inventory catalog → Account entity coupling
**Status:** Arch test skipped (`Inventory_catalog_services_should_not_reference_Account_domain_entities`). Product/Variant services still reference Account entities for cost centers/taxes until decoupling.

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
