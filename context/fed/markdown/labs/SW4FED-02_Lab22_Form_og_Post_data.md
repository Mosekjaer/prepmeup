# Lab 22 – Forms in React: indtast data og post til server

## Metadata

- **Lektion:** L22 – Lab 22
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L22/Lab 22 - Form and Post data.pdf (1 side)
- **Emner dækket:**
  - `json-server` som development server
  - Form-komponent til indtastning af data
  - POST af formdata til serveren
  - Reset af felter efter submit
  - Pagineret liste over data
  - Søgning og opdatering af eksisterende data

---

## 1. Opsætning

Installér `json-server` som globalt værktøj og brug den som development server til dette projekt.

Du kan læse om det her: https://github.com/typicode/json-server

Lav en react-app, som en lærer kan bruge til at indtaste karakterer for studerende.

## 2. Lab 1 — indtastningsformular

App'en skal have en komponent, hvor læreren kan indtaste student no., name og grade for en studerende.

Når læreren trykker på enter-tasten, sendes de indtastede data til serveren, og alle felter nulstilles og er klar til den næste indtastning.

## 3. Lab 2 — pagineret liste

Lav endnu en komponent, der viser en liste med alle data — pagineret.

## 4. Lab 3 — søg og ret

Lav endnu en komponent, hvor brugeren kan søge efter en studerende og derefter ændre den studerendes karakter.

## 5. Sammenhæng med forelæsningen

Opgaven kombinerer stoffet fra L22 (controlled forms, `onSubmit`, `preventDefault`, håndtering af flere inputs via `name`-attributten) med L19 (fetch: GET til at hente listen, POST til at oprette, og PUT/PATCH til at ændre en eksisterende karakter).

<!-- Lab-teksten i kilden indeholder ingen kodeeksempler. -->
