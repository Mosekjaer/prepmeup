# Lab 24 – Teachers context

## Metadata

- **Lektion:** L24 – React Context
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "Lab 24 Teachers context"
- **Emner dækket:**
  - React Context til delt state
  - Context provider i `app.jsx` / `main.jsx`
  - Indlæsning af data fra backend og opdatering af context
  - Navigation med id som parameter
  - Læsning af enkelt element fra context

---

## Opgaven

Tag udgangspunkt i lab 22 (eller 23), hvor du laver følgende ændringer:

1. Lav en context, som indeholder alle data om studerende i et array.
2. Lav en provider af denne context i `app.jsx` (eller `main.jsx`), således at contexten bliver tilgængelig i hele appen.
3. `List grades` indlæser data fra backend og opdaterer (sætter) context.
4. Ved klik på en studerende i `List grades` navigeres til `Edit grade`, og den pågældende studerendes id sendes med. `Edit grade` vil så læse den studerendes data fra contexten.
