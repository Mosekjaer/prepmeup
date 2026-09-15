# System Design and Interfaces (SysML and Hardware)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L13 — System Design og Interfaces |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `System Design and Interfaces-F21.pdf` (30 slides) |
| **Type** | slides |
| **Emner dækket** | HW architectural design, interface identification og description i SysML (BDD/IBD, external/internal ports, `name:type`), port-specifikationstabeller (Measurement Instrument-eksempel), type-kategorier, elektriske signaler (single ended/differential, TTL/CMOS/RS-232/RS-422/485), timing diagrams (bus read/write, X.10, I2C), logiske vs. fysiske modeller, «allocate», Counter/Home Automation/ECG-eksempler |

---

*"It is essential to know the specification of interfaces being able to design, test and develop a system."*

> Slide 1

## HW/SW architectural design

I dag: *HW architectural design and interfaces*. Samme aktivitetsoversigt som L12 (General system design → HW/SW architectural design → Interface design), nu med fokus på HW compositional break-down og design of HW/SW interfaces.

> Slide 2

## Interfaces

- **Interfaces in SysML**
- **Specifying HW interfaces in detail**
- *Specifying SW interfaces — later in course*
- *Specifying protocols — later in course* (se `12-protokoller/`)
- …

> Slide 3

## How to specify interfaces using SysML

1. Start with context and BDD and IBD diagrams
2. Define external ports on the IBD diagram
3. Define internal ports on the IBD diagram
4. Describe functionality for every block
5. Specify requirements to interfaces between parts — describe electrical requirements for all ports of all blocks in a table — ensure they fit together

> Slide 4

## Gennemgående eksempel: Measurement Instrument

### 1. Context / BDD

*Figur (uc Measurement Instrument): use case-diagram med systemboks "Measurement Instrument" og tre aktører: Sensor (venstre), External Trigger og Computer (højre). Foto af en lille boks med USB-stik og BNC-indgange.*

**bdd Measurement Instrument** — komposition:

| Blok | Dele (parts) |
|---|---|
| «block» Measurement Instrument | Amplifier, ADC, ProcessorBoard, PowerSupply |

(Sort diamant ved Measurement Instrument → de fire delblokke.)

> Slide 5

### 2. IBD — External Ports

**ibd Measurement Instrument**, eksterne porte på systemgrænsen:

| Ekstern port | Type | Forbundet til intern part |
|---|---|---|
| 220V | AC | : PowerSupply |
| Sensor | Analogue | : Amplifier |
| Trigger | Digital | : ProcessorBoard |
| Computer | USB | : ProcessorBoard |

Interne connectors (uden navngivne porte endnu): PowerSupply → Amplifier, ADC og ProcessorBoard; Amplifier → ADC; ADC → ProcessorBoard.

> Slide 6

### 2. External Ports Requirements

| Name of Block | Description of function | Port Name | Type | Port Specification |
|---|---|---|---|---|
| Measurement Instrument | Instrument to measure low voltage sensor signals. The sensor signal is digitized and data stored in memory whenever the input trigger signal is high. Measured sensor data can be transfer to a connect computer over the USB port. | 220V | AC | 200 – 250 V RMS, 50 Hz. Input current limiter of 100 mA |
| | | Sensor | Analogue | Differential Input Signal. Voltage Range -100 to +100 uV peak. Impedance 50 Ohm |
| | | Trigger | Digital | 5 V trigger input. Low when < 0.8 V. High when > 2.0 V |
| | | Computer | USB | USB 2.0 |

> Slide 7

### Examples of different type categories (mainly used in the course)

| Signals (Electrical) | Standards (Physical, Protocol) | Network (Protocol) | Information (Software) | Supply (Power) | Others |
|---|---|---|---|---|---|
| Analogue | USB | Ethernet | File | DC | Force |
| Digital | RS232 | Wireless | Image | AC | Light |
| | HDMI | Internet | String | | Sound |
| | SPI | Profinet | Barcode | | Noise |
| | I2C | | Bytes | | Liquid |
| | TTL | | Data | | |
| | CMOS | | Bool | | |

> Slide 8

### Signals (Electrical)

- Output voltage with tolerances and maximum current
- Input voltage with tolerances and maximum current
- Output and input impedance has to fit together

*Figur: "Single Ended Interface" — Source med Vout og serie-modstand Rout, forbundet til Sink med Rin og Vin, fælles ground. Tekst: "Rin = Rout is best". "Differential Input" — to indgange A og B (modsatfasede sinuskurver) ind i en differensforstærker "Diff", videre til "ADC".*

