---
title: "Automatiserede tests giver større frihed og mere tillid (avisartikel)"
source: "Autmoatiserede tests.jpg"
modul: "Lektion 13.2: Automatisering af System- og Accepttest"
type: "billede"
vision: "done"
---

# Automatiserede tests giver større frihed og mere tillid

**Figur:** Scannet avisside — Ingeniøren, 1. sektion, 15. december 2017, side 14, sektionen "TEMA / SOFTWAREUDVIKLING". Fem tekstspalter, et stort foto øverst til højre af to personer i et mødelokale ved et vinduesparti (en mand stående, en kvinde siddende med bærbar), et mindre foto i midten af en hånd der skriver på et whiteboard, samt en fremhævet citatboks.

## Rubrik og manchet

**Automatiserede tests giver større frihed og mere tillid**

Manchet: "Når produktet udrulles løbende, er det vigtigt med tests, som kan køre helt af sig selv. Det har været svært med brugerflader, men hos E-conomic er målet, at alle tests skal være automatiserede."

Byline: TESTS — Af Tania Andersen, tan@ing.dk. Fotokredit: Büro Jantzen.

Billedtekst til det store foto: "SORIN DIMOFTE og Signe Holm Julius sørger for at finde fejlene i E-conomics brugerflader. Ambitionen er, at alle tests skal være automatiserede."

## Indhold

Artiklen handler om E-conomic, et dansk-udviklet browserbaseret regnskabssystem, der er gået fra manuelle brugerfladetests til automatiserede. Kilder er Signe Holm Julius (leder fem udviklingshold hos E-conomic, startede selv som menig tester) og Sorin Dimofte (specialist i brugerfladetests, syv år hos E-conomic).

Hovedpointer:

- Testkompetencer er blandt de mest efterspurgte på it-jobmarkedet. Tests af programmer skrives som mindre programmer, ofte med værktøjer; tests af brugerflader har historisk været "problembarnet", fordi brugeradfærd varierer, og værktøjet skal kunne klikke på knapper og skrive i felter.
- Automatisering giver "meget mere frihed" og større tillid til produktet ved udrulning, fordi det basale er dækket og de samme tests kan køres igen og igen.
- **Ikke tid til at vente på testere**: strategien *continuous integration* sætter fokus på automatisering. Produktet udrulles kontinuerligt i mindre bidder frem for én stor opdatering et par gange om året, hvilket gør det nemmere at se, om man er på rette kurs i forhold til kundernes forventninger. Hyppig udrulning er uforenelig med at vente på manuelle testere; tidligere afhang man af fageksperter, og var en ekspert ikke til stede, blev udrulningen udsat en dag eller to.
- **Værktøjer**: Selenium og Cucumber med JavaScript bagved. Selenium beskrives som en udgave af Chrome-browseren, der kan fjernstyres, så systemet bagved ikke kan se forskel på et menneske og et testprogram, der klikker på knapperne; den bruges som del af det større framework Nightwatch. Cucumber er et mini-programmeringssprog, der til forveksling ligner almindeligt sprog — et såkaldt *domænespecifikt sprog*. Her skriver man "brugsscenarier", en lillebitte historie om en helt specifik anvendelse af softwaren.
- Et brugsscenarie kan læses og forstås af alle, så forskellige interessenter selv kan definere scenarier, mens testere, automationsteknikere og udviklere skriver implementeringen bag testen. Det er samtidig anvendeligt som dokumentation til fremtiden.
- **Produktejere skaber scenarier**: produktejere fra forretningsdelen sætter sig sammen med udviklere og testere og skaber scenarierne på forhånd, før koden skrives. Automationstesteren er involveret i hele udviklingsprocessen. Der testes også manuelt, hvor det er nødvendigt; derefter skrives de automatiserede tests undervejs i processen.
- Efter færdiggjort ny funktionalitet er der ikke mange tests: man forsøger at få tingene ud i små bidder, så testere og udviklere "udforsker" manuelt i de understøttede browsere, og når udviklerne har noget mere sammenhængende, laves automatiserede tests til regressionstests. Regressionstests defineres i artiklen som de tests, man udfører, når der tilføjes ny kode og funktionalitet: da ny kode i princippet kan påvirke eksisterende kode og moduler, skal den gamle kode også testes igen for at sikre, at den nye kode ikke har introduceret fejl i den gamle.
- **Kulturen skal ændres**: fokus skal ligge på kvaliteten, ikke på tests i sig selv. Argumentet er, at kode af højere kvalitet opstår, når udviklerne har det fulde ansvar for egen kode; med manuelle testere er der høj risiko for, at det rutineprægede arbejde videregives frem for at blive automatiseret. Udviklerne skal tage ejerskab for den kode, de fremstiller, og forstå hvorfor de arbejder med en bestemt "brugerhistorie" eller opgave. E-conomic har reduceret antallet af manuelle testere og erstattet dem med automationstestere, og opkvalificerer sine eksisterende testere, fordi de har opbygget en stor faglig viden — to testere er gået fra fuldtids manuel test til at stå for automatiseringen af virksomhedens tests.

Fremhævet citat i citatboksen: "Lige nu prøver jeg at lede holdene væk fra behovet for manuelle tests. Jeg tror stærkt på, at vi kan lave kode af højere kvalitet, hvis udviklerne har det fulde ansvar for deres egen kode." — Signe Holm Julius, E-conomic.
