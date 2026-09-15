---
title: "C# Events"
source: "C Events(1).html"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
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

Bruger man C# Events, skal det selvfølgelig testes at de er implementeret og bruges korrekt.

Alt dette er beskrevet i nedenstående slides.

[UsingAndTestingEvents.pdf](../shared/UsingAndTestingEvents.md)
