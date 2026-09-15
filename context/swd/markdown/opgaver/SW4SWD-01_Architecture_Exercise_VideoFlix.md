# Uge 4 — Øvelse: Software architecture (VideoFlix)

## Metadata

| Felt | Værdi |
|---|---|
| **Type** | Øvelse (gruppearbejde, 2–4 studerende) |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Hører til** | Uge 4.1 — se [../slides/SW4SWD-01_W04.1_Architecture_Process_1.md](../slides/SW4SWD-01_W04.1_Architecture_Process_1.md) |
| **Kilde** | `Architecture exercise - VideoFlix.pdf` (1 side) |
| **Emner** | Architecture objectives, key scenarios, quality attributes, application overview, key issues, candidate solutions |
| **Relateret input** | [SW4SWD-01_Application_Overview_Input.md](SW4SWD-01_Application_Overview_Input.md) — indeholder tre færdigformulerede Key Scenarios (KS1–KS3) til samme case |

---

## Opgavens rammer

> **Exercise: Software architecture - VideoFlix**
>
> In these exercises, you will work with architecture objectives, key scenarios and quality attributes.

Opgaven er formuleret som: byg **VideoFlix** — "and show NetFlix, Hulu, HBO and all the others how it should *really* be done."

> **All exercises should be solved in groups of 2 to 4 students.**

Øvelse 1 og 2 løses i timen; øvelse 3 og 4 er hjemmearbejde (slide 64 og 66 i uge 4.1-decket). Øvelse 5–8 følger i uge 4.2.

---

## Øvelse 1

The objective of the first architecture iteration is to **build a prototype of the system**.

Identify some *key scenarios* seen from the perspective of:

a) The user
b) The business
c) The system

De tre perspektiver er præcis User/Business/System-Venn-diagrammet fra forelæsningen. Pointen fra slide 51 gælder her: *look for intersections between the user, business and system views* — de interessante key scenarios ligger i overlappene, ikke inde i én enkelt cirkel.

---

## Øvelse 2

Decide which key scenarios you will address in this iteration. Why did you decide on these scenarios?

Udvælgelseskriterierne fra slide 50 og 52: critical functionality, critical non-functional requirements, exploration af ukendte områder, risk mitigation — og foretræk scenarier, der rører flere lag i arkitekturen.

---

## Øvelse 3

Based on the key scenarios you have chosen, identify quality attributes you find it relevant to address in the prototype.

Brug ISO 25010-taksonomien (slide 16) eller \*-ilities-listen (slide 17) som checkliste.

---

## Øvelse 4

Quantify the quality attributes you have found. (I.e. what does it actually mean to have "high accessibility", etc.?)
How do you plan to test that you live up to the quality attributes you have defined?

Eksemplet fra slide 67: "VideoFlix has to stream to a lot of viewers at the same time" kan ikke måles; "VideoFlix has to stream the test video clip to 100.000 viewers in 1080p@30fps" kan.

---

## Øvelse 5

Create an application overview for the prototype.

Jf. slide 54: determine application type, identify deployment constraints, determine relevant technologies, identify important architectural design styles.

---

## Øvelse 6

Identify key issues you have to address. Where are you most likely to make mistakes or run into problems?

---

## Øvelse 7

Define a candidate solution, i.e. create the architecture for the prototype.

Evalueres mod baseline-arkitekturen efter checklisten på slide 60.

---

## Øvelse 8

Which architecture objectives would you consider for the next iteration?

Processen er iterativ — trin 5 fører tilbage til trin 1 og 2.

---

## Krydsreferencer

- Forelæsningsnoter: [../slides/SW4SWD-01_W04.1_Architecture_Process_1.md](../slides/SW4SWD-01_W04.1_Architecture_Process_1.md)
- Færdige Key Scenarios (KS1–KS3) og whiteboard-input til Application Overview: [SW4SWD-01_Application_Overview_Input.md](SW4SWD-01_Application_Overview_Input.md)
- Brainstorming-board (Taskcards): https://aarhusuni.taskcards.app/#/board/d30d672e-df28-4bf0-a34e-e5cd6bac4d9f?token=cc903f50-cd66-4747-8235-6ab65351c647
