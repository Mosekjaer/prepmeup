# Kursusindeks — PrepMeUp

Emne-til-fil-opslag for alt kursusmateriale i `context/` (fem kurser, ca. 545 markdown-filer).
**Reglen: slå op her først, og læs kun de 1–3 filer der matcher emnet.** Læs aldrig mappen igennem.
Alle stier er relative fra repo-roden. Findes emnet ikke her, tjek listen nederst — så skal svaret
markeres `⚠ Ikke i kursusmaterialet: <begrundelse>`.

## Kurser

| Kode | Kursus | Mappe | Hvad det dækker |
|---|---|---|---|
| SW4BAD | Backend og databaser | `context/bad/markdown/` | ASP.NET Core Web API, REST, EF Core, SQL, validering, logging, auth, Docker, deployment, caching, OpenAPI |
| SW4SWD | Softwaredesign | `context/swd/markdown/` | OO, UML, SOLID, design smells, arkitekturproces, 4+1 og C4, GoF patterns, refactoring, DDD, concurrency, error handling |
| SW4SWT | Softwaretest | `context/swt/markdown/` | Unit test (NUnit), exceptions, CI, git workflow, design for testability, fakes/NSubstitute, coverage, BVA, integrationstest, system- og accepttest, metrikker |
| SW4FED | Frontend | `context/fed/markdown/` | MAUI/XAML, C#, data binding, async/await, layouts, JavaScript, React, hooks, routing, useEffect, fetch, forms, responsivt design, context, Vitest |
| SW4PRJ4 | Projekt (SWISE) | `context/projekt/markdown/` | Kravspecifikation, use cases, system-/accepttest, udviklingsprocesser, SysML, arkitektur og design, projektledelse, kvalitetssikring, domæneanalyse, applikationsmodel, implementation, protokoller, rapportskrivning |

## Emneopslag — backend (SW4BAD)

