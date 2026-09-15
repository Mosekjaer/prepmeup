---
title: "Flere feature filer i SpecFlow"
source: "Flere feature filer i SpecFlow.html"
modul: "Lektion 13.2: Automatisering af System- og Accepttest"
type: "brightspace-side"
kind: "indhold"
---
# Flere feature filer i SpecFlow

Reqnroll/SpecFlow prøver at optimere koden, så hvis det kan se, at det samme Step gentages i flere Features, bliver det ikke genereret på ny. Men da hver Feature har sin egen klasse, er der problemer med at dele data, fx. fra en Setup af et helt system.

Dette bliver ikke et problem i dagens øvelse, men hvis I vil prøve at lave flere features, skal man nok sætte sig ind begrebet Context Injection, der gør det muligt at genbruge objekter på tværs af features.

Context Injection in Reqnroll: [https://docs.reqnroll.net/latest/automation/context-injection.html](https://docs.reqnroll.net/latest/automation/context-injection.html)

(I den artikel bruges begrebet POCO - Plain Old C# Object. Der er udbredt uenighed, om POCO'er blot er DTO'er - Data Transfer Objects - uden forretningslogik, højst lidt validering, eller om POCO er en del af systemet, med forretningslogik, der indgår i implementation af systemet. I denne artikel bruges POCO som DTO!)