> Slide 9

### Interface with Digital Signals (Standards TTL or CMOS)

*Figur 1: CMOS vs. TTL logikniveauer ved 3.3V output. CMOS: high ≥ 70 % af Vcc, low ≤ 30 %. TTL: high ≥ 2 V, low ≤ 0.8 V.*

*Figur 2: Acceptable TTL gate levels ved Vcc = 5 V.*

| | Input | Output |
|---|---|---|
| High | ≥ 2 V | ≥ 2.7 V |
| Low | ≤ 0.8 V | ≤ 0.5 V |

*Figur 3: Seriel transmission af ASCII "U" = 85 decimal = 55 hex = 01010101 binær, med START, LSB … MSB, STOP — vist for tre signalstandarder:*

| Standard | Niveauer |
|---|---|
| TTL/CMOS | 0 V / 5 V |
| RS-232 | +12 V / −12 V (med ±3 V tærskler omkring 0 V; logisk inverteret) |
| RS-422/485 | 0 V / 5 V differentielt (A og B ledere) |

> Slide 10

### 3. Internal Connections — Ports `<name:type>`

**ibd Measurement Instrument** med alle porte navngivet `name:type`:

| Part | Port | Forbundet til |
|---|---|---|
| : PowerSupply | 220V:AC | ekstern port 220V:AC |
| : PowerSupply | ± 5V:DC | : Amplifier Power:DC |
| : PowerSupply | 3V3:DC | : ADC Power:DC |
| : PowerSupply | 5V:DC | : ProcessorBoard Power:DC |
| : Amplifier | Sensor:Analogue | ekstern port Sensor:Analogue |
| : Amplifier | Out:Analogue | : ADC In:Analogue |
| : ADC | Out:Serial | : ProcessorBoard SampleData:Serial |
| : ProcessorBoard | Computer:USB | ekstern port Computer:USB |
| : ProcessorBoard | Trigger:Digital | ekstern port Trigger:Digital |

> Slide 11

### 4–6. Example of block description and ports (Power Supply, Amplifier)

| Name of Block | Description of function | Port Name | Type | Port Specification |
|---|---|---|---|---|
| Power Supply | Converts input AC power to internal DC power supplies | 220V | AC | 200 – 250 V RMS, 50 Hz. Input current limiter of 100 mA |
| | | ±5V | DC | Dual Supply Voltage. Tolerance ±0.2 V, Max. 250 mA |
| | | 3V3 | DC | Single Supply Voltage. Tolerance ±0.3 V, Max. 250 mA |
| | | 5V | DC | Single Supply Voltage. Tolerance ±0.2 V, Max. 500 mA |
| Amplifier | Amplifies sensor input signal. • 5000 times amplification • Frequency range 0 – 3 kHz • Signal to Noise Ratio better than 65 dBFS | Power | DC | ±5V, Tolerance ±0.3 V, Max. 200 mA |
| | | Sensor | Analogue | Differential Input Signal. Voltage Range -100 to +100 uV peak. Impedance 50 Ohm |
| | | Out | Analogue | Single Ended Output Signal. Voltage Range –500 to +500 mV peak. Impedance 500 Ohm |

> Slide 12

### Your turn! — ADC og ProcessorBoard

Specificér krav til alle porte markeret med `?`:

| Name of Block | Description of function | Port Name | Type | Port Specification |
|---|---|---|---|---|
| ADC | Analogue to digital converter. • 8 kHz sample rate • 24 bits sample | Power | DC | ? |
| | | In | Analogue | ? |
| | | Out | Serial | SPI or I2S |
| Processor Board | Collects digitized sensor signals and store data in memory when trigger input is high. Possible to transfer sensor data over USB port. • Memory 1 GByte • Processor ADI BF706 | Power | DC | ? |
| | | Sample Data | Serial | SPI or I2S |
| | | Trigger | Digital | ? |
| | | Computer | USB | USB 2.0 |

> Slide 13

### Questions

- Verify that the internal DC power interfaces are correct?
- Can you see any problems with the port specifications?
  - *(Processor Board -> Power:DC - max. 600 mA!)* — PowerSupply 5V:DC leverer max. 500 mA, så en ProcessorBoard der trækker op til 600 mA passer ikke.
- Verify that the amplifier output fits with the ADC input?
- Can you see any problems with the interface?
  - *(ADC -> In:Analouge – 5 Ohm!)* — Amplifier Out har 500 Ohm udgangsimpedans; en ADC-indgang på 5 Ohm belaster den forkert (Rin = Rout er bedst).

> Slide 14

