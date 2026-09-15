---
title: "NUnit Assertions"
source: "NUnit Assertions.html"
modul: "Lektion 01.1: Introduktion + Unit Test"
type: "brightspace-side"
kind: "indhold"
---
# NUnit Assertions

Der findes to forskellige modeller for assertions i NUnit: Classic Model og Constraint Models. Vi vil benytte Constraint-modellen (med enkelte undtagelser), så derfor:

- Læs om [assertions](https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertions.html) og de to assertion modeller generelt.

- Læs mere detaljeret om [Constraint](https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertion-models/constraint.html) assertion modellen og de muligheder der findes for at skrive assertions. Bid særligt mærke i, at metoden Assert.That() er grundlæggende for alle assertions i Constraint modellen. Og der er en række "syntax helpers" som gør det nemmere at lave Constraint objekter, fx "Has.", "Is.", "Does." Se mere i slides for lektionen og under hver specifikke constraint.

- Orientér dig om de forskellige [constraints](https://docs.nunit.org/articles/nunit/writing-tests/constraints/Constraints.html) der kan bruges med Assert.That().

Bemærk hvordan en sådan assertion nærmest er "human readable". Uden at du ved noget som helst om assertions er det ret oplagt hvad **Assert.That( myString, Is.EqualTo("Hello") );** tester, ikke?

Generel dokumentation for NUnit findes [her](https://docs.nunit.org/articles/nunit/intro.html).
