# Phase 7 sign-off — Application layer standardization

**Goal:** Clarify hybrid CQRS conventions; reduce Domain command drift.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 7  
**Date:** July 2026

---

## Exit criteria

| ID | Task | Status | Evidence |
|----|------|--------|----------|
| 7.1 | Document hybrid CQRS | ✅ | [MODULE_TEMPLATE.md](./MODULE_TEMPLATE.md) § Application layer — hybrid CQRS |
| 7.2 | Move Domain commands → Application | ✅ | SubLeadger base commands, `GLSettingInputModel`, `CurrencyInputModel` |
| 7.3 | Move OutputDtos → Application | ⏭️ | Incremental backlog (large surface) |
| 7.4 | MediatR queries for reads | ⏭️ | Optional; not required for exit |
| 7.5 | Thin services | ⏭️ | Ongoing for new features per MODULE_TEMPLATE |

**Arch test:** `Accounting_Domain_should_not_contain_commands_namespace`
