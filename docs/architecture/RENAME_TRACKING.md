# Rename tracking (Phase 0 — task 0.3.2)

Track large renames separately from modular monolith migration to avoid mixed PRs.

## Issue 1: Rename `Infrastracture` → `Infrastructure`

| Field | Value |
|-------|-------|
| **Priority** | Medium |
| **Effort** | High (~1 week) |
| **Scope** | Project folder, solution, namespaces, all `using` statements |
| **Risk** | Merge conflicts if parallel feature work |
| **Recommendation** | Dedicated branch; run full test suite after rename |
| **Target** | Phase 4 or quiet sprint |

**Files affected:** `ERP.Infrastracture/` project and all references across solution.

## Issue 2: Rename `Compined` → `Combined`

| Field | Value |
|-------|-------|
| **Priority** | Medium |
| **Effort** | Medium (~2–3 days) |
| **Scope** | 19 files (controllers, services, handlers, commands) — see `TECHNICAL_DEBT.md` |
| **Risk** | API route change if controller renamed (`CompinedEntries` → `CombinedEntries`) |
| **Recommendation** | Keep route alias or coordinate frontend update |
| **Target** | Phase 4 |

**Note:** Frontend may reference `/api/CompinedEntries` — verify before renaming controller route.

## Creating GitHub issues

If using GitHub, create issues from this template:

```bash
gh issue create --title "Tech debt: Rename Infrastracture to Infrastructure" \
  --body "See docs/architecture/RENAME_TRACKING.md#issue-1"

gh issue create --title "Tech debt: Rename Compined to Combined" \
  --body "See docs/architecture/RENAME_TRACKING.md#issue-2"
```

## Status

| Issue | GitHub | Status |
|-------|--------|--------|
| Infrastructure rename | Create via `gh issue create` when CLI available | Documented — ready to file |
| Combined rename | Create via `gh issue create` when CLI available | Documented — ready to file |

> `gh` was not available in the local environment during Phase 0. Copy issue bodies from the sections above into GitHub manually, or run the commands in this file after installing the GitHub CLI.
