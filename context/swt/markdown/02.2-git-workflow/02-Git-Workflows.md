---
title: "Git-Workflows"
source: "Git-Workflows.pdf"
modul: "Lektion 2.2: Git workflow"
pages: 43
type: "slides"
vision: "done"
---
# Git-Workflows

<!-- side 1 -->

GIT WORKFLOWS
I4SWT




AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
UNIVERSITY                               FEBRUARY 2026
D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 2 -->

CONTENTS


  • Git workflow models
  • Git terminology and commands (via command line or visual studio)
  • Merge/pull requests using Gitlab




  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 3 -->

GIT WORKFLOWS
  Git Workflows describe ways to integrate Git into a (programming) workflow. There is no
  one-size-fits-all Git workflow, its implementation depend on the project and context.
  General guidelines:
           • Workflow should be simple and enhance productivity
           • Short-lived branches, integrate often
           • Enable code reviews and CI
           • Minimize and simplify reverts
           • Support a given release model
  Two main workflows:
           • Trunk-based workflow and Feature branch workflow.

  AARHUS                                                        MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                    FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 4 -->

COMMON GIT SETUP FOR WORKFLOWS

                                                   Clone
                                                                                                             Local
                                                                                                             repo
                  Add                               Pull

                                           Local                                             Remote
             Commit                        repo                                               repo

                                                   Push
                                                                                                             Local
                                                                                                             repo




  AARHUS                                                             MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                         FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 5 -->

TRUNK-BASED WORKFLOW
        • Single branch main is the only (public)
          branch
        • Commit directly to main (master) or use
          short-lived feature branches (typically
          local only)
        • Integration done by merging master into
          local work pushing if healthy.
        • Probably what you have been doing so
          far!



  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Til højre et commit-graf-diagram (kilde: Aventude) med tre lodrette baner: `master` (gule commits), `release-v1.1.0` og `release-v1.2.0` (begge med grønne commits). Til venstre for master ligger to kolonner lilla commits, som er kortlivede lokale feature-branches. Stiplede pile går fra master-commits ud til de lilla lokale commits (rebase/merge af master ind i lokalt arbejde), og fra de lilla commits tilbage ind i master. Fra to master-commits går massive pile ud i release-banerne; de grønne release-commits ender i `tag v1.1.0` hhv. `tag v1.2.0`. Pointen visuelt: kun master er offentlig, alt sideordnet arbejde er kortlivet og lilla/lokalt, og releases forgrener sig af master og tagges.

<!-- side 6 -->

TRUNK-BASED WORKFLOW - PROCEDURE
  1. CODE
  2. Write TESTs and run them
  3. CORRECT code
  4. COMMIT when all tests are passed
  5. PULL! From your common remote repository
  6. Solve any MERGE conflicts
  7. TEST! And correct until all tests are passed
  8. COMMIT merges and changes
  9. Now you can PUSH!
  10. If somebody pushed since last pull – repeat from PULL!
  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 7 -->

FEATURE BRANCH WORKFLOW
           • Not specific about main being the only long-lived branches (may e.g. have
             release branches)
           • Short-lived feature branches (typically shared/pushed to central for CI and
             code review)
           • Features typically only integrated when complete => no scattered feature
             history
           • Integration via feature branches. Code review on feature branch (merge/pull
             request).




  AARHUS                                                     MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                 FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 8 -->

GITHUB FLOW
                                                   (Also a feature)




                                                                                                   https://blog.programster.org/git-workflows




  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Commit-graf med tre baner. Nederst/midt: `Master` som en vandret kæde af blå commits (fire i alt). Øverst: `Bugfix`-branch med to røde commits, som forgrener sig fra masters første commit og merges tilbage ind i masters tredje commit. Nederst: `Feature`-branch med to grønne commits, som forgrener sig fra masters andet commit og merges tilbage ind i masters fjerde (sidste) commit. En blå pil med teksten (Also a feature) peger på Bugfix-branchen — en bugfix behandles som en almindelig feature-branch. Alle branches er kortlivede og merges direkte tilbage til master; der er ingen develop- eller release-branch.

<!-- side 9 -->

