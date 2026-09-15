---
title: "Isolation frameworks i en event-drevet applikation"
source: "Isolation frameworks i en event-drevet applikation.html"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
type: "brightspace-side"
kind: "indhold"
---
# Isolation frameworks i en event-drevet applikation

På [ECSWithEvents](https://gitlab.au.dk/au-ece-swt/ecswithevents) finder du et eksempel på, hvordan en event-drevet applikation kan testes med brug af et isolation framework.

Når man skal teste en eventdrevet applikation er der grundlæggende to ting, der adskiller sig fra tests af mere konventionelle applikationer:

- Hvis unit-under-test (UUT) skal *modtage* et event, skal man lave en fake event source, som UUT hooker sig op på og modtager events fra. Fake'n skal altså dels udstille et event som UUT kan hooke sig på, dels raise et event som UUT'en forventes at reagere på. Fake'n kan laves vha. NSubstitute. I det tilfælde er det afgørende at det event som UUT skal hooke sig på er en del af det interface, som fake'n oprettes ud fra.

- Hvis UUT skal *raise* et event, skal test suite'n kunne hooke sig på UUT's event(s) og modtage disse. Her bruges et lille kunstgreb med en anonym funktion som man i [SetUp] hooker på UUT, og som "husker" på events hvis de raises af UUT. Herefter kan man så asserte på om events er blevet raised som forventet.

I det vedhæftede er en lille demo-implementering af det efterhånden velkendte drivhus-eksempel, her implementeret som en event-drevet applikation. Funktionaliteten er ændret en smule: Når en ny temperatur er målt, notify'er TempSensor om dette gennem et event. ECS (vores UUT) er hooket op på dette event, gemmer temperaturen og kalder så den gamle Regulate, som nu burde gøres private.

her er lidt detaljer om funktionaliteten:

- For ECS (receiver): SetUp(): Test suiten fremstiller en ITempSensor fake vha. NSubstitute, som constructor injectes i Control.

- For ECS (receiver): Test cases: TempChangedEvent raises gennem fake'n med en given temperatur, og der assertes efterfølgende på om dens property er blevet ændret, dette beviser, at der blevet korrekt forbundet til eventet og data bruges korrekt. I de gamle testcases, hvor Regulate blev kaldt, testes nu for interaction ved ligeledes at raise event gennem faken med de samme temperaturer.

- For TempSensor(source): SetUp(): En lambda funktion tilknyttes eventet. Denne gemmer dels sidste eventargsparameter og tæller antallet afevents.

- For TempSensor (source): Test cases: Der testes på, om SetTemp udløser eventet, om de rigtige værdier overføres, og om eventet IKKE udføres for mange gange!

Syntax'en for anonyme funktioner og for at raise events gennem fakes er nok lidt ny, men i bund og grund er den simpel nok - blot endnu en ny måde at gøre tingene på.

Happy event-testing!
