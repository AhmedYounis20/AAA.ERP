# Phase 8 sign-off — Naming debt cleanup

**Goal:** Fix longstanding typos with compatibility shims where needed.  
**Plan:** [ARCHITECTURE_ENHANCEMENT_PLAN.md](./ARCHITECTURE_ENHANCEMENT_PLAN.md) Phase 8  
**Date:** July 2026

---

## Exit criteria

| Item | Status | Evidence |
|------|--------|----------|
| `ComandValidators` → `CommandValidators` | ✅ | Folder/namespace rename + arch test |
| `DbworkController` → `DbWorkController` | ✅ | Dev-only; route `api/DbWork` |
| `SubLeadger` → `SubLedger` | 📋 | [ADR-010](./adr/010-subledger-rename-timeline.md) — v2.0 with API stability |
| No `Compined` types | ✅ | Existing arch test |
