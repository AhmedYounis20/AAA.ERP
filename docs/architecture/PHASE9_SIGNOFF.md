# Phase 9 sign-off — Optional structural polish

**Goal:** Ownership clarity and kernel consolidation.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 9  
**Date:** July 2026

---

## Tasks

| ID | Task | Status | Notes |
|----|------|--------|-------|
| 9.1 | `src/` + `tests/` layout | ⏭️ | Declined — large diff, no functional gain |
| 9.2 | Retire `ERP.Shared` project | ✅ | Merged into BuildingBlocks; project removed |
| 9.3 | `ERP.BuildingBlocks.Infrastructure` | ✅ | `CustomExceptionHandler` in `BuildingBlocks.Application/Exceptions` (pragmatic) |
| 9.4 | Feature folders in Host | ⏭️ | Declined — flat `Controllers/` sufficient |
| 9.5 | Migrations in `ERP.Persistence` | ✅ | `ERP.Persistence/Migrations/`; `MigrationsAssembly("ERP.Persistence")` |

**`dotnet ef` usage:**

```bash
dotnet ef migrations add <Name> --project ERP.Persistence --startup-project ERP.Host
dotnet ef database update --project ERP.Persistence --startup-project ERP.Host
```
