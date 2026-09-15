# Uge 10 — Threading in C#

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Uge 10 — Threading in C# (selvstudie før uge 11) |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Forelæser** | Michael Loft (ml@ase.au.dk) |
| **Kilde** | `11 Threading in C#.pdf` (59 slides) |
| **Sprog/kode** | C# |
| **Emner dækket** | Concurrency, data- vs. functional parallelism, Amdahl's Law, multithreading-arkitektur i WIN32 og .NET, `Thread`-klassen, start/join, parameteroverførsel, `Thread.Sleep`/yield, foreground vs. background threads, stop og abort, exception handling i tråde, thread priorities, thread pool, `ThreadPool.QueueUserWorkItem`, asynchronous delegates, Task Parallel Library, `async`/`await`, `lock`, `Monitor`, `Mutex`, `Semaphore`, event wait handles |

---

## Agenda

Slide 2 lister lektionens indhold:

1. Concurrency
2. Amdahl's Law
3. Multithreading architecture in WIN32
4. Creating and starting threads*
5. Suspending, resuming and stopping threads*
6. Background and foreground threads
7. Thread priorities
8. The .Net thread pool
9. Thread synchronization

> \* Covered before class
>
> Slide 2

---

## 1. Concurrency — hvad og hvorfor

> "Things happening at the same time.
>
> (or switching tasks so fast, that we get the illusion of things happening at the same time)"
>
> Slide 3 (gentaget på slide 6)

Definitionen er bevidst løs: concurrency handler om, at flere ting er i gang samtidig — enten reelt parallelt på flere cores, eller ved at OS'et skifter mellem opgaver så hurtigt, at det ser sådan ud.

Slide 4 introducerer spørgsmålet "Concurrency – Why?" (billede: Alice der falder ned i kaninhullet). Slide 5 er en Mentimeter-afstemning:

> Go to www.menti.com — Use the code **93 71 67** — "Why Concurrency?"
>
> Slide 5

Ordskyen fra de 23 svar samler bl.a.: *responsiveness*, *not blocking*, *parallelisering*, *optimering*, *hurtighed*, *speed*, *bedre performance*, *ressourceudnyttelse*, *data integrity*, *udnyt hardware multicore*, *langsomme io opgaver*, *lille belastning af gui*, *the free lunch is over*.

---

## 2. Different types of concurrency

Slide 7 er en sektionsoverskrift: "Different types of concurrency".

### To typer parallelisme

- **Data Parallelism** — "Concurrency comes from performing the same calculation on different data elements *simultaneously*"
- **Functional Parallelism** — "Concurrency comes from working on independent functional tasks *simultaneously*"

> Slide 8

Data parallelism er fx den samme filtrering af hver pixel i et billede. Functional parallelism er fx at hente data fra netværket, mens UI'et tegnes.

### De tre varianter af functional parallelism

1. Minimizing latency in reacting to events
2. Preventing slow I/O from blocking the main thread
3. Maximizing throughput on multicore systems

> Slide 9

Bemærk, at kun den tredje handler om at gøre programmet hurtigere. De to første handler om responsivitet — de giver mening selv på en enkelt core.

### The future belongs to Multi-core CPUs

- Increasing the CPU frequency is hard.
- Performance improvements → multi-core CPU designs.
- In Herb Sutters words: **The free lunch is over!**
- Multicore HW requires concurrent SW!

> Slide 10

Illustrationen viser en "Dual CPU Core Chip" med to blokke "CPU Core and L1 Caches", der begge er forbundet til en fælles "Bus Interface and L2 Caches".

Pointen: man får ikke længere gratis performance ved at vente på næste CPU-generation. Skal programmet være hurtigere på ny hardware, skal det selv være concurrent.

---

## 3. Amdahl's Law for parallelization

Slide 11 er sektionsoverskriften "Ahmdal's law for parallelization" (stavet sådan på sliden).

### Hvad begrænser gevinsten?

What limits performance gain through parallelization?

