---
title: "Tips and tricks"
source: "Tips and tricks - Copy.html"
modul: "Lektion 06.1-07.2: Obligatorisk Handin 2"
type: "brightspace-side"
kind: "indhold"
---
# Tips and tricks

Her opsamles diverse tips og tricks til løsning af Handin 2. Bidrag gerne selv ved at sende dem til mig.

### CI Pipeline file

Til CI skal I bruge den pipeline fil, der blev beskrevet under Coverage:

[GitLabCIWithCoverage.gitlab-ci.yml](https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools/-/tree/main/gitlab?ref_type=heads)

Husk at omdøbe til .gitlab-ci.yml

### Vigtige NuGetpakker:

Husk at checke at minimum følgende NuGet pakker er en del af jeres **test**-projekt:

- JunitXml.TestLogger

- Microsoft.NET.Test.Sdk

- NUnit

- NUnit3TestAdapter

- coverlet.collector

- ReportGenerator

### Løsningsforslag til tidligere opgaver:

Testbart design:

[ECSBeforeAndAfter.pdf](../shared/ECSBeforeAndAfter.md)

Kode med tests:

[Calculator](code/CalculatorSolution.md)

[ECSSolution](code/ECSSolution.md)

#### Git rapporterer ændringer på mærkelige filer

Hvis git mener at der er ændringer eller merge konflikter på alle mulige mærkelige filer, kan det være fordi man ikke har en .gitignore fil, eller har committet første gang inden man fik en .gitignore fil oprettet.

For at få fjernet disse filer fra versionsstyring, kan man udføre følgende aktiviteter:

Sørg for at der er en .gitignore i solution folderen.

Start en kommandolinje i den folder.

Udfør følgende kommandoer nøjagtig som de står (punktummerne skal med!):

**git rm -r --cached .**

**git add .**

**git commit -m "Removed unnecessary tracking of files"**

Derefter pusher man og de andre skal pulle. Hvis man ikke kan gøre det, tager man en ny clone af gitlab repo, og udfører ovenstående sekvens og pusher. Så kan de andre pulle.
