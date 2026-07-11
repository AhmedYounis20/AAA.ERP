# Identity module

Authentication services and the **current-user abstraction** used for audit fields and authorization context. ASP.NET Identity (`ApplicationUser`, JWT, `AddIdentity<>`) remains in the **host** (`ERP.Host`); this module owns application-level auth ports and implementations.

## Purpose

- Provide `IAuthenticationService` (login/token flows) and `ICurrentUser` (request-scoped user id) without coupling business modules to `HttpContext`.
- Supply `ApplicationUser` entity type for EF Identity stores.
- Wire via `AddIdentityModule()` — called after `AddPlatformModules()` in the host.

## Project map

| Project | Role |
|---------|------|
| `Identity.Domain` | `ApplicationUser` and identity-related domain types. |
| `Identity.Application` | `IAuthenticationService`, `ICurrentUser`, auth DTOs/models. |
| `Identity.Infrastructure` | `AuthenticationService`, `CurrentUserService`, `IdentityModuleExtensions`. |

```
Modules/Identity/
├── Identity.Domain/
├── Identity.Application/
│   ├── Services/          ← IAuthenticationService, ICurrentUser
│   └── Models/Dtos/
└── Identity.Infrastructure/
    ├── Services/
    └── IdentityModuleExtensions.cs
```

**Not in this module (host-owned):**

- JWT configuration — `WebBuilderExtensions.AddAuthenticationConfiguration`
- `AddIdentity<ApplicationUser, IdentityRole>()` — `AddProjectUtilities`
- Controllers — `ERP.Host/Controllers/Identity/`

**Swagger group:** `identity`.

## How to add a feature

Typical identity changes are small and host-adjacent:

1. **DTO / contract** — Add to `Identity.Application/Models/Dtos/` or extend `IAuthenticationService`.
2. **Implementation** — `Identity.Infrastructure/Services/`; register in `IdentityModuleExtensions.AddIdentityModule()`.
3. **API** — New endpoint in `ERP.Host/Controllers/Identity/`; use MediatR only if introducing a command-style write (most auth flows call `IAuthenticationService` directly).
4. **Audit integration** — `ICurrentUser` is consumed by `ApplicationDbContext` and `Platform.Audit` `AuditService`; extend `ICurrentUser` only when all consumers are updated.

Identity does **not** own MediatR handler assemblies today; no validator assembly registration is required unless you add commands under a new Application area.

## Architecture rules

| Rule | Detail |
|------|--------|
| No business logic | Identity must not contain Accounting or Inventory rules. |
| Host composition | JWT secrets, Identity stores, and `ApplicationDbContext` wiring stay in `ERP.Host`. |
| `ICurrentUser` scope | Registered **scoped**; safe for EF `SaveChanges` audit stamping. |
| Module boundaries | Do not reference `ERP.Host` from `Identity.Infrastructure`. |

## Key integration points

| Consumer | Integration |
|----------|-------------|
| `ApplicationDbContext` | Injects `ICurrentUser?` for `CreatedBy` / `ModifiedBy` on `BaseEntity` |
| `Platform.Audit` | `AuditService` uses `ICurrentUser` for actor id |
| Controllers | `IAuthenticationService` for login/register; `[Authorize]` on module controllers |

**Not used in Identity:** `IAccountingIntegrationService`, `ITransactionCoordinator` — document here so cross-module searches stay clear.

## Tests

```bash
dotnet build AAA.ERP.sln -c Release
dotnet test AAA.ERP.sln -c Release

dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj -c Release
```

Add integration tests under `Tests/ERP.IntegrationTests/` when changing token issuance or auth-critical paths.

## Related docs

- [MODULAR_MONOLITH.md — Wave 3B](../../docs/architecture/MODULAR_MONOLITH.md#phase-3-wave-3b--complete)
- [MODULE_OWNERS.md](../../docs/architecture/MODULE_OWNERS.md)
- [CONTRIBUTING.md](../../docs/architecture/CONTRIBUTING.md)
