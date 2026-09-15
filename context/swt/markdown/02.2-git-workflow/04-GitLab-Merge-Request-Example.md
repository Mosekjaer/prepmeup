---
title: "GitLab Merge Request Example"
source: "csfiles/home_dir/Git og branches/GitWorkflow-GitlabMergeRequestExample.pdf"
modul: "Lektion 2.2: Git workflow"
pages: 9
type: "slides"
vision: "done"
---
# GitLab Merge Request Example

<!-- side 1 -->

SW4SWT

GITLAB MERGE REQUEST

AARHUS                        SWT    PETER HØGH MIKKELSEN
UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
TECHNICAL SCIENCES

<!-- side 2 -->

CREATE MERGE REQUEST




 AARHUS                       SWT     PETER HØGH MIKKELSEN
 UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
 TECHNICAL SCIENCES

**Figur:** Skærmbillede af GitLab merge request-oversigten. Brødkrumme: au-ece-swt / MicrowaveOven / Merge requests / !1. Titel "Buzzer", status-badge "Open", linjen "Peter Høgh Mikkelsen requested to merge buzzer into main 11 minutes ago". Knapper: Edit, Code, Add a to do, Expand. Fanerække med tællere: Overview 0, Commits 3, Pipelines 2, Changes 14. Beskrivelse: "Feature adding buzzer functionality has been added". Reaktionsknapper 👍 0 / 👎 0. Nederst et pipeline-panel med grønt flueben: "Pipeline #266564 passed", "Pipeline passed for 036b0536 on buzzer just now", "Test coverage 97.50% (-2.50%) from 1 job" — coverage-faldet vises i rødt.

<!-- side 3 -->

LACKING COVERAGE IS MARKED RED




 AARHUS                       SWT     PETER HØGH MIKKELSEN
 UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
 TECHNICAL SCIENCES

**Figur:** Skærmbillede af Changes-fanen i merge requesten (Overview 0, Commits 3, Pipelines 2, Changes 14 — Changes valgt). Fil-header: Microwave.Classes/Boundary/Timer.cs, +16 −0, checkbox "Viewed". Unified diff med gammelt/nyt linjenummer i to kolonner; tilføjede linjer har grøn baggrund og "+". Diffen viser:

<!-- side 4 -->

COVERAGE HITS ARE SHOWN IN CHANGES




 AARHUS                       SWT     PETER HØGH MIKKELSEN
 UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
 TECHNICAL SCIENCES

**Figur:** Skærmbillede af Changes-fanen med filen Microwave.Classes/Boundary/Buzzer.cs, markeret som ny fil (0 → 100644), +34 −0. Alle viste linjer er grønne tilføjelser:

<!-- side 5 -->

REVIEWER MARKS SECTIONS AND ADDS
COMMENTS




  AARHUS                       SWT     PETER HØGH MIKKELSEN
  UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
  TECHNICAL SCIENCES

**Figur:** Skærmbillede af Changes-fanen hvor et linjeinterval er markeret og kommenteret. Fil: Microwave.Classes/Boundary/Timer.cs, +16 −0. Linjerne 24–32 (Timer(double interval)-konstruktøren) er markeret med blå baggrund og rød dækningsmarkør i venstre margin. Under diffen står "Comment on lines +24 to +32" og en kommentartråd: "Peter Høgh Mikkelsen @au276283 · just now" med badges Author og Owner, kommentarteksten "No coverage, but overload is not used either, why not delete?". Nederst et "Reply..."-felt og knappen "Resolve thread". Bemærk at Overview-tælleren nu er 3 (kommentarerne tælles med).

<!-- side 6 -->

CREATE DIALOG TO RESOLVE ISSUE




  AARHUS                       SWT     PETER HØGH MIKKELSEN
  UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
  TECHNICAL SCIENCES

**Figur:** Skærmbillede af samme kommentartråd efter at den er løst. Overview-tælleren er nu 4. Tråden viser "Resolved just now by Peter Høgh Mikkelsen" over den oprindelige kommentar "No coverage, but overload is not used either, why not delete?", et grønt flueben-ikon markerer tråden som løst, derunder "Collapse replies" og et svar fra Peter Høgh Mikkelsen: "Will do". Knappen hedder nu "Unresolve thread" i stedet for "Resolve thread".

<!-- side 7 -->

FIX CODE AND PUSH, REVIEWER CAN
APPROVE




  AARHUS                       SWT     PETER HØGH MIKKELSEN
  UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
  TECHNICAL SCIENCES

**Figur:** Skærmbillede af merge request-oversigten efter rettelsen. Titel "Buzzer", "requested to merge buzzer into main 34 minutes ago". Badge "All threads resolved!" i stedet for åbne tråde. Faner: Overview 4, Commits 4, Pipelines 3, Changes 14 — commit- og pipeline-tælleren er steget efter det nye push. Pipeline-panel: "Pipeline #266576 passed", "Pipeline passed for 30497474 on buzzer just now", "Test coverage 100.00% (0.00%) from 1 job" — dækningen er nu 100 % mod 97,50 % på side 2. Derunder en blå "Approve"-knap med teksten "Approval is optional", og et testsammendrag: "Test summary: no changed test results, 81 total tests" med link til "Full report".

<!-- side 8 -->

REPOSITORY GRAPH SHOWS BRANCHING




 AARHUS                       SWT     PETER HØGH MIKKELSEN
 UNIVERSITY           05 APRIL 2024   SW4SWT GITLAB MERGE REQUEST
 TECHNICAL SCIENCES

**Figur:** Skærmbillede af GitLabs Repository Graph (au-ece-swt / MicrowaveOven / Graph) med branch-vælgeren sat til main. Commit-grafen læses nedefra og op langs en tidsakse (Apr 11 → Apr 16). Commits nedefra: "Initial commit", "initial add", "adding images", "updated readme" (alle på den røde main-linje), derefter forgrener to branches sig: en magenta linje mærket powertube og en grøn/turkis linje mærket buzzer. På buzzer-linjen ligger "adding buzzer feature", "added test for new timer start method", "adding power level variable", "adding timer constructor", "removed unused timer overload". Øverst løber buzzer-grenen tilbage i main med commit'en "Merge branch 'buzzer' into 'main'", hvor branch-labelen main sidder. powertube-grenen er stadig udestående (ikke merget).

<!-- side 9 -->

AARHUS
UNIVERSITY

