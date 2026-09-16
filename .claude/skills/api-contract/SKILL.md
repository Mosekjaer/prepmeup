---
name: api-contract
description: Holder OpenAPI-dokumentet fra API'et og clients/packages/api-client i sync. Brug når brugeren siger "generér klienten", "api-client er forældet", "opdatér contracten", "swagger", "openapi", "nswag", eller når et endpoint eller en DTO i Api-laget er ændret og klienten skal følge med.
---

# api-contract

`clients/packages/api-client` er **genereret**. Kontrakten er OpenAPI-dokumentet fra `PrepMeUp.Api`. Ændringer går altid den vej: ret contracten i Api-laget, generér igen.

## 1. Hent swagger.json

Brug et kørende API, eller start det:

```bash
dotnet run --project src/PrepMeUp.Api &
curl -s http://localhost:5001/swagger/v1/swagger.json -o swagger.json
```

Tjek at porten matcher `launchSettings.json` eller `docker-compose.yml`, før du antager 5001.

## 2. Generér TypeScript-klienten med NSwag

NSwag installeres som lokalt dotnet-tool, så alle i gruppen får samme version. Mangler `.config/dotnet-tools.json`:

```bash
dotnet new tool-manifest
dotnet tool install NSwag.ConsoleCore
```

Generér:

```bash
dotnet tool run nswag openapi2tsclient \
  /input:swagger.json \
  /output:clients/packages/api-client/src/client.ts \
  /template:Fetch
```

## 3. Typetjek klienten

```bash
cd clients/packages/api-client && npx tsc --noEmit
```

Fejler den, er det næsten altid contracten, der er ændret - se trin 4.

## 4. Rediger aldrig genereret kode

Ingen håndrettelser i `clients/packages/api-client/src/client.ts`. Skal noget se anderledes ud i klienten, ret det i `Api`-laget:

- forkert eller manglende type: ret contract/DTO'en eller tilføj `[ProducesResponseType(typeof(T), StatusCodes.Status200OK)]`
- grimt metodenavn: ret controller- eller action-navnet, eller `[HttpGet(Name = "...")]`
- manglende felt: ret DTO'en

Generér derefter igen fra trin 1. Brug for en håndskreven wrapper, lægges den i en **anden** fil i pakken, aldrig i `client.ts`.

## 5. Bagefter

```bash
dotnet build && dotnet test
```

og typetjek de apps, der bruger pakken. Nævn i svaret hvilke endpoints der er kommet til, ændret eller forsvundet - det er et breaking change for klienterne.

## Kilder

Afslut hvert svar med kode med en **Kilder**-sektion med markdown-links i formatet `[sti:linjer](sti#Lx-Ly)`. Ingen kildehenvisninger som kommentarer i koden. Slå emner op via `context/INDEX.md` først.

- [context/bad/markdown/13-docs-caching-background/api-documentation.md](context/bad/markdown/13-docs-caching-background/api-documentation.md) - Swagger/OpenAPI og Swashbuckle i ASP.NET Core
- [context/bad/markdown/bog/11-api-documentation.md](context/bad/markdown/bog/11-api-documentation.md) - samme emne i bogen
- [context/fed/markdown/book/MAUI_in_Action_Ch08_Enterprise_app_development.md:668-671](context/fed/markdown/book/MAUI_in_Action_Ch08_Enterprise_app_development.md#L668-L671) - NSwag som klientgenerator

⚠ Ikke i kursusmaterialet: NSwag til **TypeScript**-klienter. NSwag nævnes kun som generator af .NET-klientbiblioteker i FED-bogen. `openapi2tsclient` og Fetch-templaten er gruppens eget valg og skal kunne begrundes i rapportens diskussionsafsnit.