- Size of the part of the program that can be parallelized
- Number of CPU cores
- Context switching (not considered by Ahmdal's law!)

Which is most impeding to performance gain?

> "Ahmdal's law … provides a *theoretical upper bound* on overall system speed-up when part of the system is parallelized using multiple processors"
>
> Slide 12

### Formlen

P = fraction of the algorithm that can be parallelized
N = number of CPU cores times.

$$
\text{Overall speedup} = S(N) = \frac{1}{(1-P) + \frac{P}{N}}
$$

Eksempler fra sliden:

| Tilfælde | P | N | S(N) | Performance gain |
|---|---|---|---|---|
| 30 % af algoritmen paralleliseres på quad-core | 0,3 | 4 | 1,290 | 29 % |
| 90 % af algoritmen paralleliseres på dual-core | 0,9 | 2 | 1,818 | 82 % |

Og det åbne spørgsmål: As N→∞, S(N) →?

> Slide 13

Svaret er 1/(1−P): den sekventielle del sætter loftet. Med P = 0,9 kan man aldrig komme over en faktor 10, uanset hvor mange cores man kaster efter problemet.

### Cores ain't all

Grafen på slide 14 plotter speedup mod antal processorer (1 til 65536, logaritmisk) for parallel portion 50 %, 75 %, 90 % og 95 %. Kurverne flader ud på henholdsvis ca. 2, 4, 10 og 20 — præcis 1/(1−P). Ud over nogle hundrede cores giver flere cores praktisk talt intet.

> Slide 14

---

## 4. Multithreading-arkitektur

Slide 15 er sektionsoverskriften "WIN32 architecture".

### WIN32

- Data in one process cannot be accessed from another process.
- Threads in a process share data.

Diagrammet viser "A Single Win32 Process" med et fælles felt **Shared Data** øverst og to tråde nedenunder, Thread A og Thread B, som hver indeholder **TLS** og en **Call Stack**.

> TLS: Thread Local Storage
>
> Slide 16

| Element | Delt eller privat |
|---|---|
| Shared Data (procesens data) | Delt mellem alle tråde i processen |
| Call Stack | Privat pr. tråd |
| TLS (Thread Local Storage) | Privat pr. tråd |
| Data i en anden proces | Utilgængeligt |

Det er præcis derfor, race conditions overhovedet er mulige: trådene deler heap'en, men har hver sin stak.

### .NET og application domains

> "*Application domains* provide isolation, unloading, and security boundaries for executing managed code within a process."

Multiple application domains can run in a single process:

- No one-to-one correlation between application domains and threads.
- Several threads can belong to a single application domain.
- At any given time, a thread executes in a single application domain — but may change app domains over time.

Diagrammet viser "A Single .NET Process" indeholdende AppDomain A og AppDomain B. Hver AppDomain har sit eget **Shared Data**. AppDomain A rummer Thread A og Thread B, AppDomain B rummer en Thread A. Hver tråd har Call Stack og TLS.

> Slide 17

---

## 5. Thread Basics

Slide 18 er sektionsoverskriften "Thread Basics".

### Multithreading resources — de relevante namespaces

| Namespace | Indhold |
|---|---|
| `System.Threading` | Classes and interfaces that enable multithreaded programming: `Mutex`, `Monitor`, `AutoResetEvent`, `Thread`, `ThreadPool Timer`, … |
| `System.Threading.Tasks` | The `Task` asynchronous operation abstraction (and friends) |
| `System.Collections.Concurrent` | Thread-safe collection classes to use when multiple threads access the same collection concurrently |
| `System.Windows.Threading` | Types to support the WPF threading system, primarily `Dispatcher` and friends |

> Slide 19

---

## 6. Creating and starting threads

Den grundlæggende opskrift: en instans med arbejdet, en `Thread` der peger på metoden, og `Start()`.

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();
        Thread myThread =
                  new Thread(work.DoLotsOfWork);

        myThread.Start();
        System.Console.ReadKey();
    }
}
```

```csharp
public class LotsOfWork
{
    public void DoLotsOfWork()
    {
        for (int i = 0; i < 1000; i++)
        {
            // TODO: add a lot of work here..
            Console.WriteLine("iteration: {0}", i);
        }
    }
}
```

> Slide 20

---

## 7. Passing parameters

### Via properties

Simplest: sæt en property på arbejdsobjektet, før tråden startes.

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();
        Thread myThread =
                new Thread(work.DoLotsOfWork);
        work.NumberOfIterations = 500;
        myThread.Start();
        System.Console.ReadKey();
    }
}
```

```csharp
public class LotsOfWork
{
    public int NumberOfIterations { get; set; } = 0;

    public void DoLotsOfWork()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            // TODO: add a lot of work here..
            Console.WriteLine("iteration: {0}", i);
        }
    }
}
```

