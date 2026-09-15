# Embedded Systems Overview (Vahid & Givargis, kap. 1, s. 1–11)

## Metadata

| Felt | Værdi |
|---|---|
| **Kursus** | SWISE-01 Indledende System Engineering (SW4PRJ4-02) |
| **Kilde** | Frank Vahid & Tony Givargis, *Embedded System Design: A Unified Hardware/Software Introduction*, Wiley — Chapter 1 *Embedded Systems Overview*, pp. 1–11 |
| **Kilde-fil** | ISE Book-kompendium: `chapters/12_Vahid_Ch1_EmbeddedSystemsOverview.tex` |
| **Type** | lærebogskapitel |
| **Sprog** | engelsk (bogtekst ikke oversat; figurbeskrivelser og noter på dansk) |
| **Emner dækket** | Definition af embedded system, design metrics (NRE cost, unit cost, size, performance, power, flexibility, time-to-market, maintainability, correctness, safety), total cost = NRE + unit cost × antal, cost vs. volume, time-to-market, response time vs. throughput, metric-tradeoffs, processor technology, single-purpose / general-purpose / application-specific processors |

Note om transskriptionen: Kilden er en LaTeX-transskription af fotograferede bogsider. Transskriptionen slutter i afsnit 1.3 (Processor Technology); 1.4–1.9 er ikke med. Figurer er TikZ-rekonstruktioner og gengives som prosa + tabel. Intet er rettet eller opfundet.

---

**Kapitlets indhold (bogens egen oversigt):**

| Afsnit | Titel |
|---|---|
| 1.1 | Embedded Systems Overview |
| 1.2 | Design Challenge – Optimizing Design Metrics |
| 1.3 | Processor Technology |
| 1.4 | IC Technology |
| 1.5 | Design Technology |
| 1.6 | Tradeoffs |
| 1.7 | Summary and Book Outline |
| 1.8 | References and Further Reading |
| 1.9 | Exercises |

## 1.1 Embedded Systems Overview

Computing systems are everywhere. Millions of computing systems are built every year, and they are found in products such as automobiles, consumer electronics, home appliances, toys, industrial controllers, medical equipment, and network infrastructure. Some of these systems are obvious to users; many more are hidden inside the larger product that they control.

An *embedded system* is a computer system that is part of a larger system and that performs some dedicated function within that larger system. The embedded system may interact with the physical world through sensors and actuators, may provide a user interface, or may be almost invisible to the user. In many cases, the embedded system has real-time requirements.

Examples include cell phones, pagers, digital cameras, camcorders, video game consoles, microwave ovens, calculators, home security systems, washing machines, printers, and automobiles. Figure 1 shows a sample of such devices.

*Figur 1: A sample of embedded systems.*

Elleve afrundede bokse i et gitter (ingen forbindelser): Automobile, Phone, Camera, Printer, Washer, Microwave, Game console, Medical device, Router, Toys, Controller.

Embedded systems are usually designed for a particular application, often under tight constraints on cost, size, power, performance, and time-to-market. They are also commonly produced in large quantities. The result is that many design decisions are driven less by elegance than by competing engineering metrics.

## 1.2 Design Challenge – Optimizing Design Metrics

The design of an embedded system is a tradeoff among several metrics. Improving one metric often worsens another. A smaller system may cost more. A faster system may use more power. A cheaper system may take longer to build or be less flexible.

*Figur 2: Design metric competition – improving one metric may worsen others.*

En cirkel mærket *Design metrics* i midten med fire pile pegende ind mod cirklen fra hver sin retning: *Performance* (fra venstre), *Size* (fra øverst højre), *Power* (fra nederst højre), *NRE cost* (nedefra). Figuren illustrerer at metrikkerne trækker i hver sin retning.

### Common Design Metrics

The most common metrics are the following.

- **NRE cost**: the nonrecurring engineering cost. This is the one-time monetary cost of designing the system. Once the system is designed, additional units can be produced without incurring that design cost again.
- **Unit cost**: the monetary cost of manufacturing a single copy of the system, excluding NRE cost.
- **Size**: the physical space required by the system, often measured in gates or transistors for hardware and bytes for software.
- **Performance**: the execution time of the system, or the speed at which it completes its task.
- **Power**: the amount of power consumed by the system.
- **Flexibility**: the ability to change the functionality of the system after initial release.
- **Time-to-market**: how long it takes to bring the system to market.
- **Maintainability**: how easy it is to modify the system after release, especially for bug fixes or new features.
- **Correctness**: the degree to which the system meets its specification.
- **Safety**: the degree to which the system avoids harm.

The total cost of a product is often modeled as

