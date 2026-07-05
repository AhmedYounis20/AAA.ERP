# Architecture contributing guide

Guidelines for adding code to the **modular monolith**.  
**Migration history:** [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md) (Phases 0–7 complete)  
**Active forward plan:** [MODULE_API_MIGRATION_PLAN.md](./MODULE_API_MIGRATION_PLAN.md) (module-owned APIs)  
**Extracting an aggregate?** Follow [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md).  
**Adding/moving a controller?** Follow [MODULE_API_TEMPLATE.md](./MODULE_API_TEMPLATE.md).

## Current structure

```
ERP.Host/                  → HTTP, composition root, EF migrations
ERP.Persistence/           → Shared EF kernel (ApplicationDbContext, base services, seed)
BuildingBlocks/
  ERP.BuildingBlocks.Domain/
  ERP.BuildingBlocks.Application/
  ERP.BuildingBlocks.Api/        # Shared CRUD controller bases
  ERP.BuildingBlocks.Persistence/
  ERP.Shared/              → Thin shim (filter DTOs, ErrorMessages, exception handler)
Modules/
  Accounting/              → Domain · Application · Infrastructure · Contracts · Api (planned)
  Inventory/               → Domain · Application · Infrastructure · Api (planned)
  Identity/                → Domain · Application · Infrastructure · Api
  Platform/                → Audit · Caching · Localization · Infrastructure · Api (planned)
Tests/
  ERP.Architecture.Tests/
  ERP.IntegrationTests/
  ERP.SmokeTests/
  ERP.UnitTests/
```

Retired (do **not** recreate): `ERP.Application`, `ERP.Domain`, `ERP.Infrastracture`, `ERP.API`, `Stack.API`, `Modules/Account`, `Modules/MainAPI`, `Modules/Shared`, empty `*.API` scaffolds without controllers.

## Folder layout (per module layer)

Use **horizontal layers with feature subfolders** (e.g. `Commands/Entries/`, `Services/Entries/`):

```
{Module}.Application/
  Commands/{Feature}/
  Dtos/{Feature}/           # API/read models — not in Domain
  Repositories/{Feature}/
  Services/{Feature}/
  Validators/{Feature}/

{Module}.Infrastructure/
  Handlers/{Feature}/
  Repositories/{Feature}/
  Services/{Feature}/
  Persistence/

{Module}.Api/
  Controllers/{Feature}/    # HTTP only — references Application + BuildingBlocks.Api
```

See [CONTROLLER_MIGRATION_MAP.md](./CONTROLLER_MIGRATION_MAP.md) for feature folder names per aggregate.

## Where to put new code

| Change type | Location |
|-------------|----------|
| New **Accounting** entity | `Modules/Accounting/Accounting.Domain/...` |
| New **Inventory** entity | `Modules/Inventory/Inventory.Domain/...` |
| New create/update command | `{Module}.Application/Commands/...` (prefer Application over Domain for new code) |
| New validator | `{Module}.Application/Validators/...` or `BuildingBlocks/ERP.BuildingBlocks.Application/Validators/...` for shared base rules |
| New MediatR handler | `{Module}.Infrastructure/Handlers/...` |
| New repository interface | `{Module}.Application/Repositories/...` |
| New repository implementation | `{Module}.Infrastructure/Repositories/...` |
| New service interface | `{Module}.Application/Services/...` |
| New service implementation | `{Module}.Infrastructure/Services/...` |
| Cross-module integration (public API) | `{Module}.Contracts/` (e.g. `Accounting.Contracts`) |
| New API endpoint | `{Module}.Api/Controllers/{Feature}/...` — **not** `ERP.Host/Controllers/` |
| EF configuration | `{Module}.Infrastructure/Persistence/...` + `ModuleModelConfigurationRegistry.Register(...)` |
| Base entity / API envelope | `BuildingBlocks/ERP.BuildingBlocks.{Domain,Application}` |
| Audit / cache / localization | `Modules/Platform/Platform.{Audit,Caching,Localization}/` |
| Auth / current user | `Modules/Identity/` |

## BuildingBlocks vs Shared vs Platform

