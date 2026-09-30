# Local Setup

How to compile this project locally and keep it in sync with the team.

The LaTeX project lives in the `docs/` subfolder of the repo. Open the repo
root in VS Code, but run all `latexmk` commands from inside `docs/`.

## Prerequisites

| Tool | Why | Where |
|---|---|---|
| **MiKTeX** (Windows) or **TeX Live** (Linux, `texlive-full` + `latexmk` + `biber`) | Compiles the LaTeX sources | https://miktex.org / https://tug.org/texlive |
| **draw.io Desktop** (Linux: `snap install drawio` or the .deb) | Auto-converts `.drawio` diagrams to PDF | https://get.diagrams.net |
| **VS Code + LaTeX Workshop** *(optional)* | Ctrl+S builds the report | Extension: `James-Yu.latex-workshop` |

Tip: set MiKTeX to install missing packages automatically, otherwise the first
compile hangs on hidden consent dialogs:

```
initexmf --set-config-value "[MPM]AutoInstall=1"
```

## One-time setup after cloning

```
cd docs
copy latexmkrc.example latexmkrc      # Windows
cp   latexmkrc.example latexmkrc      # Linux / macOS
```

`latexmkrc` is **local only** (gitignored) because it has machine-specific paths
(e.g. the draw.io binary location). The file does three things:

1. `$out_dir = 'build'` — every build artifact (aux, log, PDF, ...) goes to
   `build/`, which is gitignored. Your report ends up at `build/main.pdf`.
   You can delete `build/` at any time; everything in it is regenerated.
2. `@default_files = ('main.tex')` — running plain `latexmk` builds the report.
3. **DrawIO rule** — any `assets/drawio/<name>.drawio` referenced in the report
   is automatically converted to `assets/drawio/<name>.pdf` when missing or
   outdated. The rule picks the binary by OS: `C:/Program Files/draw.io/` on
   Windows, `drawio` on PATH on Linux, the app bundle on macOS. Installed
   elsewhere? Edit the path at the bottom of `latexmkrc`.
   Linux snap note: the snap can only read non-hidden folders under your
   home, so keep the repo somewhere like `~/Documents`, not `~/.something`.

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

latexmk runs draw.io for you during compilation and regenerates
`assets/drawio/*.pdf` whenever the `.drawio` source is new or has changed.
That generated PDF is a build artifact and is gitignored — only commit the
`.drawio` source. (An example lives in `report/chapters/1_introduction.tex`.)

## Git sync

The remote is GitHub (`Mosekjaer/prepmeup`). We use GitHub Flow: never commit
directly to `main`. Work on a branch and open a pull request. The full rules are
in appendix 10.1 (`appendices/process/10-method/01-development-guidelines.tex`).

```
git switch main
git pull
git switch -c docs/update-technical-analysis
git add <files>
git commit -m "docs: update technical analysis"
git push -u origin HEAD
```

Then open a pull request against `main` on GitHub.

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
