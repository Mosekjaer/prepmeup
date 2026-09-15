---
title: "Lidt om nøgler og funktionelle afhængigheder"
source: "Au-ECEI4DAB-FunctionalDependency.pptx"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# Nøgler og funktionelle afhængigheder

*Jesper Rosholm Tørresø, AU-ECE-I4DAB.*

## Nøgler — hvorfor?

Designkrav om konsistens/korrekthed:

- Entydig identifikation af data er afgørende for databasens konsistens.
- Vi skal være sikre på, at data til enhver tid er sat sammen på en korrekt måde — **referentiel integritet** (referential integrity).

Hertil bruger vi nøgler i en relationel database. Nøglernes egenskaber kan undersøges via funktionel afhængighed.

## Konceptuelle og konkrete nøgler

**Konceptuelle nøgler:**

- Supernøgle (super key)
- Kandidatnøgle (candidate key)

**Konkrete nøgler:**

- Primær/unik nøgle (primary/unique key)
  - Sekundær nøgle (secondary/alternate key)
- Fremmednøgle (foreign key)

**Andre nøglebegreber:**

- Surrogatnøgle (surrogate key)
- Sammensat nøgle (compound key)

## Super- og kandidatnøgler

- **Supernøglen** er grundtypen for nøgler, med egenskaben at nøglen kan udpege en række entydigt. Den kan bestå af alle kombinationer af attributter i en tabel, for så vidt de kun udpeger én række.
- **Kandidatnøglen** er det minimale sæt af attributter fra en supernøgle: fjernes en attribut, er den ikke længere en supernøgle.
- Nøgleegenskaben er stærkt afhængig af **problemdomænets regler** (undtaget surrogatnøglen).

## Primær nøgle / unik nøgle

En attribut (eller flere attributter/felter), som altid entydigt udpeger en bestemt række/tuple i en tabel/relation. En bestemt værdi af attributten/attributterne må derfor kun forekomme én gang.

- Attributten kaldes derfor en **determinant**.
- Der kan kun være **én primær nøgle** i en tabel.
  - Den kan refereres til fra en fremmednøgle.
- Der kan være **flere unikke nøgler** (alternate keys).
  - De kan *ikke* refereres til fra fremmednøgler, men giver attributten samme egenskaber/begrænsninger som primærnøglen.

## Andre nøgler

**Surrogatnøgle (surrogate key):**

- En kunstig primær nøgle, som ikke har sammenhæng med problemdomænet. Værdierne genereres af "systemet".
- Kan sikre en database mod kaskadeopdatering af fremmednøgler og udfavoriserer nøgler fra problemdomænet.

**Sammensat nøgle (compound key):**

- En primær nøgle sammensat af flere felter/attributter.
- Kan være nødvendig for at sikre kandidat-egenskaben for nøglen, eller for kun at tillade bestemte kombinationer af sammensatte værdier.

## Funktionel afhængighed

**Triviel funktionel afhængighed:**

- En begrænsning/restriktion mellem to sæt af attributter i en databasetabel:
  - `FD attr1 → attr2`
  - `FD attrA, attrB → attrC, attrD, attrE`
- Venstresidens værdi bestemmer højresidens værdi.
- Venstresiden kan evt. bruges som nøgle.
- FD kræver et nøje kendskab til **problemdomænets regler**.

**Flerværdiet afhængighed (multi-valued dependency):**

- Den samme værdi af en attribut i en tabel, set over flere tuples/rækker, bestemmer en anden attributs værdier (et sæt af værdier).
- Skrives som `MVD attrib32 ->> attrib34` eller `MVD attrib32 ->-> attrib34`.
