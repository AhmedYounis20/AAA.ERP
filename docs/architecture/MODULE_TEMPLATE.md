# Module extraction template

Step-by-step playbook for moving one aggregate from horizontal `ERP.*` layers into a module project structure.  
**Reference implementation:** Taxes in `Modules/Accounting/` (Phase 2 pilot, commit `e22702b`–`4a40ebf`).

Full roadmap: [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md) · EF pattern: [CONTRIBUTING.md](./CONTRIBUTING.md) · MediatR: [MEDIATR_REGISTRATION.md](./MEDIATR_REGISTRATION.md)

---

## When to use this template

| Good pilot candidates | Poor pilot candidates |
|-----------------------|----------------------|
| Own controller + CRUD API | Child entities owned by a parent aggregate (e.g. ExpiryLevels on Product/Variant) |
| Clear module boundary | Heavy cross-module entity references |
| Existing MediatR commands | God-service with 20+ responsibilities |
| Stable schema / table name | Aggregate mid-refactor |

**Suggested Phase 3 order:** simple lookup aggregates first (Currencies, Colors), then trees (ChartOfAccounts), then transaction clusters (Entries).

---

## Target layout (per module)

```
Modules/{ModuleName}/
├── {Module}.Domain/           # Entities only (+ transitional ERP.Domain refs OK)
├── {Module}.Application/      # Commands, validators, repository/service interfaces
├── {Module}.Contracts/        # Cross-module DTOs + integration facades (optional until needed)
└── {Module}.Infrastructure/   # Handlers, repos, services, EF configs, Add{Module}Module()
```

**Host (unchanged per aggregate):**

```
ERP.Host/
├── Controllers/               # Stays in host until Phase 3+ controller move
├── Migrations/                # Host owns migrations until ERP.Persistence exists
└── Persistence/
    └── ApplicationDbContextFactory.cs   # Register module EF configs for dotnet ef
```

---

## Checklist (copy per PR)

```
[ ] 1. Scaffold / confirm module projects exist
[ ] 2. Move entity → {Module}.Domain
[ ] 3. Move commands + validators → {Module}.Application
[ ] 4. Move handler, repo, service, DbConfig → {Module}.Infrastructure
[ ] 5. Wire Add{Module}Module() + host (MediatR, validators, EF registry)
[ ] 6. Remove old files + DI from horizontal layers
[ ] 7. Update ApplicationDbContextFactory if new module
[ ] 8. Arch tests + integration/smoke test
[ ] 9. Migration drift dry-run (no table drops)
[ ] 10. dotnet build + dotnet test green
```

**Effort guide:** 1–2 person-days per simple aggregate; 3–5 pd for Entries-sized clusters.

---

## Step 1 — Module projects

Create four SDK projects under `Modules/{Module}/` and add them to `AAA.ERP.sln`.

**Project references (Accounting pattern):**

| Project | References |
|---------|------------|
| `{Module}.Domain` | `ERP.Domain` (transitional — shared base entities) |
| `{Module}.Application` | Own Domain, `ERP.Application`, `ERP.Domain` |
| `{Module}.Contracts` | BuildingBlocks / Shared only when possible |
| `{Module}.Infrastructure` | Own Application + Domain, `ERP.Infrastracture` (strangler — base repos) |
| `ERP.Host` | `{Module}.Infrastructure` only (composition root) |

Do **not** reference `{Module}.Infrastructure` from `ERP.Application` or `ERP.Domain`.

---

## Step 2 — Move entity

```
ERP.Domain/Models/Entities/.../Tax.cs
  → Modules/Accounting/Accounting.Domain/Entities/Tax.cs
```

- Keep **table name** unchanged in DbConfig (`ToTable("Taxes")`).
- Transitional: entity may inherit from types still in `ERP.Domain` (e.g. `SubLeadgerBaseEntity<Tax>`).
- Namespace: `{Module}.Domain.Entities`.

---

## Step 3 — Application layer

Move per aggregate:

| Artifact | Destination |
|----------|-------------|
| `*CreateCommand`, `*UpdateCommand` | `{Module}.Application/Commands/{Aggregate}/` |
| `*Validator` | `{Module}.Application/Validators/{Aggregate}/` |
| `I*Repository` | `{Module}.Application/Repositories/` |
| `I*Service` | `{Module}.Application/Services/` |

Commands may still reference shared command bases in `ERP.Domain` during transition — that is OK.

---

## Step 4 — Infrastructure layer

| Artifact | Destination |
|----------|-------------|
| `*CommandHandler` | `{Module}.Infrastructure/Handlers/{Aggregate}/` |
| `*Repository` | `{Module}.Infrastructure/Repositories/` |
| `*Service` | `{Module}.Infrastructure/Services/` |
| `*DbConfig` | `{Module}.Infrastructure/Persistence/` |

**DbConfig example** (`TaxDbConfig.cs`):

- Inherit existing base config from `ERP.Infrastracture` if needed.
- Explicit `ToTable("OriginalTableName")` — never rely on CLR type name for table mapping.

---

## Step 5 — Module composition root

Create `{Module}ModuleExtensions.cs` in Infrastructure:

