---
title: "GitWorkflow.pdf"
source: "GitWorkflow.pdf"
modul: "Lektion 01.2: Exceptions + Git Workflow"
pages: 16
type: "slides"
vision: "done"
---
# GitWorkflow.pdf

<!-- side 1 -->

GIT WORKFLOW
I4SWT




AARHUS                                           LUKAS ESTERLE
UNIVERSITY                         AUGUST 2025   ASSOCIATEPROFESSO R
D EP ARTME NT OF EN GI NE ERI NG

<!-- side 2 -->

DISTRIBUTED VCSS (CVSS) – E.G. GIT
  Clients hold full “mirror” (clone) of repository, not just latest & greatest
  Pros:
    • Version control
    • Works offline
    • Switch to any tracked branch
    • Server can be restored from any
      working copy
      (if you keep a strict discipline)
  Cons:
         • Takes up space for all versions and branches

  AARHUS                                                  LUKAS ESTERLE
  UNIVERSITY                               AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Diagram over distribueret VCS-topologi. Øverst boksen `Server Computer`, som indeholder en `Version Database` med tre stablede grønne kasser: version 1 (nederst), version 2, version 3 (øverst). Nedenunder to bokse side om side: `Computer A` og `Computer B`. Hver indeholder en blå kasse `file` øverst med en pil opad fra sin egen `Version Database`, der ligeledes rummer version 1, version 2, version 3. Dobbeltpile forbinder Server Computer med både Computer A og Computer B, og en dobbeltpil forbinder Computer A direkte med Computer B — klienterne kan udveksle indbyrdes uden om serveren. Alle tre maskiner har altså identisk fuld versionshistorik.

<!-- side 3 -->

SNAPSHOTS, NOT DIFFERENCES
Differences
  • Recreate files from reverse deltas
       (not as pictured)
  • Can take ages to restore older versions



Snapshots
 • Mini-filesystems
 • All versions instantly available

     AARHUS                                              LUKAS ESTERLE
     UNIVERSITY                           AUGIUST 2025   ASSOCIATE PROFESSOR
     D EP ARTME NT OF EN GI NE ERI NG

**Figur:** To diagrammer, ét pr. lagringsmodel, begge med en vandret tidsakse Checkins over time og fem søjler Version 1 … Version 5 (røde kasser) øverst.

<!-- side 4 -->

LOCAL FILE STATES AND OPERATIONS

                                                                          Cloned from somewhere or
                                                                          created on your local machine




                                                                          Note:
                                                                          VS ”Commit” and
                                                                          TortoiseGit ”commit -> master”
                                                                          combine stage and commit steps!



  AARHUS                                            LUKAS ESTERLE
  UNIVERSITY                         AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Diagrammet `Local Operations`. Tre afrundede bokse i række: `working directory` (grøn), `staging area` (gul), `git directory (repository)` (blå). Under hver løber en lodret livline.

<!-- side 5 -->

RECORDING CHANGES TO THE REPO
                                                    Tracked




                                                                             These two
                                                                               steps
                                                                            combined in
                                                                            most git GUIs



  AARHUS                                              LUKAS ESTERLE
  UNIVERSITY                         AUGIUST 2025     ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Tilstandsdiagram for en fils status i Git. Fire afrundede tilstandsbokse i række: `untracked` (rød), `unmodified` (grøn), `modified` (gul), `staged` (blå). En klamme over de tre sidste er mærket `Tracked` — untracked står uden for.

<!-- side 6 -->

WORKING WITH REMOTES
                                                                             Clone
                                      Add

                                                                               Pull
                                     Test!
                                              Local                                            Remote
                                     Commit   repo                   Test!                      repo

                                                                           Push




                                                                                       Local    Local
                                                                                       repo     repo
  AARHUS                                                               LUKAS ESTERLE
  UNIVERSITY                                          AUGIUST 2025     ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 7 -->

PULL – PUSH WORK FLOW
  1.             CODE
  2.             Write TESTs and run them
  3.             CORRECT code
  4.             COMMIT when all tests are passed
  5.             PULL! From your common remote repository
  6.             Solve any MERGE conflicts
  7.             TEST! And correct until all tests are passed
  8.             COMMIT merges and changes          (GOTO 5!)
  9.             Now you can PUSH!
  10. If somebody pushed since last pull – repeat from PULL!

  AARHUS                                                               LUKAS ESTERLE
  UNIVERSITY                                            AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 8 -->

SETTING UP REMOTES
                                       Code
                                        and
                                       Test


                                     Create repo


                                         Add


                                       Commit
                                                          Local
                                                          repo

                                                   Create or find remote repo                                           These two
                                                                                                                      steps may be
                                                                                                             Remote   combined as
                                                                                                              repo      "publish"
                                                                                 Push

  AARHUS                                                                               LUKAS ESTERLE
  UNIVERSITY                                                            AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Flowdiagram. En rød cirkel `Code and Test` øverst til venstre. En stor lodret cylinder i midten er `Local repo`; en mindre cylinder til højre er `Remote repo`. Blå blokpile peger fra venstre ind i den lokale cylinder, ovenfra og ned:
