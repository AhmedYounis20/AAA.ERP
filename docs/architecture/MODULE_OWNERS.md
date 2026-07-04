# Module ownership

Module owners review PRs that touch their area and approve architectural changes within their boundary.

| Module | Scope | Owner | Backup |
|--------|-------|-------|--------|
| **Accounting** | `Controllers/Account`, GL, entries, sub-ledgers, financial periods | Ahmed Younis | — |
| **Inventory** | `Controllers/Inventory`, products, variants, stock, transactions | Ahmed Younis | — |
| **Platform** | Audit, caching, localization, shared cross-cutting | Ahmed Younis | — |
| **Identity** | `Controllers/Identity`, auth, users, roles | Ahmed Younis | — |
| **Host** | `ERP.API` startup, DI composition, migrations, CI | Ahmed Younis | — |

> Update backup owners when the team grows beyond one developer.

## Review expectations

- **Accounting PRs** — Accounting owner reviews GL posting, entries, COA changes
- **Inventory PRs** — Inventory owner reviews stock logic and catalog changes
- **Cross-module PRs** — Both Accounting and Inventory owners review
- **Host / CI / arch tests** — Host owner reviews

## Related documents

- [MODULAR_MONOLITH.md](./MODULAR_MONOLITH.md)
- [CONTRIBUTING.md](./CONTRIBUTING.md)
- [PHASE_0_RETROSPECTIVE.md](./PHASE_0_RETROSPECTIVE.md)
