# L13 – Konfigurér ASP.NET Core-server til statiske filer (Visual Studio)

## Metadata

- **Lektion:** L13 – Use Asp.Net Core for static files
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED Config Server for static files.pdf (10 slides)
- **Emner dækket:**
  - Oprettelse af et ASP.NET Core Web Empty-projekt i Visual Studio
  - Projektnavn, location og solution name
  - Valg af framework (.NET 6.0) og HTTPS
  - Tilføjelse af `wwwroot`-mappen
  - `app.UseStaticFiles()` og `app.MapFallbackToFile("index.html")` i Program.cs
  - Tilføjelse af `index.html`
  - Kørsel af appen (Ctrl+F5 / F5)
  - Valg af server: Kestrel vs. IIS Express

---

## 1. Create a new project in Visual Studio

**Trin 1: Vælg ASP.NET Core Web Empty.**

Skærmbilledet viser Visual Studios "Create a new project"-dialog. Filtrene øverst er markeret med røde cirkler: sproget er sat til **All languages**, og projekttypen er sat til **Web** i højre dropdown. I resultatlisten er skabelonen **ASP.NET Core Empty** fremhævet med en rød pil — beskrivelsen lyder: "An empty project template for creating an ASP.NET Core application. This template does not have any content in it." Andre skabeloner i listen (ASP.NET Core Web App MVC, Blazor WebAssembly App) skal altså ikke bruges her; pointen er at starte helt tomt.

**Trin 2: Giv projektet et navn og vælg location.**

Skærmbilledet viser "Configure your new project"-dialogen med felterne udfyldt:

- **Project name:** `HtmlAndCssBasic_Demo`
- **Location:** `C:\Courses\SW4FED\Lektioner\13 HTML CSS Basics\`
- **Solution name:** `HtmlAndCssBasic_Demo`
- Checkboxen "Place solution and project in the same directory" er **ikke** markeret.

## 2. Select Web App Type

Accepter defaults i "Additional information"-dialogen:

- **Framework:** .NET 6.0 (Long-term support)
- **Configure for HTTPS:** markeret
- **Enable Docker:** ikke markeret (Docker OS-dropdown'en er derfor grået ud)

## 3. Add wwwroot folder

Højreklik på **projektet** og vælg "Add new folder", og giv den nye mappe navnet **`wwwroot`**.

Skærmbilledet af Solution Explorer viser den ønskede struktur, med en blå pil der peger på den nye mappe:

```
Solution 'HtmlAndCssBasic_Demo' (1 of 1 project)
  HtmlAndCssBasic_Demo
    Connected Services
    Dependencies
    Properties
    wwwroot          <-- den nye mappe
    appsettings.json
    Program.cs
```

`wwwroot` er den mappe ASP.NET Core som standard serverer statiske filer fra.

## 4. Add lines to Program.cs

To linjer skal tilføjes, og én linje skal fjernes:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

//app.MapGet("/", () => "Hello World!");

app.Run();
```

- **Tilføj** `app.UseStaticFiles();` og `app.MapFallbackToFile("index.html");`
- **Fjern** (eller udkommentér) `app.MapGet("/", () => "Hello World!");` — ellers vil rod-URL'en svare med teksten "Hello World!" i stedet for at serve din HTML-fil.

`UseStaticFiles()` tilføjer middleware der serverer filer fra `wwwroot`. `MapFallbackToFile("index.html")` sørger for at requests der ikke matcher noget andet, falder tilbage til `index.html`.

## 5. Add html file

Højreklik på `wwwroot` og vælg "Add new item" — vælg **HTML Page** og giv filen et navn. Navnet på top-level-filen (default) skal være **`index.html`**.

Skærmbilledet viser "Add New Item"-dialogen, hvor listen indeholder Startup Class, App Settings File, Resources File, Text File, **HTML Page** (fremhævet), JavaScript File, Style Sheet, TypeScript File og TypeScript JSX File. I `Name`-feltet nederst står `index.html`. Til højre står typebeskrivelsen: "An HTML page that can include client-side code".

## 6. Edit the file

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Web demo</title>
</head>
<body>
    <h1>My first web app</h1>
</body>
</html>
```

## 7. Test the Web App

- Vælg "Start without debugging", eller tryk **Ctrl+F5** for at køre appen.
- Eller debug med **F5**, hvis du støder på problemer med at køre din app.

Skærmbilledet viser resultatet i en browser: fanen hedder "Web demo" (fra `<title>`), adressen er `https://localhost:44306`, og siden viser overskriften **My first web app** i stor fed skrift. Bemærk `https` — det er `Configure for HTTPS`-valget fra trin 2 der giver certifikatet og den sikre forbindelse på localhost.

## 8. Select server

Du kan vælge hvilken server der skal bruges til test på den lokale maskine:

- **Kestrel** er default.
- Men du kan vælge at bruge **IIS Express**.

Slidet viser to udsnit af Visual Studios værktøjslinje. Til venstre står den grønne play-knap med projektnavnet `HtmlAndCssBasic_Demo` ved siden af — det betyder at appen køres på Kestrel (projektets egen server). Til højre står den samme knap, men med teksten `IIS Express` — dropdown'en ved siden af play-knappen er altså der man skifter server.
