# Hybrid CQRS — Sprint tracker

> **Plan:** [HYBRID_CQRS_PLAN.md](./HYBRID_CQRS_PLAN.md) §17  
> **Status:** Not started  
> **Legend:** ⬜ Not started · 🔄 In progress · ✅ Done · ⏭️ Skipped

---

## Program overview

| Sprint | Weeks | Goal | Status |
|--------|-------|------|--------|
| [0](#sprint-0) | 1 | Kickoff — ADR, PR rules | 🔄 |
| [1](#sprint-1) | 2–3 | Pilots: Tax, Color | 🔄 |
| [2](#sprint-2) | 4–5 | Import + Export transactions | ⬜ |
| [3](#sprint-3) | 6–7 | Inventory transfers | ⬜ |
| [4](#sprint-4) | 8–9 | Complex + Journal entries | ⬜ |
| [5](#sprint-5) | 10–11 | Payment + Receipt entries | ⬜ |
| [6](#sprint-6) | 12–13 | Opening + Combined entries | ⬜ |
| [7](#sprint-7) | 14–15 | Financial periods + COA | ⬜ |
| [8](#sprint-8) | 16–17 | Sub-ledgers batch 1 | ⬜ |
| [9](#sprint-9) | 18–19 | Sub-ledgers batch 2 + settings | ⬜ |
| [10](#sprint-10) | 20–21 | Inventory catalog | ⬜ |
| [11](#sprint-11) | 22–23 | Cleanup + final sign-off | ⬜ |

**Milestones:** M0 (S0) · M1 P0 (S3) · M2 P1 (S6) · M3 master data (S10) · M4 complete (S11)

---

## Standard recipe (per aggregate)

Copy for each aggregate row below:

- [ ] **A** — `I{Entity}ReadService` interface
- [ ] **B** — `{Entity}ReadService` implementation
- [ ] **C** — Create logic in handler
- [ ] **D** — Update logic in handler
- [ ] **E** — `Delete{Entity}Command` + handler
- [ ] **F** — Controller: `ISender` + read service only
- [ ] **G** — DI registration
- [ ] **H** — Tests
- [ ] **I** — Remove/obsolete write service

---

## Sprint 0

| ID | Task | Status |
|----|------|--------|
| S0-1 | Approve HYBRID_CQRS_PLAN | ⬜ |
| S0-2 | ADR-012 | ⬜ |
| S0-3 | Update MODULE_TEMPLATE link | ⬜ |
| S0-4 | PR checklist in CONTRIBUTING | ⬜ |
| S0-5 | `ControllerResponseExtensions` | ⬜ |
| S0-6 | This tracker created | ✅ |

**DoD:** ADR merged · PR template updated · Sprint 1 ready

---

## Sprint 1

| ID | Task | Status |
|----|------|--------|
| S1-1 | TAX — recipe A–I | ⬜ |
| S1-2 | COLOR — recipe A–I | ⬜ |
| S1-3 | Pilot template documented | ⬜ |
| S1-4 | Arch test: controller DI (warn) | ⬜ |
| S1-5 | Arch test: read service no writes | ⬜ |
| S1-6 | Tax handler unit tests | ⬜ |
| S1-7 | ToResult in pilots | ⬜ |
| S1-8 | BaseController retirement note | ⬜ |

**DoD:** Tax + Color migrated · ≥2 arch tests · build/test green

---

## Sprint 2

| Aggregate / task | A–I | Status |
|------------------|-----|--------|
| S2 ImportTransactions | ⬜ | ⬜ |
| S2 ExportTransactions | ⬜ | ⬜ |
| S2-7 Integration tests | — | ⬜ |
| S2-8 Arch test: no pass-through handlers | — | ⬜ |

**DoD:** Import + Export · handlers own GL orchestration · integration green

---

## Sprint 3

| Aggregate / task | A–I | Status |
|------------------|-----|--------|
| S3 InventoryTransfer (reads) | ⬜ | ⬜ |
| S3 Approve/Reject commands | ⬜ | ⬜ |
| S3 Integration tests (2 GL entries) | — | ⬜ |

**DoD:** Transfer actions all via MediatR · **M1 P0 complete**

---

## Sprint 4

| Aggregate | A–I | Status |
|-----------|-----|--------|
| Complex Entry (EntriesController) | ⬜ | ⬜ |
| Journal Entry | ⬜ | ⬜ |

---

## Sprint 5

| Aggregate | A–I | Status |
|-----------|-----|--------|
| Payment Entry | ⬜ | ⬜ |
| Receipt Entry | ⬜ | ⬜ |

---

## Sprint 6

| Aggregate | A–I | Status |
|-----------|-----|--------|
| Opening Entry | ⬜ | ⬜ |
| Combined Entry | ⬜ | ⬜ |
| Arch test: P1 handlers no pass-through | — | ⬜ |

**DoD:** 6/6 entry types · **M2 P1 complete**

---

## Sprint 7

| Aggregate | A–I | Status |
|-----------|-----|--------|
| FinancialPeriods | ⬜ | ⬜ |
| ChartOfAccounts | ⬜ | ⬜ |

---

## Sprint 8

| Aggregate | A–I | Status |
|-----------|-----|--------|
| Customer | ⬜ | ⬜ |
| Supplier | ⬜ | ⬜ |
| Bank | ⬜ | ⬜ |
| Branch | ⬜ | ⬜ |

---

## Sprint 9

| Aggregate | A–I | Status |
|-----------|-----|--------|
| CashInBox | ⬜ | ⬜ |
| FixedAsset | ⬜ | ⬜ |
| Tax (verify pilot) | ⬜ | ⬜ |
| Currency | ⬜ | ⬜ |
| CostCenter | ⬜ | ⬜ |
| CollectionBook | ⬜ | ⬜ |
| Role | ⬜ | ⬜ |
| GLSettings | ⬜ | ⬜ |
| AccountGuide | ⬜ | ⬜ |

---

## Sprint 10

| Aggregate | A–I | Status |
|-----------|-----|--------|
| Product | ⬜ | ⬜ |
| Variant | ⬜ | ⬜ |
| Item | ⬜ | ⬜ |
| StockBalance | ⬜ | ⬜ |
| AttributeDefinition | ⬜ | ⬜ |
| AttributeValue | ⬜ | ⬜ |
| Size | ⬜ | ⬜ |
| Color (done S1) | — | ⏭️ |
| PackingUnit | ⬜ | ⬜ |
| ManufacturerCompany | ⬜ | ⬜ |
| SellingPrice | ⬜ | ⬜ |

**DoD:** All catalog controllers · **M3 master data complete**

---

## Sprint 11

| ID | Task | Status |
|----|------|--------|
| S11-1 | Retire BaseController dual DI | ⬜ |
| S11-2 | Remove obsolete write services | ⬜ |
| S11-3 | Arch tests fail mode (strict) | ⬜ |
| S11-4 | Full test suite | ⬜ |
| S11-5 | Smoke checklist | ⬜ |
| S11-6 | HYBRID_CQRS_FINAL_SIGNOFF | ⬜ |

**DoD:** **M4 program complete**

---

## Out of scope

| Controller | Reason |
|------------|--------|
| AuthController | Auth service exception |
| AuditLogsController | Read-only |
| DbWorkController | Dev-only |

---

## Cumulative progress

| Metric | Current | Target |
|--------|---------|--------|
| Controllers migrated | 0 / 35 | 35 |
| P0 aggregates | 0 / 3 | 3 |
| P1 entry types | 0 / 6 | 6 |
| Sprint sign-offs | 0 / 12 | 12 |

_Update this table at each sprint retrospective._
