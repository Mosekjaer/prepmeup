# 13 — Rapport (projektrapport, rapportskrivning, dokumentation, LaTeX)

Materiale om semesterprojektrapporten: struktur- og indholdskrav (L27, SWISE-01), ECE's generelle vejledning i god rapportskrivning (Samuel Thrysøe), AU ECE's LaTeX-rapportskabelon, artiklen om lean dokumentation og kursussiden om AU Overleaf. Kilderne kommer fra to Brightspace-rum: SWISE-01 "L27-28 Projektrapport og rapport eksempler" (underviser: "Jenny" Jung Min Kim) og SW4PRJ4-02 Projekt 4 "Materiale".

Centrale hårde krav på tværs af kilderne: maks. **72.000 tegn** (30 normalsider à 2400 tegn) talt fra indledning til og med konklusion; figurer/tabeller/forside/abstract/indholdsfortegnelse/referencer/bilag tæller ikke; rapporten skal kunne læses selvstændigt; bilag afleveres separat (zip) som "supplerende materiale"; AI-deklaration SKAL afleveres som bilag hvis AI er brugt.

## Filer

| Fil | Kilde-PDF | Type | Sider | Indhold |
|---|---|---|---|---|
| `l27-projektrapport.md` | `L27_Projektrapport.pdf` (SWISE-01, zip 15) | slides | 24 | Rapportens formål/format, kapitelstruktur forside→referencer, indhold pr. kapitel, eksempler på revisionshistorik/ansvarsfordeling/bilagsoversigt/referencer, bilagsmappe, læringsmål SW2PRJ2 |
| `god-rapportskrivning.md` | `God_rapportskrivning.pdf` (SW4PRJ4-02, zip 1) | slides / vejledning | 51 | Formelle krav, bilagsstruktur, rød tråd, målgruppe, figurer/tabeller, forkortelser, referencer/citation styles, plagiering, forslag til struktur, hyppigste fejl pr. kapitel, bachelor-læringsmål, litteratursøgning (PICO, søgetrekant, databaser, in-/eksklusion, dokumentation, flowchart) |
| `rapportskabelon-au-ece.md` | `report_template.pdf` (SW4PRJ4-02, zip 1; gitlab.au.dk/au-ece-prj/student/rapport-eksempel) | skabelon | 44 | Hele skabelonens kapitel-/afsnitsstruktur med vejledende tekst og alle "Drinkbot 4000"-eksempler, V-model/IMRAD-læsevejledning, Brug af AI (AI Fluency), krav til rapport og bilag, sidebudget, tabeller over relevante sektioner/diagrammer for SW/Embedded/Electronics, kodeudsnit- og tabeleksempler |
| `lean-dokumentation.md` | `LeanDokumentation.pdf` (SW4PRJ4-02, zip 1) | artikel | 5 | Tomas Björkholm, "Practices for Lean Documentation" (InfoQ 2015): 3 regler og 6 praksisser for effektiv dokumentation |
| `overleaf-og-latex.md` | HTML-kursusside "Overleaf" (SW4PRJ4-02, zip 1) | kursusside | – | AU Overleaf-instans, link til anbefalet LaTeX-skabelon, alternativ editor (inscrive.io), guides |

## Brightspace-sider

### SWISE-01 — L27-28 Projektrapport og rapport eksempler (zip 15)

Indhold:
1. `L27_Projektrapport.pdf` → konverteret (`l27-projektrapport.md`)
2. "L28 Rapport eksampler og kigge nogle del af opgaver" — HTML-side uden materiale. Hele tekstindholdet: *"Den sidste undervisning. Kigge sammen Projektrapport eksampler. Kigge sammen 'nogen del' af opgaver."* Dvs. L28 var en gennemgang af rapporteksempler og udvalgte opgaver i klassen; der findes ingen slides eller filer for L28.

### SW4PRJ4-02 Projekt 4 — Materiale (zip 1)

Indhold (rækkefølge som på siden):
1. `2016-Scrum-Guide-US.pdf` — hører til modul 04 (udviklingsprocesser), ikke konverteret her
2. Overleaf → `overleaf-og-latex.md`
3. `DomæneModeller.pdf` — hører til modul 09 (domæneanalyse), ikke konverteret her
4. `DomænemodellerEksempel.pdf` — hører til modul 09, ikke konverteret her
5. `LeanDokumentation.pdf` → `lean-dokumentation.md`
6. `report_template.pdf` → `rapportskabelon-au-ece.md`
7. Video: "Gode rapporter" — **ikke konverterbar** (HTML-siden indeholdt kun en video-embed, ingen tekst). Emnet dækkes af slidesættet `god-rapportskrivning.md` (samme forfatter/emne)
8. God rapportskrivning → `god-rapportskrivning.md`
9. Video: "Litteratursøgning 1" — **ikke konverterbar** (kun video-embed). Emnet dækkes delvist af slides 31–37 i `god-rapportskrivning.md`
10. Video: "Litteratursøgning 2" — **ikke konverterbar** (kun video-embed). Emnet dækkes delvist af slides 38–50 i `god-rapportskrivning.md`
11. God rapportskrivning (dublet-link til samme side som nr. 8)

## Noter om konverteringen

- `God_rapportskrivning.pdf` er et slidesæt (ikke en prosa-vejledning). Råteksten fra pdftotext manglede ligaturer (ti, fi, fl, ft, tt) i mange ord, så alle 51 sider er læst visuelt fra PNG-renderingerne. Figurer (eksempelrapporter, diagrammer, tabeller) er beskrevet i prosa; tabellerne med søgestrategi/eksklusioner er gengivet fuldt.
- `report_template.pdf`: al vejledende tekst og alle eksempler er gengivet. Diagrammerne (domænemodel, context-, package-, deployment-, systemsekvens-, klasse-, sekvens-, state- og IBD-diagram) er små i renderingen; de er beskrevet i tabeller/prosa, og state-diagrammet som mermaid med forbehold for detaljer. Figur 10.1 (roteret Rust-klassediagram) er kun eksempel på layout og er beskrevet kort.
- `L27_Projektrapport.pdf` er genbrugt fra SW2PRJ2 (2. semester) og markerer sig selv som "reference, IKKE absolut regel"; struktur afviger lidt fra ECE-skabelonen (bl.a. separat "Teknisk Analyse"- og "Resultater"-kapitel). Skabelonen (`rapportskabelon-au-ece.md`) er den anbefalede struktur for SW4PRJ4.
- Videoerne "Gode rapporter", "Litteratursøgning 1" og "Litteratursøgning 2" kunne ikke konverteres (ingen tekst i HTML-eksporten).