> Slide 21

Bemærk rækkefølgen: `work.NumberOfIterations = 500;` sættes **før** `myThread.Start()`. Sættes den efter, er der en race condition mellem main-tråden og den nye tråd.

### Via `Start()`-metoden

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();

        Thread myThread = new Thread(work.DoLotsOfWork);
        myThread.Start(500);

        // --- OR ---
        Thread myThread2 = new Thread(() => work.DoLotsOfWork(300));
        myThread2.Start(); // Specify parameter in lambda expression

        System.Console.ReadKey();
    }
}
```

```csharp
public class LotsOfWork
{
    public void DoLotsOfWork(object parameter)
    {
        int numberOfIterations = (int) parameter;
        for (int i = 0; i < numberOfIterations; i++)
        {
            // TODO: add a lot of work here..
            Console.WriteLine("iteration: {0}", i);
        }
    }
}
```

> "Only one parameter, must be of type `object` and casted in thread func"
>
> Slide 22

Det er den store begrænsning ved `Thread.Start(object)`: præcis ét argument, utypet. Lambda-varianten er som regel bedre — den er typestærk og kan tage vilkårligt mange parametre.

---

## 8. Pausing og yield

### `Thread.Sleep`

```csharp
public class LotsOfWork
{
    public void DoLotsOfWork(object parameter)
    {
        int numberOfIterations = (int) parameter;
        for (int i = 0; i < numberOfIterations; i++)
        {
            // TODO: add a lot of work here..

            Console.WriteLine("iteration: {0}", i);

            Thread.Sleep(50); // 50 milliseconds
        }
    }
}
```

> "The thread will sleep for **at least** 50 milliseconds."
>
> Slide 23

"At least" er væsentligt: `Sleep` garanterer en minimumsventetid, ikke en præcis. Scheduleren bestemmer, hvornår tråden faktisk får CPU igen.

### Yield med `Thread.Sleep(0)`

```csharp
public class LotsOfWork
{
    public void DoLotsOfWork(object parameter)
    {
        int numberOfIterations = (int) parameter;
        for (int i = 0; i < numberOfIterations; i++)
        {
            // TODO: add a lot of work here..

            Console.WriteLine("iteration: {0}", i);

            Thread.Sleep(0); // Yield
        }
    }
}
```

> "`Sleep(0)` means that the thread yields.
>
> If another thread is ready to run, it will run.
>
> If no other thread is ready to run, the thread that yielded will continue."
>
> Slide 24

### Brug ikke `Thread.Suspend` og `Thread.Resume`

Begge metoder er markeret obsolete i BCL'en:

```csharp
[System.Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
public void Suspend ();
```

```csharp
[System.Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
public void Resume ();
```

Kilder på sliden:
<https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.suspend?view=netframework-4.7>
<https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.resume?view=netframework-4.7>

> Slide 25

Problemet med `Suspend` er, at man suspenderer en tråd på et vilkårligt tidspunkt — måske netop mens den holder en lås. Det er en garanteret vej til deadlock.

---

## 9. Hvornår afsluttes programmet?

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();

        Thread myThread =
            new Thread(work.DoLotsOfWork);
        myThread.Start();
        System.Console.WriteLine("Goodbye from main");

        System.Console.ReadKey();
    }
}
```

```csharp
public class LotsOfWork
{
    public void DoLotsOfWork()
    {
        for (int i = 0; i < 1000; i++)
        {
            // TODO: add a lot of work here..
            Console.WriteLine("iteration: {0}", i);
        }
    }
}
```

> Slide 26

Main skriver "Goodbye from main" og venter på et tastetryk — men programmet afsluttes ikke, før `myThread` er færdig, fordi den er en foreground thread. Det bliver eksplicit på slide 30.

---

## 10. Join — at vente på andre tråde

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();

        Thread myThread =
                new Thread(work.DoLotsOfWork);
        myThread.Start(50);

        myThread.Join();
        System.Console.WriteLine("Hello from main");

        System.Console.ReadKey();
    }
}
```

> "The `Join()` method blocks the caller until the thread on which join was called completes."
>
> Slide 27

Med timeout:

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();

        Thread myThread =
                 new Thread(work.DoLotsOfWork);
        myThread.Start(50);

        myThread.Join(1000); // continue if not joined after 1000 ms
        System.Console.WriteLine("Hello from main");

        System.Console.ReadKey();
    }
}
```