| Emne | Filer |
|---|---|
| ASP.NET Core Web API, projektopsætning | `context/bad/markdown/04-webapi-rest/web-apis-aspnet-core.md`, `context/bad/markdown/01-intro-og-docker/intro-aspnet-core.md`, `context/bad/markdown/bog/02-foerste-web-api-projekt.md` |
| REST-principper, ressourcer, HATEOAS | `context/bad/markdown/04-webapi-rest/rest-principles.md`, `context/bad/markdown/bog/03-restful-principper.md` |
| Statuskoder, ProblemDetails, fejlhåndtering i API | `context/bad/markdown/06-validering/model-validation-og-error-handling.md`, `context/bad/markdown/bog/06-validation-error-handling.md` |
| Model binding, routing, parametre | `context/bad/markdown/04-webapi-rest/model-binding.md` |
| Validering, DataAnnotations, custom validators | `context/bad/markdown/06-validering/model-validation-og-error-handling.md`, `context/bad/markdown/bog/06-validation-error-handling.md` |
| EF Core intro, DbContext, migrations | `context/bad/markdown/05-ef-core/ef-intro.md`, `context/bad/markdown/05-ef-core/key-concepts.md` |
| EF Core CRUD | `context/bad/markdown/05-ef-core/ef-core-crud.md`, `context/bad/markdown/bog/05-crud-operations.md` |
| EF Core advanced, relationer, tracking, performance | `context/bad/markdown/05-ef-core/ef-core-advanced.md`, `context/bad/markdown/bog/04-working-with-data.md` |
| LINQ | `context/bad/markdown/05-ef-core/linq.md` |
| DTO-mapping (Mapster) | `context/bad/markdown/05-ef-core/mapster.md` |
| Dapper, DbUp, connection strings | `context/bad/markdown/04-webapi-rest/dbup-og-dapper.md`, `context/bad/markdown/04-webapi-rest/connection-string.md` |
| SQL DDL (CREATE/ALTER, constraints) | `context/bad/markdown/03-sql/ddl.md` |
| SQL DML (SELECT/INSERT/UPDATE/DELETE, joins) | `context/bad/markdown/03-sql/dml.md` |
| Transaktioner, ACID, isolationsniveauer | `context/bad/markdown/03-sql/transactions.md` |
| Normalisering, 1NF–BCNF | `context/bad/markdown/03-sql/normalization.md`, `context/bad/markdown/03-sql/normalisering-intro.md`, `context/bad/markdown/03-sql/functional-dependency.md` |
| ER-modellering, konceptuel datamodel | `context/bad/markdown/02-databaser-intro/er-modellering.md`, `context/bad/markdown/02-databaser-intro/conceptual-data-modeling.md` |
| Relationel model, ER → tabeller | `context/bad/markdown/02-databaser-intro/relational-model.md`, `context/bad/markdown/02-databaser-intro/relational-mapping.md` |
| Authentication og authorization | `context/bad/markdown/10-auth/authentication-og-authorization.md`, `context/bad/markdown/bog/09-auth.md` |
| JWT | `context/bad/markdown/10-auth/json-web-token.md` |
| BCrypt, password hashing, sikret Web API | `context/bad/markdown/10-auth/secure-webapi-bcrypt.md` |
| HTTPS, ASP.NET-sikkerhed, OWASP | `context/bad/markdown/11-release-security/https.md`, `context/bad/markdown/11-release-security/asp-security.md` |
| Logging, Serilog, log levels | `context/bad/markdown/07-logging-signalr/logging.md`, `context/bad/markdown/bog/07-application-logging.md` |
| Docker, images, containere | `context/bad/markdown/01-intro-og-docker/docker.md` |
| Docker Compose | `context/bad/markdown/01-intro-og-docker/docker-compose.md` |
| Release og deployment | `context/bad/markdown/11-release-security/release-og-deployment.md`, `context/bad/markdown/bog/12-release-deployment.md`, `context/bad/markdown/11-release-security/deploy-til-azure.md` |
| Caching (in-memory, distributed, Redis, response) | `context/bad/markdown/13-docs-caching-background/caching-techniques.md`, `context/bad/markdown/bog/08-caching.md` |
| Background services, hosted services | `context/bad/markdown/13-docs-caching-background/background-services.md` |
| OpenAPI, Swagger, Swashbuckle, API-dokumentation | `context/bad/markdown/13-docs-caching-background/api-documentation.md`, `context/bad/markdown/bog/11-api-documentation.md` |
| Kald af eksterne API'er (HttpClient fra backend) | `context/bad/markdown/12-beyond-rest/calling-remote-apis.md` |
| GraphQL | `context/bad/markdown/12-beyond-rest/graphql-introduction.md`, `context/bad/markdown/12-beyond-rest/exploring-graphql-apis.md`, `context/bad/markdown/bog/10-beyond-rest.md` |
| MongoDB | `context/bad/markdown/08-mongo/mongo-intro.md`, `context/bad/markdown/08-mongo/mongo-query-og-transactions.md` |
| SignalR | `context/bad/markdown/07-logging-signalr/signalr.md` |
| Test af controllers (unit + integration i ASP.NET) | `context/bad/markdown/09-testing/unit-testing-controllers.md`, `context/bad/markdown/09-testing/integration-testing-controllers.md` |
| API-test med Postman | `context/bad/markdown/09-testing/api-testing-postman.md` |
| Fuldt REST + EF eksempelprojekt | `context/bad/markdown/04-webapi-rest/eksempler-rest-og-ef.md` |

## Emneopslag — softwaredesign (SW4SWD)

