# MediatR registration audit (Phase 0 — task 0.2.1)

**Date:** July 2026  
**Status:** Consolidated to single registration point

## Summary

| Item | Location | Count |
|------|----------|-------|
| Command handlers | `ERP.Infrastracture/Handlers/` | 68 handler classes |
| Validators | `ERP.Application/Validators/` | 83 validator classes |
| MediatR registration | `ERP.API/Utilities/WebBuilderExtensions.cs` | **Single point** |
| Pipeline behavior | `ValidationBehavior<,>` via `AddOpenBehavior` | 1 |

## Before Phase 0

MediatR was registered twice:

1. `WebBuilderExtensions.AddProjectUtilities()` — `ApplicationDbContext` assembly (Infrastructure)
2. `InfrasctructureExtensions.AddInfrastructureServices()` — Infrastructure assembly

`ValidationBehavior` was also registered as both `AddTransient` and `AddOpenBehavior`.

## After Phase 0

```csharp
// ERP.API/Utilities/WebBuilderExtensions.cs
services.AddValidatorsFromAssembly(typeof(IApplicationDbContext).Assembly);
services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(InfrasctructureExtensions).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
```

`InfrasctructureExtensions` no longer calls `AddMediatR`.

## Handler distribution

| Area | Path | Approx. handlers |
|------|------|------------------|
| Account | `Handlers/Account/` | 42 |
| Inventory | `Handlers/Inventory/` | 26 |

## Automated verification

- `MediatRRegistrationTests` in `ERP.Architecture.Tests`
- ADR-004: [adr/004-mediatr-single-write-path.md](./adr/004-mediatr-single-write-path.md)

## Rules going forward

1. New handlers go in `ERP.Infrastracture/Handlers/{Module}/`
2. New validators go in `ERP.Application/Validators/`
3. Do **not** add `AddMediatR` outside `WebBuilderExtensions`
