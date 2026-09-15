# Eksamen SW4FED-02 – Vinter 2024/25

## Metadata

- **Prøve:** SW4FED-02 Front-end Development
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Eksamenstermin:** Eksamen januar 2025 (vintereksamen 2024/25)
- **Varighed:** 24 timer
- **Institution:** Aarhus Universitet, Institut for Elektro- og Computerteknologi
- **Ansvarlig underviser:** Poul Ejnar Rovsing
- **Kilde:** L28/SW4FED-02 2024 Vinter.pdf (6 sider)
- **Emner dækket:**
  - Habit tracker som gennemgående tema for begge opgaver
  - Opgave 1: MAUI-app i C#/.NET med lokal persistering (SQLite eller filer)
  - Vejledende datamodel: `Habit` og `HabitEntry`
  - Opgave 2: React-frontend med brugeroprettelse og login (proof-of-concept)
  - Streaks og kalendervisning som feedback
  - Vejledende db.json-struktur til json-server

---

## Praktiske informationer

Eksamensbesvarelsen skal afleveres i ZIP-format.

Husk at aflevere før deadline som er angivet i Wiseflow, hvor opgaven skal afleveres.

### Hjælpemidler

Alle hjælpemidler må benyttes, herunder internettet som opslagsværktøj, men opgaven skal besvares individuelt. Og husk referencer hvis du genbruger kode fra andre opgaver eller projekter – skal indsættes som en kommentar.

Både brug af ChatGPT og AI integreret i en codeeditor så som Intellisense og GitHub CoPilot er tilladt.

## Indledning

Ved besvarelsen af opgaverne skal du huske, at du ved fremlæggelsen skal kunne demonstrere opfyldelse af kursets læringsmål ud fra opgaverne. Din opgavebesvarelse skal derfor dække så mange læringsmål som muligt.

---

## Opgave 1

Indførelse og fastholdelse af positive vaner er nøglen til selvudvikling og opnåelse af langsigtede mål. Men på grund af manglende konsekvent motivation, kæmper mange mennesker med at udvikle gode rutiner i deres hverdag. Traditionelle teknikker, såsom noter og journaler viser sig ofte at være utilstrækkelige, da de ikke giver nogen feedback og ikke tilskynder til regelmæssighed.

Du skal derfor udvikle en app som kan hjælpe mennesker med at indføre og vedligeholde positive vaner i hverdagen.

Programmet skal kodes i C# til .Net-platformen med anvendelse af MAUI og køre på en device efter eget valg. Du skal selv fastlægge programmets brugergrænseflade og softwarearkitektur, samt hvilken funktionalitet der eventuelt implementeres ud over den grundlæggende funktionalitet specificeret her.

Alle data skal persisteres lokalt. Du bestemmer selv hvordan. Du kan f.eks. benytte en SQLite database, men du kan også vælge at benytte filer.

### User stories

**1: Som bruger ønsker jeg at kunne tilføje en ny vane, for at jeg kan følge, hvor godt jeg kan holde den**

Ved oprettelse af en ny vane skal brugeren indtaste et navn for vanen, en beskrivelse samt startdato.

I den grundlæggende version er alle vaner daglige. I en fremtidig version skal brugeren kunne angive et ønsket interval for vanen. F.eks. daglig, hver anden dag, alle hverdage, 1 gang om ugen.

**2: Som bruger ønsker jeg at kunne markere en vane som udført for den aktuelle dato**

Brugeren skal let kunne vælge en vane og markerer denne for udført for den aktuelle dato.

I den grundlæggende version kan brugeren blot vælge om den er udført (ja/nej).

I en fremtidig version skal brugeren have mulighed for at angive hvorfor vaner ikke blev holdt. F.eks. syg, på ferie, havde ikke tid, gad ikke, glemte det, andet.

**3: Som bruger ønsker jeg feedback på, hvor god jeg er til at holde mine vaner**

Brugeren kan få feedback på to måder:

1. Længste rækkefølge (stribe/streak) og nuværende rækkefølge. Med rækkefølge (stribe/streak) menes antal dage i træk, hvor vaner er blevet holdt.
2. Der vises en kalender (f.eks. den seneste måned), hvor det for hver dag, vises med farver eller symboler om vanen blev holdt.

### Forslag til datamodel

Bemærk, at denne datamodel kun er vejledende.

```csharp
public class Habit
{
    public long HabitId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public long LongestStreak { get; set; }
    public long CurrentStreak { get; set; }
}

public class HabitEntry
{
    public long HabitEntryId { get; set; }
    public long HabitId { get; set; }
    public DateTime Date { get; set; }
    public bool Completed { get; set; }
}
```

---

## Opgave 2

Indførelse og fastholdelse af positive vaner er nøglen til selvudvikling og opnåelse af langsigtede mål. Men på grund af manglende konsekvent motivation, kæmper mange mennesker med at udvikle gode rutiner i deres hverdag. Traditionelle teknikker, såsom noter og journaler viser sig ofte at være utilstrækkelige, da de ikke giver nogen feedback og ikke tilskynder til regelmæssighed.