> "But you can specify a timeout."
>
> Slide 28

### Join på flere tråde med `WaitHandle`

`Join()` virker kun på én tråd ad gangen. Skal man vente på flere, bruges wait handles:

```csharp
public static void Main()
{
    WaitHandle[] handles = new WaitHandle[] // The thread wait handles
    {
        new AutoResetEvent(false), new AutoResetEvent(false)
    };

    Thread workerThread0 = new Thread(DoWorkWithHandle);
    Thread workerThread1 = new Thread(DoWorkWithHandle);

    workerThread0.Start(handles[0]);
    workerThread1.Start(handles[1]);

    WaitHandle.WaitAll(handles);

    System.Console.WriteLine("Join on WaitHandle complete");
}

public static void DoWorkWithHandle(object hdl)
{
    var handle = (AutoResetEvent) hdl;

    // Do meaningful work

    handle.Set();
}
```

> Slide 29

Hver tråd får sit eget `AutoResetEvent` med som parameter, gør sit arbejde og kalder `Set()`. Main blokerer i `WaitHandle.WaitAll(handles)`, indtil alle handles er signaleret.

---

## 11. Foreground og background threads

> "A thread's foreground/background status has *no* relation to its priority or allocation of execution time"

**Foreground threads**

- Main thread ("GUI thread") og alle tråde, du opretter eksplicit, er som udgangspunkt foreground threads.
- Application does not terminate until all foreground threads have terminated.

**Background threads**

- Threads may be "pushed" to background threads: `myThread.IsBackground = true;`
- Applications may terminate even if background threads are still running!

> Slide 30

Demonstrationen:

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();
        Thread myThread =
                new Thread(work.DoLotsOfWork);
        myThread.IsBackground = true;
        myThread.Start(50);
        System.Console.ReadKey();
    }
}
```

> "The program will now terminate instantly, when a key is pressed, because `myThread` is a background thread."
>
> Slide 31

Konsekvensen: en background thread kan blive dræbt midt i en operation, når processen lukker. Skal arbejdet være færdigt, skal tråden være foreground — eller også skal man joine den.

---

## 12. At stoppe en tråd

### Gracefully — med et flag

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();
        Thread myThread =
            new Thread(work.DoLotsOfWork);
        myThread.Start();
        System.Console.ReadKey();

        work.ShallStop = true;
    }
}
```

```csharp
public class LotsOfWork
{
    public bool ShallStop { get; set; } = false;
    public void DoLotsOfWork()
    {
        int iteration = 0;
        while (!ShallStop)
        {
            Console.WriteLine("iteration: {0}", iteration);
            Thread.Sleep(50); // 50 milliseconds
            iteration++;
        }
        Console.WriteLine("Thread is done!");
    }
}
```

> Slide 32

Tråden tjekker selv flaget og afslutter i en kendt tilstand. Det er den rigtige måde.

### Abort — ikke så pænt

```csharp
class Program
{
    static void Main(string[] args)
    {
        LotsOfWork work = new LotsOfWork();
        Thread myThread =
                  new Thread(work.DoLotsOfWork);
        myThread.Start();
        System.Console.ReadKey();

        if (myThead.IsAlive) myThread.Abort();
    }
}
```

> Slide 33 (`myThead` er en stavefejl på sliden; korrekt er `myThread`)

Advarslerne på næste slide:

> "Avoid `Abort()` if you can.
>
> `Abort()` throws an exception on the thread.
>
> You don't know what the thread was doing, when it was aborted.
>
> The exception can be caught by the thread (and the thread keeps running...)."
>
> Slide 34

Sidste punkt er det værste: `Abort()` garanterer ikke engang, at tråden stopper — den kan fange `ThreadAbortException` og fortsætte. (I moderne .NET Core/.NET 5+ kaster `Thread.Abort` i øvrigt `PlatformNotSupportedException`.)

---

## 13. Threads og exception handling

> "Exceptions thrown in child thread is not caught by creating thread, regardless of try/catch/finally in scope of creating thread"

