# L09 – Preferences

## Metadata

- **Lektion:** L09 – Preferences (Storing app settings for a MAUI App)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L09/Preferences.pdf (11 slides)
- **Emner dækket:**
  - Hvad preferences er, og hvordan de adskiller sig fra anden lagring
  - `IPreferences`-interfacet og `Preferences.Default`
  - Hvilke datatyper der kan gemmes
  - `Set`, `Get`, `ContainsKey`, `Remove`, `Clear`
  - Shared keys og containers
  - Persistens ved afinstallation

---

## 1. Hvad er preferences?

Preferences er **application wide settings** (properties), hvis værdier kan ændres for at tilpasse sig brugerens præferencer.

De **persisterer mellem application runs** — dvs. værdien overlever, at appen lukkes og startes igen.

De preferences, din app gemmer, er **kun synlige for din app**.

## 2. Preferences API

Brug .NET MAUI's `IPreferences`-interface. Det gemmer app preferences i et **key/value store**.

Default-implementationen af `IPreferences`-interfacet er tilgængelig gennem `Preferences.Default`-property'en.

## 3. Storage types

**Nøglen** er en `String`.

**Værdien** skal være en af følgende datatyper:

- `Boolean`
- `Double`
- `Int32`
- `Single`
- `Int64`
- `String`
- `DateTime`

Bemærk at der ikke er nogen collection- eller objekttype på listen. Vil man gemme et objekt, skal det serialiseres til en `String` (f.eks. som JSON) først.

## 4. Set preferences

Sæt en værdi med `Preferences.Set`-metoden ved at angive key og value:

```csharp
// Set a string value:
Preferences.Default.Set("first_name", "John");

// Set an numerical value:
Preferences.Default.Set("age", 28);

// Set a boolean value:
Preferences.Default.Set("has_pets", true);
```

## 5. Get preferences

For at hente en værdi fra preferences sender du nøglen på preference'en, efterfulgt af **default-værdien**, der bruges, når nøglen ikke findes:

```csharp
string firstName = Preferences.Default.Get("first_name", "Unknown");
int age = Preferences.Default.Get("age", -1);
bool hasPets = Preferences.Default.Get("has_pets", false);
```

Andet argument er altså ikke valgfrit — API'et tvinger dig til at bestemme, hvad der skal ske, hvis nøglen mangler. Typen af default-værdien bestemmer samtidig, hvilken overload der kaldes.

## 6. Check for a key

Det kan af og til være nyttigt at tjekke, om en nøgle findes i preferences eller ej:

```csharp
bool hasKey = Preferences.Default.ContainsKey("my_key");
```

## 7. Fjern én eller alle nøgler

Fjern en specifik nøgle fra preferences:

```csharp
Preferences.Default.Remove("first_name");
```

Fjern alle nøgler:

```csharp
Preferences.Default.Clear();
```

## 8. Shared keys

De preferences, din app gemmer, er kun synlige for din app.

Men du kan også oprette en **shared preference**, der kan bruges af andre extensions eller en watch app.

Når du sætter, fjerner eller henter en preference, kan en valgfri string-parameter angives for at specificere **navnet på den container**, preference'en gemmes i.

## 9. Persistens

Afinstallation af applikationen medfører, at alle preferences fjernes — **undtagen** når appen kører på Android 6.0 (API level 23) eller nyere og bruger **Auto Backup**-featuren.

## 10. Referencer og links

- MAUI Preferences — https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences
