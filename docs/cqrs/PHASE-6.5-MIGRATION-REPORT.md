# Phase 6.5 Migration Report — Catalog Workflow Extraction

**Status:** In progress (Variant ✅, Item ✅, Product pending)  
**Branch:** `feature/adr-013-generic-cqrs-handlers`  
**Scope:** Move catalog CRUD into workflow services; remove `I*Service` write facades

---

## Phase 6.5a — Variant (complete)

| Change | Outcome |
|--------|---------|
| `VariantService` → `VariantWorkflowService` | Write logic in `Workflows/` |
| `IVariantService` removed | `IVariantWorkflowService` + `IVariantProductOperations` |
| `ProductService` | Uses `IVariantProductOperations` |

Commit: `73eb7ee`

---

## Phase 6.5b — Item (complete)

| Change | Outcome |
|--------|---------|
| `ItemService` → `ItemWorkflowService` | Write logic in `Workflows/` |
| `IItemService` removed | Handlers use `IItemWorkflowService` only |
| Dead read methods removed | Reads on `ItemQueries` (Phase 6.4) |

---

## Validation (6.5b)

| Check | Result |
|-------|--------|
| `dotnet build AAA.ERP.sln` | **0 errors** |
| `dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj` | **91/91 passed** |
| `dotnet test Tests/Inventory.UnitTests/Inventory.UnitTests.csproj` | **6/6 passed** |

---

## Next Steps

- **Phase 6.5c:** Extract `ProductService` → `ProductWorkflowService`