```csharp
static void Main()
{
  try
  {
    new Thread (Go).Start();
  }
  catch (Exception ex)
  {
    // We'll never get here!
    Console.WriteLine ("Exception!");
  }
}

static void Go()
{
  throw new Exception(); // Will terminate application
}
```

> Slide 35

Grunden: `Start()` returnerer med det samme. Når exceptionen kastes i `Go()`, er `try`-blokken i main for længst forladt — og de to tråde har hver sin call stack, så der er ingen fælles stak at boble op ad.

Korrekt håndtering — fang exceptionen **inde i** trådens egen metode:

```csharp
static void Main()
{
  new Thread(BetterGo).Start();
  Console.ReadKey();
}

static void BetterGo()
{
  try
  {
    throw new Exception("OH NO!"); // NullReferenceException - will get caught below
  }
  catch (Exception ex)
  {
    Console.WriteLine("Exception! " + ex.Message);
  }
}
```

> Slide 36

Sammenlign med `Task`, der løser problemet på biblioteksniveau: en task gemmer exceptionen og kaster den igen hos den, der venter. Se `SW4SWD-01_W11.1_Concurrency_Parallel_Tasks.md`.

---

## 14. Thread priorities

Manipulate thread priority through `Thread.ThreadPriority`:

```csharp
// Example
myThread.Priority = ThreadPriority.Highest;
```

To do real-time work, also elevate process priority using `System.Diagnostics.Process`:

```csharp
// Example
System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
      System.Diagnostics.ProcessPriorityClass.High;
```

> "Understand what you're doing – manipulating priorities may cause all sorts of (not-so-)funny problems: Priority inversion, livelocks, starvation, …"
>
> Slide 37

Og advarslen gentages på næste slide (over et billede af en atomprøvesprængning):

> "The .NET scheduler uses thread priorities when deciding which thread to run.
>
> You almost never want to mess with this. So don't!"
>
> Slide 38

| Problem | Hvad der sker |
|---|---|
| Priority inversion | En lavprioritetstråd holder en lås, som en højprioritetstråd venter på, mens en mellemprioritetstråd blokerer den lave fra at køre færdig |
| Livelock | Tråde kører, men laver ingen fremdrift, fordi de hele tiden reagerer på hinanden |
| Starvation | En tråd får aldrig CPU-tid, fordi højere prioriterede tråde altid har noget at lave |

---

## 15. Your turn (øvelser, første del)

> "Solve exercises 1 to 8 in "Threading exercises""
>
> Slide 39

Se `../opgaver/SW4SWD-01_Threading_Exercises.md`. Opgave 1–8 dækker det materiale, der er gennemgået indtil her: oprettelse og start af tråde, parameteroverførsel, join, foreground/background og standsning af tråde.

---

## 16. Thread pooling

Slide 40 er sektionsoverskriften "Thread Pooling".

### Ineffektiv brug af tråde

> "Declaring new threads is easy in code – too easy, perhaps?"

Injudicious use of threads incurs an overhead in time and memory:

| Omkostning | Størrelse |
|---|---|
| Hukommelse pr. tråd (primært stack) | 1 MB |
| Oprettelse af en tråd | 200.000 cycles |
| Nedlæggelse af en tråd | 100.000 cycles |
| Context switch mellem tråde | 6.000–8.000 cycles |

Mange tråde → ineffektiv brug af cache.

> Slide 41

Med de tal er det klart, hvorfor man ikke opretter en tråd pr. lille arbejdsopgave: opstartsomkostningen alene kan overstige selve arbejdet.

### Thread pooling som løsning

- A bounded number of threads are declared a priori.
- Threads are recycled.
- Work items can be queued on the thread pool → fine-grained multithreading possible without performance penalty.

The Common Language Runtime (CLR) provides each application with a Thread Pool.

Some caveats:

- You cannot set the name of a thread from the TP.
- Threads from the TP are always **background** threads.

> Slide 42

Sidste punkt er en fælde: fordi pool-tråde er background threads, kan processen lukke ned midt i deres arbejde.

### Tre måder at bruge thread pool'en

1. Calling `ThreadPool.QueueUserWorkItem()`
2. Using an asynchronous delegate
3. Using the Task Parallel Library (TPL)

> Slide 43

---

## 17. `ThreadPool.QueueUserWorkItem()`

