# 03 — Systemtest og accepttest (L5/L7)

Lektionen hedder L5 i den oprindelige rækkefølge, men blev afholdt som L7 i F25 ("Der er original L5 i Rækkefølge men vi tager i L7 i F25"). Slides er fra I2ISE (Introduction to Systems Engineering); øvelsen er dateret 2014/KBE og 2016-PDF'en har author "kbe" (formentlig Kim Bjerge, jf. github.com/kimbjerge/education-ise i L13-slides). Kurset er SWISE-01 Indledende System Engineering, undervist som del af SW4PRJ4-02 Projekt 4.

## Filer

| Fil | Kilde-PDF | Type | Sider | Indhold |
|---|---|---|---|---|
| `system-test.md` | `System Test.pdf` (2024-udgaven, 31 slides) | slides | 31 | Hvorfor teste, cost of errors, testdefinition, ækvivalensklasser, black/white box, route coverage, V-model, validation/verification, unit/integration/system/acceptance test, bottom-up/top-down (drivers/stubs), UC → accepttest, accepttestskabelon med validation rules |
| `eksempel-accepttestspecifikation-ttt.md` | `TTT_Accepttestspecifikation.pdf` | eksempel | 13 | Fuld accepttestspecifikation fra et 3.-semesterprojekt (Tic Tac Toeminator): 14 use case-scenarietabeller + 3 tabeller for ikke-funktionelle krav |
| `oevelse-accepttest.md` | `AcceptTestOvelse.pdf` | øvelse | 2 | Pengeskabslåge med fingeraftryksscanner: skriv tre accepttest cases (hovedscenarie, Ext. 1, Ext. 2) ud fra en fully-dressed use case |

## Dubletter og ikke-konverterede kilder

- `System Test.pdf` findes i to udgaver i zip 4: en 2024-udgave på 31 slides (`unz/4/System Test.pdf`) og en 2016-udgave på 30 slides (`unz/4/csfiles/home_dir/I2ISE Lessons/System Test.pdf`). Kun 31-slides-udgaven er konverteret; forskellene er listet nederst i `system-test.md`. Bemærk at scratchpad-filerne er navngivet omvendt: `raw/4__System_Test_31p.txt` og `png/4__System_Test_31p/` indeholder 30-slides-udgaven, mens `raw/4__System_Test.txt` og `png/4__System_Test/` indeholder 31-slides-udgaven. Konverteringen er lavet ud fra den faktiske 31-slides-udgave.
- `AcceptTestOvelse.doc` er samme dokument som `AcceptTestOvelse.pdf` (Word-format). Ikke konverteret separat.
- `AcceptTestOvelseLosning.pdf` (løsningsforslag) er nævnt på Brightspace-siden, men findes ikke i det eksporterede materiale.

## Brightspace: L7 Systemtest

**Materialer**
- Slides: System Test.pdf
- Eksempel: TTT_Accepttestspecifikation.pdf
- AcceptTestOvelse.pdf
- AcceptTestOvelse.doc
- AcceptTestOvelseLosning.pdf

**Indhold**
- Introduktion: Hvorfor teste?
- Test og kvalitet
- Prisen for fejl
- Testtyper: Black-box test, white-box test
- Hardware og software test
- Test coverage
- Enhedstest, Integrationstest og Systemtest
- Inkrementelle tests test
- Bottom-up vs. Top-down
- Test scenarios
- Udfærdigelse af tests
- Accepttest
- Testspecification

**Læsestof**
- Test-Driven Development
- Hardware Test and Debug
- Software test:
  - https://www.ibm.com/topics/software-testing
  - https://en.wikipedia.org/wiki/Software_testing
- Eksempel på accepttest fra et 3. semesterprojekt [TTT_Accepttestspecifikation.pdf]

**Øvelser**
- AcceptTestOvelse.pdf (På klassen)