| Emne | Filer |
|---|---|
| OO-grundlag (indkapsling, arv, polymorfi) | `context/swd/markdown/slides/SW4SWD-01_W01.1b_OO_Basics.md` |
| UML klassediagram | `context/swd/markdown/slides/SW4SWD-01_W01.1c_UML_Class_Diagrams.md` |
| Design smells (rigidity, fragility, viscosity) | `context/swd/markdown/slides/SW4SWD-01_W02a_Design_Smells.md` |
| SOLID: SRP og OCP | `context/swd/markdown/slides/SW4SWD-01_W02b_SOLID_SRP_OCP.md` |
| SOLID: LSP | `context/swd/markdown/slides/SW4SWD-01_W03a_SOLID_LSP.md` |
| SOLID: ISP og DIP (dependency inversion, lagregler) | `context/swd/markdown/slides/SW4SWD-01_W03b_SOLID_ISP_DIP.md` |
| Arkitektur som proces, quality attributes | `context/swd/markdown/slides/SW4SWD-01_W04.1_Architecture_Process_1.md`, `context/swd/markdown/slides/SW4SWD-01_W04.2_Architecture_Process_2.md` |
| Arkitekturdokumentation, 4+1 og C4 | `context/swd/markdown/slides/SW4SWD-01_W05_Architecture_Documentation.md` |
| C4-modellen i dybden | `context/swd/markdown/artikler/SW4SWD-01_C4_Model.md`, `context/swd/markdown/artikler/SW4SWD-01_C4_Talk_Simon_Brown.md` |
| 4+1 view model | `context/swd/markdown/artikler/SW4SWD-01_4plus1_View_Kruchten_1995.md`, `context/swd/markdown/artikler/SW4SWD-01_4plus1_View_UML2.md` |
| Designmønstre, generelt | `context/swd/markdown/slides/SW4SWD-01_W06a_Design_Patterns_Intro.md` |
| GoF Observer | `context/swd/markdown/slides/SW4SWD-01_W06b_GoF_Observer.md`, `context/swd/markdown/bog/SW4SWD-01_HFDP_Ch02_Observer.md` |
| GoF Strategy og Template Method | `context/swd/markdown/slides/SW4SWD-01_W07.1_GoF_Template_Method_Strategy.md`, `context/swd/markdown/bog/SW4SWD-01_HFDP_Ch01_Strategy.md`, `context/swd/markdown/bog/SW4SWD-01_HFDP_Ch08_Template_Method.md` |
| GoF Factory og Abstract Factory | `context/swd/markdown/slides/SW4SWD-01_W07.2_GoF_Factory_Abstract_Factory.md`, `context/swd/markdown/bog/SW4SWD-01_HFDP_Ch04_Factory.md` |
| GoF State, state machines | `context/swd/markdown/slides/SW4SWD-01_W08a_GoF_State.md`, `context/swd/markdown/slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md`, `context/swd/markdown/bog/SW4SWD-01_HFDP_Ch10_State.md` |
| Refactoring | `context/swd/markdown/slides/SW4SWD-01_W09.1_Refactoring.md` |
| Domain Driven Design | `context/swd/markdown/slides/SW4SWD-01_W09.2_Domain_Driven_Design.md` |
| Tråde og tasks i C# | `context/swd/markdown/slides/SW4SWD-01_W10_Threading_in_CSharp.md`, `context/swd/markdown/slides/SW4SWD-01_W11.1_Concurrency_Parallel_Tasks.md` |
| Concurrency: parallelle loops, pipelines, futures | `context/swd/markdown/slides/SW4SWD-01_W11.2_Concurrency_Parallel_Loops.md`, `context/swd/markdown/slides/SW4SWD-01_W12.1_Concurrency_Dependencies_Futures.md`, `context/swd/markdown/slides/SW4SWD-01_W12.2_Concurrency_Pipelines.md` |
| Error handling (concurrency, exceptions i design) | `context/swd/markdown/slides/SW4SWD-01_W13.2_Error_Handling.md` |
| Extreme Programming | `context/swd/markdown/slides/SW4SWD-01_W01.2_Extreme_Programming.md` |
| C# interfaces (grundlag) | `context/swd/markdown/noter/SW4SWD-01_CSharp_Interfaces_Basics.md` |

## Emneopslag — test (SW4SWT)