```csharp
static void Main(string[] args)
{
    for (var i = 0; i < 1000; i++)
    {
        var id = i;

        // Execute a new instance of DoWork when a ThreadPool
        // thread becomes available
        ThreadPool.QueueUserWorkItem(DoWork, id);
    }
    Thread.Sleep(60000);
}

static void DoWork(object data)
{
    Console.WriteLine("hello from DoWork({0})", (int)data);
    Thread.Sleep(1000);
}
```

> Slide 44

Bemærk `var id = i;` — samme mønster som closure-fælden i uge 11.1: loopvariablen kopieres til en lokal variabel, før den sendes videre. 1000 work items køres af nogle få pool-tråde.

---

## 18. Asynchronous delegate

```csharp
static void Main(string[] args) {
    Func<string, int> strlenDelegate = CalcStringLength;
    IAsyncResult cookie = strlenDelegate.BeginInvoke("A very long string", new AsyncCallback(AddComplete), null);
    while (!cookie.AsyncWaitHandle.WaitOne(100,true)) {
        Console.WriteLine("Doing some work in Main()!");
    }
    Console.Read();
}

static int CalcStringLength(string text) {
    // Very complicated string length computation goes here!
    return text.Length;
}

//Target of AsyncCallback delegate should match the following pattern
public static void AddComplete(IAsyncResult iftAr) {
    Console.WriteLine("AddComplete() running on thread {0}", Thread.CurrentThread.ManagedThreadId);
    Console.WriteLine("Operation completed.");
    //Getting result
    AsyncResult ar = (AsyncResult)iftAr;
    Func<string, int> fsi = (Func<string, int>)ar.AsyncDelegate;
    int result = fsi.EndInvoke(iftAr);
    Console.WriteLine("String length: {0}", result);
}
```

> Slide 45

`BeginInvoke`/`EndInvoke`-mønstret (Asynchronous Programming Model, APM) er den gamle måde. Det er verbost og understøttes ikke længere i .NET Core. Det er med her af historiske grunde — det viser, hvad `async`/`await` afløste.

---

## 19. Task Parallel Library (TPL)

In .Net 4.0, TPL and the `Task` abstraction was introduced.

- Tasks are fast, convenient and flexible to use.
- A task should be considered a small, isolated unit of work.

```csharp
public static void Main()
{
    Task myTask = Task.Run((Action) TaskFunc);
    myTask.Wait();
    Console.WriteLine("TaskFunc complete");
}

static void TaskFunc()
{
    Console.WriteLine("Hello from TaskFunc");
    Thread.Sleep(250);
}
```

> Slide 46

Dette er overgangen fra rå tråde til tasks. Hele uge 11.1 bygger videre herfra — se `SW4SWD-01_W11.1_Concurrency_Parallel_Tasks.md`.

---

## 20. Async / Await

In .Net 4.5, `async` and `await` keywords where introduced.

```csharp
async Task<int> AccessTheWebAsync() {
    HttpClient client = new HttpClient();

    Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

    DoIndependentWork();

    string urlContents = await getStringTask;

    return urlContents.Length;
}
```

> Slide 47

Rækkefølgen er værd at bemærke: kaldet startes, `DoIndependentWork()` udføres imens, og først derefter awaites resultatet. Havde man skrevet `await` direkte på kaldet, ville det uafhængige arbejde ikke overlappe med netværkskaldet.

---

## 21. Basic Synchronization Mechanisms

Slide 48 er sektionsoverskriften "Basic Synchronization Mechanisms".

### Oversigt

- **Simple blocking:** `Thread.Sleep()`, `Thread.Join()`, `Task.Wait()`
- **Locking constructs:**
  - Exclusive: `lock()`, `Monitor.Enter()` og `Monitor.Exit()`, `Mutex`
  - Non-exclusive: `Semaphore`
- **Signalling constructs:** `Monitor.Wait()`, `xxxEvent`

> Slide 49

---

## 22. Exclusive locking using Monitor

> "The Monitor prevents collisions if used correctly, i.e. by **all** users of the shared resource"

```csharp
Monitor.Enter(o);
try{
  //critical section
  ...
}
finally{
  Monitor.Exit(o);
}
```

Egenskaber:

- `Monitor.Enter()` only blocks *other* threads — the calling thread may re-enter the monitor (monitoren er reentrant).
- Monitors can only lock **reference types** — value types are boxed (this constitutes a synchronization error!).
- Monitors can only lock within same application domain.