## Interfaces: Specifying in detail

- SysML interface-beskrivelser med flow specifications er "fine".

*Figur: ibd Microprocessor — : Processor og : Memory forbundet med fire porte: rd'/wr, ena, addr[12], data[8]. Spørgsmål: "What can you tell from this IBD? What can't you tell?"*

- Men på et tidspunkt skal interfacet beskrives i komplet og entydig detalje.

> Slide 15

### Specifying in detail: Example (bus)

- Al information om timing osv. mangler i IBD'en, så der skal et timing diagram til for at skabe HW-SW-interfacet.

*Figur: "Interface structure" = ibd Microprocessor (som ovenfor). "Interface timing diagram" = Figure 6.1: A simple bus example: (a) bus structure, (b) read protocol, (c) write protocol. Signaler: rd'/wr, enable, addr, data. Read: rd'/wr lav, enable høj, addr stabil, data gyldig efter t_setup + t_read. Write: rd'/wr høj, enable høj, addr og data stabile, t_setup + t_write.*

> Slide 16

### Specifying in detail: Timing diagram — Your turn (X.10)

Specificér et timing diagram for en X.10-modtager, inkl.:

- The 50Hz power signal P<sub>x</sub>
- An signal that toggles when a zero crossing in 50Hz signal is detected (Z<sub>c</sub>)
- A signal which is active whenever 120kHz signal is detected (Rx)
- Requirements for hold time (T<sub>h</sub>)

*Figur (opgave): 50 Hz sinus med 120 kHz-bursts (1 ms brede) lige efter hver nulgennemgang; 10 ms mellem nulgennemgange.*

*Figur (løsning, slide 18):*

| Signal | Forløb |
|---|---|
| P<sub>x</sub> | 50 Hz sinus; 120 kHz-burst (1 ms) starter ved nulgennemgang; næste nulgennemgang 10 ms senere |
| Zc | Toggler (0→1) ved første nulgennemgang, (1→0) ved næste, (0→1) ved tredje — firkantsignal på 50 Hz |
| Rx | Kort puls (1) mens 120 kHz-burst detekteres, ellers 0 |
| T<sub>h</sub> | Hold time målt fra Zc-flanke til Rx-pulsens afslutning |

> Slide 17–18

### Specifying in detail: Another example: I²C

*Figur: SDA/SCL-timing for en I²C-transaktion.*

| Fase | SDA-indhold | SCL |
|---|---|---|
| START | SDA trækkes lav mens SCL er høj | høj |
| ADDRESS | B6 B5 B4 B3 B2 B1 B0 (7 bit) | 7 clock-pulser |
| R/W | 1 bit | 1 puls |
| ACK | 1 bit fra slave | 1 puls |
| DATA | D7 D6 D5 D4 D3 D2 D1 D0 (8 bit) | 8 pulser |
| ACK | 1 bit | 1 puls |
| STOP | SDA går høj mens SCL er høj | høj |

"What you can't read from this is…" — bl.a. clock-frekvens, setup/hold-tider, spændingsniveauer, pull-up.

> Slide 19

### Specifying in detail: More details

At specificere et hardware-interface i detaljer kræver også en masse andet:

- Physical signals and boundaries
- Inputs and outputs
- Voltage and frequency limits
- Standards
- …

*Figur: udsnit af et datablad (TC Electronic-lignende): System sample rates (192/176.4 via Dual Wire; 96, 88.2, 64, 48, 44.1, 32 kHz), I/O Connectors (XLR, RJ45 TC LINK), Formats AES/EBU (24 bit), Word clock input BNC 75 ohm 0.6–10 Vpp, Analog input: XLR balanced (pin 2+, pin 3−), Impedance 10/3 kOhm (balanced/unbalanced), Selectable full scale input level +9/+15/+21/+27 dBu, Dynamic range > 113 dB, THD+N < −105 dB @ 1 kHz −3 dBFS, Crosstalk < −120 dB, A to D 24 bit dual bit delta sigma @ 4.1/5.6/6.1 MHz.*

> Slide 20

## Hardware architectural design

Tilbage til aktivitetsoversigten — nu HW architectural design.

### HW architectural design — "cookbook"

1. Create a *logical model* of the system (logical blocks)
2. Investigate the *logical* interfaces
3. Create a *HW model* of the system (physical blocks)
4. Allocate the logical blocks to the *physical blocks*
5. Define the *physical* interface between the blocks and to the environment

> Slide 21–22

### bdd: Logical to physical

"Logical functions *allocate* physical components."

