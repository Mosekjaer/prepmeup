# Eksamen 2024 Sommer – Bilag 1 (JSON-testdata)

## Metadata

- **Lektion:** L28 – Eksamensforberedelse
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** `L28/SW4FED-02 2024 Sommer bilag1.txt`
- **Emner dækket:**
  - Testdata til eksamensopgaven sommer 2024
  - JSON-struktur for værkstedsaftaler (appointments)
  - Felter til kunde, adresse, bil og opgavebeskrivelse

---

## Kontekst

Dette er **bilag 1 til eksamenssættet SW4FED-02, sommer 2024** (se `L28/SW4FED-02 2024 Sommer.pdf` for selve opgaveteksten). Det er de testdata, opgaven bygger på: en JSON-fil med et `appointments`-array, hvor hvert element er en booket værkstedsaftale.

Hvert appointment har kundens navn og adresse (`address` indeholder et linjeskift `\n` mellem gade og postnummer/by), bilens mærke, model og nummerplade, datoen for aftalen samt en beskrivelse af opgaven. `id` er nøglen, du bruger, når du skal hente eller opdatere en enkelt aftale.

Strukturen passer til en JSON Server-backend, hvor `appointments` bliver til endpointet `/appointments`, og et enkelt element hentes med `/appointments/1`.

Bemærk at data er på dansk og indeholder æ/ø/å (`Årlig service`, `Andeby`), så encoding skal håndteres korrekt.

## SW4FED-02 2024 Sommer bilag1.txt

```json
{
  "appointments": [
    {
      "customerName": "Anders And",
      "address": "Villavej 25\n8888 Andeby",
      "carBrand": "Ford",
      "carModel": "Kuga",
      "licensePlate": "AL12345",
      "date": "2024-07-01",
      "taskDescription": "Årlig service + olieskift",
      "id": 1
    },
    {
      "customerName": "Andersine",
      "address": "Bredgade 22\n8888 Andeby",
      "carBrand": "Tesla",
      "carModel": "Model Y",
      "licensePlate": "BB45678",
      "date": "2024-07-02",
      "taskDescription": "Årlig service",
      "id": 2
    }
  ]
}
```
