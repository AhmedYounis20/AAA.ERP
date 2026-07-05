# AAA.ERP — Solution diagram

> **Last updated:** July 2026 — post Increment **6b** (`ERP.Host` rename)  
> **Canonical plan:** [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md)

## Project layout (actual)

```
AAA.ERP/
├── ERP.Host/                    # Composition root — HTTP, Swagger, EF migrations
├── ERP.Persistence/             # Shared EF kernel (ApplicationDbContext, base services)
├── BuildingBlocks/
│   ├── ERP.BuildingBlocks.Domain/
│   ├── ERP.BuildingBlocks.Application/
│   └── ERP.BuildingBlocks.Persistence/
├── Modules/
│   ├── Accounting/              # Domain · Application · Infrastructure
│   ├── Inventory/
│   ├── Identity/
│   └── Platform/                # Audit · Caching · Localization · Infrastructure
├── Shared/                      # Cross-cutting DTOs, constants, resources
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
        H[Controllers · Program · Migrations]
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

    BB[BuildingBlocks]

    DB[(SQL Server)]

    H --> ACC & INV & IDN & PLT & P
    P --> ACC & INV & IDN & PLT
    ACC --> ACCA --> ACCD --> BB
    INV --> INVA --> INVD --> BB
    IDN --> IDNA --> IDND --> BB
    PLT --> BB
    ACC & INV & IDN & PLT --> DB
```

## Module integration (cross-cutting)

```mermaid
sequenceDiagram
    participant Client
    participant Host as ERP.Host
    participant Inv as Inventory.Infrastructure
    participant Acc as Accounting.Infrastructure
    participant DB as SQL Server

    Client->>Host: POST /api/ImportTransactions
    Host->>Inv: InventoryTransactionService
    Inv->>DB: Stock update
    Inv->>Acc: IAccountingIntegrationService
    Acc->>DB: GL entry (same transaction via ITransactionCoordinator)
    Inv-->>Host: Result
    Host-->>Client: JSON response
```

## Enforcement

| Guard | Project |
|-------|---------|
| Layer dependencies | `Tests/ERP.Architecture.Tests/LayerDependencyTests.cs` |
| Module boundaries | `Tests/ERP.Architecture.Tests/ModuleBoundaryTests.cs` |
| Namespace debt | `Tests/ERP.Architecture.Tests/NamespaceDebtTests.cs` |

## EF migrations

Migrations live in **`ERP.Host/Migrations/`**. Design-time and runtime both use `MigrationsAssembly=ERP.Host`.

```bash
dotnet ef migrations add <Name> --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
dotnet ef database update --project ERP.Host/ERP.Host.csproj --startup-project ERP.Host/ERP.Host.csproj
```
