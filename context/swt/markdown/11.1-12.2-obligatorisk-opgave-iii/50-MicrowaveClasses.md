---
title: "MicrowaveClasses"
source: "MicrowaveDiagramsPdf.zip/MicrowaveClasses.pdf"
modul: "11.1-12.2-obligatorisk-opgave-iii"
pages: 1
type: "diagram"
vision: "done"
---

# MicrowaveClasses

<!-- side 1 -->

Display                      Output




Button   3



             UserInterface                     CookController



Door



                                       Light    Timer              PowerTube

**Figur:** Klasse-/afhængighedsdiagram for Microwave Oven med ni klasser tegnet som simple rektangler uden attribut- eller metodefelter: `Button`, `Door`, `UserInterface`, `Display`, `Light`, `CookController`, `Timer`, `PowerTube` og `Output`.

Afhængigheder (pil peger fra den afhængige klasse mod det den afhænger af):

- `Button` → `UserInterface` (multiplicitet `3` ved pilen: tre knapper — power, time, start-cancel)
- `Door` → `UserInterface`
- `UserInterface` ⇢ `Button` (stiplet pil, altså en løsere afhængighed/callback tilbage)
- `UserInterface` ⇢ `Door` (stiplet pil)
- `UserInterface` → `Display`
- `UserInterface` → `Light`
- `UserInterface` ↔ `CookController` (pile begge veje: gensidig afhængighed)
- `Display` → `Output`
- `Light` → `Output`
- `CookController` → `Display`
- `CookController` ↔ `Timer` (pile begge veje)
- `CookController` → `PowerTube`
- `PowerTube` → `Output`

`Output` er blad i træet (ingen udgående pile) og bliver dermed det naturlige startpunkt for bottom-up integrationstest. `UserInterface` og `CookController` sidder i midten med flest afhængigheder og indgår i den eneste cykel i grafen.