| Emne | Filer |
|---|---|
| Unit test, introduktion | `context/swt/markdown/01.1-introduktion-unit-test/02-Introduction-to-Unit-Tests.pdf.md` |
| NUnit assertions, attributter | `context/swt/markdown/01.1-introduktion-unit-test/03-NUnit-Assertions.md` |
| Exceptions i C#, test af exceptions | `context/swt/markdown/01.2-exceptions-git-workflow/02-Exeption-Management-CSharp.md`, `context/swt/markdown/01.2-exceptions-git-workflow/03-Exception-Testing.md` |
| Continuous integration | `context/swt/markdown/02.1-continuous-integration/01-Continuous-Integration.md` |
| Git workflow, branches, merge requests | `context/swt/markdown/02.2-git-workflow/02-Git-Workflows.md`, `context/swt/markdown/01.2-exceptions-git-workflow/05-GitWorkflow.pdf.md` |
| Design for testability, dependency injection til test | `context/swt/markdown/03.1-2-design-for-testability/02-Design-for-Testability.md`, `context/swt/markdown/03.1-2-design-for-testability/03-Interfaces.md` |
| Black box vs. white box | `context/swt/markdown/03.1-2-design-for-testability/01-Black-And-White-Box.md` |
| Test types og fake types (stub, mock, dummy, spy) | `context/swt/markdown/04.1-test-types-and-fake-types/50-Test-Types-and-Fake-Types.md` |
| Isolation frameworks, NSubstitute | `context/swt/markdown/04.2-fakes-og-isolation-frameworks/50-Isolation-frameworks.md`, `context/swt/markdown/04.2-fakes-og-isolation-frameworks/code/EcsNSubstituteFakes.md` |
| Test af events | `context/swt/markdown/shared/UsingAndTestingEvents.md`, `context/swt/markdown/04.2-fakes-og-isolation-frameworks/06-Isolation-frameworks-i-en-event-drevet-applikation.md` |
| Code coverage (statement, branch, condition) | `context/swt/markdown/05.1-2-test-quality-1-og-2/02-Coverage.md`, `context/swt/markdown/05.1-2-test-quality-1-og-2/50-Code-Coverage-Analysis.md` |
| Boundary value analysis og ækvivalensklasser | `context/swt/markdown/05.1-2-test-quality-1-og-2/04-Boundary-Value-Analysis.md`, `context/swt/markdown/05.1-2-test-quality-1-og-2/05-Video-BVA-og-EPs.md` |
| Testkvalitet, zombie testing | `context/swt/markdown/05.1-2-test-quality-1-og-2/07-ZombieTesting.md` |
| Kvalitetsmetrikker, statisk analyse, cyclomatic complexity | `context/swt/markdown/08.1-2-software-quality-metrics/01-Static-Analysis.pdf-8.1.md`, `context/swt/markdown/08.1-2-software-quality-metrics/03-Dynamic-Analysis.pdf-8.2.md` |
| Integrationstest, dependency tree, integrationsplan | `context/swt/markdown/09.1-2-integrationstest/02-Integrationtest-introduction.pdf-9.1.md`, `context/swt/markdown/09.1-2-integrationstest/50-DependencyTreeAndIntegrationPlan.md` |
| Agil og systemintegration | `context/swt/markdown/09.1-2-integrationstest/03-Lektion-09.2-Agil-og-systemintegration.md` |
| Systemtest og accepttest, automatisering | `context/swt/markdown/13.2-automatisering-af-system-og-accepttest/01-Automated-System-Test-New.pdf.md` |
| Gherkin, Reqnroll/SpecFlow | `context/swt/markdown/13.2-automatisering-af-system-og-accepttest/03-Gherkin-dokumentation.md`, `context/swt/markdown/13.2-automatisering-af-system-og-accepttest/04-ReqnrollSpecflow-dokumentation.md` |
| Eksempel på testet state machine | `context/swt/markdown/04.2-fakes-og-isolation-frameworks/09-Eksempel-paa-implementering-og-anvendelse-af-en-State-Machine.md` |

## Emneopslag — frontend (SW4FED)

