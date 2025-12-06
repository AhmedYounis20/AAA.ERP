# Technical Debt Tracker

This document tracks known technical debt and code quality issues that need to be addressed.

## High Priority

### 1. Typo: "Compined" → "Combined" (19 files affected)
**Impact:** Code readability, professionalism
**Effort:** Medium (requires file renaming, reference updates)
**Files:**
- `ERP.API/Controllers/Account/Entries/CompinedEntriesController.cs`
- `ERP.Application/Services/Account/Entries/ICompinedEntryService.cs`
- `ERP.Application/Validators/Account/ComandValidators/Entries/CompinedEntries/*`
- `ERP.Domain/Commands/Account/Entries/CompinedEntries/*`
- `ERP.Domain/Models/Entities/Account/Entries/EntryType.cs`
- `ERP.Infrastracture/Services/Account/Entries/CompinedEntryService.cs`
- `ERP.Infrastracture/Handlers/Account/Entries/CompinedEntries/*`

**Recommendation:** Create a separate branch for this refactoring with careful testing.

### 2. Folder Name Typo: "Infrastracture" → "Infrastructure"
**Impact:** Code organization, professionalism
**Effort:** High (major folder rename, all references)
**Recommendation:** Use IDE refactoring tools, update all imports and references.

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

## Low Priority

### 6. Inconsistent Naming in DTOs
- Some use `Id` suffix, some don't
- Mix of `Dto` and `DTO` suffixes

### 7. Unused Using Statements
- Run code cleanup to remove unused imports

### 8. Consider Adding:
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