GITHUB FLOW
  Target group
           • Small teams, web applications that don’t need to support multiple versions and where releases are often
             an small increments
  Goal
           • Main branch is always deployable! Simple, which enables CI/CD
  Branches
           • Features (which can be hotfixes etc), which are all created from master
  Testing
           • Test all branches (incl. main). Remember to test after merging master into feature, before merging
             feature into master.
  Integration
           • Use pull requests to request help reviewing you work, then merge back into master (main)

  AARHUS                                                               MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                           FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 10 -->

GITHUB FLOW - PROCEDURE
  1.              PULL Latest
  2.              Create a NEW BRANCH and switch to it (checkout)
  3.              CODE
  4.              Write TESTs and run them
  5.              CORRECT code
  6.              COMMIT when all tests are passed
  7.              FETCH MAIN from your remote repository
  8.              MERGE MAIN into your branch
  9.              TEST! And correct until all tests are passed
  10. COMMIT merges and changes
  11. PUSH BRANCH!
  12. MERGE REQUEST (or checkout main, merge branch into main and push main)
  13. DELETE BRANCH locally and on remote
  AARHUS                                                                 MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                             FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 11 -->

                                                   GIT-FLOW




https://nvie.com/posts/a-successful-git-branching-model/

**Figur:** Det klassiske git-flow-diagram fra nvie.com. Fem lodrette baner, fra venstre mod højre: `feature branches` (to baner, lyserøde commits), `develop` (gule commits), `release branches` (grønne commits), `hotfixes` (rød commit) og `master` (cyan commits). En stor lodret pil helt til venstre markerer `Time` nedad.

<!-- side 12 -->

GIT-FLOW
  Target group
           • Applications that needs support for multiple (scheduled) releases, ex. multiple platforms and who
             release less often and in bigger increments, typically.
  Goal
           • Main branch is the latest stable version. Supports multiple release branches. Improve reliability through
             more testing stages.
  Branches
           • Develop, Feature, Release, Hotfix. Features integrate with Develop only. Develop integrates with
             releases, which in turn integrates with main
  Testing
           • Unit and Component testing in Feature. Further integration testing in Develop
  Integration
           • Use pull requests from feature into Develop. Fix system integration in Develop
  AARHUS                                                                MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                            FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 13 -->

GITLAB FLOW




                                                                                                   https://blog.programster.org/git-workflows
  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Commit-graf med fire vandrette, navngivne baner (nedefra og op: `Production` orange, `Staging` rød, `Master` blå, plus kortlivede farvede branches). Master (blå kæde) er øverst af hovedbanerne.

<!-- side 14 -->

GITLAB FLOW
  Target group
           • Application that releases often, but where additional levels of integration is desired. Meet-in-the-middle
             of Git-Flow and Github Flow.
  Goal
           • Production branch is readily deployable whereas Master is the local “pre-release” version, stable, but
             with probable bugs.
  Branches
           • Production, Staging, Feature, Master (Main) Features integrate with Master. Master integrates with
             Staging, which in turn integrates with Production
  Testing
           • Unit and Component testing in Feature and testing in Staging, where Master can be considered stable
  Integration
           • Use pull requests from feature into Master and run automated tests when staging
  AARHUS                                                                MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                            FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 15 -->

                                         MERGE/PULL REQUESTS
                                         VIA GITLAB



AARHUS                                                           MARTIN KNUDSEN / PETER HØGH MIKKELSEN
UNIVERSITY                                       FEBRUARY 2026
D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 16 -->

GITHUB / GITLAB CODE REVIEW
  Code review of feature branch before integration into main
  (test that it works locally!)
  Push feature branch from local to remote
  Switch to GitHub / GitLab GUI
           • Create pull request / merge request
           • Add reviewers (if applicable)
           • Once code review is completed approve pull request on GitHub
           • Feature branch is automatically merged into main
           • Optionally delete feature branch after this (it’s no longer needed) (Don’t for hand-in 3!!)
  Check out main and pull (to update locally, and optionally delete branch locally)
  AARHUS                                                            MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                        FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 17 -->

GILAB MERGE REQUEST




  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Skærmbillede af GitLabs merge request-brugerflade i to lag.

<!-- side 18 -->

