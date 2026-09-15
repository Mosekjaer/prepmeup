---
title: "Introduktion til Normalisering af Relationelle Databaser"
source: "AU-ECE-I4DAB-NormaliseringIntro.pptx"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# Introduktion til normalisering af relationelle databaser

*Jesper Rosholm Tørresø, AU-ECE-I4DAB.*

## Emner

- Normalisering er logisk design med udgangspunkt i data
- ... og en forudsætning for relationel algebra (1NF)
- Funktionel afhængighed (FD, Functional Dependency)
- Nøgler
- Normaliseringsregler: 1NF, 2NF, 3NF, BCNF og 4NF
- Opsamling

## Hvorfor normalisering?

Vi vil designe vores database, så vi:

- **undgår redundans** — at de samme oplysninger forekommer flere gange i databasen
- **sikrer konsistens** — at der ikke findes modstridende oplysninger i databasen

Har vi fulgt principperne for konceptuel ER-modellering, har vi allerede (næsten 100 %) sikret os mod disse problemer.

## Normalisering — grundbegreber

- **NF** = Normal Form.
- Værktøjet er **FD** (Functional Dependency) — ikke designregler.
- **0NF** er bare nogle data, ofte en tabel uden nøgler og uden "omtanke"/design.
- 1NF er forudsætning for 2NF, og så fremdeles op til X NF.
- Kan man eftervise at en database er på 4NF, er 1NF–3NF + BCNF pr. definition overholdt. Det er som hovedregel tilstrækkeligt.
- Jo højere normalform, jo mere robust er vores tabeller/database — dvs. mindre sårbar over for uoverensstemmelser og uregelmæssigheder. ("The higher the normal form applicable to a table, the less vulnerable it is to inconsistencies and anomalies.")

Normalisering kan opfattes som et **check** på om designet er godt nok, eller om der skal gøres mere. Det er en proces, hvor vi undersøger om tabellerne opfylder bestemte normalformer (1NF, 2NF, 3NF, Boyce-Codd NF, 4NF). Overholdes normalformen ikke, ændres tabellens design vha. nye tabeller og relationships.

## Funktionel afhængighed (FD)

En funktionel afhængighed er en stærk forbindelse mellem to eller flere attributter i en tabel:

> Én attribut er funktionelt afhængig af en anden attribut, når to rækker i tabellen, der har den samme værdi af den anden attribut, skal have den samme værdi for den første.

Klassisk eksempel: postnr → by ("8000 giver Aarhus").

### Eksempel på FD (notation med tekst-schema og pile)

movieId bestemmer title, genre, length, rating:

- Hver række med movieId 123 har de samme værdier for de øvrige attributter.
- FD-notation: `movieId → {title, genre, length, rating}`

## Gennemgående eksempel (tavlen)

Rå data — medarbejdere, afdelinger og projekter:

| Mnr | Navn    | afdnr | afdnavn      | projektnr | Projektnavn     | TimerPrUge |
|-----|---------|-------|--------------|-----------|-----------------|------------|
| 100 | Hansen  | 1     | Hovedkvarter | 1         | Rationalisering | 20         |
|     |         |       |              | 2         | Produkt X       | 10         |
|     |         |       |              | 3         | Produkt Y       | 5          |
| 101 | Jensen  | 2     | Forskning    | 2         | Produkt X       | 35         |
| 102 | Nielsen | 3     | Salg         | 2         | Produkt X       | 15         |
|     |         |       |              | 3         | Produkt Y       | 20         |

Tavlen direkte i én tabel (iteration 1):

| Mnr | Navn    | afdnr | afdnavn      | projektnr | Projektnavn     | TimerPrUge |
|-----|---------|-------|--------------|-----------|-----------------|------------|
| 100 | Hansen  | 1     | Hovedkvarter | 1         | Rationalisering | 20         |
| 100 | Hansen  | 1     | Hovedkvarter | 2         | Produkt X       | 10         |
| 100 | Hansen  | 1     | Hovedkvarter | 3         | Produkt Y       | 5          |
| 101 | Jensen  | 2     | Forskning    | 2         | Produkt X       | 35         |
| 102 | Nielsen | 3     | Salg         | 2         | Produkt X       | 15         |
| 102 | Nielsen | 3     | Salg         | 3         | Produkt Y       | 20         |

Nogle af informationerne bliver gentaget — der er **redundans**. Det giver anledning til fejlkilder:

1. **Ved indsættelse:** Hvis Jensen skal arbejde på projekt nr. 3, skal vi gentage oplysningerne om navn og afdeling. Bliver de ikke gentaget korrekt, er databasen inkonsistent.
2. **Ved ændring af data:** Skal "Hovedkvarter" rettes til "Førerbunker", skal det ske 3 steder. Sker det ikke korrekt, er der inkonsistens (så er Hansen ansat i 2 afdelinger).
3. **Ved sletning:** Sletter vi Jensen, sletter vi også oplysningerne om forskningsafdelingen.

