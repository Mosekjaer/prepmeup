# Note – Preferences i .NET MAUI

## Metadata

- **Lektion:** L09 – Lokal lagring af simple indstillinger
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "Preferences in .NET MAUI"
- **Emner dækket:**
  - Hvad Preferences bruges til i MAUI
  - `Preferences.Default.Set` og `Preferences.Default.Get`
  - Default-værdi ved læsning af en ikke-eksisterende nøgle

---

## 1. Hvad Preferences er til

Storing simple information on our devices, such as remembering an email or username, can facilitate and speed up user interactions in an app.

Preferences er altså key/value-lagring af **simple** værdier – ikke et alternativ til en database. Det svarer til det, guiden til lab 09 kalder "Settings".

## 2. Examples

Skrivning af en værdi:

```csharp
Preferences.Default.Set("user_name", "Leo");
```

Læsning af en værdi, med default-værdi hvis nøglen ikke findes:

```csharp
string userName = Preferences.Default.Get("user_name", "Unknown");
```

## 3. Læs mere

- [Exploring Preferences in .NET MAUI (telerik.com)](https://www.telerik.com/blogs/exploring-preferences-net-maui)
