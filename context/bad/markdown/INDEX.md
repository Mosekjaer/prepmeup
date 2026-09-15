# SW4BAD — kursuskontext-indeks

Alt kursusmateriale konverteret til markdown (august 2026). Originaler ligger i
`../sources/` sorteret i samme emnestruktur; dubletter fra Brightspace-ugemapper
i `../sources/_dubletter/`. Slides er omskrevet (struktur, kodeblokke,
diagrammer i prosa/mermaid); lærebogen er mekanisk oprydder pdftotext.

## 00-kursus

- [kursusbeskrivelse.md](00-kursus/kursusbeskrivelse.md) — AU-kursuskatalog: formål, læringsmål, eksamen
- [lesson-plan.md](00-kursus/lesson-plan.md) — 14-ugers skema med emner og undervisere
- [kontakter.md](00-kursus/kontakter.md) — undervisere og TA'er

## Uge 1 — Intro og Docker (`01-intro-og-docker/`)

- welcome-og-intro.md, intro-aspnet-core.md, docker.md, docker-compose.md, websites-og-webapps.md

## Uge 2 — Databaser intro og datamodellering (`02-databaser-intro/`)

- intro-til-databaser.md, database-life-cycle.md, er-modellering.md, conceptual-data-modeling.md, relational-model.md, relational-mapping.md
- database-systems-kap2-relational-model.md — forberedelseslæsning (Garcia-Molina kap. 2)

## Uge 2–3 — SQL, transaktioner og normalisering (`03-sql/`)

- dml.md, ddl.md, sql-connections.md, transactions.md
- normalization.md, normalisering-intro.md, functional-dependency.md, multi-valued-dependency.md

## Uge 4 — Web APIs, REST og Dapper (`04-webapi-rest/`)

- rest-principles.md, web-apis-aspnet-core.md, model-binding.md, dbup-og-dapper.md, connection-string.md, eksempler-rest-og-ef.md

## Uge 5–6 — EF Core og LINQ (`05-ef-core/`)

- ef-intro.md, ef-core-crud.md, ef-core-advanced.md, linq.md, mapster.md, key-concepts.md

## Uge 6 — Data validation (`06-validering/`)

- model-validation-og-error-handling.md

## Uge 7 — Logging og SignalR (`07-logging-signalr/`)

- logging.md, signalr.md

## Uge 8–9 — MongoDB (`08-mongo/`)

- mongo-intro.md, mongo-query-og-transactions.md, mongo-nested-queries-og-logging.md

## Uge 8 — Testing (`09-testing/`)

- testing-overview.md, unit-testing-controllers.md, integration-testing-controllers.md, api-testing-postman.md

## Uge 10 — Autentifikation og autorisation (`10-auth/`)

- authentication-og-authorization.md, json-web-token.md, secure-webapi-bcrypt.md

## Uge 10–11 — Release, deployment og security (`11-release-security/`)

- release-og-deployment.md, deploy-til-azure.md, asp-security.md, https.md
  (asp-security.md er en ældre version af https-decket — overlap noteret i filen)

## Uge 11–12 — Beyond REST (`12-beyond-rest/`)

- graphql-introduction.md, exploring-graphql-apis.md, calling-remote-apis.md

## Uge 12–13 — Dokumentation, background services og caching (`13-docs-caching-background/`)

- api-documentation.md, background-services.md, caching-techniques.md, tips-troubleshooting.md

## Lærebog (`bog/`)

*Building Web APIs with ASP.NET Core* (Valerio De Sanctis, Manning), 12 kapitler:

- [00-indhold.md](bog/00-indhold.md) — indholdsfortegnelse
- 01-web-apis-at-a-glance, 02-foerste-web-api-projekt, 03-restful-principper,
  04-working-with-data, 05-crud-operations, 06-validation-error-handling,
  07-application-logging, 08-caching, 09-auth, 10-beyond-rest,
  11-api-documentation, 12-release-deployment

## Kodeeksempler (`eksempler/`)

- `EfMigrations/` — EF Core migrations-demo (fra EfMigrationsF22.zip)
- `WebApplication1Demo/` — REST + EF demo-projekt (hører til eksempler-rest-og-ef.md)
- `student-example/` — SQL-demo eksempelprojekt fra underviser
