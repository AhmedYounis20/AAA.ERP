# Module API template

Playbook for creating a `{Module}.Api` project.  
**ADR:** [011-module-owned-api.md](./adr/011-module-owned-api.md)  
**Migration map:** [CONTROLLER_MIGRATION_MAP.md](./CONTROLLER_MIGRATION_MAP.md)  
**Full plan:** [MODULE_API_MIGRATION_PLAN.md](./MODULE_API_MIGRATION_PLAN.md)

---

## When to use

- Moving an existing controller from `ERP.Host/Controllers/` to a module
- Adding a **new** HTTP endpoint (never add business controllers to Host after Phase 0)

**Reference implementation:** `Modules/Identity/Identity.Api/` (Phase 0 scaffold).

---

## Project layout

```
Modules/{Module}/
├── {Module}.Api/
│   ├── {Module}.Api.csproj
│   ├── {Module}ApiAssemblyMarker.cs
│   └── Controllers/
│       └── {Feature}/
│           └── {Aggregate}Controller.cs
├── {Module}.Application/
├── {Module}.Domain/
└── {Module}.Infrastructure/
```

---

## Step 1 — Create the project

`{Module}.Api.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{Module}.Api</RootNamespace>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\{Module}.Application\{Module}.Application.csproj" />
    <ProjectReference Include="..\..\..\BuildingBlocks\ERP.BuildingBlocks.Api\ERP.BuildingBlocks.Api.csproj" />
  </ItemGroup>
</Project>
```

**Do not** reference `{Module}.Infrastructure` or `{Module}.Domain`.

---

## Step 2 — Assembly marker

```csharp
using System.Reflection;

namespace {Module}.Api;

public static class {Module}ApiAssemblyMarker
{
    public static readonly Assembly Assembly = typeof({Module}ApiAssemblyMarker).Assembly;
}
```

---

## Step 3 — Move controller

1. Copy controller from `ERP.Host/Controllers/...` to `{Module}.Api/Controllers/{Feature}/`.
2. Update namespace to `{Module}.Api.Controllers.{Feature}`.
3. Keep `[Route("api/[controller]")]` **unchanged** for API stability.
4. Inherit from `ERP.BuildingBlocks.Api.Controllers.BaseController<,,>` when applicable.
5. Inject only `{Module}.Application` interfaces and `ISender` — never repositories or DbContext.

---

## Step 4 — Wire Host

**`ERP.Host.csproj`** — add project reference:

```xml
<ProjectReference Include="..\Modules\{Module}\{Module}.Api\{Module}.Api.csproj" />
```

**`WebBuilderExtensions.cs`** — register application part:

```csharp
using {Module}.Api;

builder.Services.AddControllers()
    .AddApplicationPart({Module}ApiAssemblyMarker.Assembly)
    // ...existing parts...
```

---

## Step 5 — Delete from Host

Remove the controller file from `ERP.Host/Controllers/`. Update the ratchet test baseline in `ModuleApiLayerTests.Host_business_controller_count_baseline_for_ratchet`.

---

## Step 6 — Tests

```bash
dotnet build AAA.ERP.sln -c Release
dotnet test Tests/ERP.Architecture.Tests/ERP.Architecture.Tests.csproj
dotnet test Tests/ERP.SmokeTests/ERP.SmokeTests.csproj
```

Add smoke coverage if the endpoint is security-sensitive (auth, dev-only, etc.).

---

## Dependency rules

| From | May reference | Must NOT reference |
|------|---------------|-------------------|
| `{Module}.Api` | `{Module}.Application`, `ERP.BuildingBlocks.Api`, `ERP.BuildingBlocks.Application` | `{Module}.Domain`, `{Module}.Infrastructure`, `ERP.Persistence`, other modules |

---

## PR checklist

```
[ ] {Module}.Api project created or controller moved
[ ] Host references {Module}.Api + AddApplicationPart wired
[ ] Controller removed from ERP.Host/Controllers/
[ ] [Route] attributes unchanged (API stability)
[ ] Architecture tests pass (ModuleApiLayerTests)
[ ] Ratchet baseline updated if controller count changed
[ ] Smoke test for moved endpoint (if applicable)
```