GITLAB MERGE REQUEST

                                           A   B                                                     F                G    main
     (gitlab.au.dk)
     remote repo




                                                   create merge request
                                                   (on the web)                                       approve merge request (optional)
                                                                         F                G                               feature/delta


                                                                     push           push
     (my computer)
       local repo




                                           A   B                                                                           main


                                                        F          G                                                       feature/delta

  AARHUS                                                                      MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                  FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 19 -->

BRANCH NAMING
  Follow company branch naming policy


  Example:


                                          Name                          Description
                                          main                          Trunk
                                          feature/XXX                   Team-specific features
                                          release/YYY                   Public releases (branch or tag)
                                          user/ZZZ                      User-specific experiments



 AARHUS                                                                 MARTIN KNUDSEN / PETER HØGH MIKKELSEN
 UNIVERSITY                                             FEBRUARY 2026
 D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 20 -->

              REFERENCE:
              GIT TERMINOLOGY AND COMMANDS



AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
UNIVERSITY                               FEBRUARY 2026
D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 21 -->

OVERVIEW GIT




  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Diagrammet Git Data Transport Commands (http://osteele.com). Fire lodrette søjler/databaser fra venstre mod højre: `workspace` (grå), `index` (cyan), `local repository` (grøn), `remote repository` (gul).

<!-- side 22 -->

GIT FROM THE COMMAND LINE
Git command-line tools are standard available (Windows,
Linux, Mac)
Git for Windows provides a BASH emulation (Git BASH shell)
Using a BASH shell is MUCH more convenient than e.g. CMD
prompt (PowerShell)
Git has very good Text UI companion: Tig
(https://jonas.github.io/tig/)
Tig is part of Git for Windows package




     AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
     UNIVERSITY                               FEBRUARY 2026
     D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** To skærmbilleder af Git BASH (MINGW64) til højre.

<!-- side 23 -->

GIT BRANCH
  Create a (local) copy of main (or the branch your in!)


  Results are merged back into branched-from branch (usually master/develop)
  Enables independent development – you develop the new feature on this branch.
  “checkout” – means shifting to this <branch> (Now the active branch you work on)

                                           Command                                    Description
                                           git branch <branch>                        Create new branch in local repo
                                           git checkout <branch>
                                                                                      Checkout branch in workspace
                                           git checkout –b <branch>                   Same as above


  AARHUS                                                                              MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                          FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 24 -->

GIT BRANCHES (FEATURE)
                                                                                                                         main
  Create (and push) branch:                             A                 B                   C              D   E
                                                                                                                         origin/main
           • git checkout –b feature/myfeature                                       branch
           • git push -u <remote> <branch>
                                                                                      F                  G               feature/myfeature
  Repeatedly commit locally in myfeature
           • git commit
           • git commit
                                                                                                                         main
                                                        A                 B                   C              D   E
           • git commit                                                                                                  origin/main
                                                                                     branch
  Then push for sharing and CI                                                                                       feature/myfeature
           • git push                                                                 F                  G           origin/feature/myfeature




  AARHUS                                                         MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                     FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 25 -->

GIT TAG
  Create an alias for a commit - think of it as a bookmark pointing to
  a specific commit. Once a commit is tagged, you cannot add any commits to it (so
  different than a branch!)

                                                           Command                                             Description
                                                           git tag
  Useful for tagging releases                                                                                  Show existing tags
                                                           git tag <tag>                                       Create lightweight tag
  (an important point in time)                             git tag –a <tag> -m "comment"                       Create annotated tag
  Tagging last successful CI build                         git push origin --tags                              Push tags
                                                           git checkout <tag>                                  Check out tag (detached)
                                                           git checkout –b <branch> <tag>                      Create branch from tag

  More info on Tags: https://circleci.com/blog/git-tags-vs-branches/#what-is-a-git-tag

  AARHUS                                                               MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                           FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 26 -->

GIT REMOTE
 Remotes are simply local names for addresses of
 remote repos
 Cloning a repo automatically creates a default
 remote called ‘origin’ (where you got the code
 from)
 Can add multiple remotes: origin (main upstream),
 co-worker repos, release repos, …
 Git commands: push, pull, fetch all takes a
 <remote> arg (default origin)




    AARHUS                                                     MARTIN KNUDSEN / PETER HØGH MIKKELSEN
    UNIVERSITY                                 FEBRUARY 2026
    D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Skærmbillede af Git BASH (MINGW64, /c/swt/CashRegisterCore på branch main):
```
$ git remote
origin

<!-- side 27 -->

REMOTE TRACKING BRANCHES
                                                      remote repo
                                                      (gitlab.au.dk)                                  push by others
Local “origin” has reference to the state of
remote branches
                                                               A                    B                      C       D       E        main
Git updates it when doing network
                                                                                                fetch
communication (pull, fetch, pull)
So, GIT tracks the remotes.
Local branches can be derived from these                                                        B                              origin/main   (Local
                                                                                                                                             branch)
Local branches can be merged with remotes:                                                                     merge
git pull = git fetch + git merge
                                                                                                               B       F            main
                                                      local repo
                                                      (my computer)




    AARHUS                                                         MARTIN KNUDSEN / PETER HØGH MIKKELSEN
    UNIVERSITY                                 FEBRUARY 2026
    D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 28 -->

GIT FETCH
  Fetches from remote repo                        remote repo
                                                  (gitlab.au.dk)                                  push by others


                                                           A                    B                      C       D       E        main
  Updates local repo (ex. branch
  origin/main) with new code from                                                           fetch
  remote, but not the local
  workspace (ex. main)
                                                                                            B                              origin/main   (Local
                                                                                                                                         branch)
                                                                                                               merge
  No merge of code.
                                                        A                       F                      G       H                main
                                                  local repo
                                                  (my computer)



  AARHUS                                                       MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 29 -->

FETCH-RELATED COMMANDS

         Command                                             Description
         git fetch <remote> <branch>                         Fetch from remote
         git fetch origin main
         git log <branch>..<remote-branch>                   Show remote changes
         git log main..origin/main
         git checkout main                                   Checkout main in workspace
         git merge origin/main
                                                             Merge remote changes to local main




  AARHUS                                                     MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                 FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 30 -->

GIT PULL
  Fetches remote branches AND merges


  git pull = git fetch + git merge ( in current branch)


  Merging may use different strategies (merge commit, fast-forward, rebase)


  We will look at merge strategies in a moment




  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 31 -->

GIT PULL
(gitlab.au.dk)
remote repo




                                                          A   B               C           D                E                              main
                                                                  fetch                                                fetch

                                                                                                                                                              Fetch
                                                                                                                                                            + merge
(my computer)




                                                          A          B        C           D                E                              origin/main
                                                                                                                                                               Pull
  local repo




                                                                          merge                                            merge

                                                                          B       F                                    M                  main


                                                                                                                                   M contains C-E and F-G
                                                                                                                                    and merge conflict
                 AARHUS                                                                           MARTIN KNUDSEN / PETER HØGH MIKKELSEN resolutions
                 UNIVERSITY                                                       FEBRUARY 2026
                 D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Tidslinje-diagram delt af en vandret stiplet linje: over linjen `remote repo (gitlab.au.dk)`, under linjen `local repo (my computer)`.

<!-- side 32 -->

GIT PUSH
  Uploads local branch (and code changes) to remote (and updates tracking
  branches)


  Git rejects if local branch tip is behind remote tracking – do pull first


  Git push will synchronize local branch and remote tracking


  git push <remote> <branch>



  AARHUS                                                   MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** Skærmbillede af en fejlslagen push i terminalen:
```
$ git push
To https://github.com/hejersbo/CashRegisterCore.git
 ! [rejected]        main -> main (non-fast-forward)
error: failed to push some refs to 'https://github.com/hejersbo/CashRegisterCore.git'
hint: Updates were rejected because the tip of your current branch is behind
hint: its remote counterpart. Integrate the remote changes (e.g.
hint: 'git pull ...') before pushing again.
hint: See the 'Note about fast-forwards' in 'git push --help' for details.
```

<!-- side 33 -->

GIT PUSH
         (gitlab.au.dk)
         remote repo




                                           A   B               C                D                    E                 M          main


                                                       pull
                                                                                                                      push
         (my computer)




                                           A       B           C                D                    E                       origin/main
           local repo




                                                         B                F                                       M                main




  AARHUS                                                                  MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                              FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 34 -->

GIT MERGE
  Integrates commits from one branch into another
  Merge conflict results in separate merge commit (M)
  Fast forward (default), when no merge conflict exists
           • No separate merge commit is created
           No fast-forward option always results in merge commit created
  Squash merge
           • Fold several commits into one big commit – useful if you merge feature branch with many
             small commits back into main branch (so main branch history shows 1 commit, not all 17 in
             local branch).



  AARHUS                                                           MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                       FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 35 -->

MERGE WITHOUT FAST-FORWARD


                git checkout main
                git merge feature/delta


                                           A     B        C          D                E                    M                main
                                               branch/                                                  merge
                                               checkout
                                                              B         F                                                   feature/delta

                                                                                                      M contains E and F.



  AARHUS                                                                          MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                      FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 36 -->

MERGE WITH AUTOMATIC FAST-FORWARD

              git checkout main
                                                                                         Only possible if no merge conficts
              git merge feature/delta                                                    between feature and mina



                                           A   B                                                               main

                                                   branch
                                                   F                   G                                       feature/delta


                                           A   B   fast-forward
                                                        F                  G                                   main
                                                                                                               feature/delta
  AARHUS                                                               MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                           FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 37 -->

SQUASH MERGE

             git checkout main
             git merge --squash feature/delta
                                                                                                                   S contains changes
                                                                                                                      from F-G in 1
                                                                                                                         commit


                                           A   B       C        D                 E                   S                     main

                                                                                                   squash
                                                   B       F            G                                                   feature/delta


  AARHUS                                                                       MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                   FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

**Figur:** To vandrette pilebjælker med commits. Øverst: A – B – C – D – E – S, mærket `main`. Nederst: B – F – G, mærket `feature/delta`. En grå pil går fra B på main ned til B på feature/delta (branch-punktet), og en grå pil går fra enden af feature/delta (efter G) skråt op til S på main, med etiketten squash ved siden af. Taleboble peger på S: S contains changes from F-G in 1 commit. Bemærk at F og G ikke optræder som selvstændige commits på main — kun den samlede commit S.

<!-- side 38 -->

GIT REBASE - ONLY PRO USE!
  Rebase put current branch commits aside
  And fast-forward merges branched-from branch into current branch
  And applies the aside commits to the current branch


  Rebasing allows for a clean history without merge commits
  Merge conflicts can still occur and must be resolved
  Creates new commits (and destroys old ones) by rewriting history
  Golden Rule of Rebasing: Never rebase published branches
           • (… unless you dare to/know how to force push (-with-lease))
  Git pull can either merge (default) or rebase
           • git pull --rebase
  AARHUS                                                              MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                          FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 39 -->

REBASE
             git checkout feature/delta
             git rebase main

                                           A   B       C        D                 E                                         main


                                                   B       F         G                                                      feature/delta


                                           A   B       C        D                 E                                         main
                                                                                                      rebase
                                                                                                          F’           G’   feature/delta
  AARHUS                                                                       MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                   FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 40 -->

GIT MERGE OR REBASE
                                           is non-destructive, but may result in lots of merge commits (non-clean history)

        Merge                              maintains history and traceability and makes for easier reverts


                                           often used as strategy for integrating feature branch back to main
                                           (either --ff-only or --no-ff)


                                           is destructive, but has no merge commits and thus a clean linear history

        Rebase                             (conflicts still needs resolving)
                                           no history of branches, harder to revert

                                           often used as strategy for repeatedly integrating main into feature branch

                                           often used to clean up local, in-progress feature branch (interactive rebase)



  AARHUS                                                                                 MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                             FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 41 -->

GIT MERGE AND REBASE RECOMMENDATION
  Rebase when updating feature branch


  Merge when updating main
                                                                                                                    main


                                           branch              rebase                                       merge



                                                                                                                    feature/delta

  AARHUS                                                            MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                        FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 42 -->

ADVANCED MERGING
  Cherry-picking for release branch
  Interactive rebasing for cleaning up feature branch before integration into main


                                           Command                                    Description
                                           git cherry-pick <commit>                   Merges individual commits
                                           git rebase –i <branch>                     Rewrite local commit history.
                                                                                      Several rewrite options including squash




  AARHUS                                                                              MARTIN KNUDSEN / PETER HØGH MIKKELSEN
  UNIVERSITY                                                          FEBRUARY 2026
  D EP A RTM EN T O F E N G IN E ERI N G

<!-- side 43 -->

AARHUS
UNIVERSITY

