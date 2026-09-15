# Garbage Collection (Managed Heap)

## Metadata

- **Lektion:** L05.4 – Garbage Collection, AKA Managed Heap
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L05/Garbage Collection.pdf (12 slides)
- **Emner dækket:**
  - Hukommelsesproblemer: memory leak og memory corruption
  - Managed heap og `NextObjPtr`
  - Mark and Compact-algoritmen
  - Roots og marking-fasen
  - Compacting-fasen
  - Generations og generational GC
  - Værktøjer til at monitorere GC
  - Performance: er C# langsom?

---

## 1. Memory overview

Ethvert program bruger memory — files, memory buffers, screen space, network connections, database resources og så videre. En klasse identificerer en form for resource.

Det sædvanlige programmeringsparadigme:

1. Allokér memory til resourcen (`new`-operatoren)
2. Initialisér memory så resourcen kan bruges (constructor)
3. Brug resourcen (tilgå members) — gentag efter behov
4. Riv resourcen ned (destructor)
5. Frigiv memory

De almindelige problemer:

- At glemme at frigive memory → **memory leak**
- At bruge memory efter frigivelse → **memory corruption**

Disse bugs er værre end de fleste andre bugs, fordi konsekvens og timing er uforudsigelige.

## 2. Løsningen: Garbage Collection

Garbage Collection (GC) gør disse bugs til fortid.

- Alle reference types allokeres på den **managed heap**.
- Din kode frigiver aldrig et objekt selv.
- GC'en frigiver objekter, når de ikke længere er reachable.
- Hver process får sin egen managed heap — en region i virtual address space.
- `new`-operatoren allokerer altid objekter i enden af heapen.
- Hvis heapen er fuld, sker der en GC. I virkeligheden: GC sker, når generation 0 er fuld, eller når processoren har idle time (background GC).

Positionen for næste allokering holdes i pointeren `NextObjPtr`.

## 3. GC-algoritmen — overblik

**Allokering af objekter:**

- `new` allokerer altid objekter i enden af heapen.
- Næsten lige så hurtigt som en stack-allokering.
- Meget hurtigere end unmanaged `new`/`malloc`/`HeapAlloc`.
- Large objects allokeres fra en særlig heap:
  - Large objects flyttes ikke i memory (kan dog gøre det under visse omstændigheder).
  - Large objects er >= 85.000 bytes (kan ændre sig).

**Garbage Collection:**

- **Marking:** objekter, der refereres af appens variable (the Roots), markeres.
- **Compacting:** markerede objekter flyttes ned over umarkerede objekter.
- Hvis alle objekter er markeret, sker der ingen compacting — `new` kaster `OutOfMemoryException`.

Algoritmen kaldes **"Mark and Compact"**.

## 4. Roots og marking-fasen

Når en garbage collection starter, betragtes alle objekter på heapen som garbage.

Marking-fasen:

1. Objekter, der er reachable fra Roots, markeres.
   - En **Root** er en memory-lokation, der kan referere til et objekt (eller være null):
     - Static fields defineret i en type
     - Arguments sendt til en metode
     - Local variables deklareret i en metode
     - CPU-registre (enregistered fields, arguments eller variable)
   - Roots er altid reference types, aldrig value types.
   - Hver metode har en root table, produceret af JIT-compileren.
2. Hvert markeret objekt får sine fields tjekket; disse objekter markeres også, rekursivt.
3. GC'en går op ad trådens call stack og bestemmer roots for de kaldende metoder ved at tilgå hver metodes root table.
   - Allerede markerede objekter springes over. Det forbedrer performance og forhindrer uendelige løkker på grund af cirkulære referencer.
   - Static fields tjekkes; de objekter markeres også.

Slidesene viser heapen "before a collection" og "after a collection" som diagrammer over objekter i heapen, hvor de umarkerede objekter er forsvundet efter collection.

## 5. Compacting-fasen

Compacting-fasen komprimerer de markerede objekter:

- Markerede objekter flyttes ned med en simpel memory copy.
- Der opstår ingen address space-fragmentering, i modsætning til den unmanaged heap.
- Hver root opdateres til at pege på objektets nye memory-adresse.
- Efter compacting placeres `NextObjPtr`-pointeren efter det sidste overlevende objekt.

## 6. Generations

Generational GC bygger på antagelser om din kode:

- Jo nyere et objekt er, desto kortere bliver dets levetid.
- Jo ældre et objekt er, desto længere bliver dets levetid.
- Nye objekter har en stærk relation og tilgås sammen.

Undersøgelser viser, at antagelserne holder for mange apps.

Generational GC forbedrer performance ved kun at collecte nye objekter:

- Gamle objekter markeres ikke og gennemgås ikke rekursivt.
- Kun nye overlevende objekter komprimeres.
- Kun nye objekters roots skal opdateres.

## 7. Værktøjer til at monitorere GC'en

- **PerfMon** — grafer over mange .NET-relaterede objects/counters. Følger med Windows.
- **CLR Profiler** — viser objekter efter most-allocated og size, samt function call graphs, loaded types m.m. Søg efter "Writing High-Performance Managed Applications: A Primer" af Gregor Noriskin.
- Kommercielle produkter som Red Gates ANTS Profiler (http://www.red-gate.com/) og dotMemory fra JetBrains (https://www.jetbrains.com/dotmemory/).

## 8. Er C# langsom?

Det afhænger stærkt af algoritmen.

Generelt kan man forvente, at C#-programmer kører cirka 10 % langsommere end C++-programmer på grund af garbage collections og range checks på arrays m.m.

Men head-to-head benchmarks C++ vs. .NET viser, at C# nogle gange performer bedre og nogle gange er 2–3 gange langsommere end C++.

Reference: http://www.codeproject.com/KB/cross-platform/BenchmarkCppVsDotNet.aspx

## 9. Referencer og links

- Garbage collection — https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/
