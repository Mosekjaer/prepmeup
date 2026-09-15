---
title: "Dynamic Analysis.pdf  (8.2)"
source: "csfiles/home_dir/SoftwareQualityMetrics/Dynamic Analysis.pdf"
modul: "Lektion 08.1+2: Software Quality Metrics"
pages: 16
type: "slides"
vision: "done"
---
# Dynamic Analysis.pdf  (8.2)

<!-- side 1 -->

SOFTWARE QUALITY
METRICS
DYNAMIC ANALYSIS

AARHUS                        SWT    PETER HØGH MIKKELSEN
UNIVERSITY           19 MARCH 2024   DYNAMIC ANALYSIS
TECHNICAL SCIENCES

<!-- side 2 -->

AGENDA
Recap – overview software metrics
Intro to dynamic analysis
Qualitative monitoring and ressource usage.
Quantitative mononitoring.
Performance example - memory




     AARHUS                                            SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                               19 MARCH 2024   DYNAMIC ANALYSIS
     TECHNICAL SCIENCES
                                                                                     -2-

<!-- side 3 -->

SOFTWARE METRICS CLASSIFICATION
                                       Quantitative                                                                Qualitative

               Lines of Code (LOC), Cyclomatic Complexity,                             Code Readability, Usability, and Documentation
Static




                           and Defect Density                                                             Quality.

                               Metrics based on static analysis                       Metrics derived from code review and inspection

                  Execution Time, Memory Usage, and Code                                User Satisfaction, Crash Management, and Risk
                                  Coverage                                                                  Exposure
Dynamic




               Metrics derived from profiling during program                       Metrics are based on user feedback, data collection
                                 execution                                                      and post-mortem analysis

                   How many resources are used during run                                                Catch crashes and tell during run


          AARHUS                                                           SWT    PETER HØGH MIKKELSEN
          UNIVERSITY                                              19 MARCH 2024   DYNAMIC ANALYSIS
          TECHNICAL SCIENCES
                                                                                                                                             -3-

<!-- side 4 -->

STATIC ANALYSIS RECAP
Qualitative            Deducible errors                                      Incompatible types
                                                                             Uninitialized / unused variables
                                                                             Race conditions

                       Coding style                                          Formatting

                                                                             Naming


Quantitative           Complexity metrics                                    Cyclomatic

                                                                             Halstead

                                                                             Synthesized (Maintenance Index)

                                                                             Architectural (Class coupling, ...)

  AARHUS                                       SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                          19 MARCH 2024   DYNAMIC ANALYSIS
  TECHNICAL SCIENCES
                                                                                                                   -4-

<!-- side 5 -->

DYNAMIC ANALYSIS
Qualitative            Discover problems                                   When running application


                                                                           When running test suite



Quantitative           Discover resource usage                             When running application


                                                                           When running benchmarks /
                                                                           profiling

  AARHUS                                     SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                        19 MARCH 2024   DYNAMIC ANALYSIS
  TECHNICAL SCIENCES
                                                                                                       -5-

<!-- side 6 -->

RUN-TIME PROBLEMS
Typical Problem:

 • unhandled exceptions
 • uninitialized variables
 • buffer overflow
 • stack overflow
 • resource leaks
 • race conditions
 • ...

   AARHUS                             SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                19 MARCH 2024   DYNAMIC ANALYSIS
   TECHNICAL SCIENCES
                                                                    -6-

<!-- side 7 -->

QUALITATIVE MONITORING
Instrumentation         Compiler inserts sanity checks
                        Inserts check for memory leaks, race conditions, …
                        Generates special executable
                        Performance typically 2x – 10x slower
                        Linux Address/Thread Sanitizers

Emulation               Executable is run by emulator
                        Emulator checks for memory leaks, race conditions, …
                        Works on production executable (Release version)
                        Performance typically 10x – 20x slower
                        Valgrind (Defaults to memory checks)
                        Helgrind (thread checks)
   AARHUS                                     SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                        19 MARCH 2024   DYNAMIC ANALYSIS
   TECHNICAL SCIENCES
                                                                               -8-

<!-- side 8 -->

ASSERTIONS
Manual instrumentation – “Traps”
System.Diagnostics.Debug.Assert(condition)
 • Corresponds to : if (condition) {} else { DisplayError(); }
 • Debug/Release                   public static void MyMethod(Type type, Type baseType)
 • Can have a performance          {
                                      Debug.Assert(type != null, "Type parameter is null");
   impact                             // Perform some processing.
                                           }
                                                                                Learn.microsoft.com

Run a test with Debug generated code


     AARHUS                                      SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                         19 MARCH 2024   DYNAMIC ANALYSIS
     TECHNICAL SCIENCES
                                                                                                      -9-

<!-- side 9 -->

LOGGING
Manual instrumentation – log useful info to help with debugging.
Error logging              using Microsoft.Extensions.Logging;

  • Context of failure     using var loggerFactory = LoggerFactory.Create(static builder =>
                           {
  • Stack traces             builder
                             .AddFilter("Microsoft", LogLevel.Warning)
  • Arguments (e.g. which    .AddFilter("System", LogLevel.Warning)
    file cannot be opened) .AddFilter("LoggingConsoleApp.Program",
                             .AddConsole();
                                                                     LogLevel.Debug)


Tracing                    });


  • History leading up to  ILogger logger = loggerFactory.CreateLogger<Program>();
                           logger.LogDebug("Hello {Target}", "Everyone");
    failure
                                                                                Learn.microsoft.com


     AARHUS                                     SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                        19 MARCH 2024   DYNAMIC ANALYSIS
     TECHNICAL SCIENCES
                                                                                                 -10-

