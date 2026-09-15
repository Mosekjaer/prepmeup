# L02.3 – Events i C#

## Metadata

- **Lektion:** L02 – Event in C#
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L02/Lectures/Event in Csharp.pdf (21 slides)
- **Emner dækket:**
  - Hvad et event er, og hvad event sender betyder
  - Event listener, event handler og delegate
  - Raise og handle et event i C# og i XAML
  - `+=`-operatoren til at koble handler på event
  - Signaturen for en event handler: sender og EventArgs
  - Sammenhængen mellem XAML-attributten og metoden i code-behind

---

## 1. Hvad er et event?

Et event er en besked sendt af et objekt for at signalere, at en handling er sket.

Handlingen kan være forårsaget af brugerinteraktion — fx et button click — eller den kan komme fra anden programlogik, fx at værdien af en property ændrer sig.

Objektet, der raiser eventet, kaldes **event sender**. Event senderen ved ikke, hvilket objekt eller hvilken metode der vil modtage (handle) de events, den raiser.

## 2. Declare an Event

Når man arbejder med events, er der en event listener, der lytter efter et specifikt event, og som derefter notificerer en **event handler** — en metode, der indeholder kode skrevet af udvikleren, og som eksekveres som respons på det event, der opstår i applikationen.

For at eventet og responsen fungerer korrekt, har man brug for den **delegate**, der forbinder eventet med handler-metoden, og for den klasse, der holder event-dataene.

## 3. Raise and Handle an Event

For at raise et event skal man invoke eventet.

I C#-kode gøres det med `+=`-operatoren. Det betyder, at event handleren kaldes, når eventet sker. `+=`-operatoren har intet med aritmetiske operatorer at gøre.

```csharp
this.Clicked += (s,e) => {
           MessageBox.Show(((MouseEventArgs)e).Location.ToString()); };
```

I XAML-kode:

```xml
<Button ...
        Clicked="Button_Clicked" />
```

For at handle et event skal det, når det raises, håndteres af en event handler. Event handleren indeholder den kode, der kræves for at respondere på eventet.

## 4. Event Handler

Alle event handlers tager to parametre:

- Et objekt, der er det element, som raisede eventet (kaldet **sender**).
- Et objekt af typen `EventArgs` (eller en klasse afledt af `EventArgs`).

Per konvention hedder `EventArgs`-parameteren `e`.

I de fleste tilfælde er vi ligeglade med sender, og den simple `EventArgs` er tom og tjener kun som baseklasse for afledte klasser, der leverer yderligere information til event handleren. Man kan altså have en type afledt af `EventArgs`, der leverer den information, event handleren har brug for.

Event handler-navnet i code-behind matcher den event handler, der er identificeret i XAML.

## 5. Event og Event Handler — sammenhængen

XAML:

```xml
<Button
    x:Name="CounterBtn"
    Text="Click me"
    SemanticProperties.Hint="Counts the number of times you click"
    Clicked="OnCounterClicked"
    HorizontalOptions="Center" />
```

Her er en `Button`, der definerer event handleren `OnCounterClicked`, som knappens `Clicked`-event delegerer til.

Code-behind:

```csharp
private void OnCounterClicked(object sender, EventArgs e)
{
        /* Necessary code here */
}
```

## 6. References & Links

- Introduction to Event:
  https://learn.microsoft.com/en-us/dotnet/csharp/events-overview
  https://learn.microsoft.com/en-us/dotnet/standard/events/
- C# Concepts: Delegates and Events:
  https://learn.microsoft.com/en-us/dotnet/csharp/delegates-overview
