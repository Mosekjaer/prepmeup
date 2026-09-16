---
name: vertical-slice
description: Byg en ny funktion gennem alle lag i PrepMeUp i fast rækkefølge - Domain, Application, Infrastructure, Api, api-client og til sidst en Expo-skærm. Brug når brugeren siger "ny funktion", "vertical slice", "hele vejen igennem", "fra database til skærm", "nyt endpoint med skærm" eller nævner en feature der kræver både backend og klient.
argument-hint: "[kort beskrivelse af funktionen]"
---

# vertical-slice

Byg funktionen gennem alle lag, **ét lag ad gangen med stop imellem**. Spring aldrig et lag over og byg aldrig to lag i samme tur.

## Før du skriver noget

1. Slå emnet op i `context/INDEX.md` og læs kun de filer, der matcher. Læs aldrig `context/` igennem i blinde.
2. Læs lagreglerne i `docs/standalone/architecture-pitch.tex` (slide 5-6) og tjek dit design mod dem:
   - `Domain` refererer ingenting - intet andet projekt overhovedet.
   - `Application` kender ikke HTTP, `ControllerBase`, EF-typer eller SQL.
   - `Api` kender ikke `DbContext`; kun `Program.cs` (composition root) kender `Infrastructure`.
   - `Infrastructure` træffer ingen beslutninger - den henter og gemmer.
   - `clients/apps` duplikerer ikke forretningsregler og går ikke uden om `api-client`.
3. Skitsér i to-tre linjer hvad hvert lag skal indeholde, og få det bekræftet før lag 1.

## Rækkefølge

### 1. Domain
Entitet eller value object plus invarianter. Ingen referencer, ingen attributter fra EF, ingen `virtual` for lazy loading. Invarianter håndhæves i konstruktør og metoder - ikke i controlleren.

### 2. Application
Use case-klasse plus det repository-interface den har brug for. Interfacet defineres her, ikke i `Infrastructure` (DIP: højniveaumodulet ejer abstraktionen). Returnér domænetyper eller egne resultattyper - ikke DTO'er fra Api.

### 3. Infrastructure
`IEntityTypeConfiguration<T>` for entiteten, repository-implementering og migration:
`dotnet ef migrations add <Name> -p src/PrepMeUp.Infrastructure -s src/PrepMeUp.Api`

### 4. Api
Controller, contract/DTO, model binding, validering og statuskoder. `201 Created` med `CreatedAtAction` ved oprettelse, `400` med `ProblemDetails` ved ugyldigt input, `404` når ressourcen ikke findes. Mapping mellem contract og domæne sker her. Al tekst brugeren ser - også fejlbeskeder - er på engelsk.

### 5. clients/packages/api-client
Regenerér klienten. Kør skillen `api-contract`. Rediger **aldrig** genereret kode i hånden.

### 6. Expo-skærm
Skærm under `clients/apps/mobile/` der kalder `api-client`. Dæk loading, tom, fejl og data. Ingen forretningsregler i UI.

## Efter hvert lag (obligatorisk)

```bash
dotnet build
```

Vis resultatet. Stop derefter med en kort opsummering:

> **Lag N færdig.** Lavet: `<filer>`. Næste lag: `<navn>` - `<hvad>`. Sig til når jeg skal fortsætte.

Fortsæt ikke uden svar. Lag 5 og 6 verificeres i stedet med `npx tsc --noEmit` i den ramte pakke.

Til sidst: `dotnet build` og `dotnet test` skal begge være grønne.

## Kilder

Afslut hvert svar med kode med en **Kilder**-sektion med markdown-links i formatet `[sti:linjer](sti#Lx-Ly)`. Ingen kildehenvisninger som kommentarer i koden. Findes mønsteret ikke i kursusmaterialet, skriv `⚠ Ikke i kursusmaterialet: <begrundelse>` - det gælder bl.a. Expo, TanStack Query og klientgenerering til TypeScript.

Relevante kilder til denne skill:

- [context/bad/markdown/04-webapi-rest/rest-principles.md](context/bad/markdown/04-webapi-rest/rest-principles.md) - ressourcenavne, verber, statuskoder
- [context/bad/markdown/04-webapi-rest/web-apis-aspnet-core.md](context/bad/markdown/04-webapi-rest/web-apis-aspnet-core.md) - controllers, routing, DI
- [context/bad/markdown/04-webapi-rest/model-binding.md](context/bad/markdown/04-webapi-rest/model-binding.md) - model binding og binding sources
- [context/bad/markdown/05-ef-core/key-concepts.md](context/bad/markdown/05-ef-core/key-concepts.md) - DbContext, entiteter, konfiguration
- [context/bad/markdown/05-ef-core/ef-core-crud.md](context/bad/markdown/05-ef-core/ef-core-crud.md) - CRUD og migrations
- [context/bad/markdown/05-ef-core/ef-core-advanced.md](context/bad/markdown/05-ef-core/ef-core-advanced.md) - relationer, Include, tracking
- [context/bad/markdown/06-validering/model-validation-og-error-handling.md](context/bad/markdown/06-validering/model-validation-og-error-handling.md) - validering, ProblemDetails
- [context/swd/markdown/slides/SW4SWD-01_W03b_SOLID_ISP_DIP.md](context/swd/markdown/slides/SW4SWD-01_W03b_SOLID_ISP_DIP.md) - ISP og DIP
- [docs/standalone/architecture-pitch.tex:176-221](docs/standalone/architecture-pitch.tex#L176-L221) - lagregler og modulafhængigheder
