---
name: report-check
description: Kritiserer projektrapporten i docs/report - tegnoptælling mod 72.000, kapitelstruktur mod AU-ECE-skabelonen, engelsk fagsprog, figur- og tabelreferencer og kildehenvisninger. Brug når brugeren siger "tjek rapporten", "report-check", "hvor mange tegn", "tegngrænse", "er rapporten klar", "læs korrektur" eller vil have feedback på et kapitel.
argument-hint: "[valgfrit kapitel, fx 04-architecture]"
---

# report-check

**Du skriver aldrig rapportprosa.** Ikke omskrevne sætninger, ikke forslag til formuleringer, ikke udfyldte afsnit. Rapporten er et bedømt læringsmål. Du leverer kritik, gruppen skriver.

## 1. Tegnoptælling

```bash
cd docs
for f in report/chapters/0[1-9]-*.tex; do
  printf "%-40s %s\n" "$f" "$(texcount -char -template='{SUM}' "$f")"
done
for f in report/chapters/0[1-9]-*.tex; do
  texcount -char -template='{SUM}' "$f"; echo
done | awk '{s+=$1} END {printf "%d / 72000 (%d tilbage)\n", s, 72000-s}'
```

`-template='{SUM}'` er det eneste flag, der giver ét rent tal pr. fil. `-q -sum` printer stadig en hel blok, som `awk` ikke kan summere.

Vis både totalen, tallet pr. kapitel og hvor mange tegn der er tilbage. Tæller **ikke** med: forside, abstract, indholdsfortegnelse, referencer, figurer, tabeller og bilag - derfor kun `chapters/01` til `09`, og derfor `10-appendix.tex` aldrig.

## 2. Struktur mod AU-ECE-skabelonen

Læs kapitelrækkefølgen i `context/projekt/markdown/13-rapport/rapportskabelon-au-ece.md` og hold den op mod `docs/report/chapters/`. Rapporten har i dag: 01 introduction, 02 method, 03 requirements, 04 architecture, 05 design, 06 integration-testing, 07 acceptance-testing, 08 discussion, 09 conclusion. Rapportér afvigelser: manglende afsnit fra skabelonen, afsnit i forkert kapitel, tomme sektioner.

## 3. Fund

Ét fund pr. linje, altid i formatet:

```
fil:linje — problem — hvorfor — kilde
```

Aldrig omskrevet tekst. Tjek:

- **Engelsk sprog og fagterminologi** - rapporten er på engelsk. Danglish, inkonsistente termer (brug kursets ord: *layers*, ikke *Clean Architecture*), forkortelser uden første-gangs-forklaring.
- **Selvstændig læsbarhed** - kan en læser uden for gruppen forstå kapitlet uden at kende koden eller mødereferaterne? Udefinerede domænebegreber er et fund.
- **Figurer og tabeller** - hver figur og tabel skal have caption, label og mindst én `\ref` i brødteksten. `\ref`-loops og forældede numre er fund.
- **Kildehenvisninger** - påstande om metode, arkitektur og test skal have citation. Tjek at hver `\cite` findes i bib-filen, og at hver kilde bruges.
- **Begrundelser** - "vi valgte X" uden hvorfor er et fund. Alternativer og trade-offs hører i kapitlet eller i discussion.

## 4. AI-deklarationen

Mind altid om til sidst:

- `docs/appendices/process/11-team/03-ai-declaration.tex` **skal** afleveres som bilag, når AI er brugt. Den udfyldes manuelt af gruppen.
- Rapportens afsnit om brug af AI (kapitel 2) skal dække alle fire: **Delegation** (hvad blev uddelegeret), **Description** (hvordan blev værktøjet brugt - prompts, workflow), **Discernment** (hvordan blev outputtet bedømt og verificeret) og **Diligence** (ansvarlighed og transparens - hvem står inde for koden).

## Kilder

Afslut svaret med en **Kilder**-sektion med markdown-links i formatet `[sti:linjer](sti#Lx-Ly)`. Slå op via `context/INDEX.md` før du læser kursusmateriale. Findes et krav ikke i materialet, skriv `⚠ Ikke i kursusmaterialet: <begrundelse>`.

- [context/projekt/markdown/13-rapport/rapportskabelon-au-ece.md](context/projekt/markdown/13-rapport/rapportskabelon-au-ece.md) - kapitelstruktur, AI-afsnittet
- [context/projekt/markdown/13-rapport/god-rapportskrivning.md](context/projekt/markdown/13-rapport/god-rapportskrivning.md) - skriveregler
- [context/projekt/markdown/13-rapport/l27-projektrapport.md](context/projekt/markdown/13-rapport/l27-projektrapport.md) - krav til projektrapporten
- [context/projekt/markdown/13-rapport/lean-dokumentation.md](context/projekt/markdown/13-rapport/lean-dokumentation.md) - omfang og dokumentationsniveau
- [context/projekt/markdown/13-rapport/overleaf-og-latex.md](context/projekt/markdown/13-rapport/overleaf-og-latex.md) - LaTeX-opsætning