1. `Create repo`
2. `Add`
3. `Commit`
Derefter en lang blokpil `Create or find remote repo`, som starter til venstre for Local repo, passerer forbi den og peger ind i Remote repo, og til sidst `Push`, en blokpil fra Local repo ind i Remote repo. En klamme om de to sidste pile ender i boblen These two steps may be combined as "publish".

<!-- side 9 -->

HOW TO ACCESS A GLOBAL REPOSITORY

 Use Gitlab to store your repositories.
     • It’s preferred to setup SSH keys and use git protocol, ex:
              • git@gitlab.au.dk:au-ece-swt/ecswithevents.git


 Create a GitLab group for your team
 Create your own team repositories (projects) on GitLab for each
 solution/exercise/hand-in!


  AARHUS                                                          LUKAS ESTERLE
  UNIVERSITY                                       AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 10 -->

STUFF NOT TO PUT IN YOUR REPOSITORY
 Do not check in any intermediate editor
 and build files
    ✓*.ncb
    ✓*.suo
    ✓*.ilk
    ✓*.pdb
    ✓*.user
    ✓*.obj
    ✓(…etc)!


 Do not check in the build directories             Git can use a .gitignore if you put it in your project
    ✓Debug, Release                                      Visual Studio can generate one for you, if you
    ✓obj, bin                                            follow one of the methods described in the
    ✓(Etc.)
                                                         following
   AARHUS                                                 LUKAS ESTERLE
   UNIVERSITY                              AUGIUST 2025   ASSOCIATE PROFESSOR
   D EP ARTME NT OF EN GI NE ERI NG

<!-- side 11 -->

LOCAL FILE STRUCTURE




  AARHUS                                            LUKAS ESTERLE
  UNIVERSITY                         AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Skærmbillede af Windows Stifinder i mappen …tegration (Git Basics) ▸ Scheduler_1420. Filliste med kolonnerne Name og Date modified:
- `.git` (almindelig mappe-ikon, ingen overlay) — 18-09-2014 11
- `packages` (grønt flueben-overlay) — 18-09-2014 09
- `Scheduler` (grønt flueben) — 18-09-2014 11
- `Scheduler.Tests.Unit` (grønt flueben) — 18-09-2014 11
- `.gitattributes` (grønt flueben) — 20-08-2014 08
- `.gitignore` (grønt flueben) — 20-08-2014 08
- `Scheduler.sln` (grønt flueben) — 19-08-2014 15
- `Scheduler.sln.DotSettings.user` — 18-09-2014 11
- `Scheduler.v12.suo` — 18-09-2014 12
To blå pile fremhæver `.git`-mappen (selve repositoriet) og `.gitignore`-filen. De grønne flueben er TortoiseGit-overlays for versionsstyrede/uændrede elementer; `.user`- og `.suo`-filerne har intet flueben — de er ikke i versionsstyring.

<!-- side 12 -->

VISUAL STUDIO FOCUS POINTS
  Let VS create the local repository
  Make sure the .gitignore file exists and is suitable for VS solutions
  Let VS push or publish to a remote repository




  AARHUS                                              LUKAS ESTERLE
  UNIVERSITY                           AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 13 -->

CLONE FROM GITLAB IN VISUAL STUDIO
 • Use the Clone function from Visual Studio (Start View or Team Explorer)
 • Be careful where you store the clone
 • Make sure Visual Studio's Solution Explorer is in Solution View: Select
 • Execute a Build – Rebuild Solution command from the menu – this should remove
   all red marks
 • Run the tests to see if everything works!
 • Subsequently, a clone is NOT needed - a Pull or a Sync is enough
 • When you have Sync/Pulled - and resolved any Merge conflicts - then run a Build-
   Rebuild Solution command!


    AARHUS                                                     LUKAS ESTERLE
    UNIVERSITY                                  AUGIUST 2025   ASSOCIATE PROFESSOR
    D EP ARTME NT OF EN GI NE ERI NG

**Figur:** Til højre et lille skærmbillede af Visual Studio med panelet Solution Explorer – Folder View åbent. To røde pile peger på henholdsvis knappen i Solution Explorer-værktøjslinjen, der skifter visning, og på posten `NetCoreCashRegister.sln` — altså skiftet fra Folder View til Solution View, som punktet Make sure Visual Studio's Solution Explorer is in Solution View: Select henviser til. Under mappen NetCoreCashRegister (C:\Users\au23729?\…) ses træet: `NetCoreCashRegister`, `NetCoreCashRegister.Test.Unit`, `.gitattributes`, `.gitignore`, `NetCoreCashRegister.sln`.

<!-- side 14 -->

OTHER WORK MODES

  Almost everything can be done by TortoiseGit
  Everything can be done from the command line, using git




  AARHUS                                            LUKAS ESTERLE
  UNIVERSITY                         AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 15 -->

GIT RESOURCES ARE ABUNDANT
  Pro Git book ( http://git-scm.com/book )is really good - PDF, online
  http://gitready.com (med masser af reklamer og click-bait)




  AARHUS                                             LUKAS ESTERLE
  UNIVERSITY                          AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

<!-- side 16 -->

TOPICS NOT COVERED

    Handling merge conflicts


    Branching
           • Covered later in the course




  AARHUS                                                  LUKAS ESTERLE
  UNIVERSITY                               AUGIUST 2025   ASSOCIATE PROFESSOR
  D EP ARTME NT OF EN GI NE ERI NG

