# L13 – Serve statiske filer med ASP.NET Core (VS Code-version)

## Metadata

- **Lektion:** L13 – Use Asp.Net Core for static files, VS Code version
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/Serve static files - VS code.pdf (6 slides)
- **Emner dækket:**
  - Oprettelse af et .NET-projekt fra VS Code's welcome screen
  - Valg af projekttype (ASP.NET Core Web App MVC)
  - De to linjer i `Program.cs` der er nok til at serve statiske filer
  - Tilpasning af mappestrukturen (`wwwroot/html`, slet Controllers/Models/Views)
  - `index.html` som default-fil

---

## 1. Create a new project in VS Code

Skærmbilledet viser VS Code's Welcome-side med ingen mappe åbnet. I Explorer-panelet til venstre står teksten "You have not yet opened a folder" med knappen **Open Folder**, en note om at klone et repository med knappen **Clone Repository**, og nederst — det relevante her — teksten "You can open a folder containing a .NET project or solution, or create a new .NET project." med den blå knap **Create .NET Project**. Det er den knap der bruges til at starte projektet. Midt på siden er de sædvanlige Start-links (New File, Open File, Open Folder, Clone Git Repository, Connect to) og en Recent-liste med tidligere kursusprojekter.

Create .NET Project-knappen kræver C# Dev Kit-extensionen; alternativt kan man køre `dotnet new` i en terminal.

## 2. Select project type

- Vælg **ASP.NET Core Web App (Model-View-Controller)** — MVC, Web.
- Vælg mappe til projektet.
- Default-optionerne er OK.

Bemærk forskellen fra Visual Studio-versionen af samme øvelse: der bruges "ASP.NET Core Empty", her bruges MVC-skabelonen, og de dele man ikke skal bruge slettes bagefter (se afsnit 4).

## 3. Changes in Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.MapFallbackToFile("html/index.html");

app.Run();
```

Slidet markerer med en pil at de to linjer `app.UseStaticFiles();` og `app.MapFallbackToFile("html/index.html");` er dem der skal **tilføjes**, og med en stor pil nedefra: "This is all that is needed to serve static files."

Bemærk stien `"html/index.html"` — den peger på undermappen `html` inde i `wwwroot`, i modsætning til Visual Studio-versionen hvor filen ligger direkte i `wwwroot`.

## 4. Adjust folder structure

- Tilføj `html` til `wwwroot`.
- **Slet** (hvis du ikke skal bruge dem til andet):
  - `Controllers`
  - `Models`
  - `Views`

Skærmbilledet af VS Code's Explorer viser den ønskede struktur efter oprydningen:

```
WEBAPPDEMO
  WebAppDemo
    bin
    obj
    Properties
    wwwroot
      css
      html
        index.html
      js
      lib
    favicon.ico
    appsettings.Development.json
    appsettings.json
    Program.cs
    WebAppDemo.csproj
  WebAppDemo.sln
```

Controllers-, Models- og Views-mapperne er væk; tilbage under `wwwroot` er `css`, den nye `html`-mappe med `index.html`, samt `js` og `lib` som MVC-skabelonen selv lagde der.

## 5. Add content

- Tilføj HTML-filer til mappen `wwwroot/html`.
- Filen `index.html` bliver default-filen (home page) — det er den `MapFallbackToFile` peger på.
