# Modular Monolith Architecture Plan — AAA.ERP

> **Status:** Proposed  
> **Last updated:** July 2026 — **Phase 0 complete (M0)**  
> **Audience:** Developers, solution architects, tech leads

---

## Table of contents

1. [Executive summary](#1-executive-summary)
2. [Context and business drivers](#2-context-and-business-drivers)
3. [Current architecture assessment](#3-current-architecture-assessment)
4. [Target architecture](#4-target-architecture)
5. [Module definitions](#5-module-definitions)
6. [Data architecture](#6-data-architecture)
7. [Application layer standardization](#7-application-layer-standardization)
8. [Integration patterns](#8-integration-patterns)
9. [API and frontend stability](#9-api-and-frontend-stability)
10. [Migration roadmap](#10-migration-roadmap)
    - [10.0 Planning assumptions](#100-planning-assumptions)
    - [10.1 Master timeline](#101-master-timeline)
    - [10.2 Phase 0 — Foundation](#102-phase-0--foundation-and-governance)
    - [10.3 Phase 1 — Integration facade](#103-phase-1--accounting-integration-facade)
    - [10.4 Phase 2 — Pilot module](#104-phase-2--vertical-slice-pilot)
    - [10.5 Phase 3 — Module extraction](#105-phase-3--module-extraction)
    - [10.6 Phase 4 — Hardening](#106-phase-4--hardening)
    - [10.7 Milestones and checkpoints](#107-milestones-and-checkpoints)
11. [Testing strategy](#11-testing-strategy)
12. [Team and process](#12-team-and-process)
13. [Decision matrix](#13-decision-matrix)
14. [What we are NOT doing](#14-what-we-are-not-doing)
15. [Success metrics](#15-success-metrics)
16. [Immediate action list](#16-immediate-action-list)
17. [Glossary](#17-glossary)
18. [Architecture decision records](#18-architecture-decision-records)

---

## 1. Executive summary

**Recommendation:** Evolve the current layered monolith into a **modular monolith**. Do **not** split into microservices at this stage.

AAA.ERP is an accounting + inventory ERP where business transactions routinely span both domains (e.g. inventory receipt posts a GL entry). The codebase already reflects business areas in folder structure (`Account/`, `Inventory/`), but boundaries are not enforced at compile time. The main symptoms are a God `IUnitOfWork` (50+ repositories), direct cross-module entity access, and dual write paths (Services + MediatR).

A modular monolith preserves ERP strengths:

- **ACID transactions** across inventory and general ledger (one database, one deployment)
- **Simple operations** (one API process, one connection string)
- **Clear module ownership** with enforced dependency rules
- **Credible extraction path** if a module ever needs independent scaling

---

## 2. Context and business drivers

### 2.1 What AAA.ERP is today

AAA.ERP is a **.NET 8** backend for an ERP system with:

| Area | Capabilities |
|------|--------------|
| **Accounting** | Chart of accounts, GL entries (journal, payment, receipt, opening, combined), financial periods, currencies, cost centers, sub-ledgers |
| **Inventory** | Products/variants, stock balances, import/export transactions, transfers, attributes, thresholds, expiry levels |
| **Platform** | JWT auth, audit trail, caching, localization (en/ar), global exception handling |
| **Frontend** | Separate SPA consuming REST APIs |

Deployment model: **one API process** → **one SQL Server database**.

### 2.2 Why architecture matters now

Recent development (inventory expansion, product/variant model, stock balances, audit, caching) increased:

- Number of entities and repositories
- Cross-domain workflows (inventory movement → GL posting)
- Team cognitive load (where does new code go? who owns what?)

The project is at the point where a layered monolith begins eroding into tight coupling unless boundaries are made explicit.

### 2.3 Business constraints that drive the decision

| Business fact | Architectural implication |
|---------------|---------------------------|
| Inventory receipt must post GL entry in same operation | Need **ACID** across domains → not microservices yet |
| Branches link to chart-of-account accounts | Cross-module **read** of stable references is OK; **write** must be coordinated |
| Arabic/English UI | Platform module owns localization; modules emit message keys |
| Single company / single deployment (today) | One host, one DB is correct |
| Growing inventory module | Needs isolated ownership without breaking accounting |
| Small-to-medium dev team | Modular monolith > microservices ops burden |

---

## 3. Current architecture assessment

### 3.1 Layered monolith structure

```
ERP.API            → HTTP, Swagger, migrations, startup
ERP.Application    → IRepository, IService interfaces, validators
ERP.Domain         → Entities, commands, DTOs
ERP.Infrastracture → Implementations (repos, services, handlers, DbContext)
Shared             → Base types, CQRS primitives, exceptions, filters
```

Intended dependency flow:

```
API → Infrastructure → Application → Domain → Shared
```

This is **technically** clean architecture, but **organizationally horizontal**: every feature cuts through all layers. There is no compile-time "Inventory module" — only folders.

### 3.2 Current solution projects

| Project | Role |
|---------|------|
| `ERP.API` | 42 controllers, startup, EF migrations |
| `ERP.Application` | Repository + service interfaces, validators |
| `ERP.Domain` | Entities, commands, DTOs (~279 files) |
| `ERP.Infrastracture` | Repos, services, handlers, DbContext |
| `Shared` | Base entities, CQRS, exceptions, filters, localization |
| `ERP.UnitTests` | Unit tests |

### 3.3 What already works well

- **MediatR + FluentValidation pipeline** — solid CQRS foundation
- **Base controller hierarchy** — consistent CRUD, pagination, localization
- **EF configuration per entity** — `ApplyConfigurationsFromAssembly` maps to per-module ownership
- **Cross-cutting improvements** — audit fields, global exception handler, pagination, soft delete, filters, audit trail, caching (see `TECHNICAL_DEBT.md`)
- **Domain folders mirror business** — `Account/` and `Inventory/` under Domain

### 3.4 Structural problems

#### Problem A — God `IUnitOfWork`

`IUnitOfWork` exposes 50+ repositories spanning Accounting and Inventory.

**Why this hurts:**

1. No encapsulation — any service can call any repository
2. Constructor explosion in `UnitOfWork`
3. Change amplification — adding one entity touches UoW interface, implementation, and all mocks
4. False sense of transaction boundary — one UoW means "access to everything," not one aggregate

**Fix:** Module-scoped unit of work (`IAccountingUnitOfWork`, `IInventoryUnitOfWork`) + `ITransactionCoordinator` for cross-module operations.

#### Problem B — Cross-module entity coupling

`InventoryTransactionService` and `InventoryTransferService` directly use Accounting entities (`Currency`, `Branch`, `ChartOfAccount`, `FinancialPeriod`), build `CompinedEntryCreateCommand`, and call `_mediator.Send()`.

**Why this hurts:**

1. Inventory depends on Accounting's internal command model
2. Business rules for GL posting are split across modules
3. Inventory cannot be tested in isolation
4. Future service extraction becomes a rewrite

**Fix:** `IAccountingIntegrationService` in `Accounting.Contracts` with request DTOs — Inventory never imports Accounting domain types.

#### Problem C — Dual write paths

| Path | Example |
|------|---------|
| Controller → `ISender.Send(command)` | Create via handler |
| Controller → `IService.ReadAll()` | Reads via service |
| Service → logic + `_mediator.Send` | Inventory transaction |

MediatR is registered in **two assemblies** (`WebBuilderExtensions` and `InfrasctructureExtensions`).

**Fix:** Standardize writes on MediatR handlers; consolidate registration in Host.

#### Problem D — `Shared` project overload

`ERP.Shared` references EF Core, Identity, Swagger, MediatR, AutoMapper, etc. Domain primitives sit next to infrastructure concerns.

**Fix:** Split into `ERP.BuildingBlocks.Domain`, `ERP.BuildingBlocks.Application`, and optional `ERP.BuildingBlocks.Infrastructure`.

#### Problem E — Naming drift

- `Infrastracture` typo
- `Compined` → should be `Combined`
- `ComandValidators` typo
- Namespace inconsistency (`Domain.Account.DBConfiguration` vs `ERP.Domain`)

Modular monolith + architecture tests helps prevent further drift.

---

## 4. Target architecture

### 4.1 Core principle

> **One deployable, one database, many modules** — each module owns its domain; modules communicate through explicit public APIs (contracts), not shared repositories.

### 4.2 Target solution layout

```
AAA.ERP/
├── src/
│   ├── ERP.Host/                              # Composition root (today's ERP.API)
│   │   ├── Program.cs / startup
│   │   ├── Controllers/
│   │   ├── appsettings.json
│   │   └── Migrations/                        # Until ERP.Persistence exists
│   │
│   ├── BuildingBlocks/
│   │   ├── ERP.BuildingBlocks.Domain/
│   │   ├── ERP.BuildingBlocks.Application/
│   │   └── ERP.BuildingBlocks.Infrastructure/   # Optional early phase
│   │
│   ├── Modules/
│   │   ├── Accounting/
│   │   │   ├── Accounting.Domain/
│   │   │   ├── Accounting.Application/
│   │   │   ├── Accounting.Contracts/          # PUBLIC surface for other modules
│   │   │   └── Accounting.Infrastructure/
│   │   │
│   │   ├── Inventory/
│   │   │   ├── Inventory.Domain/
│   │   │   ├── Inventory.Application/
│   │   │   ├── Inventory.Contracts/
│   │   │   └── Inventory.Infrastructure/
│   │   │
│   │   ├── Identity/
│   │   │   ├── Identity.Domain/
│   │   │   ├── Identity.Application/
│   │   │   └── Identity.Infrastructure/
│   │   │
│   │   └── Platform/
│   │       ├── Platform.Audit/
│   │       ├── Platform.Caching/
│   │       └── Platform.Localization/
│   │
│   └── ERP.Persistence/                       # Phase 3+ optional
│
└── tests/
    ├── Architecture.Tests/
    ├── Accounting.UnitTests/
    ├── Inventory.UnitTests/
    └── IntegrationTests/
```

### 4.3 Dependency rules

```
┌─────────────────────────────────────────────────────────────┐
│  ERP.Host                                                    │
│  References: every *.Infrastructure (composition only)       │
└─────────────────────────────────────────────────────────────┘
         │
         ▼
┌──────────────────┐     ┌──────────────────┐
│ Module.Infra     │────▶│ Module.Application│
└──────────────────┘     └────────┬─────────┘
                                  │
                    ┌─────────────┼─────────────┐
                    ▼             ▼             ▼
            Module.Domain   OtherModule.Contracts   BuildingBlocks
```

| From | May reference | Must NOT reference |
|------|---------------|-------------------|
| `*.Domain` | BuildingBlocks.Domain | EF, MediatR, other modules |
| `*.Application` | Own Domain, BuildingBlocks, other `*.Contracts` | Other modules' Domain/Infrastructure |
| `*.Infrastructure` | Own Application, Domain, EF | Other modules' Domain |
| `*.Contracts` | BuildingBlocks only (DTOs, interfaces) | Domain entities |
| `ERP.Host` | All Infrastructure projects | Domain directly (ideally) |

**Enforcement:** `Architecture.Tests` in CI — PRs that violate rules fail the build.

### 4.4 Architecture diagram

```mermaid
graph TB
    subgraph Host["ERP.Host (single API process)"]
        API[ERP.API]
    end

    subgraph Modules
        ACC[Accounting.Module]
        INV[Inventory.Module]
        IDN[Identity.Module]
        PLT[Platform.Module]
    end

    subgraph Kernel
        CORE[ERP.BuildingBlocks]
    end

    DB[(SQL Server - single DB)]

    API --> ACC
    API --> INV
    API --> IDN
    API --> PLT

    ACC --> CORE
    INV --> CORE
    IDN --> CORE
    PLT --> CORE

    INV -.->|Integration API| ACC

    ACC --> DB
    INV --> DB
    IDN --> DB
    PLT --> DB
```

---

## 5. Module definitions

### 5.1 Accounting module

**Purpose:** Own the general ledger and everything required to post financially correct entries.

**Owns:**

- ChartOfAccounts, Entries, FinancialPeriods, Currencies, CostCenters
- SubLedgers (Customer, Supplier, Bank, Branch, Tax, CashInBox, FixedAsset)
- GLSettings, CollectionBooks

**Public contract (`Accounting.Contracts`):**

```csharp
namespace Accounting.Contracts.Integration;

public record InventoryReceiptPostingRequest(
    Guid BranchId,
    Guid PartyAccountId,
    DateTime TransactionDate,
    decimal TotalAmount,
    InventoryTransactionType Direction
);

public record InventoryTransferPostingRequest(
    Guid SourceBranchId,
    Guid DestinationBranchId,
    DateTime TransferDate,
    decimal TotalCost,
    InventoryTransferType TransferType
);

public record FinancialPeriodInfo(Guid Id, DateTime StartDate, DateTime EndDate, bool IsClosed);

public record BranchAccountInfo(Guid BranchId, Guid ChartOfAccountId);

public interface IAccountingIntegrationService
{
    Task<ApiResponse<Guid>> PostInventoryReceiptEntryAsync(
        InventoryReceiptPostingRequest request, CancellationToken ct = default);

    Task<ApiResponse<Guid>> PostInventoryTransferEntryAsync(
        InventoryTransferPostingRequest request, CancellationToken ct = default);

    Task<ApiResponse<FinancialPeriodInfo>> GetOpenPeriodForDateAsync(
        DateTime date, CancellationToken ct = default);

    Task<ApiResponse<BranchAccountInfo>> GetBranchAccountAsync(
        Guid branchId, CancellationToken ct = default);

    Task<ApiResponse<Guid>> ResolvePartyAccountAsync(
        Guid transactionPartyId, CancellationToken ct = default);
}
```

Implementation lives in `Accounting.Infrastructure` and maps requests to internal commands (e.g. `CompinedEntryCreateCommand`). Other modules never import those commands.

### 5.2 Inventory module

**Purpose:** Product catalog, stock levels, inventory movements, transfers.

**Owns:**

- Products, Variants, StockBalances, InventoryTransactions, InventoryTransfers
- Attributes, Thresholds, ExpiryLevels, PackingUnits, SellingPrices

**Critical workflow — receipt with GL posting:**

```
1. Validate command (validator)
2. Begin DB transaction
3. Validate financial period (via IAccountingIntegrationService)
4. Validate stock (for issues)
5. Persist inventory transaction
6. Update stock balances
7. Call IAccountingIntegrationService.PostInventoryReceiptEntryAsync
8. Commit transaction
```

**Public contract (`Inventory.Contracts`) — for reports / future modules:**

```csharp
public interface IInventoryQueryService
{
    Task<ApiResponse<decimal>> GetStockQuantityAsync(
        Guid variantId, Guid branchId, Guid packingUnitId, CancellationToken ct = default);

    Task<ApiResponse<StockValuationDto>> GetBranchValuationAsync(
        Guid branchId, CancellationToken ct = default);
}
```

### 5.3 Identity module

**Purpose:** Authentication and authorization.

**Owns:** `ApplicationUser`, roles, refresh tokens, JWT configuration.

**Exposes:**

```csharp
public interface ICurrentUser
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
    IReadOnlyList<string> Roles { get; }
}
```

Other modules use `ICurrentUser` — never reference `ApplicationUser` directly.

### 5.4 Platform module

**Purpose:** Technical capabilities shared by all business modules.

| Sub-area | Current location | Target |
|----------|------------------|--------|
| Audit trail | `AuditService`, `AuditLog` | `Platform.Audit` |
| Caching | `CacheService` | `Platform.Caching` |
| Localization | `Resource.resx`, middleware | `Platform.Localization` |
| API envelope | `ApiResponse<T>` | `BuildingBlocks.Application` |
| Exception handling | `CustomExceptionHandler` | `Platform` or Host |

Platform has **no business rules** about debits/credits or stock quantities.

---

## 6. Data architecture

### 6.1 Single database (recommended)

Keep **one SQL Server database** for the foreseeable future.

**Reasons:**

1. Inventory and GL posting in one transaction
2. Cross-module reports are simple SQL joins
3. One backup, one migration pipeline
4. Matches ERP operational reality

### 6.2 DbContext strategy (phased)

#### Phase 1 — Composed monolithic DbContext

One `ApplicationDbContext`. Each module contributes EF configurations from its Infrastructure assembly:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContextMarker).Assembly);
    builder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContextMarker).Assembly);
    builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContextMarker).Assembly);
}
```

#### Phase 2 — Optional SQL schemas

```
accounting.ChartOfAccounts
accounting.Entries
inventory.StockBalances
inventory.Products
```

EF: `builder.Entity<Entry>().ToTable("Entries", "accounting");`

Apply after modules are stable in code — not day one.

#### Phase 3 — Separate databases

Not recommended now. Would require sagas/outbox for inventory + GL.

### 6.3 Migrations

| Phase | Location |
|-------|----------|
| Today | `ERP.API` |
| Short term | Keep in Host (no ops change) |
| Medium term | `ERP.Persistence` referenced only by Host |

**Rule:** Only one project runs `dotnet ef migrations add`.

### 6.4 Transaction boundaries

| Operation | Transaction scope |
|-----------|-------------------|
| Create Currency | Accounting module only |
| Create Product | Inventory module only |
| Inventory receipt with GL | Cross-module — `ITransactionCoordinator` |
| Read stock balance | Read-only, no transaction |

```csharp
public interface ITransactionCoordinator
{
    Task<ApiResponse<T>> ExecuteAsync<T>(
        Func<CancellationToken, Task<ApiResponse<T>>> action,
        CancellationToken ct);
}
```

Inventory and Accounting share the same scoped `DbContext` within one HTTP request.

---

## 7. Application layer standardization

### 7.1 CQRS convention

| Operation type | Pattern | Location |
|----------------|---------|----------|
| Create / Update / Delete | `IRequest<ApiResponse<T>>` + Handler | Application command + Infrastructure handler |
| Complex reads | Query + Handler | Same |
| Simple paged lists | Query handler (preferred) or read service | Application |
| Cross-module side effect | Integration service | `*.Contracts` interface |

### 7.2 Where business logic lives

| Logic type | Place |
|------------|-------|
| Input validation | FluentValidation in Application |
| Single-aggregate rules | Domain entity / domain service |
| Cross-aggregate within module | Application handler |
| Cross-module rules | Orchestrating handler + integration services |
| Persistence | Infrastructure repositories |

### 7.3 MediatR registration (target)

Single registration in Host:

```csharp
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(AccountingApplicationMarker).Assembly,
        typeof(InventoryApplicationMarker).Assembly,
        typeof(IdentityApplicationMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

services.AddValidatorsFromAssemblies(same assemblies);
```

Remove duplicate registration from `InfrasctructureExtensions`.

---

## 8. Integration patterns

### 8.1 Pattern comparison

| Pattern | Consistency | Use in AAA.ERP |
|---------|-------------|----------------|
| Direct entity access across modules | Strong | **Remove** |
| Shared God UoW | Strong | **Remove** |
| Module integration service (sync) | Strong | **Primary for GL posting** |
| In-process domain events | Eventual (same process) | Audit, cache invalidation |
| Outbox + worker | Eventual | Future notifications |
| REST/events between services | Eventual | **Not now** |

### 8.2 Inventory → Accounting flow (target)

```mermaid
sequenceDiagram
    participant Inv as InventoryTransactionHandler
    participant UoW as TransactionCoordinator
    participant Acc as IAccountingIntegrationService

    Inv->>UoW: Begin transaction
    Inv->>Inv: Validate stock, update balances
    Inv->>Acc: PostInventoryReceiptEntryAsync(request)
    Acc-->>Inv: EntryId
    Inv->>UoW: Commit
```

**Why synchronous, not domain events, for GL posting:**

Inventory + GL must succeed or fail together. Event-driven posting after commit creates stock-without-entry (or vice versa) and reconciliation problems.

### 8.3 In-process domain events (secondary)

Use **after** successful commit for:

- Audit log writes
- Cache invalidation
- Future notifications

### 8.4 Idempotency (future-proofing)

- Add `SourceDocumentId` (inventory transaction id) on GL entries
- Accounting checks for existing entry before creating duplicate

---

## 9. API and frontend stability

### 9.1 External contract unchanged

This is **internal restructuring**. The frontend should see:

- Same routes: `/api/Products`, `/api/StockBalances`, etc.
- Same `ApiResponse<T>` shape
- Same auth and localization headers

### 9.2 Optional Swagger grouping

```csharp
options.SwaggerDoc("accounting", new OpenApiInfo { Title = "Accounting API" });
options.SwaggerDoc("inventory", new OpenApiInfo { Title = "Inventory API" });
```

Documentation benefit only — no breaking changes.

---

## 10. Migration roadmap

### 10.0 Planning assumptions

| Assumption | Value |
|------------|-------|
| Team size | 2–3 backend developers |
| Architecture capacity | **0.5 FTE** (alongside features) or **1 FTE** (dedicated track) |
| Working week | 5 days; estimates in **person-days (pd)** |
| Risk buffer | +15% added to each phase |
| Start date (example) | Week of **6 Jul 2026** |
| Feature freeze during Phase 1 spike | None — integrate with ongoing inventory work |

**Effort legend:** S = 0.5–1 pd · M = 2–3 pd · L = 4–5 pd · XL = 6+ pd

**Two timeline tracks:**

| Track | Phase 0 | Phase 1 | Phase 2 | Phase 3 | Total to modular monolith |
|-------|---------|---------|---------|---------|---------------------------|
| **0.5 FTE** | 3 weeks | 5 weeks | 7 weeks | 24 weeks | **~9 months** |
| **1 FTE** | 2 weeks | 3 weeks | 4 weeks | 14 weeks | **~5 months** |

---

### 10.1 Master timeline

Calendar view (**0.5 FTE** track — recommended for a team shipping features in parallel):

```mermaid
gantt
    title Modular Monolith Migration (0.5 FTE)
    dateFormat YYYY-MM-DD
    axisFormat %b %d

    section Phase 0
    Governance and arch tests     :p0, 2026-07-06, 15d

    section Phase 1
    Accounting integration        :p1, after p0, 25d

    section Phase 2
    Pilot module Taxes            :p2, after p1, 35d

    section Phase 3
    BuildingBlocks Platform       :p3a, after p2, 20d
    Identity module               :p3b, after p3a, 15d
    Accounting extraction         :p3c, after p3b, 45d
    Inventory extraction          :p3d, after p3c, 55d
    Retire horizontal layers      :p3e, after p3d, 10d

    section Phase 4
    Hardening ongoing             :p4, after p3e, 60d
```

| Phase | Calendar (0.5 FTE) | Calendar (1 FTE) | Person-days | Exit milestone |
|-------|--------------------|------------------|-------------|----------------|
| **0** Foundation | 6 Jul – 24 Jul 2026 | 6 Jul – 17 Jul 2026 | 8–12 pd | M0: Arch tests in CI |
| **1** Integration facade | 27 Jul – 28 Aug 2026 | 20 Jul – 7 Aug 2026 | 15–20 pd | M1: Inventory decoupled |
| **2** Pilot module | 31 Aug – 16 Oct 2026 | 10 Aug – 4 Sep 2026 | 18–25 pd | M2: Module template proven |
| **3** Full extraction | 19 Oct 2026 – 9 Apr 2027 | 7 Sep 2026 – 12 Dec 2026 | 80–120 pd | M3: Horizontal layers gone |
| **4** Hardening | Apr 2027+ (ongoing) | Dec 2026+ (ongoing) | 2–4 pd/sprint | M4: Production-grade ops |

---

### 10.2 Phase 0 — Foundation and governance

**Duration:** 2 weeks (1 FTE) · 3 weeks (0.5 FTE)  
**Goal:** Stop erosion; add guardrails without moving business code.  
**Owner:** Tech lead / architect

#### Week 1 (6–10 Jul 2026)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 0.1.1 | Stakeholder kickoff — present plan, assign module owners | S | — | Tech lead | Signed ADR-001 in repo |
| 0.1.2 | Create `Tests/Architecture.Tests` project; add NetArchTest.Rules | S | 0.1.1 | Dev A | Project builds in solution |
| 0.1.3 | Write baseline rule: `Domain` must not reference `Infrastracture` | S | 0.1.2 | Dev A | Passing test |
| 0.1.4 | Write baseline rule: `Application` must not reference `Infrastracture` | S | 0.1.2 | Dev A | Passing test |
| 0.1.5 | Write baseline rule: `API` must not reference `Domain` entities directly | S | 0.1.2 | Dev A | Passing test |
| 0.1.6 | Add ratchet test for Inventory→Account refs (marked `[Fact(Skip = "Phase 1")]`) | S | 0.1.2 | Dev A | Skipped test documented |
| 0.1.7 | Wire `Architecture.Tests` into CI pipeline (fail on violation) | M | 0.1.3–0.1.5 | Dev A | CI green |

#### Week 2 (13–17 Jul 2026)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 0.2.1 | Audit MediatR registration — list handlers in both assemblies | S | — | Dev B | Inventory doc in PR |
| 0.2.2 | Move all MediatR + validator registration to `WebBuilderExtensions` only | M | 0.2.1 | Dev B | Single registration point |
| 0.2.3 | Remove `AddMediatR` from `InfrasctructureExtensions` | S | 0.2.2 | Dev B | PR merged |
| 0.2.4 | Smoke test all CRUD endpoints (Swagger / Postman collection) | M | 0.2.3 | Dev B | Test checklist signed |
| 0.2.5 | Remove duplicate DI registrations (`IAttributeDefinitionService`, `IInventoryTransferService`, etc.) | S | — | Dev B | Clean `InfrasctructureExtensions` |
| 0.2.6 | Document freeze rule: no new repos on `IUnitOfWork` (PR template checkbox) | S | 0.1.1 | Tech lead | PR template updated |

#### Week 3 — buffer (20–24 Jul 2026, 0.5 FTE only)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 0.3.1 | Create `docs/architecture/CONTRIBUTING.md` — where to put new code | S | 0.1.1 | Tech lead | Contributor guide |
| 0.3.2 | Open tracking issues for rename branches (`Infrastracture`, `Compined`) | S | — | Tech lead | GitHub issues |
| 0.3.3 | Phase 0 retrospective; confirm Phase 1 start date | S | 0.2.4 | Team | Go/no-go for Phase 1 |

**Phase 0 exit criteria (M0):**

- [x] Architecture tests run on every PR (`Tests/ERP.Architecture.Tests`)
- [x] MediatR registered in exactly one place (`WebBuilderExtensions`)
- [x] No duplicate service registrations in `InfrasctructureExtensions`
- [x] ADR-001 to ADR-004 committed under `docs/architecture/adr/`
- [x] Module owners assigned — [MODULE_OWNERS.md](./MODULE_OWNERS.md)
- [x] CI workflow runs build + tests (`.github/workflows/ci.yml`)
- [x] Contributor guide (`docs/architecture/CONTRIBUTING.md`)
- [x] PR template with `IUnitOfWork` freeze rule (`.github/pull_request_template.md`)
- [x] MediatR audit documented (`docs/architecture/MEDIATR_REGISTRATION.md`)
- [x] Smoke tests automated (`Tests/ERP.SmokeTests`)
- [x] Phase 0 retrospective — [PHASE_0_RETROSPECTIVE.md](./PHASE_0_RETROSPECTIVE.md)

**Sample arch tests:**

```csharp
[Fact]
public void Domain_should_not_reference_Infrastructure()
{
    Types.InAssembly(DomainAssembly)
        .Should().NotHaveDependencyOn("ERP.Infrastracture")
        .GetResult().IsSuccessful.Should().BeTrue();
}

[Fact(Skip = "Enable after Phase 1")]
public void Inventory_should_not_reference_Account_Entities()
{
    Types.InAssembly(InventoryServicesAssembly)
        .Should().NotHaveDependencyOn("ERP.Domain.Models.Entities.Account")
        .GetResult().IsSuccessful.Should().BeTrue();
}
```

---

### 10.3 Phase 1 — Accounting integration facade

**Duration:** 3 weeks (1 FTE) · 5 weeks (0.5 FTE)  
**Goal:** Decouple Inventory from Accounting domain internals.  
**Owner:** Inventory module owner + Accounting module owner

#### Sprint 1 — Contracts and facade skeleton (Week 1)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 1.1.1 | Create folder `src/Modules/Accounting/Accounting.Contracts/` (or project when ready) | S | M0 | Dev A | Contracts assembly |
| 1.1.2 | Define `InventoryReceiptPostingRequest`, `InventoryTransferPostingRequest` records | S | 1.1.1 | Dev A | DTOs committed |
| 1.1.3 | Define `FinancialPeriodInfo`, `BranchAccountInfo` records | S | 1.1.1 | Dev A | DTOs committed |
| 1.1.4 | Define `IAccountingIntegrationService` interface | S | 1.1.2–1.1.3 | Dev A | Interface in Contracts |
| 1.1.5 | Add `Accounting.Contracts` reference to Application or Inventory area | S | 1.1.4 | Dev A | Builds green |
| 1.1.6 | Create `AccountingIntegrationService` stub returning `NotImplemented` | S | 1.1.4 | Dev B | Class + DI registration |

#### Sprint 2 — Implement facade (Week 2)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 1.2.1 | Implement `GetOpenPeriodForDateAsync` — extract from inventory validators | M | 1.1.6 | Dev B | Unit tests |
| 1.2.2 | Implement `GetBranchAccountAsync` | M | 1.1.6 | Dev B | Unit tests |
| 1.2.3 | Implement `ResolvePartyAccountAsync` | M | 1.1.6 | Dev B | Unit tests |
| 1.2.4 | Implement `PostInventoryReceiptEntryAsync` — move logic from `CreateCompinedEntryCreateCommand` | L | 1.2.1–1.2.3 | Dev B | Maps to `CompinedEntryCreateCommand` internally |
| 1.2.5 | Implement `PostInventoryTransferEntryAsync` — move from `InventoryTransferService.CreateEntryCommand` | L | 1.2.4 | Dev B | Transfer posting works |
| 1.2.6 | Register `IAccountingIntegrationService` in DI | S | 1.2.5 | Dev B | Scoped service |

#### Sprint 3 — Refactor inventory services (Week 3)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 1.3.1 | Inject `IAccountingIntegrationService` into `InventoryTransactionService` | S | 1.2.6 | Dev A | Constructor updated |
| 1.3.2 | Replace `_dbContext.Set<Currency/Branch/ChartOfAccount>` with facade calls | M | 1.3.1 | Dev A | No Account entity usings |
| 1.3.3 | Replace `_mediator.Send(compinedEntryCommand)` with `PostInventoryReceiptEntryAsync` | M | 1.3.2 | Dev A | Receipt path refactored |
| 1.3.4 | Refactor `InventoryTransferService` to use facade | M | 1.2.5 | Dev A | Transfer path refactored |
| 1.3.5 | Refactor import/export transaction services if they share same pattern | M | 1.3.3 | Dev A | All transaction types covered |
| 1.3.6 | Delete private methods `CreateCompinedEntryCreateCommand` from inventory services | S | 1.3.5 | Dev A | Dead code removed |

#### Sprint 4 — Testing and gate (Week 4–5, 0.5 FTE buffer)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 1.4.1 | Integration test: receipt → stock up + GL entry created | L | 1.3.3 | Dev A | Test in unit/integration project |
| 1.4.2 | Integration test: receipt failure → stock unchanged, no entry | M | 1.4.1 | Dev A | Rollback verified |
| 1.4.3 | Integration test: export with insufficient stock → no entry | M | 1.4.1 | Dev A | Validation path |
| 1.4.4 | Integration test: transfer direct + staged types | M | 1.3.4 | Dev A | Both transfer types |
| 1.4.5 | Golden test: compare GL lines before/after refactor (same amounts/accounts) | M | 1.4.1 | Dev B | No behavioral regression |
| 1.4.6 | Enable arch test — Inventory must not reference Account entities | S | 1.3.6 | Dev B | CI gate active |
| 1.4.7 | Code review + merge; update plan status | S | 1.4.6 | Tech lead | M1 complete |

**Before / after:**

```
BEFORE: InventoryTransactionService → DbContext.Set<Branch> → CompinedEntryCreateCommand → MediatR
AFTER:  InventoryTransactionService → IAccountingIntegrationService.PostInventoryReceiptEntryAsync
```

**Phase 1 exit criteria (M1):**

- [ ] Zero `using ERP.Domain.Models.Entities.Account` in Inventory services
- [ ] All inventory transaction types use facade for GL posting
- [ ] Integration tests pass for receipt, export, transfer
- [ ] Arch test enabled and green
- [ ] No production API contract changes

---

### 10.4 Phase 2 — Vertical slice pilot

**Duration:** 4 weeks (1 FTE) · 7 weeks (0.5 FTE)  
**Goal:** Prove module project structure end-to-end with one feature.  
**Pilot feature:** **Taxes** (Accounting module — small, clear boundary)  
**Owner:** Accounting module owner

#### Week 1 — Scaffold module projects

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 2.1.1 | Create solution folder `src/Modules/Accounting/` | S | M1 | Dev A | Solution structure |
| 2.1.2 | Create `Accounting.Domain` project; move `Tax` entity | M | 2.1.1 | Dev A | Entity compiles |
| 2.1.3 | Create `Accounting.Application`; move `TaxCreate/UpdateCommand`, validators | M | 2.1.2 | Dev A | Commands compile |
| 2.1.4 | Create `Accounting.Infrastructure`; move handler, repo, `TaxDbConfig` | L | 2.1.3 | Dev A | Handler compiles |
| 2.1.5 | Create `Accounting.Contracts` (may already exist from Phase 1) | S | 2.1.1 | Dev A | Contracts project |
| 2.1.6 | Add arch test: `Accounting.Domain` must not reference `Inventory.*` | S | 2.1.2 | Dev B | Test passes |

#### Week 2 — Composition and DI

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 2.2.1 | Implement `AddAccountingModule(IServiceCollection)` extension | M | 2.1.4 | Dev A | DI extension |
| 2.2.2 | Register Tax repo, handler, validator inside module extension | M | 2.2.1 | Dev A | Tax CRUD via module DI |
| 2.2.3 | Call `AddAccountingModule()` from `WebBuilderExtensions` / Host | S | 2.2.2 | Dev A | Host composes module |
| 2.2.4 | Remove old Tax registrations from `InfrasctructureExtensions` | S | 2.2.3 | Dev A | No duplicate DI |
| 2.2.5 | Remove old Tax files from horizontal projects | M | 2.2.4 | Dev A | Dead files deleted |
| 2.2.6 | Verify `TaxesController` still works (no route changes) | S | 2.2.5 | Dev B | Smoke test |

#### Week 3 — EF and migrations

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 2.3.1 | Ensure `TaxDbConfig` picked up via `ApplyConfigurationsFromAssembly` | M | 2.1.4 | Dev A | Model builds |
| 2.3.2 | Run existing migrations — confirm no schema drift | S | 2.3.1 | Dev A | Migration OK |
| 2.3.3 | Document pattern: module owns configs, Host owns migrations | S | 2.3.2 | Tech lead | Section in CONTRIBUTING.md |
| 2.3.4 | Add arch test: module dependency rules for Accounting projects | M | 2.1.6 | Dev B | 4–5 rules in CI |

#### Week 4 — Template and second micro-pilot (optional)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 2.4.1 | Write `docs/architecture/MODULE_TEMPLATE.md` — step-by-step for next aggregate | M | 2.2.6 | Tech lead | Reusable playbook |
| 2.4.2 | Optional: migrate **ExpiryLevels** (Inventory) using template | L | 2.4.1 | Dev B | Second proof point |
| 2.4.3 | Team review — validate structure, naming, build time | S | 2.4.1 | Team | Sign-off |
| 2.4.4 | Measure build/test time before vs after | S | 2.4.3 | Dev B | Metrics recorded |

#### Weeks 5–7 — buffer (0.5 FTE)

| ID | Task | Effort | Depends on | Owner | Deliverable |
|----|------|--------|------------|-------|-------------|
| 2.5.1 | Fix any circular reference or namespace issues found in pilot | M | 2.4.3 | Dev A | Clean build |
| 2.5.2 | Present module template to full team (30 min walkthrough) | S | 2.4.1 | Tech lead | Team trained |

**Phase 2 exit criteria (M2):**

- [ ] Taxes fully lives in `Accounting.*` projects
- [ ] `AddAccountingModule()` is the composition pattern for all future Accounting work
- [ ] `MODULE_TEMPLATE.md` published
- [ ] Arch tests cover Accounting module dependency rules
- [ ] No API breaking changes

---

### 10.5 Phase 3 — Module extraction

**Duration:** 14 weeks (1 FTE) · 24 weeks (0.5 FTE)  
**Goal:** Move all code from horizontal layers into modules; retire `ERP.Application`, `ERP.Domain`, `ERP.Infrastracture`.  
**Strategy:** One aggregate (or aggregate group) per PR; 2–3 migrations per week at steady state.

#### Wave 3A — BuildingBlocks + Platform (Weeks 1–4)

| ID | Task | Effort | Owner | Deliverable |
|----|------|--------|-------|-------------|
| 3A.1 | Create `ERP.BuildingBlocks.Domain` — move `BaseEntity`, tree entities | M | Dev B | Domain primitives |
| 3A.2 | Create `ERP.BuildingBlocks.Application` — `PaginatedResult`, `BaseFilterDto`, behaviors | M | Dev B | App primitives |
| 3A.3 | Move `ApiResponse`, `MessageTemplate` to BuildingBlocks.Application | M | Dev B | Response types |
| 3A.4 | Move `CustomExceptionHandler`, `NotFoundException`, etc. | M | Dev B | Exception types |
| 3A.5 | Create `Platform.Audit` — move `AuditService`, `AuditLog` | M | Dev B | Audit module |
| 3A.6 | Create `Platform.Caching` — move `CacheService` | M | Dev B | Cache module |
| 3A.7 | Create `Platform.Localization` — move resources, middleware | L | Dev B | Localization module |
| 3A.8 | Update all project references from `Shared` → BuildingBlocks | L | Dev A+B | Shared project thinned |
| 3A.9 | Delete moved files from `Shared`; remove or deprecate project | M | Dev A | Shared deprecated |

**Wave 3A exit:** `Shared` project empty or removed; Platform modules registered via `AddPlatformModules()`.

#### Wave 3B — Identity (Weeks 5–6)

| ID | Task | Effort | Owner | Deliverable |
|----|------|--------|-------|-------------|
| 3B.1 | Create `Identity.Domain` — `ApplicationUser` (or keep in BuildingBlocks if shared) | M | Dev A | Identity domain |
| 3B.2 | Create `Identity.Application` — `IAuthenticationService`, commands | M | Dev A | Auth contracts |
| 3B.3 | Create `Identity.Infrastructure` — auth service, JWT helpers | L | Dev A | Auth impl |
| 3B.4 | Introduce `ICurrentUser`; replace direct `IHttpContextAccessor` in audit | M | Dev A | Abstraction |
| 3B.5 | `AddIdentityModule()` + smoke test `AuthController` | S | Dev A | Auth works |

#### Wave 3C — Accounting extraction (Weeks 7–14)

Migrate aggregates in dependency order. **~1–2 pd per simple aggregate; 3–5 pd for Entries cluster.**

| Week | Aggregates to migrate | PRs | Owner |
|------|----------------------|-----|-------|
| 7 | Currencies, GLSettings, AccountGuides | 2–3 | Dev A |
| 8 | FinancialPeriods, CostCenters, CollectionBooks | 2–3 | Dev A |
| 9 | ChartOfAccounts (tree) | 1–2 | Dev A |
| 10 | Sub-ledgers: Banks, CashInBoxes, Branches | 2–3 | Dev A |
| 11 | Sub-ledgers: Customers, Suppliers, Taxes, FixedAssets | 3–4 | Dev A |
| 12 | Roles | 1 | Dev B |
| 13 | Entries — Journal, Payment, Receipt | 2–3 | Dev A |
| 14 | Entries — Opening, Combined, EntryService core | 3–4 | Dev A |

**Per-aggregate checklist (repeat for each):**

| Step | Task | Effort |
|------|------|--------|
| 1 | Move entity → `Accounting.Domain` | S |
| 2 | Move commands + validators → `Accounting.Application` | S–M |
| 3 | Move handler + repository + DbConfig → `Accounting.Infrastructure` | M |
| 4 | Register in `AddAccountingModule()` | S |
| 5 | Delete from horizontal projects | S |
| 6 | Arch tests + smoke test | S |
| **Total per simple aggregate** | | **1–2 pd** |
| **Total per Entries subgroup** | | **3–5 pd** |

**Wave 3C exit:** All `ERP.API/Controllers/Account/*` backed by Accounting module projects.

#### Wave 3D — Inventory extraction (Weeks 15–22)

| Week | Aggregates to migrate | PRs | Owner |
|------|----------------------|-----|-------|
| 15 | Colors, Sizes, PackingUnits, ManufacturerCompanies | 3–4 | Dev B |
| 16 | SellingPrices, AttributeDefinitions, AttributeValues | 2–3 | Dev B |
| 17 | Products (+ related: codes, attachments, packing units) | 2–3 | Dev B |
| 18 | Variants (+ related: suppliers, taxes, cost centers) | 3–4 | Dev B |
| 19 | StockBalances, InventoryThresholds, ExpiryLevels | 2–3 | Dev B |
| 20 | InventoryTransactions (import/export) | 2 | Dev B |
| 21 | InventoryTransfers | 2 | Dev B |
| 22 | Legacy Items (if still in use — deprecate or migrate) | 1–2 | Dev B |

**Wave 3D exit:** All `ERP.API/Controllers/Inventory/*` backed by Inventory module projects.

#### Wave 3E — Retire horizontal layers (Weeks 23–24)

| ID | Task | Effort | Owner | Deliverable |
|----|------|--------|-------|-------------|
| 3E.1 | Verify `ERP.Domain` has no remaining source files | S | Dev A | Empty project |
| 3E.2 | Verify `ERP.Application` has no remaining source files | S | Dev A | Empty project |
| 3E.3 | Verify `ERP.Infrastracture` has no remaining source files | S | Dev A | Empty project |
| 3E.4 | Remove projects from solution; rename `ERP.API` → `ERP.Host` (optional) | M | Tech lead | Clean solution |
| 3E.5 | Replace God `IUnitOfWork` with module UoWs or remove entirely | L | Dev A | UoW deprecated |
| 3E.6 | Full regression test suite | L | Dev A+B | All green |
| 3E.7 | Update README, solution diagram | S | Tech lead | Docs current |

**Phase 3 exit criteria (M3):**

- [ ] No horizontal `ERP.Domain` / `ERP.Application` / `ERP.Infrastracture` projects
- [ ] Every controller backed by a module
- [ ] God `IUnitOfWork` removed or empty
- [ ] All arch tests green
- [ ] Full API smoke suite passes

---

### 10.6 Phase 4 — Hardening

**Duration:** Ongoing from M3; front-loaded **8 weeks**  
**Goal:** Production-grade modular monolith — observability, ops, debt, optional DB schemas.

#### Month 1 after M3

| ID | Task | Effort | Owner | Deliverable |
|----|------|--------|-------|-------------|
| 4.1.1 | Add Serilog enrichers: `Module`, `CommandName`, `CorrelationId` | M | Dev B | Structured logs |
| 4.1.2 | Swagger grouped by module (Accounting, Inventory, Identity) | M | Dev B | Improved docs |
| 4.1.3 | Rename `Infrastracture` → `Infrastructure` (if not done in 3E) | L | Dev A | Tech debt cleared |
| 4.1.4 | Rename `Compined` → `Combined` (19 files) | L | Dev A | Tech debt cleared |
| 4.1.5 | Add `ITransactionCoordinator` for cross-module flows (formalize Phase 1 pattern) | M | Dev A | Coordinator in BuildingBlocks |

#### Month 2 after M3

| ID | Task | Effort | Owner | Deliverable |
|----|------|--------|-------|-------------|
| 4.2.1 | Evaluate SQL schema separation (`accounting.*`, `inventory.*`) | M | Tech lead | ADR decision |
| 4.2.2 | If approved: migration to add schemas + update EF configs | L | Dev A | Schema separated |
| 4.2.3 | Add `SourceDocumentId` on entries for idempotent inventory posting | M | Dev A | No duplicate entries |
| 4.2.4 | Module README per module (`Accounting/README.md`, etc.) | M | Module owners | Onboarding docs |
| 4.2.5 | Performance review: N+1 queries on Products/Variants list endpoints | L | Dev B | Perf fixes |

#### Ongoing (every sprint)

| ID | Task | Cadence | Owner |
|----|------|---------|-------|
| 4.3.1 | Arch test review — no new violations | Per PR | All |
| 4.3.2 | Module owner review on cross-module PRs | Per PR | Module owners |
| 4.3.3 | Dependency graph check (no new cycles) | Monthly | Tech lead |
| 4.3.4 | ADR review — microservices triggers still not met? | Quarterly | Tech lead |

**Phase 4 exit criteria (M4):**

- [ ] Structured logging in production
- [ ] Naming debt resolved
- [ ] Module READMEs complete
- [ ] Optional schemas deployed (if ADR approved)
- [ ] Onboarding doc: new feature in under 1 day

---

### 10.7 Milestones and checkpoints

| Milestone | Target (0.5 FTE) | Target (1 FTE) | Validation |
|-----------|------------------|----------------|------------|
| **M0** Phase 0 complete | 24 Jul 2026 | 17 Jul 2026 | Arch tests in CI; MediatR consolidated |
| **M1** Phase 1 complete | 28 Aug 2026 | 7 Aug 2026 | Inventory uses `IAccountingIntegrationService` only |
| **M2** Phase 2 complete | 16 Oct 2026 | 4 Sep 2026 | Taxes in module projects; template published |
| **M3** Phase 3 complete | 9 Apr 2027 | 12 Dec 2026 | Horizontal layers retired |
| **M4** Phase 4 baseline | 4 Jun 2027 | 6 Feb 2027 | Logging, debt, docs complete |

**Go/no-go checkpoints:**

| Checkpoint | When | Question | If NO |
|------------|------|----------|-------|
| CP-1 | End Phase 0 | Do arch tests run reliably in CI? | Fix CI before Phase 1 |
| CP-2 | End Phase 1 | Do integration tests prove atomicity? | Do not start module extraction |
| CP-3 | End Phase 2 | Is module template workable for the team? | Revise structure before Wave 3A |
| CP-4 | Mid Phase 3 (Week 14) | Is velocity sustainable (~2 aggregates/week)? | Extend timeline; reduce parallel feature load |
| CP-5 | End Phase 3 | Full regression green? | Delay M3; keep horizontal projects until stable |

---

### 10.8 Risk register and buffers

| Risk | Impact | Likelihood | Mitigation | Buffer |
|------|--------|------------|------------|--------|
| Feature pressure slows migration | High | High | 0.5 FTE dedicated; migrate 1 aggregate per sprint minimum | +4 weeks on Phase 3 |
| GL posting regression | High | Medium | Golden tests in Phase 1; manual QA on receipt/transfer | +1 week Phase 1 |
| Circular project references | Medium | Medium | Arch tests; Contracts projects | +1 week Phase 2 |
| Build time increases | Low | Medium | Measure in Phase 2; split only if over 2 min | Monitor only |
| Team disagreement on boundaries | Medium | Low | ADRs; module owners; checkpoint reviews | Kickoff workshop |

**Recommended contingency:** Add **4 calendar weeks** to the 0.5 FTE track for production releases and unplanned bugs → **~10 months total** to M3.

---

## 11. Testing strategy

### 11.1 Test pyramid

```
        ┌─────────────┐
        │  E2E / API  │  Few — receipt, transfer, journal
        ├─────────────┤
        │ Integration │  Handlers + real EF
        ├─────────────┤
        │   Unit      │  Domain rules, validators, facade mapping
        ├─────────────┤
        │ Architecture│  Dependency rules — every CI build
        └─────────────┘
```

### 11.2 Critical scenarios

| Scenario | Assert |
|----------|--------|
| Inventory receipt | Stock ↑, GL entry created, same transaction |
| Insufficient stock on export | No stock change, no GL entry |
| Closed financial period | Rejected before persistence |
| Inventory transfer | Correct accounts via integration service |
| Missing default currency | Structured error from Accounting facade |

### 11.3 Module isolation

- Inventory unit tests **mock** `IAccountingIntegrationService`
- Accounting posting tests use request DTOs only — no product catalog needed

---

## 12. Team and process

### 12.1 Module ownership

| Module | Review rule |
|--------|-------------|
| Accounting | PRs touching `Accounting.*` need module owner review |
| Inventory | Same |
| Platform | Senior / rotating owner |
| Host | Tech lead — DI, migrations |

### 12.2 Definition of Done (per feature)

- Code in correct module projects
- Validator + handler for writes
- Arch tests pass
- Integration test if cross-module
- No new repositories on global `IUnitOfWork`

---

## 13. Decision matrix

| Criterion | Layered monolith (today) | Modular monolith | Microservices |
|-----------|--------------------------|------------------|---------------|
| Transactional ERP workflows | Medium | **High** | Low |
| Team scalability (2–8 devs) | Medium | **High** | Medium |
| Time to first value | High (built) | Medium (incremental) | Low |
| Operational complexity | **Low** | **Low** | High |
| Future extractability | Low | **High** | High |
| Fits current business | Eroding | **Best fit** | Over-engineered |

---

## 14. What we are NOT doing

| Option | Why not now |
|--------|-------------|
| Microservices | Inventory+GL atomicity; ops cost; no scaling requirement |
| Full event sourcing | Overkill; reporting needs relational model |
| Multiple DbContexts with separate transactions | Race conditions without distributed transactions |
| Big bang rewrite | ERP downtime risk; frontend coupled to current API |
| Keeping God UnitOfWork | Continues erosion as inventory grows |

---

## 15. Success metrics

| Metric | Target (6 months) |
|--------|-------------------|
| Cross-module entity references (Inventory→Account) | 0 |
| Repositories on global `IUnitOfWork` | 0 (deprecated) |
| MediatR registration points | 1 (Host) |
| Arch test violations in CI | 0 |
| Production incidents: inventory without GL | 0 |
| Onboarding: "where to add a feature" | < 1 day |

---

## 16. Phase 0 status — **COMPLETE (M0)**

Phase 0 finished 4 July 2026. See [PHASE_0_RETROSPECTIVE.md](./PHASE_0_RETROSPECTIVE.md) for go/no-go and [MODULE_OWNERS.md](./MODULE_OWNERS.md) for ownership.

Run all verification tests:

```bash
dotnet test AAA.ERP.sln
```

| Suite | Project |
|-------|---------|
| Architecture | `Tests/ERP.Architecture.Tests` |
| Smoke | `Tests/ERP.SmokeTests` |
| Unit | `Tests/ERP.UnitTests` |

**Next:** Phase 1 — `IAccountingIntegrationService` (§10.3).

---

## 17. Glossary

| Term | Meaning |
|------|---------|
| **Modular monolith** | One deployable app with explicit module boundaries |
| **Bounded context** | Business area with its own model (Accounting, Inventory) |
| **Aggregate** | Cluster of entities changed together (Entry, StockBalance) |
| **Contracts project** | Public API of a module for other modules |
| **Integration service** | Facade implementing cross-module use cases |
| **Strangler fig** | Gradually replace old structure; avoid big-bang rewrite |
| **BuildingBlocks** | Shared technical kernel (not business Shared) |
| **Transaction coordinator** | Explicit cross-module DB transaction wrapper |

---

## 18. Architecture decision records

| ADR | Title | Decision |
|-----|-------|----------|
| ADR-001 | Adopt modular monolith | Evolve in place; do not microservice |
| ADR-002 | Single database | Composed DbContext; optional schemas later |
| ADR-003 | Synchronous GL integration | Inventory posting via `IAccountingIntegrationService` in same transaction |
| ADR-004 | MediatR as sole write path | Deprecate direct service writes for new features |
| ADR-005 | Microservices trigger criteria | Separate team, independent scale, or regulatory isolation required |

---

## References

- Current codebase: `ERP.Application/Repositories/IUnitOfWork.cs` (God UoW)
- Cross-module coupling: `ERP.Infrastracture/Services/Inventory/InventoryTransactionService.cs`
- Technical debt tracker: `TECHNICAL_DEBT.md`
- DI registration: `ERP.Infrastracture/Utilities/InfrasctructureExtensions.cs`, `ERP.API/Utilities/WebBuilderExtensions.cs`