| Emne | Filer |
|---|---|
| React, overblik og komponentmodel | `context/fed/markdown/slides/SW4FED-02_L16.1_React_Overview.md`, `context/fed/markdown/slides/SW4FED-02_L17.1_Functional_components.md` |
| JSX | `context/fed/markdown/book/React_Quickly_1ed_Ch03_Introduction_to_JSX.md` |
| useState | `context/fed/markdown/slides/SW4FED-02_L17.2_React_useState_Hook.md` |
| useEffect, side effects | `context/fed/markdown/slides/SW4FED-02_L18.1_React_side_effects.md`, `context/fed/markdown/labs/SW4FED-02_Lab18_useEffect.md` |
| useRef | `context/fed/markdown/slides/SW4FED-02_L20.2_React_useRef_Hook.md` |
| Routing (client-side, React Router) | `context/fed/markdown/slides/SW4FED-02_L17.3_React_Router.md`, `context/fed/markdown/book/React_Quickly_1ed_Ch13_React_routing.md` |
| fetch, promises, datahentning i React | `context/fed/markdown/slides/SW4FED-02_L19.2_Fetch.md`, `context/fed/markdown/slides/SW4FED-02_L19.3_React_fetching_data.md`, `context/fed/markdown/slides/SW4FED-02_L19.1_Promises.md` |
| Events i React | `context/fed/markdown/slides/SW4FED-02_L21.1_Handling_Events_i_React.md`, `context/fed/markdown/book/React_Quickly_1ed_Ch06_Handling_events_in_React.md` |
| Forms i React (controlled components) | `context/fed/markdown/slides/SW4FED-02_L22.1_React_Forms.md`, `context/fed/markdown/labs/SW4FED-02_Lab22_Form_og_Post_data.md` |
| Application state, context | `context/fed/markdown/slides/SW4FED-02_L24.1_React_managing_application_state.md`, `context/fed/markdown/labs/SW4FED-02_Lab24_Teachers_context.md` |
| Redux / Redux Toolkit | `context/fed/markdown/slides/SW4FED-02_L26.1_React_Redux.md` |
| Styling af React-komponenter | `context/fed/markdown/slides/SW4FED-02_L23.1_React_Styling.md` |
| Vitest, test af komponenter | `context/fed/markdown/slides/SW4FED-02_L23.2_Testing_med_Vitest.md`, `context/fed/markdown/labs/SW4FED-02_Lab27_Vitest.md` |
| TypeScript | `context/fed/markdown/slides/SW4FED-02_L25.1_TypeScript.md` |
| JavaScript, funktioner, objekter og arrays | `context/fed/markdown/slides/SW4FED-02_L15.1_JavaScript.md`, `context/fed/markdown/slides/SW4FED-02_L15.2_Functions_i_JS.md`, `context/fed/markdown/slides/SW4FED-02_L15.3_Objects_og_Arrays_i_JS.md` |
| DOM, ES modules, npm | `context/fed/markdown/slides/SW4FED-02_L15.4_The_DOM.md`, `context/fed/markdown/slides/SW4FED-02_L20.1_Modules.md`, `context/fed/markdown/slides/SW4FED-02_L15.5_npm.md` |
| JSON, WebStorage og cookies | `context/fed/markdown/slides/SW4FED-02_L18.2_JSON.md`, `context/fed/markdown/slides/SW4FED-02_L18.3_WebStorage_og_Cookies.md` |
| Responsivt design, media queries | `context/fed/markdown/slides/SW4FED-02_L26.2_Responsive_Web_Design.md`, `context/fed/markdown/labs/SW4FED-02_Lab23_Responsivt_design.md` |
| Flexbox, CSS Grid, page layout | `context/fed/markdown/slides/SW4FED-02_L13.8_Flexbox.md`, `context/fed/markdown/slides/SW4FED-02_L13.9_CSS_Grid.md`, `context/fed/markdown/slides/SW4FED-02_L13.7_Page_Layout.md` |
| HTML5 og CSS3 basics | `context/fed/markdown/slides/SW4FED-02_L13.3_HTML5_basics.md`, `context/fed/markdown/slides/SW4FED-02_L13.5_CSS3_basics.md` |
| async/await i C# | `context/fed/markdown/slides/SW4FED-02_L05.2_async_await.md` |
| HttpClient (klientside .NET) | `context/fed/markdown/slides/SW4FED-02_L05.3_HttpClient.md` |
| MAUI, XAML, data binding | `context/fed/markdown/slides/SW4FED-02_L01.3_XAML.md`, `context/fed/markdown/slides/SW4FED-02_L02.2_Data_Binding_Basics.md`, `context/fed/markdown/slides/SW4FED-02_L01.6_MAUI_introduction.md` |
| MAUI layouts og controls | `context/fed/markdown/slides/SW4FED-02_L04.2_Layouts.md`, `context/fed/markdown/slides/SW4FED-02_L03.1_Controls.md` |
| MAUI pages og navigation, Shell/routes | `context/fed/markdown/slides/SW4FED-02_L06.1_Pages_and_Navigation.md`, `context/fed/markdown/slides/SW4FED-02_L06.2_Shell_and_Routes.md` |
| MVVM | `context/fed/markdown/slides/SW4FED-02_L08.1_The_MVVM_Pattern.md`, `context/fed/markdown/slides/SW4FED-02_L09.1_MVVM_Community_Toolkit.md` |
| Dependency injection, generic host builder | `context/fed/markdown/slides/SW4FED-02_L07.3_Dependency_Injection_og_Generic_Host_builder.md` |
| Klient-auth, enterprise app-mønstre, NSwag-klienter | `context/fed/markdown/slides/SW4FED-02_L07.4_Authentication_i_MAUI.md`, `context/fed/markdown/book/MAUI_in_Action_Ch08_Enterprise_app_development.md` |
| C# introduktion, .NET-typesystem og arkitektur | `context/fed/markdown/slides/SW4FED-02_L01.4_Csharp_Introduction.md`, `context/fed/markdown/slides/SW4FED-02_L03.2_NET_typesystem.md`, `context/fed/markdown/slides/SW4FED-02_L04.1_NET_Architecture.md` |

