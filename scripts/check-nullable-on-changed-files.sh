#!/usr/bin/env bash
# Phase 3 — fail CI when changed .cs files introduce nullable reference warnings (CS86**).
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

BASE_REF="${GITHUB_BASE_REF:-origin/main}"
if ! git rev-parse --verify "${BASE_REF}" >/dev/null 2>&1; then
  BASE_REF="origin/main"
fi
if ! git rev-parse --verify "${BASE_REF}" >/dev/null 2>&1; then
  echo "Base ref '${BASE_REF}' not found; skipping nullable gate."
  exit 0
fi

mapfile -t CHANGED < <(git diff --name-only --diff-filter=ACMRT "${BASE_REF}"...HEAD -- '*.cs' | grep -vE '/(bin|obj)/' || true)

if [ ${#CHANGED[@]} -eq 0 ]; then
  echo "No .cs files changed; nullable gate passed."
  exit 0
fi

echo "Nullable gate — checking CS86** warnings in changed files:"
printf '  - %s\n' "${CHANGED[@]}"

dotnet restore AAA.ERP.sln >/dev/null
dotnet build AAA.ERP.sln -c Release --no-restore 2>&1 | tee /tmp/erp-nullable-build.log

VIOLATIONS=0
for file in "${CHANGED[@]}"; do
  if grep -E "warning CS86[0-9]{2}:.*${file}([:(]|$)" /tmp/erp-nullable-build.log >/dev/null; then
    echo "::error file=${file}::Nullable reference warning in changed file"
    grep -E "warning CS86[0-9]{2}:.*${file}" /tmp/erp-nullable-build.log || true
    VIOLATIONS=1
  fi
done

if [ "$VIOLATIONS" -ne 0 ]; then
  echo "Nullable gate failed. Fix CS86** warnings in changed files before merging."
  exit 1
fi

echo "Nullable gate passed."
