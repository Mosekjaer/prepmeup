#!/usr/bin/env python3
"""Genererer Antigravity-subagenter og -skills ud fra Claude Codes .claude/.

Kilden er ALTID .claude/agents/ og .claude/skills/. Ret aldrig .agents/agents/ eller
.agents/skills/ i hånden — kør dette script i stedet, ellers driver de to sæt fra hinanden.

    python3 .agents/sync-agents.py          # skriv filerne
    python3 .agents/sync-agents.py --check  # fejl hvis de er ude af sync (til CI)

Agenter: forskellen mellem de to formater er ét felt. Claude Codes "model: sonnet" er
Claude-specifikt og fjernes. Resten er identisk markdown + YAML-frontmatter.

Skills: kun dem i SHARED_SKILLS kopieres byte for byte. De øvrige bruger Claude Code-
specifikke værktøjer og hooks. Kopi frem for symlink, fordi git-symlinks bliver til
tekstfiler på Windows uden core.symlinks.
"""
import re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
SRC, DST = ROOT / ".claude/agents", ROOT / ".agents/agents"
SKILL_SRC, SKILL_DST = ROOT / ".claude/skills", ROOT / ".agents/skills"
SHARED_SKILLS = ["drawio-skill"]
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

def files(root: Path) -> set:
    return {p.relative_to(root) for p in root.rglob("*")
            if p.is_file() and "__pycache__" not in p.parts}

for name in SHARED_SKILLS:
    src_root, dst_root = SKILL_SRC / name, SKILL_DST / name
    want = files(src_root)
    have = files(dst_root) if dst_root.exists() else set()
    for rel in sorted(want):
        src, dst = src_root / rel, dst_root / rel
        if rel not in have or dst.read_bytes() != src.read_bytes():
            stale.append(f"skills/{name}/{rel}")
            if not check:
                dst.parent.mkdir(parents=True, exist_ok=True)
                dst.write_bytes(src.read_bytes())
    for rel in sorted(have - want):           # ryd op efter slettede filer
        stale.append(f"skills/{name}/{rel} (forældet)")
        if not check:
            (dst_root / rel).unlink()

if SKILL_DST.exists():                        # ryd op efter skills fjernet fra SHARED_SKILLS
    for dst_root in sorted(p for p in SKILL_DST.iterdir() if p.is_dir()):
        if dst_root.name not in SHARED_SKILLS:
            stale.append(f"skills/{dst_root.name} (forældet)")
            if not check:
                for p in sorted(dst_root.rglob("*"), reverse=True):
                    p.unlink() if p.is_file() else p.rmdir()
                dst_root.rmdir()

if check and stale:
    print("ude af sync: " + ", ".join(stale) + "\nkør: python3 .agents/sync-agents.py")
    sys.exit(1)
print(f"{len(stale)} opdateret" if stale else "alle agenter og skills er i sync")
