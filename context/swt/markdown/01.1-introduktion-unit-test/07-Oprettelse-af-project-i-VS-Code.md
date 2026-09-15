---
title: "Oprettelse af project i VS Code"
source: "Oprettelse af project i VS Code.html"
modul: "Lektion 01.1: Introduktion + Unit Test"
type: "brightspace-side"
kind: "indhold"
---
# Oprettelse af project i VS Code

Denne video viser hvordan man opretter en solution med hhv console, class lib og test projekter i VS Code.

Det er vigtig at I tilføjer følgende Nuget pakker til testprojektet:

NSubstitute

NUnit3TestAdapter

NunitXml.TestLogger

coverlet.collector

ReportGenerator

I kan gøre det samme med følgende script:

[https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools/-/blob/main/dotnet/scripts/create-dot-net-solution.sh?ref_type=heads](https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools/-/blob/main/dotnet/scripts/create-dot-net-solution.sh?ref_type=heads)
