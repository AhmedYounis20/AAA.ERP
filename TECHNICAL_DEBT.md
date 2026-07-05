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
| C3 | Inventory validators → `ValidationMessages` | **Done** — `6f232b8` |
| C4 | Service/controller error keys → `ErrorMessages` / `SuccessMessages` | **Done** — `6f232b8` |
| C5 | XML docs on Accounting service/repository interfaces | **Done** — `6f232b8` |
| C6 | Identity/Platform error keys → `ErrorMessages` | **Done** — `e8ffd06` |
| C7 | XML docs on Inventory service/repository interfaces | **Done** — `e8ffd06` |
| C8 | Identity interfaces XML docs + Platform audit error key | **Done** — `e8ffd06` |
| D1 | Persistence/exception handler error keys → `ErrorMessages` | **Done** |
| D2 | Identity.Infrastructure namespace cleanup | **Done** |
| D3 | Platform `ICacheService` XML docs | **Done** |
| D4 | Controller/Swagger XML docs (`ERP.Host` + auth DTOs) | **Done** |
| D5 | `BaseService` `RecordNotFound` key + arch test for Identity namespace | **Done** |
| D6 | Doc sync (C3–C8 commit refs) | **Done** |
| L1 | Localization keys → `Platform.Localization` resources | **Done** |

**Ongoing:** 4.3.x arch test review per PR — see [MODULAR_MONOLITH.md §16](docs/architecture/MODULAR_MONOLITH.md#16-phase-status). **Post-M4 quality plan complete** — see [POST_M4_QUALITY_SIGNOFF.md](docs/architecture/POST_M4_QUALITY_SIGNOFF.md).

## High Priority

### 1. Typo: "Compined" → "Combined"
**Status:** **Done** (Phase 4 — task 4.1.4). Types renamed to `Combined*`; legacy API route `/api/CompinedEntries` and JSON wire value `"Compined"` preserved for `EntryType.Combined` (intentional — not filenames).

### 2. Folder Name Typo: "Infrastracture" → "Infrastructure"
**Status:** **Done** (Phase 4 — task 4.1.3). Project folder is `ERP.Persistence`; namespaces are `ERP.Infrastructure.*`; `InfrastructureExtensions.cs` replaces `InfrasctructureExtensions.cs`.

## Medium Priority

### 3. Inconsistent Error Message Keys
**Impact:** Localization difficulties
**Status:** **Done** (C4 + C6 + D1). Module services, host controllers, `CustomExceptionHandler`, and horizontal `BaseService` / tree services use `ErrorMessages` / `SuccessMessages`.

### 4. Missing XML Documentation
**Impact:** Developer experience, API documentation
**Status:** **Done** (C5 + C7 + C8 + D3 + D4). Application interfaces, `ICacheService`, and all `ERP.Host` controller public actions documented; Swagger `IncludeXmlComments` wired.

### 5. Magic Strings in Validators
**Impact:** Maintainability, localization
**Status:** **Done** (C1–C3). All Accounting and Inventory FluentValidation keys use `ValidationMessages` in `ERP.BuildingBlocks.Application.Constants`.

### 6. Inventory catalog → Account entity coupling
**Status:** **Done** (Increment 8b + Phase 5 / ADR-007). Supplier existence checks use `IInventoryCatalogValidationQueries` (Contracts); arch test `Inventory_catalog_services_should_not_reference_Account_domain_entities` enabled. Junction entities hold Guid FKs only — cross-module EF navigations configured in `Inventory.Infrastructure`; `Inventory.Domain` no longer references `Accounting.Domain`.

## Low Priority

### 7. Inconsistent Naming in DTOs
- Some use `Id` suffix, some don't
- Mix of `Dto` and `DTO` suffixes

### 8. Unused Using Statements
**Status:** Partial — `dotnet format` applied to touched projects (July 2026). Full-solution cleanup deferred.

### 9. Consider Adding:
- FluentValidation rule sets for different scenarios *(Phase 5 backlog)*
- More specific exception types *(Phase 5 backlog)*
- Request/Response logging middleware *(covered by Serilog + `LoggingBehavior`; no additional middleware required)*

### 10. Localization resource parity
**Status:** **Done** (L1). `ErrorMessages`, `SuccessMessages`, and `ValidationMessages` wire keys synced to `Platform.Localization/Resources/Resource.resx` and `Resource.ar-EG.resx`; `LocalizationResponseMiddleware` resolves at runtime.

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