*Figur: bdd [Package] Camera Electronics [Structural allocation from logical blocks to HW units].*

| «block» «logical» | «allocate» → | «block» «physical» | Del af |
|---|---|---|---|
| Image Detector | → | CCD Imaging Chipset | Camera PCB |
| MPEG Converter | → | MPEG Chipset | Camera PCB |
| Image Processor | → | Vector Processor | Camera Mother Board |
| Focus Controller | → | Vector Processor | Camera Mother Board |
| | | Control Processor | Camera Mother Board |

«logical» Camera Electronics er komponeret af de fire logiske blokke. «physical» Camera PCB komponerer CCD Imaging Chipset og MPEG Chipset; «physical» Camera Mother Board komponerer Control Processor og Vector Processor. Bemærk: to logiske blokke allokeres til samme fysiske blok (Vector Processor), og Control Processor har ingen logisk blok allokeret i figuren.

> Slide 23

### Counter example — logical blocks and interfaces

*Figur: bdd Counter [logical model]. Counter komponerer (sort diamant) seks logiske blokke. Porte pr. blok:*

| Blok | Ports |
|---|---|
| Counter | in mode: Mode; in range: Range; in edge: Edge; in reset: Reset; out Results: Results |
| User I/F | out mode: Mode; out range: Range; out edge: Edge; out reset: Reset; in formattedMeasData: FormattedData |
| Measure | in cmdMode: Mode; in cmdRange: Range; in cmdEdge: Edge; in masterReset: Reset; in refWindow: Window; in refFreq: Freq; out cmdFreq: Freq; out cmdWindow: Window |
| Data Format and Output | in measuredValueRaw: RawValue; out formattedMeasData: FormattedData |
| Power System | in reset: Reset |
| Time Base | in masterReset: Reset; in cmdFreq: Freq; in cmdWindow: Window {1s…1h} |
| Input | in signal: Signal |

> Slide 24

### Logical structure of your semester project?

Brug et par minutter på at diskutere en logisk struktur for semesterprojektet:

- Logical blocks?
- Logical system interfaces?
- Logical internal interfaces?

> Slide 25

### Eksempler på logiske modeller

**Home Automation** — *ibd Home Automation [simple logical model]*:

| Element | Forbindelse |
|---|---|
| Aktør Bruger | → : Fjern Betjening |
| : Fjern Betjening | → Netværk |
| Netværk | → e1: Enhed, e2: Enhed |
| e1: Enhed | → aktør Kaffemaskine |
| e2: Enhed | → aktør Lys |

**ECG Monitor** — *ibd ECG Monitor [logcal model]*:

| Element | Forbindelse |
|---|---|
| Aktør Patient | → : ECG Sensor |
| : ECG Sensor | → : Amplifier |
| : Amplifier | → : ADC |
| : ADC | → : Display ECG Graph, : ST Elevation Alarm |
| : Display ECG Graph, : ST Elevation Alarm | → aktør Nurse |

> Slide 26–27

### Mapping logical blocks to physical blocks

- Fra den logiske systemstruktur og kravene¹ udledes en HW-arkitektur med et passende sæt HW-blokke
  - Processors, peripherals, buses, etc.
- Derefter allokeres (iterativt) de logiske blokke, dvs. *funktioner*, til de fysiske blokke, dvs. *hardware*.

¹ Og fra tilgængelig, påkrævet eller ønsket HW, erfaring, cost, og en stribe andre kilder…

> Slide 28

### Mapping logical blocks to physical blocks — «allocates»

*Figur: bdd Counter [Logical-to-physical allocation]. Venstre: Counter komponerer de logiske blokke. Højre: Counter komponerer de fysiske blokke. Stiplede «allocate»-pile:*

| Logisk blok | «allocate» → fysisk blok |
|---|---|
| User I/F | Display, Front Panel Controls |
| Data Format and Output | Microprocessor |
| Power System | Power System |
| Time Base | Counter – Divider Chain and Control |
| Input | Counter – Divider Chain and Control |
| Measure | Counter – Divider Chain and Control, Clock System |

(Præcis hvilke af Time Base/Input/Measure der peger på Counter – Divider Chain and Control hhv. Clock System er svært at aflæse; tabellen viser bedste læsning. Fysiske blokke: Display, Front Panel Controls, Microprocessor, Power System, Counter – Divider Chain and Control, Clock System.)

> Slide 29

### Physical structure of your semester project?

- Diskutér en fysisk struktur for semesterprojektet: Physical blocks?
- Diskutér derefter hvordan de logiske blokke mapper til de definerede fysiske blokke.

> Slide 30
