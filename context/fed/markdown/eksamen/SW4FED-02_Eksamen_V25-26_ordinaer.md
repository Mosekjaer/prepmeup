# Eksamen SW4FED-02 – V25/26 ordinær (januar 2026)

## Metadata

- **Prøve:** SW4FED-02 Front-end Development
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Eksamenstermin:** Eksamen januar 2026 (sidefoden i opgavesættet angiver "vintereksamen 2025")
- **Varighed:** 24 timer
- **Institution:** Aarhus Universitet, Institut for Elektro- og Computerteknologi
- **Ansvarlig underviser:** Poul Ejnar Rovsing
- **Kilde:** L28/SW4FED-02 Front-end udvikling, V25-26-o.pdf (3 sider)
- **Emner dækket:**
  - Opgave 1: MAUI-app i C#/.NET til lagring og søgning af vittigheder
  - Opgave 2: React SPA med samme funktionalitet mod lokal json-server
  - Persistering via SQLite eller json-server tilgået med HttpClient
  - Tilføj, browse og søg på emneord (tags)

---

## Praktiske informationer

Eksamensbesvarelsen skal afleveres i ZIP-format.

Husk at aflevere før deadline som er angivet i Wiseflow, hvor opgaven skal afleveres.

### Hjælpemidler

Alle hjælpemidler må benyttes, herunder internettet som opslagsværktøj, men opgaven skal besvares individuelt. Og husk referencer hvis du genbruger kode fra andre opgaver eller projekter – skal indsættes som en kommentar.

Både brug af ChatGPT og AI integreret i en kodeeditor så som Intellisense og GitHub CoPilot er tilladt.

## Indledning

Ved besvarelsen af opgaverne skal du huske, at du ved fremlæggelsen skal kunne demonstrere opfyldelse af kursets læringsmål ud fra opgaverne. Din opgavebesvarelse skal derfor dække så mange læringsmål som muligt.

---

## Opgave 1

Der ønskes udviklet en applikation som kan bruges til at lagre vittigheder, for efterfølgende at kunne finde vittigheder ved at søge på et emneord.

Sammen med selve vittigheden (som blot er noget tekst) ønskes lagret dato for indtastning, kilde (hvor har man hørt/læst den) og emneord (tags).

Programmet skal kodes i C# til .Net-platformen med anvendelse af MAUI og køre på en platform (device) efter eget valg. Du skal selv fastlægge programmets brugergrænseflade og softwarearkitektur.

Alle data skal persisteres. Du kan enten benytte en SQLite database, eller du kan vælge at benytte en json-server (som i opgave 2), som du kan tilgå med HttpClient.

### Eksempler på vittigheder med tilhørende informationer

```text
Dato: 13.6.2016
Tekst: "Hvorfor gik kyllingen over vejen? For at komme over på den anden side."
Kilde: PHP-bog
Emneord: kylling, gåde

Dato: 13.6.2016
Tekst: "Hvorfor gik kalkunen over vejen? Fordi det var kyllingens fridag."
Kilde: Standup-show
Emneord: kalkun, kylling, gåde

Dato: 14.6.2016
Tekst: "Hvorfor gik fasanen over vejen? For at bevise at den ikke var en kylling."
Kilde: Sofie
Emneord: fasan, kylling, gåde

Dato: 14.6.2016
Tekst: "Hvorfor gik kyllingen over Möbius bånd? For at komme over på den samme side."
Kilde: Math nørd
Emneord: kylling, gåde, matematik
```

### Krav til funktionalitet

**1 Tilføj vittighed**

Brugeren af appen skal kunne vælge tilføj vittighed hvorved følgende informationer kan indtastes og lagres i databasen:

- Tekst
- Kilde
- Emneord

Når brugeren så trykker gem, så tilføjes dags dato automatisk.

**2 Vis vittigheder**

Brugeren af appen skal kunne vælge at browse i vittighederne – enten ved at scrolle igennem dem, eller ved at bladre fra side til side (du bestemmer, hvad du vil implementere).

**3 Søg vittigheder**

Brugeren kan kunne søge efter vittigheder med et indtastet emneord. Hvorefter alle vittigheder indeholdende det søgte emneord vises.

---

## Opgave 2

Du skal udvikle en front-end til en Web applikation, som kan bruges til at lagre vittigheder, for efterfølgende at kunne finde vittigheder ved at søge på et emneord.

Front-enden skal udvikles som en React single page application, SPA. Du skal selv fastlægge brugergrænsefladen, men funktionaliteten skal være som anført nedenfor.

Der er ikke krav om login, og som server bruges en lokal json-server som vist i lektion 19 "React Fetching data" samt i lab 22. Du kan læse mere brug af json-server her: https://github.com/typicode/json-server.

### Krav til funktionalitet

**1 Tilføj vittighed**

Brugeren af appen skal kunne vælge tilføj vittighed hvorved følgende informationer kan indtastes og lagres i databasen:

- Tekst
- Kilde
- Emneord

Når brugeren så trykker gem, så tilføjes dags dato automatisk.

**2 Vis vittigheder**

Brugeren af appen skal kunne vælge at browse i vittighederne – enten ved at scrolle igennem dem, eller ved at bladre fra side til side (du bestemmer, hvad du vil implementere).

**3 Søg vittigheder**

Brugeren kan kunne søge efter vittigheder med et indtastet emneord. Hvorefter alle vittigheder indeholdende det søgte emneord vises.
