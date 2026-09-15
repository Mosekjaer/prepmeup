---
title: "Gitlab Login med auid og passwd"
source: "Gitlab Login med auid og passwd.pdf"
modul: "Eksamensinformation"
pages: 2
type: "slides"
vision: "done"
---
# Gitlab Login med auid og passwd

<!-- side 1 -->

Hvis du ikke kan logge ind på dit eksamensrepo via invitationen og får fejlbeskeden:

"The invitation can not be found with the provided invite token."

Skyldes det sandynligvis at du har oprettet en au email som ikke er
<studienummer>@post.au.dk som er den alle invitationerne er udsendt til. Det kan fikses ved
at tilføje din studienummer email til din Gitlab konto:

Gå til gitlab.au.dk og login via UNI-AD (Brightspace login og passwd)




Login med auid og passwd. Samme login og passwd som Brightspace, mit.au.dk mm.

Dernæst




Derefter

**Figur:** Tre skærmbilleder af GitLab med røde pile, der markerer, hvad der skal klikkes. (1) Login-siden på gitlab.au.dk: GitLab-rævelogoet, overskriften «Aarhus Universitet - Gitlab», felterne «Username or primary email» og «Password» (med øje-ikon til at vise koden), linket «Forgot your password?», afkrydsningsfeltet «Remember me» og den blå «Sign in»-knap. Under skillelinjen «or sign in with» ligger knappen «UNI-AD» med endnu et «Remember me»-felt — en rød pil peger på UNI-AD-knappen. (2) Brugermenuen i GitLab-topbjælken, foldet ud under avataren: brugernavnet «Peter Høgh Mikkelsen» / «@au276283» og punkterne «Set status», «Edit profile», «Preferences» og «Sign out» — en rød pil peger på «Edit profile». (3) Siden «Emails» under User settings, med venstremenuen «Account, Applications, Chat, Access tokens, Emails (markeret), Password, Notifications, SSH Keys». Hovedområdet hedder «Linked emails ✉ 2» og viser `phm@ece.au.dk` med grønt «Verified»-mærke og underpunkterne «Primary email» (bruges til avatar-detection), «Commit email» (bruges til web-baserede operationer som edits og merges) og «Default notification email», samt en anden adresse `phm@ase.au.dk` med Verified-mærke og et papirkurv-ikon. En rød pil peger på knappen «Add new email» øverst til højre.

<!-- side 2 -->

Tilføj <studienummer>@post.au.dk, hvor studienummer er det 9-cifrede nummer som
begynder med et årstal.

Det burde være nok.

Hvis det stadigvæk ikke virker, så prøv under Profile at ændre din commit email til din
<studienummer>@post.au.dk adresse.

**Figur:** Skærmbillede af GitLab-siden «User Settings / Edit Profile» med overskriften «Main settings». Venstremenuen viser «User settings» med punkterne Profile (markeret), Account, Applications, Chat og Access tokens; topbjælken har tællere for issues (4), merge requests (3) og todos samt et søgefelt «Search or go to...». I hovedområdet feltet «Public email» sat til «Do not show on profile» med hjælpeteksten «This email will be displayed on your public profile», og derunder feltet «Commit email» sat til «Use primary email (phm@ece.au.dk)» med hjælpeteksten «This email is used for web-based operations, such as edits and merges.» og linket «What is a private commit email?». En rød pil peger på Commit email-dropdownen — det er her, adressen skal skiftes til <studienummer>@post.au.dk. Nederst begynder feltet «Website URL».

