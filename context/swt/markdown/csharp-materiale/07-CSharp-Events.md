---
title: "C# Events"
source: "C Events.html"
modul: "C# materiale"
type: "brightspace-side"
kind: "indhold"
---
# C# Events

Hvad er C# events?

Det er C#'s indbyggede løsning til Provider-Consumer mønsteret, som i Software Design kurset løses med GoF Observer Pattern. Med denne feature reduceres arbejdet, så man ikke skal lave så mange interfaces og klasser.

Det er nødvendigt at definere en klasse, som er de data, Consumer vil abonnere (Subscribe) på. (EventArgs)

Der skal så være en Provider klasse, der stiller disse data til rådighed gennem en Event - som er et forbindelsespunkt for Consumeren. Et Event er i princippet bare en liste af metodepointere til metoder i andre klasser/objekter.

Consumer-klassen skal så implementere en sådan metode, der har den prototype - dvs. parametre, som eventet forventer. Og forbinde denne metode til Event.

Når nye data er klar i Provider klassen, kalder den blot alle de metoder, der har forbundet sig til eventet, med de nye data som parameter.

Se evt (materiale fra lektion 5): [UsingAndTestingEvents.pdf](../shared/UsingAndTestingEvents.md)

og kodeeksempel: [https://gitlab.au.dk/au-ece-swt/ecswithevents](https://gitlab.au.dk/au-ece-swt/ecswithevents)

[https://learn.microsoft.com/en-us/dotnet/csharp/events-overview](https://learn.microsoft.com/en-us/dotnet/csharp/events-overview)
