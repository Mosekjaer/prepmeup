---
title: "Løsningsforslag til 'Door Control med Isolation Framework'"
source: "Lsningsforslag til Door Control med Isolation Fram.html"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
type: "brightspace-side"
kind: "indhold"
---
# Løsningsforslag til "Door Control med Isolation Framework"

[DoorControlConstructorInjection.zip](code/DoorControlConstructorInjection.md)

Her finder du løsningsforslag til DoorControl, lavet med brug af NSubstitite. Et par bemærkninger til løsningsforslaget:

- Klassen DoorControl's state er private og kan derfor ikke bruges til at assert'e på. Det må den heller ikke kunne, for det ville have gjort tests til whitebox-tests, der ville have været meget skrøbelige. For eksempel ville den foreslåede ændring af staten OPEN til CLOSING i det tilfælde have knækket test cases i frådende vildskab, fordi *navnet* på staten er ændret - ikke selve programmets *virkemåde*. Det er jo ærgerligt. I stedet er det eftervist at en given sekvens af handlinger på DoorControl medfører det rette resultat (her: den rette adfærd). Det er en black box test som holder, uanset om DoorControl er implementeret vha. en state machine, et bundt if-else's eller på en helt tredje måde.
