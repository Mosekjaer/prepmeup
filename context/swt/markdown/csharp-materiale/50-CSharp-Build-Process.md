---
title: "C# Build Process"
source: "csfiles/home_dir/Introduction/C#BuildProcess.pptx"
modul: "C# materiale"
pages: 7
type: "slides"
vision: "done"
note: "pptx konverteret via LibreOffice"
---

# C# Build Process

<!-- side 1 -->

C# Build og Test?
  Hvad sker der under overfladen?

<!-- side 2 -->

Første skridt: Compile
                                                                                  Intermediate
                                                                                    Language




• Computeren forstår ikke C# kode, som bare er tekst. Det skal
  oversættes – compiles – til noget computeren forstår.
• Her er første skridt.

Billeder lånt fra codeeasy.net: https://codeasy.net/lesson/c_sharp_compilation_process
Der er også yderligere forklaringer.

**Figur:** Vandret flowdiagram på lysegul baggrund med tre kasser forbundet af sorte højrepile:

`Your C# code` (grøn kasse, indeholder en stiplet underkasse med filnavnet **MyProgram.cs**) → `C# compiler` (grå kasse) → `IL code` (blå kasse, indeholder en stiplet underkasse med **MyProgram.exe** og **MyProgram.dll**).

En blå callout-boble med hvid tekst, "Intermediate Language", peger ned på `IL code`-kassen og forklarer forkortelsen IL.

Hele diagrammet er altså: kildekode i tekstform → compiler → IL-kode pakket i en .exe eller .dll.

<!-- side 3 -->

Compile time og run time
                                             Just in Time




• Når programmet skal køre, sker der yderligere et skridt, for at
  computeren kan køre din kode.
• Dette skridt er (så godt som) usynligt - hvis det går godt!

**Figur:** Samme vandrette flowdiagram som forrige slide, nu udvidet med runtime-delen. Fem kasser på lysegul baggrund, forbundet af grå højrepile:

`Your C# code` (grøn, **MyProgram.cs**) → `C# compiler` (grå) → `IL code` (blå, **MyProgram.exe** / **MyProgram.dll**) → `JIT compiler` (grå) → `Native code` (rød, stiplet underkasse med hex-bytes `21 0a 00 00` / `0c 10 00 06`).

En blå callout-boble, "Just in Time", peger ned på `JIT compiler`-kassen.

Under kasserne markerer to vandrette klammer faserne: den venstre klamme spænder fra `Your C# code` til og med `IL code` og er mærket **Compile time**; den højre klamme spænder fra `JIT compiler` til `Native code` og er mærket **Runtime**. Skillelinjen ligger altså mellem IL-koden og JIT-compileren.

<!-- side 4 -->

Start vs. Build

                   Build – Build Solution
                   (Ctrl+Shift+B)
                        Compiletime

Debug – Start …
(F5 el. Ctrl+F5)




                          Runtime

**Figur:** To sammenkoblede elementer: et flowchart til højre og et klamme-hierarki til venstre, der binder Visual Studio-kommandoer til flowchartets faser.

Flowchart (lysegule kasser med gul kant, pile nedad):
1. `You write C# code`
2. `You run C# compiler`
3. Rombe: `Are there any errors?` — grenen **Yes** går ud til højre og løber op langs siden tilbage til kasse 1 (`You write C# code`), altså en rettelsesløkke. Grenen **No** går nedad.
4. `You get **exe** or **dll** file`
5. `You run your **exe** file`
6. `**JIT** compiler compiles **IL** code from your **exe** file into **native code** which instantly executes by processor`

Venstre side: en stor sort klamme spænder om hele flowchartet og er mærket med den blå kasse `Debug – Start … (F5 el. Ctrl+F5)` — F5 dækker altså hele forløbet.

Inden i den, to mindre klammer:
- Den øverste klamme omslutter trin 1–4 (skriv kode → compiler → fejltjek → exe/dll) og er mærket med den blå kasse `Build – Build Solution (Ctrl+Shift+B)` samt en orange kasse `Compiletime`.
- Den nederste klamme omslutter trin 5–6 (kør exe → JIT til native code) og er mærket med en orange kasse `Runtime`.

Pointen: Ctrl+Shift+B dækker kun compiletime-delen, mens F5 kører hele kæden inklusive runtime.

<!-- side 5 -->

A program is not an island!
                                         Compile time
