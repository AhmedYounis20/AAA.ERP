## Summary

<!-- What does this PR do? -->

## Type of change

- [ ] Feature
- [ ] Bug fix
- [ ] Refactor
- [ ] Architecture / modular monolith migration

## Architecture checklist (Phase 0+)

- [ ] `dotnet build` succeeds
- [ ] `dotnet test` succeeds (unit + architecture + smoke)
- [ ] **No new repositories added to `IUnitOfWork`** (use dedicated repository injection)
- [ ] MediatR registered only in `WebBuilderExtensions` (not in Infrastructure)
- [ ] No duplicate DI registrations in `InfrasctructureExtensions`
- [ ] New commands have FluentValidation validators
- [ ] Cross-module changes reviewed by both module owners (if applicable)

## Module

- [ ] Accounting
- [ ] Inventory
- [ ] Platform / Identity
- [ ] Host / CI

## Test plan

<!-- How was this tested? -->
