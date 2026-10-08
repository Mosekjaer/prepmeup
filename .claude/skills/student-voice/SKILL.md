---
name: student-voice
description: Omskriver et bilag eller standalone-dokument i docs/ til almindelig studenterstemme, som bilag 2.1 blev det - kort versionshistorik, ingen \texttt i løbende tekst, prosa frem for fed-start-bullets, korte sætninger med "we". Indholdet, kilderne og labels bevares. Brug når brugeren siger "student-voice", "skriv det som en studerende", "det lyder som AI", "for teknisk", "omskriv bilaget", "gør teksten simplere" eller peger på en .tex-fil i docs/, der er skrevet tæt og teknisk.
argument-hint: "<sti til .tex-fil i docs/>"
---

# student-voice

Omskriver ét dokument ad gangen, så det lyder som en studerende på 4. semester har skrevet det. Forlægget er `docs/appendices/technical/02-analysis/01-technical-analysis.tex` og commit `49a432f` (`git show 49a432f` viser før og efter).

Reglen kommer fra `tasks/lessons.md` (2026-09-30, "Rapporttekst skal lyde som studerende, ikke som AI").

## Afgrænsning

- **Bilag og standalone** (`docs/appendices/`, `docs/standalone/`): omskriv.
- **Rapportkapitler** (`docs/report/chapters/`): rapportprosa er et bedømt læringsmål. Lever fund i formatet fra `report-check` (`fil:linje — problem — hvorfor`), og omskriv kun, hvis et gruppemedlem beder om det for det konkrete kapitel.
- **Dagsordener og referater**: brug `meeting-docs`, ikke denne skill.
- Du ændrer **form, ikke indhold**. Ingen nye påstande, ingen nye kilder, ingen slettede beslutninger. Finder du en påstand, der er forkert eller mangler kilde, så sig det i chatten i stedet for at rette den i stilhed.

## 1. Før du skriver

1. Læs hele dokumentet. Læs også 30-40 linjer af forlægget, så tonen sidder.
2. Gem det, der skal overleve omskrivningen:

```bash
f=<fil>.tex
grep -oE '\\cite(\[[^]]*\])?\{[^}]*\}' "$f" | grep -oE '\{[^}]*\}$' | tr ',' '\n' | tr -d '{} ' | sort -u > /tmp/cites.before
grep -oE '\\(label|ref)\{[^}]*\}' "$f" | sort -u > /tmp/refs.before
grep -oE '(FR|NFR)-[0-9]+' "$f" | sort -u > /tmp/reqs.before
```

3. Find ud af, om andre dokumenter peger ind i dette (`grep -rn "<bilagsnummer>\|<label>" docs --include=*.tex`). Afsnit, de henviser til, skal stadig findes.

## 2. Reglerne

**Stemme**

- "We" og aktiv form. Korte sætninger, én pointe ad gangen.
- Almindelige ord: "the main branch", "the Domain project", "the health endpoint", "the production compose file".
- Sig tingene ligeud. "We chose X because Y." "That is fine for us, because Z."
- Fortæl det i den rækkefølge, det skete eller blev besluttet.

**Væk med**

| Mønster | I stedet |
|---|---|
| `\texttt{}` i løbende tekst | almindelige ord; identifieren i ren tekst kun hvis læseren skal skrive den |
| filstier, porte, config-nøgler, klassenavne i prosa | "the realm file", "the API's CORS policy" |
| `\item \textbf{Overskrift.} tekst` | prosa, eller en almindelig `\item` der starter med sætningen |
| " -- " og " --- " som indskud | komma, punktum eller en ny sætning |
| semikolon mellem hovedsætninger | to sætninger |
| "The question is not X but Y", "not a habit; it is the shape of the graph" | sig Y direkte |
| "The price is", "There is a limit:", "It is worth noting" | "The downside is", eller bare påstanden |
| tre-leddede opremsninger for effektens skyld, slagord til sidst i et afsnit | slet |
| "the course says / like in the course" med slide som kilde | påstanden med den originale kilde, se Referencer i `docs/AGENTS.md` |
| detaljer, der står i et andet bilag | "see appendix 10.3" |
| omtale af AI-værktøjer som medforfatter eller af implementeringsstatus, der hurtigt forældes | slet, medmindre dokumentet handler om det |

**Bliver**

- Tabeller med alternativer (Option / For / Against) og tabeller med faktiske værdier.
- `lstlisting`-blokke, når læseren skal køre kommandoen eller se den præcise linje. Forklar blokken i almindelige ord bagefter.
- Nummererede lister for en procedure. Punktlister for korte, sideordnede ting.
- Alle `\cite`, `\label`, `\ref` og kravnumre (FR-xx, NFR-xx).

**Versionshistorik**

Én kort frase pr. version: "First version", "Added client technologies". Omskrivningen får ikke sin egen prale-linje; brug "Simpler text" eller læg den sammen med dagens anden ændring. Forfatterlinjen er `SW4PRJ4 - Group 3`.

**Eksempel** (fra `49a432f`)

Før:
> The course describes the layered style as concerns split into stacked groups, where dependencies are only allowed from a higher layer to a lower one. The benefits it lists are separation of concerns, lower coupling, and that a layer can be replaced and tested on its own when the layers talk through interfaces and dependency injection.

Efter:
> A layered architecture splits the code into layers stacked on top of each other, where a layer may only use the layers below it. The idea is to keep things apart, so a layer can be changed or tested on its own.

Før:
> `\item \textbf{Local development:} \texttt{docker compose up} starts \texttt{api} (port 5001), \texttt{db} and \texttt{keycloak} (port 8082) on the network \texttt{prepmeup-net}.`

Efter:
> Locally, one docker compose command starts the API, the database and Keycloak.

## 3. Efter du har skrevet

```bash
f=<fil>.tex
grep -oE '\\cite(\[[^]]*\])?\{[^}]*\}' "$f" | grep -oE '\{[^}]*\}$' | tr ',' '\n' | tr -d '{} ' | sort -u | diff /tmp/cites.before - && echo "cites ok"
grep -oE '\\(label|ref)\{[^}]*\}' "$f" | sort -u | diff /tmp/refs.before - && echo "refs ok"
grep -oE '(FR|NFR)-[0-9]+' "$f" | sort -u | diff /tmp/reqs.before - && echo "reqs ok"
printf 'texttt=%s bold-items=%s dashes=%s semicolons=%s\n' \
  "$(grep -o '\\texttt' "$f" | wc -l)" "$(grep -c '\\item \\textbf' "$f")" \
  "$(grep -oE ' ---? ' "$f" | wc -l)" "$(grep -o '; ' "$f" | wc -l)"
```

- En forskel i `cites`, `refs` eller `reqs` skal kunne forklares i chatten. Ellers er noget faldet ud.
- De fire tællere skal være 0 eller tæt på. `\texttt` i en tabelcelle eller i præamblen er i orden; `\texttt` i en sætning er ikke.
- Byg dokumentet, se Build i `docs/AGENTS.md`: `latexmk -pdf -outdir=build <fil>.tex` i bilagets mappe. Ingen udefinerede citations eller referencer.
- Læs resultatet igennem én gang som læser. Lyder en sætning som en lærebog eller en reklame, så skriv den om.

## 4. Rapportér

Kort i chatten: hvad der er ændret i form, hvad der er slettet og hvorfor, og hvilke påstande du faldt over undervejs, som gruppen bør tjekke. Indholdsmæssige rettelser er gruppens.