> total cost = NRE cost + (unit cost × # of units).

This makes the volume of production an important design parameter. At low volume, a design with low NRE cost is often preferable. At high volume, a design with low unit cost can dominate even if the NRE cost is larger.

*Figur 3: Total cost versus volume for three technologies.*

Graf med x-akse *Number of units* og y-akse *Total cost*, overskrift "Cost versus volume", og tre linjer:

| Linje | Skæring med y-aksen (NRE cost) | Hældning (unit cost) | Tegnet forløb i transskriptionen |
|---|---|---|---|
| Technology A | lavest | stejlest | stiger fra lav startværdi til højeste slutværdi (knæk ca. en fjerdedel inde) |
| Technology B | mellem | mellem | stiger moderat (knæk knap halvvejs) |
| Technology C | højest | lavest | tegnet som en ret linje fra højeste startværdi til laveste slutværdi |

Desuden en vandret stiplet referencelinje. Bemærk: linjen for Technology C er i TikZ-rekonstruktionen tegnet *faldende*; teksten beskriver C som højest NRE cost og lavest unit cost, dvs. i originalen en stigende linje med lavest hældning. Tolkning: A er billigst ved lav volumen, C ved høj volumen, B i mellemområdet.

For example, if technology A has a low NRE cost but a high unit cost, it is best for low volumes. If technology B has intermediate values, it may be best over a medium range. If technology C has the highest NRE cost but the lowest unit cost, it becomes best when volume is high enough.

Time-to-market is a separate concern. A product that reaches the market earlier can generate revenue sooner and can avoid losing market share to competitors. Even a technically better design may fail commercially if it arrives too late.

*Figur 4: Time-to-market can dominate product success.*

Graf med x-akse *Time* og y-akse *Revenue / cost*. To kurver: *On-time entry* stiger stejlt fra et tidligt tidspunkt og flader derefter ud på et højt niveau; *Delayed entry* ligger fladt på et lavt niveau i lang tid og stiger først senere. En lodret stiplet linje mærket *launch* markerer et tidspunkt mellem de to stigninger.

### Performance Metrics

Performance is usually discussed in terms of response time and throughput. Response time is the time between the start of a task and the completion of that task. Throughput is the number of tasks completed per unit time. In many embedded systems, response time is the primary concern, because a task must complete within a deadline.

### Design Challenge – Improving One Metric May Hurt Another

The central design challenge is to balance the competing metrics for the target product and market. A smaller system may require more expensive components. A faster system may consume more power. A more flexible system may reduce performance. A lower-cost system may increase time-to-market or reduce maintainability. The right solution depends on the specific application.

## 1.3 Processor Technology

We can define *technology* as a manner of accomplishing a task, especially using technical processes, methods, or knowledge. This chapter uses three broad technology categories for embedded systems: processor technology, IC technology, and design technology. These categories are interrelated, but processor technology is the starting point for describing many embedded systems.

Processor technology refers to the architecture of the computation engine used to implement a system's required functionality. Some systems use a processor that is highly programmable. Others use a very specialized processor or a custom digital circuit. The choice is driven by the required combination of performance, power, size, flexibility, and cost.

### Processor Types

The book distinguishes three useful processor types:

- **Single-purpose processors**: processors or circuits designed for one task.
- **General-purpose processors**: programmable processors that can run many different applications.
- **Application-specific processors**: processors tailored to a family of related applications.

Single-purpose designs can be very fast and compact, but they are inflexible. General-purpose designs are flexible and inexpensive to reuse, but they may waste area, power, or execution time. Application-specific processors occupy the middle ground.

*Figur 5: Different processor types.*

Tre blokdiagrammer side om side. Alle tre har en *Controller*-boks til venstre forbundet til en *Datapath*-boks (som indeholder *Registers* og *ALU*) og en *Data memory*-boks under Datapath.

| | Single-purpose | General-purpose | Application-specific |
|---|---|---|---|
| Controller | ja | ja | ja |
| Datapath (Registers, ALU) | ja | ja | ja |
| Data memory | ja | ja | ja |
| Program memory | — | ja | ja |
| I/O | — | ja | ja |

I TikZ-rekonstruktionen er General-purpose- og Application-specific-diagrammerne identiske (Controller, Datapath, Data memory, Program memory, I/O); Single-purpose mangler Program memory og I/O.

General-purpose processors are the familiar CPUs used in desktop computers and many embedded systems. Their strength is flexibility. They execute a wide variety of programs, and the same hardware can be reused across many products. Their weakness is that they may not be optimal for any one product.

Single-purpose processors implement a single function. In practice, such a design may be written in hardware as a digital circuit rather than as a software-programmed processor. The advantage is performance and efficiency. The disadvantage is that the functionality is fixed.

Application-specific processors are a compromise. They may be programmable, but they are tailored to a particular class of applications. Such processors offer better performance or lower power than a general-purpose processor while retaining some flexibility.

*(transskription: kildens afsluttende note — "The following subsections in the source continue with IC technology and further design technologies. The photographed pages supplied for this transcription end during the processor-technology discussion." Afsnittene 1.4–1.9 er derfor ikke med.)*
