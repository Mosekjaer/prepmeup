---
title: "Forberedelse: Download og installér Git tools"
source: "Forberedelse Download og installr Git tools.html"
modul: "Lektion 01.2: Exceptions + Git Workflow"
type: "brightspace-side"
kind: "indhold"
---
# Forberedelse: Download og installér Git tools

**OBS! Git for Windows kan også installeres som komponenter ved hjælp af Visual Studio Installer.**

Den basale git service er nødvendig, git for Windows. Download den [her](https://git-for-windows.github.io/). (Brug den download, der står øverst på siden, den store blå knap)!

Vær sikker på, at du får sagt **Git Credential Manager** også installeres. Det vil gøre livet som git bruger meget nemmere.

Git for Windows indeholder et GUI, der kan bruges til at arbejde med sine filer i git, uden at man skal skrive git-kommandoer i et terminalvindue.

Som en del af denne pakke, tilbydes også GitBash, som er en speciel kommandlinje for git kommandoer. Det bliver betydeligt nemmere at udføre git-kommandoer i den rigtige folder, hvis man beder om at få installeret Windows Explorer integration af kommandoen "Git Bash Here".

**Installer dem inden forelæsningen.**

Som et Windows- og brugervenligt interface til git, kan bruges TortoiseGit. git for Windows indeholder også et GUI, som er langt simplere. Eller vælg et git GUI efter eget valg.

Download TortoiseGit [her.](https://tortoisegit.org/download/)

Alternativt kan Git plugin til VS Code installeres. Denne er desuden cross-platform. Denne kræver sandsynligvis at Git er installeret på platformen. Dette kan gøres via relevant pakkemanager (brew, apt etc.)

Endelig kan man også bruge [Github Desktop](https://github.com/apps/desktop), blot skal man huske at angive at det ikke er et github repo som man ønsker at åbne.

Når du skal bruge gitlab.au.dk er det en god idé at opsætte en ssh key. På windows (powershell), Mac/Linux terminal kan man oprettet et nøglepar med "ssh-keygen". Indholdet af public key filen (.pub) kopieres ind på gitlab. Herefter kan I kommunikere mellem git klient og gitlab via ssl. Visual Studio kan godt finde på at lave et pop-up vindue når man forsøger at clone første gang, hvor den skriver at den ikke kender authenticity af serveren x.y.z.z gitlab.au.dk, her skal I bare skrive "yes".
