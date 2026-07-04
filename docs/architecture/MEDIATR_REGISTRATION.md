# MediatR registration audit (Phase 0 — task 0.2.1)

**Date:** July 2026  
**Status:** Host registers Infrastructure + Accounting module assemblies

## Summary

| Item | Location | Count |
|------|----------|-------|
| Command handlers (horizontal) | `ERP.Infrastracture/Handlers/` | ~68 handler classes |
| Command handlers (Accounting module) | `Accounting.Infrastructure/Handlers/` | Tax + future aggregates |
| Validators (horizontal) | `ERP.Application/Validators/` | ~83 validator classes |
| Validators (Accounting module) | `Accounting.Application/Validators/` | Tax + future aggregates |
| MediatR registration | `ERP.API/Utilities/WebBuilderExtensions.cs` | **Single composition point** |
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

## Phase 2 — Accounting module (Tax pilot)

The host still has a **single** `AddMediatR` call; it registers **two** handler assemblies:

```csharp
services.AddValidatorsFromAssembly(typeof(IApplicationDbContext).Assembly);
services.AddValidatorsFromAssembly(typeof(TaxCreateValidator).Assembly);
services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(InfrasctructureExtensions).Assembly);
    config.RegisterServicesFromAssembly(typeof(AccountingModuleExtensions).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
```

New module handlers and validators live under `Modules/{Module}/`; the host wires their assemblies at composition time. Do **not** add a third `AddMediatR` call in module extensions.

## Handler distribution

| Area | Path | Approx. handlers |
|------|------|------------------|
| Account | `Handlers/Account/` | 42 |
| Inventory | `Handlers/Inventory/` | 26 |
| Accounting (module) | `Accounting.Infrastructure/Handlers/` | 2 (Tax) |

## Automated verification

- `MediatRRegistrationTests` in `ERP.Architecture.Tests`
- ADR-004: [adr/004-mediatr-single-write-path.md](./adr/004-mediatr-single-write-path.md)

## Rules going forward

1. New handlers in horizontal layers → `ERP.Infrastracture/Handlers/{Module}/`
2. New handlers in extracted modules → `{Module}.Infrastructure/Handlers/`
3. New validators → matching `*.Application/Validators/` assembly
4. Register new module assemblies in `WebBuilderExtensions` only — never call `AddMediatR` inside `Add{Module}Module()`
