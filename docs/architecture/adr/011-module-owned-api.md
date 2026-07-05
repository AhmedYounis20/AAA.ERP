# ADR-011: Module-owned API surface

## Status

Accepted — July 2026

## Context

The modular monolith migration (Phases 0–7) moved business logic into `Modules/{Name}/` projects, but all 38 business HTTP controllers remain in `ERP.Host/Controllers/`. This creates:

- No compile-time module ownership of the API surface
- Host grows with every new endpoint
- Unclear where new controllers belong (`CONTRIBUTING.md` still pointed to Host)

The frontend depends on stable routes (`/api/{Controller}`); those must not change.

## Decision

1. **Each module owns its HTTP controllers** in a dedicated `{Module}.Api` class library (not in Infrastructure).
2. **`ERP.Host` is composition root only** — `Program.cs`, middleware, Swagger, DI wiring, migrations. It discovers module controllers via `AddApplicationPart`.
3. **Shared CRUD controller bases** live in `ERP.BuildingBlocks.Api` (not Host).
4. **`{Module}.Api` may reference** only `{Module}.Application` and `ERP.BuildingBlocks.Api`.
5. **`{Module}.Api` must NOT reference** `{Module}.Domain`, `{Module}.Infrastructure`, or `ERP.Persistence`.
6. **API routes unchanged** — existing `[Route("api/[controller]")]` attributes are preserved when controllers move.

## Target layout

```
Modules/Accounting/Accounting.Api/Controllers/...
Modules/Inventory/Inventory.Api/Controllers/...
Modules/Identity/Identity.Api/Controllers/...
Modules/Platform/Platform.Audit.Api/Controllers/...

BuildingBlocks/ERP.BuildingBlocks.Api/Controllers/   # BaseController hierarchy

ERP.Host/                                          # No business controllers after Phase 5
```

## Consequences

- **Positive:** Module teams own full vertical slice including HTTP; Host stays thin; arch tests enforce Api layer isolation.
- **Positive:** Swagger XML docs can be per-module project.
- **Neutral:** One extra project per module (~4 new csproj files).
- **Risk:** Controller DI still resolved from Host-registered Infrastructure — unchanged; Api projects only declare constructor dependencies on Application interfaces.
- **Migration:** Phased rollout (Phase 0 scaffold → Phase 2 pilot → Phases 3–4 bulk move). See [MODULE_API_MIGRATION_PLAN.md](../MODULE_API_MIGRATION_PLAN.md).

## Supersedes

- `MODULE_TEMPLATE.md` §Step 6 — "Controller stays in `ERP.Host` for now" is **retired** by this ADR.
- `CONTRIBUTING.md` — "New API endpoint → `ERP.Host/Controllers`" is **retired**; new endpoints go in `{Module}.Api`.

## Enforcement

`Tests/ERP.Architecture.Tests/ModuleApiLayerTests.cs` — Api must not reference Domain/Infrastructure/Persistence.
