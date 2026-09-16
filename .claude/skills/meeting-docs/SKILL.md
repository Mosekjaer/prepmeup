---
name: meeting-docs
description: Skriver og opdaterer dagsordener og mødereferater på dansk i LaTeX under docs/appendices/process. Brug når brugeren siger "meeting-docs", "dagsorden", "referat", "mødenoter", "skriv det i referatet", "næste møde", "færdig" under et møde, eller bare skriver løse noter fra et igangværende møde.
argument-hint: "[fri tekst fra mødet, eller: færdig]"
---

# meeting-docs

Dagsordener og referater er **på dansk** (undtagelsen fra rapportens engelske sprog). Der er ingen kladdefil - dagens `.tex`-fil er selv kladden. Brugeren skriver løst undervejs, du holder én ren fil opdateret.

## Filer

- Dagsorden: `docs/appendices/process/12-meeting-invitations/NN-agenda-YYYY-MM-DD.tex`
- Referat: `docs/appendices/process/13-meeting-minutes/NN-meeting-minutes-YYYY-MM-DD.tex`

`NN` = højeste eksisterende nummer i **den mappe** + 1 (`ls` mappen først). Findes dagens fil allerede, læs og opdatér den - opret aldrig en fil nummer to for samme dag.

Nye filer oprettes fra `01-agenda-template.tex` / `01-meeting-minutes-template.tex` i samme mappe. Ved et nyt referat: kopiér dagsordenspunkterne ind fra dagens agenda, hvis den findes.

## Sektioner i referatet

Samme struktur som `02-meeting-minutes-2026-09-15.tex`: Mødeinformation (Dato, Tid, Sted, Referent), Deltagere, Fraværende, Dagsorden (`enumerate`), Noter (underafsnit nummereret efter dagsordenspunkt), Beslutninger, Opgaver, Næste møde. Dagsordenen har kun Mødeinformation og Dagsordenspunkter.

Opgaver skrives altid:

```latex
\item \textbf{Ansvarlig:} Malthe \textbf{Opgave:} Docker Compose \textbf{Frist:} fredag
```

`\textbf{Frist:}` udelades, hvis der ikke er nævnt en.

## Placér selv beskeden i den rigtige sektion

Gæt ud fra indholdet, og skriv beskeden om til ordentligt dansk - fulde sætninger, ingen forkortelser fra talesprog. Er det tvetydigt, spørg med ét spørgsmål.

| Brugeren skriver | Du gør |
|---|---|
| `dagsorden: status på backend, Keycloak ja/nej, sprintplanlægning` | Ny agenda for i dag med tre punkter under **Dagsordenspunkter** |
| `Ida og Marie er her ikke, Alexander H tager referat` | Ida og Marie under **Fraværende**, Alexander H som **Referent** |
| `vi valgte postgres fordi coolify har en template` | **Beslutninger**: "Databasen bliver Postgres, fordi Coolify har en færdig template til den." Begrundelsen bliver stående |
| `Malthe laver docker compose til fredag` | **Opgaver**: `\textbf{Ansvarlig:} Malthe \textbf{Opgave:} Docker Compose \textbf{Frist:} fredag` |
| `næste møde onsdag 10` | **Næste møde**: dato og tidspunkt, regnet ud fra dagens dato |
| `michel siger vi skal forstå al kode vi genererer` | **Noter** under det dagsordenspunkt det hører til |

## LaTeX-hygiejne

Escape `&` `%` `_` `#` `$` som `\&` `\%` `\_` `\#` `\$`. Punkter i `itemize`, dagsordener i `enumerate`. Behold `\usepackage[danish]{babel}` og `\usepackage{../../appendix-style}`. Danske anførselstegn: ``` ``tekst'' ```.

## Efter hver ændring

```bash
cd docs/appendices/process/13-meeting-minutes && latexmk -pdf NN-meeting-minutes-YYYY-MM-DD.tex
```

Rapportér fejl ordret. Svar altid: hvad blev tilføjet i hvilken sektion + link til filen i formatet `[sti:linjer](sti#Lx-Ly)`.

## `/meeting-docs færdig`

1. Læs hele filen igennem.
2. Fjern dubletter og slå gentagne punkter sammen.
3. Tjek at hver opgave har en ansvarlig - mangler en, spørg hvem.
4. Tjek at Næste møde er udfyldt.
5. Kompilér.
6. Foreslå opgaverne som **Kaneo-kommentarer** - vis den tekst du ville skrive, og på hvilket kort. **Opret aldrig kort og flyt aldrig noget i Kaneo.** Claude må kun læse og kommentere; statusskift er gruppens beslutning og et bedømt læringsmål.

## Kilder

Denne skill skriver ikke kode og har normalt ingen **Kilder**-sektion. Skriver du undtagelsesvis kode, gælder den almindelige regel: slå op via `context/INDEX.md`, afslut med **Kilder** og markdown-links i formatet `[sti:linjer](sti#Lx-Ly)`, ingen kildehenvisninger i koden, og `⚠ Ikke i kursusmaterialet: <begrundelse>` når mønsteret ikke findes.

- [context/projekt/markdown/07-projektledelse/](context/projekt/markdown/07-projektledelse/) - møde- og procesdokumentation
- [docs/appendices/process/13-meeting-minutes/02-meeting-minutes-2026-09-15.tex](docs/appendices/process/13-meeting-minutes/02-meeting-minutes-2026-09-15.tex) - sektionsstrukturen
