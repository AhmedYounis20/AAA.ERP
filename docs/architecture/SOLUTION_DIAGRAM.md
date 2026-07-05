# AAA.ERP — Solution diagram

> **Last updated:** July 2026 — module-owned API migration complete ([PHASE_5_API_MIGRATION_SIGNOFF](./PHASE_5_API_MIGRATION_SIGNOFF.md))  
> **Migration history:** [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md)  
> **Active plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md)

## Project layout (actual)

Solution root is **flat** (no `src/` folder). Retired horizontal layers (`ERP.Application`, `ERP.Domain`, `ERP.Infrastracture`) and `ERP.Host/Controllers/` have been removed.

```
AAA.ERP/
├── ERP.Host/                    # Composition root — startup, Swagger, EF migrations, Dev/DbWork
├── ERP.Persistence/             # Shared EF kernel (ApplicationDbContext, base services)
├── BuildingBlocks/
│   ├── ERP.BuildingBlocks.Domain/
│   ├── ERP.BuildingBlocks.Application/
│   ├── ERP.BuildingBlocks.Api/      # Shared CRUD controller bases
│   ├── ERP.BuildingBlocks.Persistence/
│   └── ERP.Shared/                  # Solution folder; physical path: Shared/
├── Modules/
│   ├── Accounting/              # Domain · Application · Infrastructure · Contracts · Api (21)
│   ├── Inventory/               # Domain · Application · Infrastructure · Api (14)
│   ├── Identity/                # Domain · Application · Infrastructure · Api (Auth)
│   └── Platform/                # Audit.Api · Audit · Caching · Localization · Infrastructure
└── Tests/
    ├── ERP.Architecture.Tests/
    ├── ERP.IntegrationTests/
    ├── ERP.SmokeTests/
    └── ERP.UnitTests/
```

## Dependency flow

```mermaid
graph TB
    subgraph Host["ERP.Host"]
        H[Program · Middleware · Migrations · Dev/DbWork]
    end

    subgraph Api["*.Api (HTTP surface)"]
        ACCAPI[Accounting.Api]
        INVAPI[Inventory.Api]
        IDNAPI[Identity.Api]
        PLTAPI[Platform.Audit.Api]
    end

    subgraph Persistence["ERP.Persistence"]
        P[ApplicationDbContext · Base services · Seed]
    end

    subgraph Modules
        ACC[Accounting.Infrastructure]
        INV[Inventory.Infrastructure]
        IDN[Identity.Infrastructure]
        PLT[Platform.Infrastructure]
    end

    subgraph Application["*.Application ports"]
        ACCA[Accounting.Application]
        INVA[Inventory.Application]
        IDNA[Identity.Application]
    end

    subgraph Domain["*.Domain"]
        ACCD[Accounting.Domain]
        INVD[Inventory.Domain]
        IDND[Identity.Domain]
    end

    BBAPI[ERP.BuildingBlocks.Api]
    Contracts[Accounting.Contracts]
    BB[BuildingBlocks + ERP.Shared]

    DB[(SQL Server)]

    H --> ACCAPI & INVAPI & IDNAPI & PLTAPI & ACC & INV & IDN & PLT & P
    ACCAPI & INVAPI & IDNAPI & PLTAPI --> BBAPI
    ACCAPI --> ACCA
    INVAPI --> INVA
    IDNAPI --> IDNA
    PLTAPI --> BB
    ACC --> ACCA --> ACCD --> BB
    INV --> INVA --> INVD --> BB
    INVA -.-> Contracts
    IDN --> IDNA --> IDND --> BB
    PLT --> BB
    P --> ACC & INV & IDN & PLT
    ACC & INV & IDN & PLT --> DB
```

## Module integration (cross-cutting)

```mermaid
sequenceDiagram
    participant Client
    participant Host as ERP.Host
    participant Api as Inventory.Api
    participant Inv as Inventory.Infrastructure
    participant Acc as Accounting.Infrastructure
    participant DB as SQL Server

    Client->>Host: POST /api/ImportTransactions
    Host->>Api: ImportTransactionsController
    Api->>Inv: InventoryTransactionService
    Inv->>DB: Stock update
    Inv->>Acc: IAccountingIntegrationService
    Acc->>DB: GL entry (same transaction via ITransactionCoordinator)
    Inv-->>Api: Result
    Api-->>Host: Result
    Host-->>Client: JSON response
```

## Enforcement

| Guard | Project |
|-------|---------|
| Layer dependencies | `Tests/ERP.Architecture.Tests/LayerDependencyTests.cs` |
| Module boundaries | `Tests/ERP.Architecture.Tests/ModuleBoundaryTests.cs` |
| Module API isolation | `Tests/ERP.Architecture.Tests/ModuleApiLayerTests.cs` |
| Namespace debt | `Tests/ERP.Architecture.Tests/NamespaceDebtTests.cs` |

## EF migrations

Migrations live in **`ERP.Host/Migrations/`**. Design-time and runtime both use `MigrationsAssembly=ERP.Host`.

```bash
dotnet ef migrations add <Name> --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
dotnet ef database update --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
```
