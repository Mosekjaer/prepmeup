#!/usr/bin/env bash
# Single source of truth for branch names and commit/PR headers.
# Used by .githooks/commit-msg, .githooks/pre-push and .github/workflows/conventions.yml.
#
#   check-conventions.sh branch <name>
#   check-conventions.sh header <text>

set -euo pipefail

TYPES='feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert'
BRANCH_RE="^($TYPES)/[a-z0-9]+(-[a-z0-9]+)*$"
HEADER_RE="^($TYPES)(\([a-z0-9-]+\))?!?: [^ ].*$"
MAX_HEADER=72

fail() { printf '%s\n' "$@" >&2; exit 1; }

case "${1:-}" in
  branch)
    name="${2:-}"
    [[ "$name" =~ $BRANCH_RE ]] || fail \
      "Invalid branch name: '$name'" \
      "Expected <type>/<slug>, e.g. feat/pmu-12-household-profile" \
      "Types: ${TYPES//|/, }. Slug: lowercase letters, digits, single hyphens."
    ;;
  header)
    header="${2:-}"
    [[ "$header" =~ $HEADER_RE ]] || fail \
      "Invalid header: '$header'" \
      "Expected <type>(<scope>): <description>, e.g. feat: add household profile endpoint" \
      "Types: ${TYPES//|/, }. Scope is optional, lowercase."
    [ "${#header}" -le "$MAX_HEADER" ] || fail \
      "Header is ${#header} characters, max is $MAX_HEADER: '$header'"
    ;;
  *)
    fail "usage: $0 branch <name> | header <text>"
    ;;
esac
