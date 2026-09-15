---
title: "Hjælp - Jeg glemte en .gitignore første gang jeg pushede!!!"
source: "Hjælp - Jeg glemte en .gitignore første gang jeg pushede!!!.html"
modul: "Lektion 01.2: Exceptions + Git Workflow"
type: "brightspace-side"
kind: "indhold"
---
# Hjælp - Jeg glemte en .gitignore første gang jeg pushede!!!

Hvis git mener at der er ændringer eller merge konflikter på alle mulige mærkelige filer, kan det være fordi man ikke har en .gitignore fil, eller har committet første gang inden man fik en .gitignore fil oprettet.

For at få fjernet disse filer fra versionsstyring, kan man udføre følgende aktiviteter:

Sørg for at der er en .gitignore i solution folderen.

Start en kommandolinje i den folder.

Udfør følgende kommandoer nøjagtig som de står (punktummerne skal med!):

**git rm -r --cached .**

**git add .**

**git commit -m "Removed unnecessary tracking of files"**

Derefter pusher man og de andre skal pulle. Hvis man ikke kan gøre det, tager man en ny clone af gitlab repo, og udfører ovenstående sekvens og pusher. Så kan de andre pulle.
