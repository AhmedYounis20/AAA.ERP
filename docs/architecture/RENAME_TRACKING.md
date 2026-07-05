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
| **Target** | Phase 4 |

**Outcome (Phase 4.1.3):** Horizontal project removed in Wave 3E; persistence kernel is `ERP.Persistence/` with namespaces `ERP.Infrastructure.*`. Root namespace in `.csproj` is `ERP.Persistence`. Full folder rename to match namespace is a **3E follow-up** (Increment 6).

## Issue 2: Rename `Compined` → `Combined`

| Field | Value |
|-------|-------|
| **Priority** | Medium |
| **Effort** | Medium (~2–3 days) |
| **Scope** | Controllers, services, handlers, commands, validators |
| **Risk** | API route / JSON wire breaking change |
| **Recommendation** | Keep route alias and legacy JSON enum wire value |
| **Target** | Phase 4 |

**Outcome (Phase 4.1.4):** All `Compined*.cs` type/file names removed. **Intentionally retained** (not debt):

| Artifact | Legacy value | Purpose |
|----------|--------------|---------|
| Route | `/api/CompinedEntries` | Frontend compatibility (`CombinedEntriesController`) |
| JSON `EntryType` | `"Compined"` | `EntryTypeJsonConverter` reads/writes legacy wire name |
| EF value converter | SQL legacy rows | `EntryTypeValueConverter` reads `Compined` from DB |

Verified: `NamespaceDebtTests.Solution_should_not_contain_legacy_Compined_type_names` — no `*Compined*` CLR type names.

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
| Infrastructure rename | — | **Done** (Phase 4.1.3 — July 2026) |
| Combined rename | — | **Done** (Phase 4.1.4 — July 2026) |

**Related:** [TECHNICAL_DEBT.md](../../TECHNICAL_DEBT.md) · Module onboarding [READMEs](../../Modules/Accounting/README.md) (Phase 4.2.4)

> `gh` was not available in the local environment during Phase 0. Copy issue bodies from the sections above into GitHub manually, or run the commands in this file after installing the GitHub CLI.
