# Platform module

Cross-cutting capabilities shared by all business modules: **audit trail**, **in-memory caching**, and **localization** (en-US / ar-EG). Platform has no business aggregates; it extends the host through composable sub-modules.

## Purpose

- **Audit** — Persist change audit logs via `IAuditService`; EF config for `AuditLog`.
- **Caching** — `ICacheService` abstraction (in-memory implementation).
- **Localization** — Resource files and response localization middleware.
- Single entry point: `AddPlatformModules()` from `Platform.Infrastructure`.

## Project map

Platform is split into sub-modules; each follows Application + Infrastructure (Localization is a single project).

| Sub-module | Projects | Composition |
|------------|----------|-------------|
| **Audit** | `Platform.Audit.Application`, `Platform.Audit.Infrastructure` | `AddPlatformAudit()` |
| **Caching** | `Platform.Caching.Application`, `Platform.Caching.Infrastructure` | `AddPlatformCaching()` |
| **Localization** | `Platform.Localization` | `AddPlatformLocalization()` |
| **Facade** | `Platform.Infrastructure` | `AddPlatformModules()` — calls all three |

```
Modules/Platform/
├── Platform.Infrastructure/PlatformModuleExtensions.cs
├── Platform.Audit/
│   ├── Platform.Audit.Application/     ← IAuditService
│   └── Platform.Audit.Infrastructure/
│       ├── Services/AuditService.cs
│       └── Persistence/AuditLogDbConfig.cs
├── Platform.Caching/
│   ├── Platform.Caching.Application/   ← ICacheService
│   └── Platform.Caching.Infrastructure/
└── Platform.Localization/
    ├── Resources/                      ← .resx en-US, ar-EG
    └── PlatformLocalizationModuleExtensions.cs
```

**Host wiring:**

```csharp
builder.Services.AddPlatformModules();   // before AddIdentityModule()
```

**Swagger group:** `platform` (for any Platform-tagged controllers; most Platform code is middleware/services).

## How to add a feature

### New cross-cutting concern (new sub-module)

1. Create `{Name}.Application` (interfaces) and `{Name}.Infrastructure` (implementation).
2. Add `{Name}ModuleExtensions.cs` with `Add{Name}()` — register services and `ModuleModelConfigurationRegistry` if EF entities are involved.
3. Call from `PlatformModuleExtensions.AddPlatformModules()`.
4. Reference new Infrastructure from `ERP.API` only if the host must compose it (follow Audit pattern).

### Extend existing sub-module

| Area | Steps |
|------|--------|
| Audit | Extend `IAuditService` / `AuditService`; update `AuditLogDbConfig` if schema changes |
| Caching | Extend `ICacheService` or swap implementation in `Platform.Caching.Infrastructure` |
| Localization | Add keys to `Platform.Localization/Resources/`; configure cultures in `WebBuilderExtensions` |

**MediatR:** Platform does not own command handlers. Middleware and services are registered in module extensions.

## Architecture rules

| Rule | Detail |
|------|--------|
| No domain logic | Platform must not encode Accounting or Inventory rules. |
| Identity for actor | Audit uses `ICurrentUser` from Identity — reference `Identity.Application`, not controllers. |
| EF configs | Register via `ModuleModelConfigurationRegistry` inside each `AddPlatform*()` method. |
| Host-only HTTP | New REST endpoints for platform concerns go in `ERP.API` with Swagger group `platform`. |

## Key integration points

| Service | Used by |
|---------|---------|
| `IAuditService` | Application services that record business audit events |
| `ICacheService` | Read-heavy lookups (module services via DI) |
| `ICurrentUser` | **Consumed** by `AuditService` (Identity module) |
| Localization middleware | Pipeline in `WebBuilderExtensions` / host startup |

**Not in Platform:** `IAccountingIntegrationService`, `ITransactionCoordinator`.

## Tests

```bash
dotnet build AAA.ERP.sln -c Release
dotnet test AAA.ERP.sln -c Release

dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj -c Release
```

## Related docs

- [CONTRIBUTING.md — BuildingBlocks vs Platform](../../docs/architecture/CONTRIBUTING.md#buildingblocks-vs-shared-vs-platform)
- [MODULE_OWNERS.md](../../docs/architecture/MODULE_OWNERS.md)