## Emneopslag — projekt (SW4PRJ4/SWISE)

| Emne | Filer |
|---|---|
| Kravspecifikation, FURPS+, MoSCoW, traceability | `context/projekt/markdown/01-kravspecifikation/system-specification.md`, `context/projekt/markdown/bog/01-larman-ch5-requirements.md` |
| Use cases, fully dressed, aktører | `context/projekt/markdown/02-use-cases/use-cases-funktionelle-krav.md`, `context/projekt/markdown/02-use-cases/fully-dressed-use-case-skabelon.md`, `context/projekt/markdown/bog/02-larman-ch6-use-cases.md` |
| User stories | `context/projekt/markdown/00-kursus/sw4prj4-introduktion.md`, `context/swd/markdown/slides/SW4SWD-01_W01.2_Extreme_Programming.md` |
| Systemtest og accepttest, testspecifikation | `context/projekt/markdown/03-systemtest/system-test.md`, `context/projekt/markdown/03-systemtest/eksempel-accepttestspecifikation-ttt.md` |
| Udviklingsprocesser, Kanban, Scrum | `context/projekt/markdown/04-udviklingsprocesser/development-processes.md`, `context/projekt/markdown/04-udviklingsprocesser/kanban.md`, `context/projekt/markdown/04-udviklingsprocesser/scrum-guide-2016.md` |
| SysML overblik og quick guide | `context/projekt/markdown/05-sysml/sysml-introduction.md`, `context/projekt/markdown/05-sysml/sysml-quick-guide.md` |
| SysML BDD og IBD | `context/projekt/markdown/05-sysml/sysml-structural-diagrams-1-bdd.md`, `context/projekt/markdown/05-sysml/sysml-structural-diagrams-2-ibd.md` |
| Sekvensdiagram (SysML SD) | `context/projekt/markdown/05-sysml/sysml-sequence-diagrams.md` |
| State machine (SysML STM) | `context/projekt/markdown/05-sysml/sysml-state-machine-diagrams.md` |
| Arkitektur og systemdesign, coupling/cohesion, interfaces | `context/projekt/markdown/06-arkitektur-og-design/system-architecture-and-design.md`, `context/projekt/markdown/06-arkitektur-og-design/system-design-and-interfaces.md` |
| Projektledelse, WBS, PERT, risikoanalyse | `context/projekt/markdown/07-projektledelse/project-management.md`, `context/projekt/markdown/bog/14-vinje-projektledelse.md` |
| Kvalitetssikring, review, konfigurationsstyring | `context/projekt/markdown/08-kvalitetssikring/quality-management.md`, `context/projekt/markdown/bog/06-spu-vejledning-review.md` |
| Domæneanalyse, domænemodel | `context/projekt/markdown/09-domaeneanalyse/system-domain-analysis.md`, `context/projekt/markdown/09-domaeneanalyse/eksempel-domaenemodel.md`, `context/projekt/markdown/bog/07-larman-ch9-domain-models.md` |
| Applikationsmodel (boundary/control/entity) | `context/projekt/markdown/10-applikationsmodel/system-application-models-1.md`, `context/projekt/markdown/10-applikationsmodel/system-application-models-2.md`, `context/projekt/markdown/10-applikationsmodel/system-application-models-3.md` |
| Fra design til implementation | `context/projekt/markdown/11-implementation/applikationsmodel-minutur.md`, `context/projekt/markdown/11-implementation/implementation-minutur-gui-wpf.md` |
| Protokoller: OSI, HTTP, UART/I2C/SPI | `context/projekt/markdown/12-protokoller/swise-protocols.md`, `context/projekt/markdown/bog/11-peckol-ch16-7-network-architecture.md` |
| Rapportskrivning, struktur, sprog | `context/projekt/markdown/13-rapport/god-rapportskrivning.md`, `context/projekt/markdown/13-rapport/l27-projektrapport.md` |
| AU ECE-rapportskabelon, kapitelstruktur, AI-deklaration | `context/projekt/markdown/13-rapport/rapportskabelon-au-ece.md` |
| LaTeX og Overleaf | `context/projekt/markdown/13-rapport/overleaf-og-latex.md` |
| Lean dokumentation | `context/projekt/markdown/13-rapport/lean-dokumentation.md` |