<!-- side 10 -->

QUANTITATIVE RESOURCE USAGE

  System load – tools           • Need faster hardware?
    to measure this

                                • Battery (especially on mobile apps)
          Efficiency            • Flash storage wear
         measurements           • Carbon emission…



                  Scalability

  AARHUS                                      SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                         19 MARCH 2024   DYNAMIC ANALYSIS
  TECHNICAL SCIENCES
                                                                            -11-

<!-- side 11 -->

QUANTITATIVE RESOURCE METRICS
                              • Instructions per cycle
                       CPU    • Cache hits
                              • Branch prediction


                   System     • Memory footprint, memory fragmentation
                              • File handles, sockets, …
                  resources   • Network bandwidth


               Other          • Virtual memory vs. physical memory
                              • Lock contention
             bottlenecks      •…



  AARHUS                                     SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                        19 MARCH 2024   DYNAMIC ANALYSIS
  TECHNICAL SCIENCES
                                                                           -12-

<!-- side 12 -->

QUANTITATIVE MONITORING
Instrumentation         Compiler inserts usage counters of functions
                        Collect metrics by instrumenting each function or line with counters
                        Intrusive, fast, precise
                        Visual Studio Performance Suite, GNU gprof

Emulation               Executable is run in emulator
                        Collect metrics during emulation
                        Non-intrusive, slow, precise
                        Valgrind (callgrind, cachegrind)

Sampling                External tool monitors usage
                        Collect metrics at regular intervals
                        Non-intrusive, fast, imprecise
                        Linux perf
   AARHUS                                            SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                               19 MARCH 2024   DYNAMIC ANALYSIS
   TECHNICAL SCIENCES
                                                                                               -13-

<!-- side 13 -->

PROFILING
                       int main()                        Count how many times a line is executed
                       {
                           int i = 0;                    *
                           for (i = 0; i < 5; i++)       ******
                           {
                               f(i);                     *****
                           }
                           return 0;                     *
                       }

                       int f(int n)
                       {
                           int i;                        *****
                           for (i = 0; i < n; i++)       ***************
                           {
                               sum += i;                 **********
                           }

                           return sum;                   *****
                       }

  AARHUS                                                      SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                         19 MARCH 2024   DYNAMIC ANALYSIS
  TECHNICAL SCIENCES
                                                                                                   -14-

<!-- side 14 -->

BOTTLENECKS
Important to locate bottlenecks
• Parallelization?
• Memory bandwidth?
• Locality of reference?

High performance requires addressing all
aspects, if only one aspect (ex parallelization) is
addressed, improvement may be minimal (See
Amdahl’s law)


Locate and focus on part that consumes the
most time. When updated, re-assess
                                                                                      Wikipedia (Amdahls law)



      AARHUS                                            SWT    PETER HØGH MIKKELSEN
      UNIVERSITY                               19 MARCH 2024   DYNAMIC ANALYSIS
      TECHNICAL SCIENCES
                                                                                                                -16-

**Figur:** Graf med titlen «Amdahl's Law». X-akse «Number of processors», logaritmisk med mærker 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536. Y-akse «Speedup» fra 0 til 20 med gitterlinjer for hver 2. Fire kurver, hver for sin parallelliserbare andel (forklaring «Parallel portion» i boks til højre): lyseblå heltrukken 50 %, rød prikket 75 %, lilla stiplet-prikket 90 %, grøn stiplet 95 %. Alle starter i speedup 1 ved 1 processor og flader ud mod hver sin vandrette asymptote — 50 % mod 2, 75 % mod 4, 90 % mod 10 og 95 % mod 20 — markeret med vandrette stiplede hjælpelinjer. Kurverne er praktisk talt flade allerede omkring 512-1024 processorer: flere kerner giver ingen gevinst, når den serielle del dominerer. Indsat i grafen formlen S_latency(s) = 1 / ((1 − p) + p/s).

<!-- side 15 -->

SOFTWARE METRICS CLASSIFICATION - OVERVIEW
                               Quantitative                                          Qualitative
             Complexity metrics                               Coding style
                Cyclomatic                                    Deducible errors or warnings
Static




                Maintainability                                  Pointer errors
                ...                                              Memory leaks
                                                                 Uninitialized variables
             Profiling resource usage                         Assertions
                  CPU/Memory/OS                               Monitoring
                  Cache hits                                       Resource leaks
Dynamic




                  Branch prediction                                Race conditions
             Bottlenecks                                      Post-mortem analysis
                  Memory/CPU/ Network                              Stack trace
                                                                   Memory dump
                                                                   Logging/Tracing



          AARHUS                                       SWT    PETER HØGH MIKKELSEN
          UNIVERSITY                          19 MARCH 2024   DYNAMIC ANALYSIS
          TECHNICAL SCIENCES
                                                                                                   -18-

<!-- side 16 -->

AARHUS
UNIVERSITY