```csharp
public static IServiceCollection AddAccountingModule(this IServiceCollection services)
{
    ModuleModelConfigurationRegistry.Register(ApplyAccountingModuleConfigurations);

    services.AddScoped<ITaxRepository, TaxRepository>();
    services.AddScoped<ITaxService, TaxService>();

    return services;
}

public static void ApplyAccountingModuleConfigurations(this ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new TaxDbConfig());
}
```

**Rules:**

- Register **DI** inside `Add{Module}Module()`.
- Do **not** call `AddMediatR` inside the module — host registers handler assemblies.
- `ModuleModelConfigurationRegistry.Register` is idempotent; test factories may call `Clear()` if needed.

---

## Step 6 — Host wiring (`ERP.Host`)

**`WebBuilderExtensions.BuildApplication`:**

```csharp
builder.Services.AddInfrastructureServices();
builder.Services.AddAccountingModule();   // after infra, before utilities
builder.Services.AddProjectUtilities();
```

**`AddProjectUtilities` — MediatR + validators (single composition point):**

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

**Controllers:** update `using` to `{Module}.Application.Commands` and `{Module}.Domain.Entities`. Controller stays in `ERP.Host` for now.

---

## Step 7 — Remove horizontal duplicates

Delete moved files from `ERP.Domain`, `ERP.Application`, `ERP.Infrastracture`.

Remove old DI registrations from `InfrasctructureExtensions.cs` (search aggregate name).

**Do not** add new repositories to `IUnitOfWork` — inject `I*Repository` directly.

---

## Step 8 — EF design-time

`ApplicationDbContext` applies module configs via:

```csharp
foreach (var configure in ModuleModelConfigurationRegistry.Configurators)
    configure(builder);
```

For `dotnet ef`, bootstrap configs in `ERP.Host/Persistence/ApplicationDbContextFactory.cs`:

```csharp
ModuleModelConfigurationRegistry.Clear();
ModuleModelConfigurationRegistry.Register(AccountingModuleExtensions.ApplyAccountingModuleConfigurations);
// Add future modules here until a shared design-time bootstrapper exists
```

Verify:

```bash
dotnet ef dbcontext info --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
```

---

## Step 9 — Migration drift check

After moving an entity, CLR type in the snapshot may change while the **table name stays the same**.

```bash
dotnet ef migrations add {Aggregate}EntityMoveCheck --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
```

Review the generated migration:

- **Accept:** empty `Up()` or snapshot-only rename.
- **Reject / investigate:** `DropTable("YourTable")`, unexpected FK changes.
- Remove verification migration if no real schema change: `dotnet ef migrations remove ...`

---

## Step 10 — Tests

| Test type | What to add |
|-----------|-------------|
| Architecture | Rules in `ModuleBoundaryTests.cs` — Domain/Application/Infrastructure dependency direction |
| Integration | MediatR create/read via `IntegrationTestWebApplicationFactory` (see `TaxIntegrationTests`) |
| Smoke | Protected endpoint returns 401 without auth (`ApiSmokeTests`) |

```bash
dotnet test AAA.ERP.sln -c Release
dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj
```

---

## Architecture rules (enforce in CI)

From [MODULAR_MONOLITH.md §4.3](./MODULAR_MONOLITH.md#43-dependency-rules):

| Layer | May reference | Must NOT reference |
|-------|---------------|-------------------|
| `{Module}.Domain` | Shared / ERP.Domain bases (transitional) | EF, MediatR, other modules' Domain, Infrastructure |
| `{Module}.Application` | Own Domain, Contracts, ERP.Application | Infrastructure, API |
| `{Module}.Infrastructure` | Own Application/Domain, ERP.Infrastracture bases | API, other modules' Domain |
| `ERP.Host` | All `*.Infrastructure` | — |

Copy the Accounting rules in `Tests/ERP.Architecture.Tests/ModuleBoundaryTests.cs` when adding Inventory module.

---

## Inventory module (Phase 3)

When creating `Modules/Inventory/`, mirror Accounting:

1. `AddInventoryModule()` + `ApplyInventoryModuleConfigurations`
2. Register handler assembly in `WebBuilderExtensions`
3. Extend `ApplicationDbContextFactory` with Inventory configs
4. Add Inventory arch tests (Domain must not reference Account entities where enforced)

**ExpiryLevels deferred:** managed only through Product/Variant services — extract with Products/Variants in Wave 3D, not as a standalone micro-pilot.

---

## PR description template

```markdown
## Module extraction: {Aggregate} → {Module}

- [ ] Entity + commands in {Module}.Domain / .Application
- [ ] Handler, repo, service, DbConfig in {Module}.Infrastructure
- [ ] Add{Module}Module() wired; old horizontal code removed
- [ ] EF design-time factory updated (if new module)
- [ ] Migration drift check: no table drops
- [ ] Arch + integration tests
- [ ] dotnet build / dotnet test green

**API:** No breaking changes to routes or DTOs.
```

---

## Related commits (Tax pilot)

| Commit | Milestone |
|--------|-----------|
| `e22702b` | Scaffold Accounting + Tax vertical slice |
| `4a40ebf` | EF design-time, arch rules, Tax integration test |
