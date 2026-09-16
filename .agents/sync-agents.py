#!/usr/bin/env python3
"""Genererer Antigravity-subagenter ud fra Claude Codes .claude/agents/*.md.

Kilden er ALTID .claude/agents/. Ret aldrig .agents/agents/ i hånden — kør dette
script i stedet, ellers driver de to sæt fra hinanden.

    python3 .agents/sync-agents.py          # skriv filerne
    python3 .agents/sync-agents.py --check  # fejl hvis de er ude af sync (til CI)

Forskellen mellem de to formater er ét felt: Claude Codes "model: sonnet" er
Claude-specifikt og fjernes. Resten er identisk markdown + YAML-frontmatter.
"""
import re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
SRC, DST = ROOT / ".claude/agents", ROOT / ".agents/agents"
check = "--check" in sys.argv

def convert(text: str) -> str:
    return re.sub(r"^model: .*\n", "", text, flags=re.M)

DST.mkdir(parents=True, exist_ok=True)
stale = []
for src in sorted(SRC.glob("*.md")):
    want = convert(src.read_text(encoding="utf-8"))
    dst = DST / src.name
    if not dst.exists() or dst.read_text(encoding="utf-8") != want:
        stale.append(src.name)
        if not check:
            dst.write_text(want, encoding="utf-8")

for dst in sorted(DST.glob("*.md")):          # ryd op efter slettede agenter
    if not (SRC / dst.name).exists():
        stale.append(dst.name + " (forældet)")
        if not check:
            dst.unlink()

if check and stale:
    print("ude af sync: " + ", ".join(stale) + "\nkør: python3 .agents/sync-agents.py")
    sys.exit(1)
print(f"{len(stale)} opdateret" if stale else "alle agenter er i sync")