> Slide 50

Boxing-fælden er alvorlig: låser man på en `int`, boxes den til et nyt objekt ved hvert kald, så hver tråd låser på sit eget objekt — og der er ingen synkronisering overhovedet. Compileren siger ikke fra.

`try`/`finally` er ikke valgfrit: uden det bliver låsen aldrig frigivet, hvis den kritiske sektion kaster en exception.

---

## 23. Exclusive locking using `lock()`

> "The C# `lock()`-statement is shorthand for using monitors.
>
> Best practice is to create a lock object.
>
> You can lock on any object, including `this`, but then others can lock on the same object as well and prevent concurrency."

```csharp
class Counter
{
    private int c1 = 0;
    private object myLock = new object();

    public void Increment()
    {
        lock (myLock)
        {
            c1++;
        }
    }

    public int Count
    {
        get
        {
            lock (myLock)
            {
                return c1;
            }
        }
    }
}
```

> Slide 51

Sliden markerer, at `lock (myLock) { … }` oversættes til `Monitor.Enter()` ved blokkens start og `Monitor.Exit()` ved dens slutning (indpakket i `try`/`finally` af compileren).

Bemærk, at **også** getteren låser. Ellers kunne `Count` læse `c1` midt i en ikke-atomisk opdatering.

---

## 24. Exclusive locking using Mutex

- A Mutex can lock across application domains/processes.
- Mutex locks on itself (not an arbitrary object).
- `Mutex.WaitOne()` acquires the mutex — blocks until mutex is available or optional timeout elapses.
- `Mutex.ReleaseMutex()` releases the mutex — only owning thread may release the mutex, otherwise an `AplicationException` is thrown.
- Named mutexes can span processes.

> Slide 52 (`AplicationException` er stavet sådan på sliden; klassen hedder `ApplicationException`)

Eksempel:

```csharp
class SynchDemo
{
  private static int _c1 = 0;
  private static Mutex _mutex = new Mutex();

  public static void Main(string[] args)
  {
    var tasks = new Task[] { Task.Run((Action) IncC1), Task.Run((Action) IncC1) };
    Task.WaitAll(tasks);
  }

  private static void IncC1()
  {
    if (!_mutex.WaitOne(TimeSpan.FromSeconds(3)))
    {
        Console.WriteLine("Another task is holding C1 - bye!");
        return;
    }
    Console.WriteLine("Press a key to increment C1");
    Console.ReadKey(true);
    ++_c1;
    _mutex.ReleaseMutex();
  }
}
```

> Slide 53

Timeout-varianten af `WaitOne` er mønstret at kende: i stedet for at blokere for evigt giver den mulighed for at give op og reagere fornuftigt.

---

## 25. Non-exclusive locking using Semaphore

- Windows semaphores are **counting** semaphores:
  - Unnamed semaphore → local to hosting application domain
  - Named semaphore → accessible system-wide
- Enter the semaphore by `Semaphore.WaitOne()`:
  - Semaphore > 0 → Entry granted, semaphore decremented
  - Semaphore = 0 → Caller blocked
- Release semaphore by `Semaphore.Release()`:
  - No threads waiting → Semaphore incremented
  - Threads waiting → Some thread released (**no order**)
- Semaphores are **thread-agnostic** (vs. mutexes).
- Windows semaphores have a max count (!).

> Slide 54

To ting adskiller semaforen fra mutexen: den tillader N samtidige indehavere i stedet for én, og den er thread-agnostic — en anden tråd end den, der tog den, må godt frigive den. Det gør semaforer velegnede til producer/consumer, hvor den ene tråd signalerer den anden.

---

## 26. Comparison of Locking Constructs

| Construct | Purpose | Cross-process? | Overhead* |
|---|---|---|---|
| `lock` (`Monitor.Enter` / `Monitor.Exit`) | Ensures just one thread can access a resource (or section of code) at a time | – | 20 ns |
| `Mutex` | (samme formål) | Yes | 1000 ns |
| `SemaphoreSlim` (introduced in Framework 4.0) | Ensures not more than a specified number of concurrent threads can access a resource | – | 200 ns |
| `Semaphore` | (samme formål) | Yes | 1000 ns |
| `ReaderWriterLockSlim` (introduced in Framework 3.5) | Allows multiple readers to coexist with a single writer | – | 40 ns |
| `ReaderWriterLock` (effectively deprecated) | (samme formål) | – | 100 ns |

