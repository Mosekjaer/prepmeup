#!/usr/bin/env bash
# Tjekker om det lokale repo er bagud i forhold til origin, og fortæller Claude det.
#
# To roller:
#   SessionStart  — fetcher altid, og lægger status i konteksten.
#   PreToolUse    — kører før Edit/Write. Fetcher kun hvis sidste fetch er
#                   ældre end THROTTLE sekunder, så hver filredigering ikke
#                   koster et netværkskald. Blokerer aldrig.
#
# Hooken hverken puller, merger eller rebaser. Den fortæller kun.

set -uo pipefail

REPO="${CLAUDE_PROJECT_DIR:-$(pwd)}"
THROTTLE=300          # sekunder mellem fetch i PreToolUse
FETCH_TIMEOUT=10      # hård grænse, så en død netværksforbindelse ikke hænger
EVENT="${1:-PreToolUse}"
STAMP="$REPO/.git/.claude-last-fetch"

cd "$REPO" 2>/dev/null || exit 0
git rev-parse --git-dir >/dev/null 2>&1 || exit 0
git remote get-url origin >/dev/null 2>&1 || exit 0

emit() {  # $1 = tekst til Claude
  python3 - "$EVENT" "$1" <<'PY'
import json, sys
print(json.dumps({"hookSpecificOutput": {
    "hookEventName": sys.argv[1],
    "additionalContext": sys.argv[2],
}}))
PY
}

now=$(date +%s)
last=0
[ -f "$STAMP" ] && last=$(cat "$STAMP" 2>/dev/null || echo 0)

if [ "$EVENT" = "SessionStart" ] || [ $((now - last)) -ge "$THROTTLE" ]; then
  timeout "$FETCH_TIMEOUT" git fetch --quiet origin 2>/dev/null && echo "$now" > "$STAMP"
fi

branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null)
upstream=$(git rev-parse --abbrev-ref --symbolic-full-name '@{upstream}' 2>/dev/null) \
  || upstream="origin/$branch"
git rev-parse --verify --quiet "$upstream" >/dev/null || exit 0

read -r behind ahead < <(git rev-list --left-right --count "$upstream...HEAD" 2>/dev/null | tr '\t' ' ')
[ -z "${behind:-}" ] && exit 0

if [ "$behind" -gt 0 ]; then
  subjects=$(git log --format='  %h %an: %s' -3 "HEAD..$upstream" 2>/dev/null)
  emit "git: $branch er $behind commit(s) bagud for $upstream (og $ahead foran).
Nyeste på remote:
$subjects
Sig det til brugeren og foreslå 'git pull' FØR du redigerer filer, der kan være ændret. Pull ikke selv."
elif [ "$EVENT" = "SessionStart" ]; then
  emit "git: $branch er ajour med $upstream ($ahead commit(s) foran, ikke pushet)."
fi
exit 0