| Project | Owns | Do not put here |
|---------|------|-----------------|
| **BuildingBlocks.Domain** | `BaseEntity`, tree bases | Business entities |
| **BuildingBlocks.Application** | `ApiResponse`, `PaginatedResult`, CQRS markers, behaviors, exceptions, `ValidationMessages` | Module-specific DTOs |
| **BuildingBlocks.Api** | `BaseController`, `BaseTreeController`, shared CRUD bases | Module-specific controllers |
| **BuildingBlocks.Persistence** | `IApplicationDbContext`, `ITransactionCoordinator`, `IUnitOfWork` (coordinator only) | Business repositories |
| **ERP.Shared** | Filter DTOs, `ErrorMessages`, `CustomExceptionHandler`, `AttachmentDto` | Module business logic |
| **Identity.*** | `ApplicationUser`, auth services, `ICurrentUser` | Accounting/Inventory rules |
| **Platform.*** | Audit, caching, localization | Accounting/Inventory rules |

Namespaces in BuildingBlocks still use `Shared.*` for some types during transition (see [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 6).

## Module EF and migrations

- **Module owns** `IEntityTypeConfiguration<>` classes (e.g. `Accounting.Infrastructure/Persistence/TaxDbConfig.cs`).
- **Module registers** configs via `ModuleModelConfigurationRegistry.Register(...)` inside `Add{Module}Module()` or `AddPlatformModules()`.
- **Host owns** migrations in `ERP.Host/Migrations/`.
- **Design-time:** `ERP.Host/Persistence/ApplicationDbContextFactory.cs` bootstraps module configs without full DI startup.

```bash
dotnet ef dbcontext info --project ERP.Host --startup-project ERP.Host
dotnet ef migrations add <Name> --project ERP.Host --startup-project ERP.Host
```

After adding a module aggregate, run a dry-run migration and confirm existing tables are not dropped.

## Host composition

```csharp
builder.Services.AddInfrastructureServices();
builder.Services.AddAccountingModule();
builder.Services.AddInventoryModule();
builder.Services.AddPlatformModules();
builder.Services.AddIdentityModule();
builder.Services.AddProjectUtilities();
```

MediatR is registered **only** in `ERP.Host/Utilities/WebBuilderExtensions.cs`.

## Dependency rules (enforced by `ERP.Architecture.Tests`)

| Layer | Must NOT reference |
|-------|-------------------|
| `*.Domain` | `ERP.Persistence`, `*.Infrastructure`, `ERP.Host`, other modules' Domain |
| `*.Application` | `*.Infrastructure`, `ERP.Host`, other modules' Domain (use `*.Contracts`) |
| `*.Infrastructure` | `ERP.Host`, other modules' Domain (except EF `*DbConfig` per ADR-007) |
| `ERP.Persistence` | `ERP.Host` |
| `ERP.Shared` | `ERP.Persistence`, module Domain/Infrastructure |
| `*.Api` | `{Module}.Domain`, `{Module}.Infrastructure`, `ERP.Persistence`, other modules |
| `ERP.Host` | Direct `*.Domain` references (use `*.Application` / `*.Api`) |
| `ERP.BuildingBlocks.*` | Module projects, `ERP.Persistence`, `ERP.Host` |

**`*.Api` may reference:** `{Module}.Application`, `ERP.BuildingBlocks.Api`, `ERP.BuildingBlocks.Application`.

**`ERP.Host` may reference:** all `*.Infrastructure`, all `*.Api` (composition root only).

Run architecture tests before opening a PR:

```bash
dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj
```

## Rules for new features

1. **Do not add repository properties to `IUnitOfWork`** — inject `I*Repository` directly.
2. **Do not register MediatR outside `WebBuilderExtensions`**.
3. **Cross-module writes** go through `*.Contracts` integration services (e.g. `IAccountingIntegrationService`).
4. **Cross-module reads** prefer Contracts query ports over foreign Domain types.
5. **Do not add business controllers to `ERP.Host`** — use `{Module}.Api` ([ADR-011](./adr/011-module-owned-api.md)).

## Pull request checklist

- [ ] Code builds: `dotnet build AAA.ERP.sln -c Release`
- [ ] All tests pass: `dotnet test AAA.ERP.sln -c Release`
- [ ] Architecture tests pass: `dotnet test Tests/ERP.Architecture.Tests`
- [ ] No new `IUnitOfWork` repository properties
- [ ] Validators added for new commands
- [ ] No duplicate DI registrations across module extension methods
- [ ] Cross-module changes reviewed by module owner ([MODULE_OWNERS.md](./MODULE_OWNERS.md))

## Module owners

See [MODULE_OWNERS.md](./MODULE_OWNERS.md).