Du skal derfor udvikle en front-end til en Web applikation som brugere kan benytte til at registrere deres overholdelse af gode vaner.

Front-enden skal udvikles som en React app. Du skal selv fastlægge brugergrænsefladen, men funktionaliteten skal være som anført nedenfor. Eksemplet på hvordan data kunne struktureres i "dokumentdatabasen" (json-filen), som er vist på næste side, er kun vejledende, du må gerne lave din løsning anderledes.

Som server bruges en lokal json-server, som vist i lektion 19 "React Fetching data" samt i lab 22 (https://github.com/typicode/json-server).

Bemærk at alt indtastet skal persisteres via et REST api som tilbydes af json-server.

### User stories

**1: Som bruger ønsker jeg at kunne oprette mig som bruger af webappen**

Ved oprettelse af en ny bruger skal der registreres: fornavn, efternavn, e-mail-adresse og password.

**2: Som bruger ønsker jeg at kunne logge ind til webappen**

I denne prof-of-concept udgave lagres password ukrypteret, og frontend står for at validerer password ved login.

**3: Som bruger ønsker jeg at kunne tilføje en ny vane, for at jeg kan følge, hvor godt jeg kan holde den**

Ved oprettelse af en ny vane skal brugeren indtaste et navn for vanen, en beskrivelse samt startdato.

I den grundlæggende version er alle vaner daglige. I en fremtidig version skal brugeren kunne angive et ønsket interval for vanen. F.eks. daglig, hver anden dag, alle hverdage, 1 gang om ugen.

**4: Som bruger ønsker jeg at kunne markere en vane som udført for den aktuelle dato**

Brugeren skal let kunne vælge en vane og markerer denne for udført for den aktuelle dato.

I den grundlæggende version kan brugeren blot vælge om den er udført (ja/nej).

I en fremtidig version skal brugeren have mulighed for at angive hvorfor vaner ikke blev holdt. F.eks. syg, på ferie, havde ikke tid, gad ikke, glemte det, andet.

**5: Som bruger ønsker jeg feedback på, hvor god jeg er til at holde mine vaner**

Brugeren kan få feedback på to måder:

1. Længste rækkefølge (stribe/streak) og nuværende rækkefølge. Med rækkefølge (stribe/streak) menes antal dage i træk, hvor vanen er blevet holdt.
2. Der vises en kalender (f.eks. den seneste måned), hvor det for hver dag, vises med farver eller symboler om vanen blev holdt

### Forslag til strukturering af datafil (db.json)

Forslag til strukturering af datafil, som definerer API'et (db.json). Bemærk, at dette kun er vejledende.

```json
{
    "users": [
      {
        "id": "1",
        "firstName": "John",
        "lastName": "Doe",
        "email": "jd@example.com",
        "password": "123456"
      },
      {
        "id": "2",
        "firstName": "Jane",
        "lastName": "Smith",
        "email": "js@example.com",
        "password": "asdfghj"
      }
    ],
    "habits": [
      {
        "id": "1",
        "name": "Exercise",
        "description": "Run 5 miles",
        "startDate": "2025-01-01",
        "frequency": "Daily",
        "userId": "1"
      },
      {
        "id": "2",
        "name": "Read",
        "description": "Read 30 minutes",
        "startDate": "2025-01-01",
        "frequency": "Daily",
        "userId": "1"
      },
      {
        "id": "3",
        "name": "Meditate",
        "description": "Meditate for 10 minutes",
        "startDate": "2025-01-02",
        "frequency": "Daily",
        "userId": "2"
      },
      {
        "id": "2c6d",
        "name": "Motionere",
        "description": "Løb 5 kilometer",
        "startDate": "2025-01-02",
        "frequency": "daily",
        "userId": "2"
      }
    ],
    "HabitEntries": [
      {
        "id": "1",
        "date": "2025-01-01T13:06:42.010Z",
        "habitId": "1",
        "completed": true
      },
      {
        "id": "2",
        "date": "2025-01-01T13:07:42.010Z",
        "habitId": "2",
        "completed": true
      },
      {
        "id": "3",
        "date": "2025-01-01T13:08:42.010Z",
        "habitId": "3",
        "completed": true
      },
      {
        "id": "4",
        "date": "2025-01-02T13:06:42.010Z",
        "habitId": "1",
        "completed": true
      },
      {
        "id": "7089",
        "date": "2025-01-13T13:06:42.010Z",
        "habitId": "3",
        "completed": true
      },
      {
        "id": "7ebe",
        "date": "2025-01-13T13:06:42.010Z",
        "habitId": "3",
        "completed": true
      },
      {
        "id": "a3aa",
        "date": "2025-01-13T13:06:43.365Z",
        "habitId": "2c6d",
        "completed": true
      }
    ]
}
```
