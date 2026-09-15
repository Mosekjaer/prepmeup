---
title: "Løsningforslag til DoorControl med factory injection"
source: "Lsningforslag til DoorControl med factory injectio.html"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
type: "brightspace-side"
kind: "indhold"
---
# Løsningforslag til DoorControl med factory injection

[DoorControlFactoryInjection.zip](code/DoorControlFactoryInjection.md)
[DoorControlManualMockFactory.zip](code/DoorControlManualMockFactory.md)

Her er vedhæftet et par løsninger, hvor DoorControl benytter sig af factory injection, for at kunne få sine dependencies.

Factory injection har sine fordele og ulemper.

I den ene løsning anvendes en mock factory der benytter sig af mock versioner af afhængighederne. Dvs. at både factory og de enkelte mocks skal implementeres.

Den anden løsning benytter sig af NSubstitute's geniale recursive mock feature, som gør, at en substitute for et interface, der indeholder metoder, der skal returnere objekter der opfylder et andet interface, automatisk laver en fake som om man selv havde kaldt Substitute.For<>. Man er sikret altid at få den samme mock, så den kan både sættes op som stub og bruges som mock i selve test koden. Se dette [link](https://nsubstitute.github.io/help/auto-and-recursive-mocks/).

Factory injection bliver mere tung at arbejde med, når man skal blande fake dependencies og ægte versioner af andre dependencies, fx som under en integrationstest.