## Ikke dækket af kursusmaterialet

Findes ikke i `context/`. Brug `⚠ Ikke i kursusmaterialet: <begrundelse>` når svaret bygger på disse.

| Teknologi | Status (verificeret med grep i `context/`) |
|---|---|
| Coolify | Nul forekomster. Deployment er kun dækket generisk (Docker, Azure) i `context/bad/markdown/11-release-security/release-og-deployment.md`. |
| TanStack Query / React Query | "TanStack" har nul forekomster. "React Query" optræder ét sted som et rent eksternt link i `context/fed/markdown/slides/SW4FED-02_L19.3_React_fetching_data.md` — ikke som pensum. Datahentning undervises med rå `fetch` + `useEffect`. |
| Expo og Expo Router | Nul forekomster af `Expo` som ord. React Native nævnes kun som sammenligning i `context/fed/markdown/slides/SW4FED-02_L16.1_React_Overview.md` og `context/fed/markdown/book/MAUI_in_Action_Ch01_Introducing_NET_MAUI.md`. Kurset dækker React til web, ikke Expo. React-hooks, routing-begreber og fetch er dækket; Expo-specifikke API'er er ikke. |
| Keycloak | Nævnes ét sted, som ét punkt på en liste over identity providers: `context/bad/markdown/10-auth/authentication-og-authorization.md` (linje 808). Ingen opsætning, ingen integration, ingen OIDC-flow-gennemgang. |
| NSwag til TypeScript | NSwag optræder to steder, begge som .NET-værktøj: `context/fed/markdown/book/MAUI_in_Action_Ch08_Enterprise_app_development.md` (genererer C#-klienter) og `context/bad/markdown/bog/03-restful-principper.md` (nævnt som alternativ til Swashbuckle). TypeScript-klientgenerering er ikke dækket. |
| openapi-typescript | Nul forekomster. |
| N+1-problemet | Ikke behandlet i SW4BAD. "N+1" optræder kun i en anden betydning i `context/swd/markdown/slides/SW4SWD-01_W05_Architecture_Documentation.md`. EF Core-performance og relationer er dækket i `context/bad/markdown/05-ef-core/ef-core-advanced.md`, men uden N+1-begrebet. |
| GitHub Actions | Ikke dækket. CI undervises med GitLab CI: `context/swt/markdown/02.1-continuous-integration/01-Continuous-Integration.md`. Principperne overføres, værktøjet gør ikke. |
