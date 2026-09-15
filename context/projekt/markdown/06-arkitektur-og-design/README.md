# 06 — Arkitektur og design

Lektion L12 (Fra arkitektur til systemdesign) og L13 (System Design og Interfaces). Slides er mærket "I2ISE" (tidligere kursuskode). Underviser ikke angivet i materialet.

| Fil | Kilde-PDF | Type | Sider | Indhold |
|---|---|---|---|---|
| `system-architecture-and-design.md` | `System Architecture and Design.pdf` | slides | 36 | Designprincipper (decomposition, coupling, cohesion, abstraktion, testability), RVM-arkitekturer, designkriterier, arkitekturstrategi (layering, half-sync/half-async, frameworks), init/termination, error handling |
| `system-design-and-interfaces.md` | `System Design and Interfaces-F21.pdf` | slides | 30 | Interface-specifikation i SysML (BDD/IBD, porte `name:type`, port-tabeller), Measurement Instrument-eksempel, elektriske signaler/TTL/CMOS, timing diagrams (bus, X.10, I2C), logiske vs. fysiske modeller, «allocate» |

Ikke konverteret: `System Design and Interfaces with Solution.pdf` (nævnt på Brightspace, ikke i kildematerialet).

## Brightspace: L12 from System Arkitektur to System Design

**Indhold:**
- Summary Architecture (BDD, IBD, SD, STM)
- How to: from System Architecture to System Design
- Decomposition
- System Design Principles
- Coupling and Cohesion

**Materials:** System Architecture and Design.pdf

**Emner:** Arkitektur, design og dokumentation. Arkitektur og subsystemer. Dekomponering. Designkriterier og principper. "Coupling". "Cohesion". Strategi for arkitektur.

## Brightspace: L13 Grænseflader

**Indhold:**
- Interface identification
- Interface description
- Hardware interfaces
- Logical and physical models
- *Software Interfaces (extra included and continues next week L14)
- Serial Interface (Serial Communication)
- Hvordan kan sende og modtage data til/fra microcontroller
- Hvordan Arduino kommunikeres (Arduino interfaces) med komponenter
- Hvordan RPI kommunikeres (RPI interfaces) med ?
- Hvordan Arduino og RPI kommunikeres sammen

**Materials:** System Design and Interfaces.pdf; System Design and Interfaces with Solution.pdf (åbne efter forelæsning)

Bemærk: Punkterne om seriel kommunikation, Arduino- og RPi-interfaces dækkes ikke af `System Design and Interfaces-F21.pdf` men af protokol-decket i `../12-protokoller/swise-protocols.md` (UART/SPI/I2C, Arduino Mega 2560- og Raspberry Pi-interfacetabeller).
