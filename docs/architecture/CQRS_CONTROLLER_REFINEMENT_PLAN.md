# CQRS & Controller Refinement Plan

> **Superseded by:** [HYBRID_CQRS_PLAN.md](./HYBRID_CQRS_PLAN.md) — the complete, authoritative hybrid CQRS plan (MediatR for actions, read services for GET).

The document [HYBRID_CQRS_PLAN.md](./HYBRID_CQRS_PLAN.md) includes:

- One-page golden rule (HTTP verb → MediatR vs read service)
- Full project layout, naming conventions, and code templates
- Controller, command handler, and read service patterns
- All 35+ aggregate inventory (Accounting + Inventory)
- Cross-module transactional flows
- Testing, architecture enforcement, phased rollout

Use **HYBRID_CQRS_PLAN.md** for all new development and migration work.
