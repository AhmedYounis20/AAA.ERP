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
- [ ] **No nullable reference warnings (CS86\*\*) in changed `.cs` files** — enforced on PR via `scripts/check-nullable-on-changed-files.sh`
- [ ] **No new business controllers in `ERP.Host`** — use `{Module}.Api` ([MODULE_API_TEMPLATE.md](docs/architecture/MODULE_API_TEMPLATE.md))
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
