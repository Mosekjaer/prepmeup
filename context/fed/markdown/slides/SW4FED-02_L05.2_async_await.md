# Asynkroni med async/await

## Metadata

- **Lektion:** L05.2 – Asynchrony with async/await
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L05/FED async-await.pdf (9 slides)
- **Emner dækket:**
  - Task-based Asynchronous Pattern (TAP)
  - Asynchronous functions og `async`-modifieren
  - `Task<TResult>` og unwrapping med `await`
  - Hvad `await` reelt gør — continuations
  - Tilladte returtyper fra async-metoder
  - `Task.Run` til at offloade synkront arbejde

---

## 1. Task-based Asynchronous Pattern

.NET eksponerer asynkrone versioner af rigtig mange operationer, som følger **Task-based Asynchronous Pattern (TAP)**.

Kort sagt: the future is asynchronous.

## 2. Asynchronous functions

C# understøtter fra version 5 konceptet asynchronous function i sproget. Det er altid enten en metode eller en anonym funktion, som er deklareret med `async`-modifieren, og som kan indeholde `await`-expressions.

### Kodeeksempel

```csharp
private async void btnGetHtml_Click(object sender,
                                    RoutedEventArgs e)
{
    tbxLength.Text = "Fetching...";
    string url = tbxUrl.Text;
    HttpClient client = new HttpClient();
    string text = await client.GetStringAsync(url);
    tbxLength.Text = text.Length.ToString();
}
```

## 3. Task&lt;TResult&gt;

Splitter man kaldet til `HttpClient.GetStringAsync` fra `await`-expressionen, kan man se de involverede typer:

```csharp
HttpClient client = new HttpClient();
Task<string> task = client.GetStringAsync(url);
string text = await task;
```

Typen af `GetStringAsync` er `Task<string>`, men typen af `await task`-expressionen er bare `string`. `await`-expressionen udfører altså en "unwrapping"-operation.

## 4. await

`await` er en sprogkonstruktion, der lader dig "awaite" en asynkron operation.

Hovedformålet med `await` er at undgå at blokere, mens vi venter på, at en tidskrævende operation færdiggøres.

Denne "awaiting" ligner meget et normalt blocking call, i den forstand at resten af din kode ikke fortsætter, før operationen er færdig — men det lykkes den at gøre uden faktisk at blokere den aktuelt kørende tråd.

### await uncovered

`await`s trick er, at metoden faktisk returnerer, så snart vi rammer `await`-expressionen.

- Indtil det punkt eksekverer den synkront på UI-tråden ligesom enhver anden event handler.
- Når `await` nås, tjekker koden, om resultatet allerede er tilgængeligt. Hvis ikke, planlægges en **continuation**, som eksekveres, når den awaitede operation er færdig.
- Continuationen eksekveres på GUI-tråden — den kaldende tråd.

```csharp
private async void btnGetHtml_Click(object s, RoutedEventArgs e)
{
    string url = tbxUrl.Text;
    HttpClient client = new HttpClient();
    string text = await client.GetStringAsync(url);
    tbxLength.Text = text.Length.ToString();   // <- continuation
}
```

Linjen efter `await` er continuationen.

## 5. Returtyper fra async-metoder

Async-metoder er begrænset til følgende returtyper:

- `void`
- `Task`
- `Task<TResult>`

`Task` og `Task<TResult>` repræsenterer en operation, som måske endnu ikke er færdig:

- `Task<TResult>` repræsenterer en operation, der returnerer en værdi af typen `T`.
- `Task` producerer ikke et resultat.

**Bemærk:** du kan ikke bruge `out`- eller `ref`-modifiers på parametrene til en async operation-deklaration.

## 6. Hvordan man awaiter et synkront kald

Brug `Task.Run` omkring en synkron, langsom metode, hvis du har brug for at offloade arbejdet:

```csharp
async void button_Click(…)
{
    await Task.Run(() => DoSlowWork());
    …
}
```

`DoSlowWork` er her en synkron funktion, der tager mere end 30 ms at gennemføre.

## 7. Referencer og links

- Asynchronous Programming with Async and Await in ASP.NET Core — https://code-maze.com/asynchronous-programming-with-async-and-await-in-asp-net-core/
