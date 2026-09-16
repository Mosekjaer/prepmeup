---
name: defend-my-code
description: Forbereder én person på at forklare sin egen kode for gruppen. Finder personens commits i src/, clients/ og tests/ siden sidste mødereferat og udspørger på dansk. Brug når brugeren skriver /defend-my-code, "forsvar min kode", "hør mig i det jeg har lavet", "hvad har jeg committet siden sidst" eller skal kunne forklare sine egne ændringer til mødet.
argument-hint: "[person] [--since YYYY-MM-DD]"
---

# defend-my-code

Udspørg én person om dennes egne commits, så personen kan forklare dem for gruppen. Dansk.

## Argumenter

- `[person]`: navn eller e-mail. Standard: output af `git config user.email`.
- `--since <dato>`: standard er datoen fra det nyeste mødereferat.

## Regler

- Kun `src/`, `clients/` og `tests/`. Kig aldrig i `docs/`.
- Skriv intet til disk. Ret ingen kode.
- Stil ÉT spørgsmål ad gangen. Vent altid på svar.
- Sig det direkte, når et svar er tyndt, og hvad der manglede.
- Ingen kildehenvisninger som kommentarer i kode.

## Link-format (brug det overalt)

`[src/PrepMeUp.Application/HouseholdService.cs:42-51](src/PrepMeUp.Application/HouseholdService.cs#L42-L51)`

Linkteksten er altid selve stien med linjenumre.

## Progress-bar

Vis den FØR hvert spørgsmål:

```
■■□  2 / 3   ·   defend: src/PrepMeUp.Application
```

## Forløb

### 1. Find dato og person

Er `--since` ikke angivet, kør:

```bash
ls docs/appendices/process/13-meeting-minutes/*-meeting-minutes-*.tex | sed -E 's/.*minutes-([0-9-]+)\.tex/\1/' | sort | tail -1
```

Er `[person]` ikke angivet, kør `git config user.email`.

### 2. Hent commits

```bash
git log --use-mailmap --author="<person>" --since="<dato>" --no-merges --stat -p -- src clients tests
```

Er outputtet tomt: sig det i to linjer, foreslå en tidligere `--since` eller et andet navn/e-mail, og nævn at `.mailmap` skal indeholde alle personens adresser, ellers rammer `--author` kun en del af historikken. Stop derefter.

### 3. Oversigt

Vis kort, før første spørgsmål:

- Antal commits og perioden.
- Filerne grupperet efter lag: `Domain`, `Application`, `Infrastructure`, `Api`, `clients`, `tests`. Lag uden ændringer udelades.
- Antal ændrede linjer i alt.

### 4. Antal spørgsmål

1 spørgsmål pr. ca. 40 ændrede linjer (tilføjede + slettede). Mindst 3, højst 10. Sig tallet i oversigten.

### 5. Spørgsmål

Format:

```
■□□  1 / 4   ·   defend: src/PrepMeUp.Api

**Spørgsmål 1**
<spørgsmålet>

Dine ændringer:
[src/PrepMeUp.Api/HouseholdController.cs:31-48](src/PrepMeUp.Api/HouseholdController.cs#L31-L48)
```

Spørgsmålene går efter hvorfor, ikke hvad:

- Hvorfor ligger det i dette lag, og hvad brød du, hvis du flyttede det?
- Hvad sker der, hvis input er ugyldigt? Hvem validerer, og hvad ser klienten?
- Hvilken test dækker den gren? Findes der ingen, hvorfor ikke?
- Hvad ville en reviewer brokke sig over i den her diff?
- Kan du forklare denne linje uden at kigge på skærmen?

Aldrig: "hvad hedder variablen", "hvor mange linjer ændrede du".

### 6. Vent og giv feedback

Vent på svaret. Brugeren kan skrive `spring over`, `hint` eller `stop` — samme betydning som i eksamination: `spring over` giver det korte rigtige svar og går videre, `hint` giver ét hint uden svaret, `stop` afslutter med det samme.

Feedback er højst fem linjer: hvad der var rigtigt, hvad der manglede, og ét pensum-link. Slå emnet op i `context/INDEX.md` og læs kun den matchende fil. Findes `context/INDEX.md` ikke, brug `context/bad/markdown/INDEX.md`, `context/swt/markdown/INDEX.md`, `context/swd/markdown/README.md`, `context/fed/markdown/README.md`, `context/projekt/markdown/README.md`. Ingen kilde i pensum: `⚠ Ikke i kursusmaterialet: <begrundelse>`.

### 7. Afslutning

Afslut med de to-tre steder personen bør læse igennem igen inden mødet, med links:

```
Læs igennem inden mødet:
1. [src/...](src/...#L..-L..) — <hvorfor>
2. [tests/...](tests/...#L..-L..) — <hvorfor>
```
