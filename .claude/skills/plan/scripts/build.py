#!/usr/bin/env python3
"""Build a self-contained visual plan HTML from a markdown plan file.

Usage:
    python3 build.py <plan.md> [--out <plan.html>]

Output: one self-contained HTML file (vendored marked + mermaid + highlight.js
inlined). Open directly via file:// — no server, no network.
"""
import argparse
import base64
import datetime
import json
import re
import sys
from pathlib import Path

SKILL_DIR = Path(__file__).resolve().parent.parent
TEMPLATE = SKILL_DIR / "assets" / "template.html"
LOGO = SKILL_DIR / "assets" / "prepmeup-logo.svg"
VENDOR = SKILL_DIR / "assets" / "vendor"


def js_safe(code: str) -> str:
    return code.replace("</script", "<\\/script")


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("plan", help="path to markdown plan file")
    ap.add_argument("--out", help="output html path (default: alongside plan)")
    args = ap.parse_args()

    plan_path = Path(args.plan).resolve()
    if not plan_path.exists():
        print(f"fejl: {plan_path} findes ikke", file=sys.stderr)
        return 1

    source = plan_path.read_text(encoding="utf-8")
    slug = plan_path.stem
    m = re.search(r"^#\s+(.+)$", source, re.MULTILINE)
    title = m.group(1).strip() if m else slug
    built = datetime.datetime.now().strftime("%Y-%m-%d %H:%M")
    logo_svg = LOGO.read_text(encoding="utf-8").strip()
    favicon_svg = logo_svg.replace("currentColor", "#4A7C59")
    favicon = base64.b64encode(favicon_svg.encode("utf-8")).decode("ascii")

    html = TEMPLATE.read_text(encoding="utf-8")
    html = html.replace("__TITLE__", title)
    html = html.replace("__SLUG__", slug)
    html = html.replace("__BUILT__", built)
    html = html.replace("__PLAN_LOGO__", logo_svg)
    html = html.replace("__PLAN_FAVICON__", favicon)
    html = html.replace("__PLAN_JSON__", json.dumps(source).replace("</", "<\\/"))
    html = html.replace("__SLUG_JSON__", json.dumps(slug))
    html = html.replace("__MARKED__", js_safe((VENDOR / "marked.min.js").read_text(encoding="utf-8")))
    html = html.replace("__HLJS__", js_safe((VENDOR / "highlight.min.js").read_text(encoding="utf-8")))
    html = html.replace("__MERMAID__", js_safe((VENDOR / "mermaid.min.js").read_text(encoding="utf-8")))

    out_path = Path(args.out).resolve() if args.out else plan_path.with_suffix(".html")
    out_path.write_text(html, encoding="utf-8")
    print(out_path)
    print(f"file://{out_path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
