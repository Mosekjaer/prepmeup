# Local Setup

How to compile this project locally and keep it in sync with the team.

The LaTeX project lives in the `docs/` subfolder of the repo. Open the repo
root in VS Code, but run all `latexmk` commands from inside `docs/`.

## Prerequisites

| Tool | Why | Where |
|---|---|---|
| **MiKTeX** (incl. `latexmk`) | Compiles the LaTeX sources | https://miktex.org |
| **draw.io Desktop** | Auto-converts `.drawio` diagrams to PDF | https://get.diagrams.net |
| **VS Code + LaTeX Workshop** *(optional)* | Ctrl+S builds the report | Extension: `James-Yu.latex-workshop` |

Tip: set MiKTeX to install missing packages automatically, otherwise the first
compile hangs on hidden consent dialogs:

```
initexmf --set-config-value "[MPM]AutoInstall=1"
```

## One-time setup after cloning

```
cd docs
copy latexmkrc.example latexmkrc
```

`latexmkrc` is **local only** (gitignored) because Overleaf would otherwise read
it and break — Overleaf compiles with its own defaults. The file does three things:

1. `$out_dir = 'build'` — every build artifact (aux, log, PDF, ...) goes to
   `build/`, which is gitignored. Your report ends up at `build/main.pdf`.
   You can delete `build/` at any time; everything in it is regenerated.
2. `@default_files = ('main.tex')` — running plain `latexmk` builds the report.
3. **DrawIO rule** — any `assets/drawio/<name>.drawio` referenced in the report
   is automatically converted to `assets/drawio/<name>.pdf` when missing or
   outdated. If draw.io is installed somewhere other than
   `C:/Program Files/draw.io/`, edit the path at the bottom of `latexmkrc`.

## Compiling

All commands run from `docs/` (that is where `latexmkrc` and `main.tex` live):

```
cd docs
latexmk                 # build the report -> build/main.pdf
latexmk -pvc            # watch mode: rebuilds on every save
latexmk -C              # clean all build output
```

Appendices are standalone documents and compile individually:

```
cd docs/appendices/technical/02-analysis
latexmk -pdf 01-technical-analysis.tex
```

## Inserting DrawIO diagrams

1. Save the diagram as `assets/drawio/<name>.drawio` (commit the source!)
2. In the report, insert it with (all three arguments are required):

```latex
\drawiofig{<name>}{Caption text}{fig:my-label}
\drawiofig[0.5\linewidth]{<name>}{Half width}{fig:small}
```

latexmk runs draw.io for you during compilation. **Commit the generated
`assets/drawio/*.pdf` as well** — Overleaf cannot run draw.io, it uses the
committed PDF. (An example lives in `report/chapters/1_introduction.tex`.)

## Git sync

The remote is GitHub (`Mosekjaer/prepmeup`). **Always pull before pushing**:

```
git pull
git add .
git commit -m "..."
git push
```

First push/pull asks for credentials: username `git` (sometimes only your overleaf email works, which ever was used to log in with), password is a **Git token**
generated in Overleaf under Account Settings -> Git integration (not your AU
password). Windows caches it after the first time.

If the project is also opened in Overleaf (via GitHub sync), set
**Menu -> Main document -> `docs/main.tex`**.

## The `.vscode/` folder

Shared VS Code settings for LaTeX Workshop live in the **repo root** `.vscode/`
(VS Code only reads settings from the folder you open, so they must sit next to
`docs/`, not inside it). They build with the `latexmk` recipe and output to
`build/` next to the `.tex` file, so Ctrl+S behaves exactly like running
`latexmk` in the terminal instead of dumping aux files in the source folder.

## Folder overview

```
.vscode/              Shared LaTeX Workshop settings (repo root)
docs/                 The LaTeX project - run latexmk from here
main.tex              The report entry point (must stay in docs/ root)
report/
  meta.sty            Title, authors, supervisor - edit here
  styles/             dependencies.sty (ALL packages go here), commands, ...
  frontmatter/        01-titlepage ... 04-table-of-contents
  chapters/           01-introduction ... 10-appendix (the polished report)
appendices/           Raw standalone documents (technical/ + process/),
                      numbered like the SW3PRJ3 hand-in
standalone/           One-off documents (proposal, pitch, domain models) - not part of the report
assets/
  drawio/             .drawio sources + auto-generated PDFs
  images/             Photos, logos
  figures/            Other figure files
  references.bib      Bibliography
build/                All build output (gitignored, safe to delete)
```
