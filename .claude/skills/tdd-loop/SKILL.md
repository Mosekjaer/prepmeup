---
name: tdd-loop
description: Red-green-refactor med NUnit og NSubstitute i PrepMeUp. Starter altid med en godkendt tabel over testtilfælde. Brug når brugeren siger "tdd", "skriv en test", "test først", "testtilfælde", "red green refactor", "unit test" eller vil have dækket en regel i Domain eller Application med tests.
argument-hint: "[klasse eller regel der skal testes]"
---

# tdd-loop

## Trin 1 - testtilfælde (STOP)

Slå emnet op i `context/INDEX.md`, læs den relevante kode, og lav en tabel:

| # | ækvivalensklasse eller grænseværdi | input | forventet | begrundelse |
|---|---|---|---|---|

Dæk gyldige og ugyldige ækvivalensklasser og grænseværdierne omkring hver grænse (lige under, på, lige over). Afslut med præcis denne linje:

> Godkend, ret eller fjern tilfælde før jeg skriver første test.

**Skriv ingen testkode før brugeren har svaret.** Testdesign er et bedømt læringsmål i SW4SWT og må ikke uddelegeres. Gætter du på godkendelsen, snyder du gruppen for læringsmålet.

## Trin 2 - loop, én test ad gangen

For hvert godkendt tilfælde:

1. **Red** - skriv én test. Kør og vis, at den fejler af den rigtige grund:
   ```bash
   dotnet test --filter FullyQualifiedName~<TestName>
   ```
   Fejler den ikke, er testen forkert - ret den, før du går videre.
2. **Green** - skriv den mindst mulige implementering. Kør `dotnet test` igen og vis, at den er grøn.
3. **Refactor** - ryd op i produktions- og testkode med grønne tests hele vejen. Kør `dotnet test` en sidste gang.

Gå først til næste tilfælde, når det nuværende er grønt.

## NUnit

`[TestFixture]`, `[SetUp]`, `[Test]`, `[TestCase(...)]` til parametriserede grænseværdier. Assert med constraint-modellen: `Assert.That(actual, Is.EqualTo(expected))`, `Assert.That(() => sut.Do(), Throws.TypeOf<DomainException>())`. Navngiv `Method_Scenario_ExpectedResult`. Arrange-Act-Assert i den rækkefølge, én logisk assertion pr. test.

## NSubstitute og isolation

Isolér altid via interfaces fra `Application`-laget - det er derfor de ligger der (DIP). `Domain` har ingen afhængigheder og har normalt ingen brug for fakes.

```csharp
var repo = Substitute.For<IHouseholdRepository>();
repo.GetAsync(id).Returns(household);          // stub: styrer input
await sut.HandleAsync(command);
await repo.Received(1).SaveAsync(household);   // mock: verificerer interaktion
```

Brug `Returns` til indirekte input og `Received`/`DidNotReceive` til indirekte output. Verificér ikke kald, der ikke er en del af det testede krav - det gør testen skør.

## Tests der kræver database eller HTTP

Hører i `tests/PrepMeUp.Tests.Integration` med `WebApplicationFactory`, ikke i unit-testene. `tests/PrepMeUp.Tests.Unit` refererer kun `Application` og `Domain`.

## Kilder

Afslut hvert svar med kode med en **Kilder**-sektion med markdown-links i formatet `[sti:linjer](sti#Lx-Ly)`. Ingen kildehenvisninger som kommentarer i koden. Findes mønsteret ikke i kursusmaterialet, skriv `⚠ Ikke i kursusmaterialet: <begrundelse>`.

Relevante kilder til denne skill:

- [context/swt/markdown/01.1-introduktion-unit-test/02-Introduction-to-Unit-Tests.pdf.md](context/swt/markdown/01.1-introduktion-unit-test/02-Introduction-to-Unit-Tests.pdf.md) - hvad en unit test er
- [context/swt/markdown/01.1-introduktion-unit-test/03-NUnit-Assertions.md](context/swt/markdown/01.1-introduktion-unit-test/03-NUnit-Assertions.md) - NUnit-assertions
- [context/swt/markdown/03.1-2-design-for-testability/02-Design-for-Testability.md](context/swt/markdown/03.1-2-design-for-testability/02-Design-for-Testability.md) - design for testability
- [context/swt/markdown/03.1-2-design-for-testability/03-Interfaces.md](context/swt/markdown/03.1-2-design-for-testability/03-Interfaces.md) - isolation via interfaces
- [context/swt/markdown/04.1-test-types-and-fake-types/50-Test-Types-and-Fake-Types.md](context/swt/markdown/04.1-test-types-and-fake-types/50-Test-Types-and-Fake-Types.md) - stubs, mocks og fakes
- [context/swt/markdown/04.2-fakes-og-isolation-frameworks/02-Slides-til-lektionen.md](context/swt/markdown/04.2-fakes-og-isolation-frameworks/02-Slides-til-lektionen.md) - isolation frameworks
- [context/swt/markdown/04.2-fakes-og-isolation-frameworks/04-Endelig-loesning-med-NSubstitute-og-fuld-implementering.md](context/swt/markdown/04.2-fakes-og-isolation-frameworks/04-Endelig-loesning-med-NSubstitute-og-fuld-implementering.md) - NSubstitute i praksis
- [context/swt/markdown/05.1-2-test-quality-1-og-2/04-Boundary-Value-Analysis.md](context/swt/markdown/05.1-2-test-quality-1-og-2/04-Boundary-Value-Analysis.md) - ækvivalensklasser og grænseværdier
- [context/swt/markdown/05.1-2-test-quality-1-og-2/02-Coverage.md](context/swt/markdown/05.1-2-test-quality-1-og-2/02-Coverage.md) - dækningsgrader
