# Architecture contributing guide

Guidelines for adding code during the **modular monolith** migration.  
Full plan: [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md)  
**Extracting an aggregate?** Follow [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md).

## Current structure (transitional)

```
ERP.API            → HTTP, composition root, migrations
ERP.Application    → Interfaces, validators
ERP.Domain         → Entities, commands, DTOs
ERP.Infrastracture → Repositories, services, MediatR handlers
BuildingBlocks/    → Technical kernel (Phase 3 Wave 3A)
  ERP.BuildingBlocks.Domain      → BaseEntity, tree entities, Attachment
  ERP.BuildingBlocks.Application → ApiResponse, PaginatedResult, CQRS, behaviors, exceptions
Modules/Platform/  → Cross-cutting platform services
  Platform.Audit.*       → Audit trail (IAuditService, AuditLog)
  Platform.Caching.*       → In-memory cache (ICacheService)
  Platform.Localization    → en/ar resources, response localization middleware
  Platform.Infrastructure  → AddPlatformModules() composition
Modules/Identity/  → Authentication and current-user abstraction (Wave 3B)
  Identity.Domain          → ApplicationUser
  Identity.Application     → IAuthenticationService, ICurrentUser, auth DTOs
  Identity.Infrastructure  → AuthenticationService, AddIdentityModule()
Shared/            → Thin shim (filter DTOs, ErrorMessages)
```

## Where to put new code

| Change type | Location |
|-------------|----------|
| New entity | `ERP.Domain/Models/Entities/{Account\|Inventory}/...` |
| New create/update command | `ERP.Domain/Commands/...` |
| New validator | `ERP.Application/Validators/...` |
| New MediatR handler | `ERP.Infrastracture/Handlers/...` |
| New repository interface | `ERP.Application/Repositories/...` |
| New repository implementation | `ERP.Infrastracture/Repositories/...` |
| New service interface | `ERP.Application/Services/...` (or `Platform.*.Application` for platform) |
| New service implementation | `ERP.Infrastracture/Services/...` (or `Platform.*.Infrastructure`) |
| New API endpoint | `ERP.API/Controllers/...` |
| EF configuration | `ERP.Infrastracture/DBConfiguration/Config/...` (legacy) or `{Module}.Infrastructure/Persistence/` (modules) |
| Base entity / API envelope | `BuildingBlocks/ERP.BuildingBlocks.{Domain,Application}` |
| Audit / cache / localization | `Modules/Platform/Platform.{Audit,Caching,Localization}/` |

## BuildingBlocks vs Shared vs Platform

| Project | Owns | Do not put here |
|---------|------|-----------------|
| **BuildingBlocks.Domain** | `BaseEntity`, tree bases, `Attachment` | Business entities, Identity |
| **BuildingBlocks.Application** | `ApiResponse`, `PaginatedResult`, CQRS markers, `ValidationBehavior`, exceptions | Module-specific DTOs |
| **Shared** (thin) | Filter DTOs, message key constants | Anything moved to BuildingBlocks, Platform, or Identity |
| **Identity.*** | `ApplicationUser`, auth services, `ICurrentUser` | Accounting/Inventory business logic |
| **Platform.*** | Audit, caching, localization infrastructure | Accounting/Inventory business logic |

Namespaces in BuildingBlocks still use `Shared.*` during transition to avoid mass import churn.

## Module EF and migrations (Phase 2+)

- **Module owns** `IEntityTypeConfiguration<>` classes (e.g. `Accounting.Infrastructure/Persistence/TaxDbConfig.cs`).
- **Module registers** configs via `ModuleModelConfigurationRegistry.Register(...)` inside `Add{Module}Module()` or `AddPlatformModules()`.
- **Host owns** migrations in `ERP.API/Migrations/` until `ERP.Persistence` exists.
- **Design-time:** `ERP.API/Persistence/ApplicationDbContextFactory.cs` bootstraps module configs without full DI startup (required for `dotnet ef`).

```bash
dotnet ef dbcontext info --project ERP.API --startup-project ERP.API
dotnet ef migrations add <Name> --project ERP.API --startup-project ERP.API
```

After adding a module aggregate, run a dry-run migration and confirm existing tables are not dropped (entity CLR type may change while table name stays the same).

## Host composition (Phase 3+)

```csharp
builder.Services.AddInfrastructureServices();
builder.Services.AddAccountingModule();
builder.Services.AddPlatformModules();   // Audit, Caching, Localization
builder.Services.AddProjectUtilities();
```

## Dependency rules (enforced by `ERP.Architecture.Tests`)

| Layer | Must NOT reference |
|-------|-------------------|
| `ERP.Domain` | `ERP.Infrastracture`, `ERP.API` |
| `ERP.Application` | `ERP.Infrastracture`, `ERP.API` |
| `ERP.Infrastracture` | `ERP.API` |
| `Shared` | `ERP.Domain`, `ERP.Application`, `ERP.Infrastracture` |
| `ERP.BuildingBlocks.Domain` | `ERP.Domain`, `ERP.Application`, `ERP.Infrastracture` |
| `ERP.BuildingBlocks.Application` | `ERP.Domain`, `ERP.Infrastracture` |

Run architecture tests before opening a PR:

```bash
dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj
```

## Phase 0 freeze rules

Until module extraction (Phase 3):

1. **Do not add new repositories to `IUnitOfWork`** — use dedicated repository injection instead.
2. **Do not register MediatR outside `WebBuilderExtensions`** — single composition root.
3. **Do not add cross-module entity references** in Inventory services to Account entities (Phase 1 will introduce `IAccountingIntegrationService`).

## Pull request checklist

- [ ] Code builds: `dotnet build`
- [ ] Unit tests pass: `dotnet test`
- [ ] Architecture tests pass: `dotnet test Tests/ERP.Architecture.Tests`
- [ ] No new `IUnitOfWork` repository properties
- [ ] Validators added for new commands
- [ ] No duplicate DI registrations in `InfrastractureExtensions`

## Module owners

See [MODULE_OWNERS.md](./MODULE_OWNERS.md).