• Ved Compile Time checkes
  koden imod alle referencer
  (using …;).

• Ved Run Time skal alle disse
  være tilstede, for at det kan
  køre (fx ved Test)




                                            Run time

**Figur:** Lodret blokdiagram over hele kæden fra projekt til operativsystem, med to farvede ovaler lagt hen over for at markere faserne.

Øverst en lyseblå gruppe mærket `Visual C# Project` (med et mappeikon): den indeholder `C# Source File(s)` til venstre og `Resources` samt `References` stablet til højre. Sorte pile fra både `C# Source File(s)` og `References` peger ned i `C# Compiler`.

Fra `C# Compiler` går en pil nedad, mærket **Creates**, til kassen `Managed Assembly (.exe or .dll) MSIL Metadata`.

Derfra en pil nedad mærket **IL metadata & references loaded by CLR** til en lyseblå gruppe mærket `.NET Framework`, som indeholder `Common Language Runtime — Security / Garbage Collection / JIT Compiler`. Fra denne kasse går en pil til højre mærket **Uses** til `.NET Framework Class Libraries`.

Fra CLR-kassen går en pil nedad mærket **Converted to native machine code** til `Operating System`.

Oven på diagrammet er tegnet to store afrundede ovaler: en blå oval, mærket **Compile time**, omkranser Visual C# Project, C# Compiler og Managed Assembly; en rød oval, mærket **Run time**, omkranser Managed Assembly, .NET Framework/CLR og Operating System. De to ovaler overlapper netop ved `Managed Assembly` — assemblyen er både compilerens output og runtimens input.

<!-- side 6 -->

Ét klassebibliotek – flere anvendelser

  Calculator.App.cs          Calculator.cs           Calculator.Test.Unit.cs




                                   Compile




 Calculator.App.exe   uses   Calculator.dll   uses   Calculator.Test.Unit.dll




                               Assemblies

**Figur:** Blokdiagram i to rækker med et compile-trin imellem.

Øverste række, tre blå kasser: `Calculator.App.cs`, `Calculator.cs`, `Calculator.Test.Unit.cs`. Fra hver går en tynd blå pil nedad gennem en bred vandret blå bjælke mærket `Compile` og videre ned til den tilsvarende kasse i nederste række.

Nederste række, tre blå kasser: `Calculator.App.exe`, `Calculator.dll`, `Calculator.Test.Unit.dll`.

To tykke sorte pile peger vandret ind mod midten, begge mærket **uses**: fra `Calculator.App.exe` mod `Calculator.dll`, og fra `Calculator.Test.Unit.dll` mod `Calculator.dll`. Både applikationen og unit-testene bruger altså samme klassebibliotek.

Nederst en grøn kasse mærket `Assemblies` med tre grønne pile op til hver af de tre kasser i nederste række — alle tre outputfiler er assemblies.

<!-- side 7 -->

.Net Platform: Framework el. bare .Net
                                           Core

• .Net er fremtidens                  .Net Framework

  platform
• Man kan ikke konvertere
  sit projekt på en nem
  måde
• Brug .Net 6 eller senere
  (kræver VS 2022)             Core

• Opdater altid din aktuelle                                   Core


  Visual Studio!
• .Net ændrer sig hele tiden
  - der kan stadig være                        Windows Only!      Linux
  problemer på SWT                                                MacOS
                                                                  iOs & Android

**Figur:** Samme .NET-blokdiagram som på side 5 (Visual C# Project → C# Compiler → Managed Assembly (.exe or .dll) MSIL Metadata → .NET Framework med Common Language Runtime (Security / Garbage Collection / JIT Compiler), der **Uses** .NET Framework Class Libraries → Operating System), men her overtegnet med røde rettelser, der opdaterer det fra .NET Framework til .NET Core:

- Ved projektgruppens label `.NET Framework` er ordet "Framework" streget over med rødt og erstattet med håndskrevet **Core**.
- Samme rettelse ved runtime-gruppens label `.NET Framework`: overstreget, erstattet af **Core**.
- Samme rettelse ved `.NET Framework Class Libraries`: "Framework" overstreget, erstattet af **Core**.
- Nederst ved `Operating System` er teksten "Windows Only!" streget over med rødt og erstattet med listen **Linux**, **MacOS**, **iOs & Android**.

Rettelserne udtrykker skiftet fra Windows-bundet .NET Framework til krydsplatform-.NET Core.

