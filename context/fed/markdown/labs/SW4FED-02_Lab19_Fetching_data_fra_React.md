# Lab 19 – Fetching data fra React

## Metadata

- **Lektion:** L19 – Lab 19
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L19/FED Lab19 Fetching data from React.pdf (2 sider)
- **Emner dækket:**
  - API-kald fra React
  - Fetch af data i `useEffect`
  - OpenWeatherMap API: query på bynavn, zip + landekode
  - Geolocation API (valgfri delopgave)
  - Faldgruben med uendelige løkker i `useEffect`

---

## 1. Formål

At opnå erfaring med API-kald fra React.

## 2. Forudsætninger

At du har opnået et grundlæggende kendskab til React.

## 3. Fetch data with useEffect hook

Lav en app, der fetcher det aktuelle vejrdata fra https://home.openweathermap.org/

Du kan bruge api-nøglen: `b8642f6a6885b73b64fe78b8c6e4631e`

Dette er strukturen af en url:

```text
https://api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
```

API'et er dokumenteret her: https://openweathermap.org/current

## 4. Delopgave 1

Lav en komponent, hvor brugeren kan få vejrdata for en by, som brugeren indtaster i et input-felt (navnet på byen).

## 5. Delopgave 2

Lav endnu en komponent, hvor brugeren kan indtaste en zip code og country code og derefter få vejrinformation for det område med et klik på en knap.

## 6. Delopgave 3

Hvis tiden tillader det, kan du modtage brugerens lokation fra browseren (https://developer.mozilla.org/en-US/docs/Web/API/Geolocation_API) og derefter automatisk fetche den aktuelle temperatur og vise den.

## 7. HINT — pas på uendelige løkker

Hvis du eller en anden kommer til at lave en uendelig løkke (sker desværre let ved `useEffect`), så får underviseren en advarselsmail fra OpenWeatherMap. Derfor kan du opleve, at den angivne API key ikke virker.

Den klassiske faldgrube er en `useEffect`, der kalder en `setState`, uden at have et korrekt dependency-array — hver state-opdatering trigger en ny render, som trigger effecten igen, som kalder API'et igen. Sørg for at angive et dependency-array (fx `[]` for kun at fetche ved mount, eller `[city]` for kun at fetche når byen ændrer sig).

<!-- Sliden med selve advarselsmailen fra OpenWeatherMap er et screenshot og indgår ikke i råteksten. -->
