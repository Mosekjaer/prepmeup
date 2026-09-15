# 11 — Fra applikationsmodel til implementation (L22–L23)

Modulet dækker lektionerne **L22 Application Models og Implementation** og **L23 Application Model og Implementation II** i SWISE-01 Indledende System Engineering (Brightspace-modul "L22-L23 - Fra Applikationsmodel til implementation"). Underviser er ikke angivet i materialet (kodeforfatter i Atmel Studio-projekterne: `au237297`; MSYS-driverne `led`/`switch` er af Henning Hargaard).

Ét gennemgående eksempel: et digitalt **minutur** (start/stop/reset, display mm:ss). L22 tager applikationsmodellen og omformer den til noget, der kan implementeres på en Arduino (ATmega2560) — i C, i C++ med polling og i C++ med interrupts. L23 gør det samme som PC-applikation i C#/WPF. Pointen i begge lektioner: controller-klassen Ur og dens tilstandsmaskine er uændret; det er boundary-klasserne og "hovedprogrammet", der tilpasses platformen.

## Filer

| Fil | Kilde | Type | Sider | Indhold |
|---|---|---|---|---|
| `uml-light-ur.md` | `UML-Light-Ur.pdf` | eksempel | 12 | Kapitel 5 af UML-Light (IHA 2003): minutur-eksemplet fra beskrivelse → klassediagram m. ansvar → STM → SD → C++ og C implementation (ét/flere objekter pr. klasse) |
| `applikationsmodel-minutur.md` | `ApplicationModel.pdf` | eksempel | 3 | Applikationsmodellen for minuturet med «boundary»/«controller»: klassediagram, STM for Ur, SD med aktør |
| `implementation-minutur-arduino.md` | `ImplementationIntermediate.pdf` + `ImplementationFinal.pdf` | løsningsforslag (L22) | 3 + 3 | Omformning af applikationsmodellen til polling-implementation i to trin: Hovedprogram tilføjes, boundary-klasser får `checkForTast`/`checkTast`/`checkForTimeout`, SD med loop/opt/alt |
| `implementation-minutur-gui-wpf.md` | `ImplementationFinalGUI.pdf` | løsningsforslag (L23) | 4 | Applikationsmodellen tilpasset WPF: MainWindow, Button/TextBox/DispatchTimer wrappet i boundary-klasser, event-drevet SD |
| `kode-minutur-c.md` | `MinutUrC.zip` | eksempel (kode) | 9 filer | Ren C på ATmega2560 med MSYS-drivere (`led`, `switch`), `static`-moduler, Timer 1 |
| `kode-minutur-cpp.md` | `MinutUrCpp.zip` | eksempel (kode) | 9 filer | C++ klasser 1:1 med `ImplementationFinal`, polling |
| `kode-minutur-cpp-interrupt.md` | `MinutUrCppInt.zip` | eksempel (kode) | 9 filer | Samme C++ men `timeout` via `ISR(TIMER1_OVF_vect)`; kun `main.cpp` og `Timer.cpp` ændret |
| `kode-minutur-wpf.md` | Visual Studio solution `MinuturWPF` (L23) | eksempel (kode) | 7 filer | C#/WPF: `MainWindow`, `Boundary/DisplayBoundary`, `Boundary/TimerBoundary`, `Control/Ur` |

Anbefalet læserækkefølge: `uml-light-ur.md` → `applikationsmodel-minutur.md` → `implementation-minutur-arduino.md` → `kode-minutur-cpp.md` (→ `-c.md`, `-cpp-interrupt.md`) → `implementation-minutur-gui-wpf.md` → `kode-minutur-wpf.md`.

## Brightspace-sider

### L22 Application Models og Implementation

**Indhold**

Demonstrere, hvordan man kan komme fra de omhyggeligt udarbejdede Applicationsmodeller til at lave noget kode, der implementerer funktionen for systemet.

I denne lektion, L22, tage udgangspunkt i et lille digitalt ur, som vil være perfekt at implementere på en Arduino, det microcontroller system nogle af jer brugte i sidste semester til MSYS, og som I også skal bruge til jeres semesterprojekt.

Alle kan komme ud for, at skulle lave embeddede systemer på en µ-processor!

**Materials**

- Læs den eksempel og vi kigger gennem den sammen i klassen (fra Applikationmodel til implementation — Hvordan): `UML-Light-Ur.pdf`
- Læs også denne Application Model Eksampler fra UML-Light-Ur: `ApplicationModel.pdf`
- Referencer om interrupts her: https://da.wikipedia.org/wiki/Interrupt (Google selv andre forklaringer, hvis det ikke er nok.)

**Løsninger:** Kan ses på Brightspace efter forelæsningen.

### L22 Løsningsforslag

- Intermediate step: `ImplementationIntermediate.pdf`
- Final: `ImplementationFinal.pdf`
- Code example: `MinutUrC.zip`, `MinutUrCppInt.zip`, `MinutUrCpp.zip`

Her finder I de omformede applikationsmodeller frem imod at lave en implementation, der understøtter specielt boundaryklasserne.

Der er også vedhæftet Atmel Studio løsninger for C++ implementationer med og uden anvendelse af interrupts, og en ren C implementation, der anvender de moduler, E og SW kender fra MSYS i sidste semester.

### L23 Application Model og Implementation II

**Indhold**

Fortsættelse fra lektion L22. Der anvendes den samme opgave og applikationsmodel.

Viser hvordan man kan implementere minuturet i en GUI, ud fra applikationsmodellen.

På samme måde, at E'erne og SW'erne kan lære noget af dette. Alle kan komme ud for at skulle lave et GUI!

**Lektion 23**

I lektion L23 vise, hvordan man kan gøre det samme med en PC applikation, programmeret i C#.

### L23 Løsningsforslag

Diagrammerne tilpasset en GUI løsning (bare for eksampel): `ImplementationFinalGUI.pdf`

Den tidligere løsning var baseret på Windows Forms, nu er der lavet en løsning på WPF (Windows Presentation Foundation) — bruge af C# og .NET framework.

### Table of Contents (Brightspace-modulet)

1. L22 Application Models og Implementation
2. L23 Application Model og Implenentation II
3. L22 Løsningsforslag
4. L23 Løsningsforslag
5. L23 Visual Studio solution med implementation

## Ikke konverteret

- `MinutUrWPF/Properties/*`, `App.config`, `.csproj`/`.sln`, `.cppproj`, `.gitignore`, `.tfignore` — projekt-/build-filer uden fagligt indhold (jf. opgavebeskrivelsen).
- Den ældre Windows Forms-løsning, som L23-siden nævner, findes ikke i materialet.
