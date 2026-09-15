---
title: "Eksempel på implementering og anvendelse af en State Machine"
source: "Eksempel p implementering og anvendelse af en Stat.html"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
type: "brightspace-side"
kind: "indhold"
---
# Eksempel på implementering og anvendelse af en State Machine

[ExampleStatemachine.zip](code/ExampleStatemachine.md)

Her finder du et eksempel på implementering og anvendelse af en State Machine (STM).

Den demonstrerer bl.a. at state-based testing ikke drejer sig om klassens STM, men direkte og indirekte synlige tilstande - hvor man så må ty til interaction-based testing for at afsløre tilstanden.

Når man tester en klasse hvis adfærd er defineret af en STM, er det vigtigt at den interne state (OFF, LOW, HIGH, POWER_SAVE) *ikke* eksponeres for omgivelserne - tilstanden er objekt-intern og skal kunne ændres/udvides uden at test suiten skal ændres. Det er med andre ord væsentligt at man laver black-box tests.

En black box test kan for eksempel laves ved at test suite'n ikke benytter den konkrete type for uut (FlashLightCtrl), men dens interface (IFlashLightCtrl), så det er garanteret at test suite'n kun benytter det samme interface som rigtige klienter. Det er ikke altid muligt eller ønskværdigt, men når det giver mening vil det enforce en black box-test.

Jeg har inkluderet en black box test suite til den lille eksempel-STM, hvor jeg netop søger at sikre at FlashLightCtrl's adfærd er i overensstemmelse med det, vi formulerede i en STM, uden at kende til state'n. Igen: State er objekt-intern, og test suite'n skal søge at teste UUT uden at kende interne detaljer.