## 1NF

- Der skal være en **primærnøgle**.
- Der må ikke være gentagende grupper (en tabel i tabellen).

Hvis en tabel har en repeterende gruppe af information inden for samme primærnøgle, skal disse felter flyttes ud i en tabel for sig selv sammen med en kopi af primærnøglen.

Resultat af opsplitningen:

**Medarbejder(1):**

| Mnr | Navn    | Afdnr | Afdnavn      |
|-----|---------|-------|--------------|
| 100 | Hansen  | 1     | Hovedkvarter |
| 101 | Jensen  | 2     | Forskning    |
| 102 | Nielsen | 3     | Salg         |

**Projekt(1)** — regel: en medarbejder deltager kun én gang i et projekt:

| Mnr | Pnr | Pnavn           | TimerPrUge |
|-----|-----|-----------------|------------|
| 100 | 1   | Rationalisering | 20         |
| 100 | 2   | Produkt X       | 10         |
| 100 | 3   | Produkt Y       | 5          |
| 101 | 2   | Produkt X       | 35         |
| 102 | 2   | Produkt X       | 15         |
| 102 | 3   | Produkt Y       | 20         |

## 2NF

- Tabellen er i 1NF.
- Alle felter skal være **funktionelt afhængige af hele primærnøglen**.

Hvis en tabel har en sammensat primærnøgle, og der findes felter som kun afhænger af en del af denne, skal disse felter flyttes over i en ny tabel sammen med en kopi af den del af primærnøglen, som de er afhængige af.

**Funktionel afhængighed** (definition): Et samspil mellem minimum 2 felter i en tabel. Felt2 er funktionelt afhængigt af Felt1, hvis et givet indhold i Felt1 står ud for samme indhold i Felt2 i én eller flere poster i en tabel.

Eksempler: postnr → by (postnr determinerer by); cprnr → navn (cprnr determinerer navn).

I Projekt(1) er Pnavn kun afhængigt af Pnr (en del af nøglen {Mnr, Pnr}) — så tabellen splittes:

**Projekt(1A):**

| Pnr | Pnavn           |
|-----|-----------------|
| 1   | Rationalisering |
| 2   | Produkt X       |
| 3   | Produkt Y       |

**Projekt(1B):**

| Mnr | Pnr | TimerPrUge |
|-----|-----|------------|
| 100 | 1   | 20         |
| 100 | 2   | 10         |
| 100 | 3   | 5          |
| 101 | 2   | 35         |
| 102 | 2   | 15         |
| 102 | 3   | 20         |

## 3NF

- Tabellen er i 2NF.
- Intet felt må være **transitivt afhængigt** af primærnøglen.

Hvis en tabel har felter, der er indbyrdes afhængige og ikke er en del af primærnøglen, skal én eller flere af disse felter flyttes over i en ny tabel sammen med en kopi af det tilbageblevne felt (som derved bliver fremmednøgle).

**Transitiv afhængighed** (definition): Et samspil mellem minimum 3 felter i en tabel. Hvis Felt1 → Felt2 og Felt2 → Felt3, så er Felt3 transitivt (indirekte) afhængigt af Felt1.

Eksempel: CPRNR, NAVN, ADRESSE, POSTNR, BY — hvor CPRNR → POSTNR og POSTNR → BY.

I Medarbejder(1) er Afdnavn afhængigt af Afdnr, som ikke er en del af primærnøglen — så tabellen splittes:

**Medarbejder(2):**

| Mnr | Navn    | Afdnr |
|-----|---------|-------|
| 100 | Hansen  | 1     |
| 101 | Jensen  | 2     |
| 102 | Nielsen | 3     |

**Afdeling:**

| Afdnr | Afdnavn      |
|-------|--------------|
| 1     | Hovedkvarter |
| 2     | Forskning    |
| 3     | Salg         |

## Boyce-Codd Normalform (BCNF)

- **Alle determinanter i en tabel skal være en kandidatnøgle.**
- Normalt giver vi en entitet en surrogatnøgle som PK/ID, og dermed er BCNF typisk overholdt.

Eksempel: R(a, b, c, d) med:

- FD1: a, c → b, d
- FD2: a, d → b

FD1's determinant {a, c} er kandidatnøgle; FD2's determinant {a, d} er det ikke — så R er ikke i BCNF.

## 4. Normalform (4NF)

- **Ingen flerværdiede afhængigheder** (multi-valued dependencies).
- Klassisk eksempel fra Wikipedia: [Fourth normal form](http://en.wikipedia.org/wiki/Fourth_normal_form) — pizzalevering: Hvorledes leveres en given pizza-variant? Per distrikt eller til alle distrikter?
- Med 4NF betyder **domæneviden alt**: Nogle gange gælder 4NF, andre gange er 4NF uden betydning. Domænet bestemmer!