> \*Time taken to lock and unlock the construct once on the same thread (assuming no blocking), as measured on an Intel Core i7 860.
>
> Note til `SemaphoreSlim`: "Lightweight alternative to `Semaphore` for use within same process (no naming)"
>
> Slide 55

Konklusionen ligger i tallene: `lock` er 50 gange billigere end en `Mutex`. Brug kun de cross-process-egnede konstruktioner, når man faktisk skal synkronisere på tværs af processer.

---

## 27. Signaling with Event Wait Handles

> "Signaling: When one thread waits until it is signaled by another thread."

Event wait handles are simplest form. Not related to C# `event`.

Three flavors:

| Type | Semantik |
|---|---|
| `AutoResetEvent` | `Set()` unblocks waiter once → Think "turnstile" |
| `ManualResetEvent` | `Set()` unblocks waiter until next `Reset()` → Think "gate" |
| `CountdownEvent` | Predetermined number of `Set()` → unblock (from .Net 4.0) |

> Slide 56

Billedsproget er præcist: en turnstile (drejekors) lukker én person igennem pr. aktivering og lukker igen af sig selv; en gate (port) står åben, indtil nogen lukker den.

`AutoResetEvent` er den, der bruges i `WaitHandle`-eksemplet på slide 29.

---

## 28. Your turn (øvelser, anden del)

> "Continue with ex. 1 to 8 and then solve exercises 9 to 12 in "Threading exercises""
>
> Slide 57

Se `../opgaver/SW4SWD-01_Threading_Exercises.md`. Opgave 9–12 dækker synkroniseringsdelen: race conditions, `lock`/`Monitor`, `Mutex`, `Semaphore` og signalering.

---

## 29. References and image sources

Images:

- Down the rabbit hole: <https://i.pinimg.com/originals/3b/a4/1c/3ba41c16b44edd7d9c5c9faeec965fad.gif>
- Computer keyboard: <http://stockmedia.cc/computing_technology/slides/DSD_8790.jpg>
- Nuclear explosion: <http://www.greenpeace.org/international/en/multimedia/photos/mushroom-cloud/>

> Slide 58

Slide 59 er en afsluttende Aarhus University-slide uden indhold.

---

## Opsummering

- Concurrency er enten reel samtidighed eller hurtig task switching. Den kommer i to former: data parallelism (samme beregning på forskellige data) og functional parallelism (uafhængige opgaver samtidig) — sidstnævnte bruges til latency, ikke-blokerende I/O og throughput.
- Amdahl's Law sætter loftet: S(N) = 1/((1−P) + P/N). Den sekventielle andel bestemmer den maksimale speedup — flere cores hjælper ikke, når P er lav.
- Tråde i én proces deler data (heap), men har hver sin call stack og TLS. I .NET ligger dette inde i application domains, uden 1:1-forhold mellem domæner og tråde.
- `Thread` startes med `Start()`, ventes på med `Join()` (eventuelt med timeout) og på flere tråde med `WaitHandle.WaitAll()`. Parametre sendes via properties, via `Start(object)` (kun ét, utypet argument) eller via en lambda.
- Foreground threads holder applikationen i live; background threads (`IsBackground = true`) gør ikke. Tråde stoppes gracefully med et flag — `Abort()` skal undgås, og `Suspend`/`Resume` er deprecated.
- Exceptions i en child thread fanges ikke af den oprettende tråd. `try`/`catch` skal ligge inde i trådens egen metode.
- Tråde er dyre (1 MB stak, 200.000 cycles at oprette). Thread pool'en genbruger et begrænset antal background threads; TPL og `Task` er den moderne indgang til den.
- Synkronisering: `lock`/`Monitor` (billigst, samme proces, kun reference types), `Mutex` (cross-process, ejerskabsbundet), `Semaphore`/`SemaphoreSlim` (tælleren tillader N samtidige, thread-agnostic) og event wait handles til signalering.

---

## Krydsreferencer

- Øvelser: `../opgaver/SW4SWD-01_Threading_Exercises.md` (opgave 1–8 efter slide 39, opgave 9–12 efter slide 57)
- Fortsættelsen med tasks og TAP: `SW4SWD-01_W11.1_Concurrency_Parallel_Tasks.md`
