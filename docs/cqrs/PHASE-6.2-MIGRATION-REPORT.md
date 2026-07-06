# Phase 6.2 Migration Report — Entry-Group Query Cleanup

**Status:** Complete  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Remove thin typed entry read services; queries call core read services directly

---

## Summary

Phase 6.2 removes five transitional typed entry services that only forwarded read calls to `IEntryService` or `IComplexEntryService` with a fixed `EntryType`. Queries now call the core read services directly.

| Typed service removed | Query now uses |
|----------------------|----------------|
| `IJournalEntryService` | `IEntryService` + `EntryType.Journal` |
| `IOpeningEntryService` | `IEntryService` + `EntryType.Opening` |
| `IPaymentEntryService` | `IComplexEntryService` + `EntryType.Payment` |
| `IReceiptEntryService` | `IComplexEntryService` + `EntryType.Receipt` |
| `ICombinedEntryService` | `IComplexEntryService` + `EntryType.Combined` |

**Retained:** `IEntryService` / `EntryService` and `IComplexEntryService` / `ComplexEntryService` — these contain real read logic (DTO projection, pagination, entry numbering).

---

## Validation

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |

---

## Next Steps (Phase 6.3+)

- Migrate inventory transaction `*Queries` off `IImportTransactionService` / `IExportTransactionService`
- Migrate catalog `*Queries` (Item, Product, Variant) off write-capable services
- Consider inlining `IComplexEntryService` read logic into `ComplexEntryQueries` (optional follow-up)
