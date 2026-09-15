# Hardware Test and Debug (Peckol, kap. 10)

## Metadata

| Felt | Værdi |
|---|---|
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | James K. Peckol, *Embedded Systems Design, A Contemporary Design Tool*, Wiley, ISBN 978-0-471-72180-2 — Chapter 10 *Hardware Test and Debug*, pp. 401–407 (ISE Book-kompendiet s. 48–56; LaTeX-transskription `03_Peckol_Ch10_HardwareTestDebug.tex` + `spillover/04_to_03.tex`) |
| **Type** | lærebogskapitel |
| **Sprog** | engelsk |
| **Emner dækket** | testvokabular (accuracy, resolution, variance, bias, golden unit, STI, test limits), debugging vs. testing vs. troubleshooting, test plan → test specification → test procedure/test cases, test coverage, test suite, egoless design, design reviews, smoke test / module debug |

> Gengivelsen er en kondenseret parafrase i egne ord af kapitlets indhold (alle afsnit, figurer og datatabeller er med), ikke en ordret afskrift af bogteksten. Se `.tex`-kilden for den fulde ordlyd.

---

*Peckol, ch. 10, pp. 401–407.*

> **Things to look for…** (chapter opener) — the vocabulary of testing; why we debug, troubleshoot and test; why planning, specifications, test procedures and test cases are needed; steps and heuristics for debugging; how common faults in combinational and sequential circuits are identified and isolated; tests done for the designer versus tests done for the customer.

## Introduction

The test/debug/troubleshooting strategy should be formulated early, ideally concurrently with hardware and software development. Like any design, it needs a plan: without one you don't know what you're looking for, what a find would look like, or when you're done.

The chapter's route: (1) vocabulary, so test requirements and the capabilities/limits of test equipment can be understood (earlier material on word size and real-number computation underpins how measurements are interpreted); (2) a high-level model of a test strategy; (3) motivation for plans, specifications, procedures and test cases; (4) black-box to white-box views of test; (5) testing through the stages of the product life cycle.

> Peckol s. 401

## Some Vocabulary

Measuring or generating signals is easy; doing it *properly* is the challenge. Physical entities have attribute values that exist independently of our ability to measure or replicate them — that is where the difficulty lies.

| Term | Meaning (condensed) |
|---|---|
| *True Value* | The actual/inherent value of a physical quantity. |
| *UUT / DUT* | Unit / Device Under Test. |
| *Accuracy* | How closely an instrument can approach the true (absolute) value. |
| *Resolution* | Ability to discern the value of a measurement — e.g. 1, 2 or 3 decimal places give three levels of resolution. |
| *Variance* | Unitless; indicates repeatability — how closely a series of repeated measurements agree with each other. |
| *Mean* | Central value of a set of measurements: $\text{mean} = \frac{1}{N}\sum_{i=0}^{N-1} m_i$, where $m_i$ is an individual measurement. |
| *Root Mean Square* | Square root of the average of the squared values: $\text{rms} = \sqrt{\sum_N \frac{(y_i)^2}{N}}$. |
| *Bias* | How closely the mean of repeated measurements approaches the true value. |
| *Residual* | Measured value minus the mean. |
| *Golden Unit* | A unit with completely known behaviour, used as a standard. |
| *Statistical Tolerance Interval (STI)* | Estimate of measurement variability caused by the test system itself (excluding UUT variability). Test limits must lie outside the STI limits. |
| *Test Limits* | Upper and lower physical limits of the measurement. |

> Peckol s. 401–402

## Putting Together a Strategy

*Debugging*, *testing* and *troubleshooting* share objectives but are three different tasks, done at different times with different assumptions:

| Task | When | Starting assumption | Purpose |
|---|---|---|---|
| Debugging | Early phases | The design has never worked | Find the cause of problems while the design/implementation is incrementally made to work |
| Testing | Before delivery | The design is correct | Find faults introduced during manufacture; ensure a working product reaches the customer |
| Troubleshooting | After delivery | The design is correct and the product once worked | Identify which hardware component(s) failed (software does not wear out) |

Early debugging plans are informal — mostly in the designer's head, since the designer usually knows what to look for and what could cause anomalies. Still, writing down an orderly strategy focuses the work. As the design matures, formality becomes essential: production needs formal test procedures based on a formal test plan, and field troubleshooting procedures are part of the product deliverables.

The terms *test plan*, *test specification*, *test procedure* and *test cases* are used generically for debug, test and troubleshooting alike; only the presumptions and focus differ. The chapter focuses on hardware, but the vocabulary and philosophy apply equally to software.

> Peckol s. 402

## Formulating a Plan

Testing is taken seriously in competent companies. Large ones have dedicated, highly qualified test groups; smaller ones pull senior engineers off design work to try to break a new product before it is released to production. Engineers who know nothing about the internals and simply try to break the box are doing *stress testing*.

Testing is integral to every development phase, including design. The four principal reasons to test (Figure 10.0):

> - To verify that any of the following performs as intended:
>   - Code module or hardware prototypes
>   - Subsystem or collection of subsystems
>   - System interfaces
> - To ensure that the system meets specification
> - To ensure that any changes to the system work — do not alter other intended functionality.
> - To ensure that the system functions properly after being built

*Figure 10.0: Principal Reasons to Test*

Each reason has its own objective and scope. All of them should appear in the *System Test Plan* and the subsequent *Test Specification*. Like the System Requirements Document, the Test Plan identifies *what* must be tested, in general terms: what is tested, the test order within each test type, assumptions made, and algorithms that may be used.

Formality increases as the system approaches later stages. Functional checks early on can be informal (but still planned); as the system comes together, formality must increase — modern systems are too complex to risk missing subtle points.

A *Test Plan* starts the process: what must be tested, based on the *Requirements Specification*. It may also say how testing will be done, the order within each category (input, output, processing), and assumptions.

**Example 10.0**

A simple AND gate as UUT/DUT (Figure 10.1).

*Figur: et enkelt AND-gate-symbol (UUT). To indgange til venstre mærket `IN1` og `IN2`, én udgang til højre mærket `OUT1`.*

*Figure 10.1: Unit Under Test*

A test plan for it, in outline:

> *Static behaviour:* (a) confirm the circuit implements the logical AND truth table; (b) verify the signal levels $V_{\mathrm{OHmin}}$, $V_{\mathrm{OLmax}}$, $V_{\mathrm{IHmin}}$, $V_{\mathrm{ILmax}}$.
> *Dynamic behaviour:* (a) confirm the timing parameters $\tau_{\mathrm{PDHL}}$, $\tau_{\mathrm{PDLH}}$, $\tau_{\mathrm{rise}}$, $\tau_{\mathrm{fall}}$.

Keep the plan simple, precise and to the point — it captures the essence of what must be tested, nothing more.

The plan establishes strategy. In debugging an informal plan guides high-level checks (is there 5 V on this pin? does the counter count? is the control-signal sequence right?) — informal but precise. As the design matures, tests must be based on concrete values and tolerances.

> Peckol s. 402–404

## Formalizing the Plan—Writing a Specification

The *Test Specification* grows out of and formalizes the test plan, analogous to how the *Design Specification* relates to the *Requirements Specification*. It describes and specifies each test. Its focus is still *what* is tested, but it also begins to fix *how*: appropriate stimuli and test limits, with concrete values, limits and tolerances for every parameter, derived from the design specification. Those values in turn become constraints/requirements on the test equipment.

The design-phase test specification usually becomes the base of the production test strategy. Production does not need the full design-phase test set — the design is assumed correct — but a subset confirming continued compliance is appropriate.

**Example 10.1**

The UUT specification in formal form.

*Static behaviour.* (a) The circuit shall function as a logical AND gate per its truth table:

| IN1 | IN2 | OUT1 |
|---|---|---|
| 0 | 0 | 0 |
| 0 | 1 | 0 |
| 1 | 0 | 0 |
| 1 | 1 | 1 |

(b) Signal values and limits:

| Parameter | Limit |
|---|---|
| $V_{\mathrm{OHmin}}$ | $2.4 \pm 0.003\ V_{\mathrm{DC}}$ |
| $V_{\mathrm{OLmax}}$ | $0.4 \pm 0.001\ V_{\mathrm{DC}}$ |
| $V_{\mathrm{IHmin}}$ | $2.0 \pm 0.003\ V_{\mathrm{DC}}$ |
| $V_{\mathrm{ILmax}}$ | $0.8 \pm 0.001\ V_{\mathrm{DC}}$ |

*Dynamic behaviour.* Parameters to confirm:

| Parameter | Limit |
|---|---|
| $\tau_{\mathrm{PDHL}}$ | $15.0 \pm 0.05\ \mathrm{ns}$ |
| $\tau_{\mathrm{PDLH}}$ | $15.0 \pm 0.05\ \mathrm{ns}$ |
| $\tau_{\mathrm{rise}}$ | $5.0 \pm 0.001\ \mathrm{ns}$ |
| $\tau_{\mathrm{fall}}$ | $5.0 \pm 0.001\ \mathrm{ns}$ |

The specification quantifies what must be tested; next the tests must actually be carried out. In debugging that may rest on heuristics and experience; in production it gives way to formal methods.

> Peckol s. 404–405

## Executing the Plan—The Test Procedure and Test Cases

The *Test Procedure* and *Test Cases* define *how* plan and specification are implemented: detailed equipment lists and the steps of each test. The plan is decomposed into blocks (as the design was decomposed into functional modules), each testing a specific behaviour, parameter or related parameter set. For each step they give order, stimulus values/ranges applied to the UUT, and the expected measurement values/ranges. A set of related test cases is a *test suite*.

Test-case design matters at every level. Early on, the design must be exercised with three kinds of values:

- expected values;
- unexpected values;
- the boundaries of the expected range — inside, outside and exactly at the boundary (cf. the safety and reliability chapter).

Random test vectors or statistically based patterns work for combinational logic but break down for sequential relationships (see the earlier discussion of testing sequential circuits).

*Test coverage* expresses a test's efficacy: the percentage of the hardware, software or system exercised by a test or test series. Every path through the system should be traversed at least once — with both signal polarities for hardware, and with nominal and extreme variable values for software.

Testing observes behaviour mainly through hardware signals (firmware exists to produce the specified hardware behaviour, so software results are observed indirectly through the same signals). Access is via *test points* and *test connectors* designed into the circuit. Where internal signals of a complex component are not reachable, *boundary scan* or similar techniques are used.

The test suite must evolve: fixed faults suggest further tests for similar faults not covered originally. Procedure details depend on the test system — typically commercial instruments (power supplies, function/digital word generators) plus proprietary circuitry. Debugging may add data generators, logic analyzers and oscilloscopes; production assumes a good design and hunts manufacturing defects.

> Peckol s. 405–406

## Applying the Strategy—Egoless Design

The process starts with testing for ourselves and moves to testing for the customer. A key early element is *egoless design*: at this stage testing means evaluating preliminary designs against the initial specification, through design reviews, code walkthroughs and code inspections (Chapter 8). Believing you have just designed the best widget ever must not get in the way of an unbiased assessment.

Reviews, walkthroughs and inspections must be done by someone else — as with proofreading, our brain shows us what we intended rather than what is actually written or drawn.

> Peckol s. 406

## Applying the Strategy—Design Reviews

Testing for ourselves does not begin on the lab bench or with the first coded algorithms; it begins much earlier, in design. Hold design reviews as the design progresses:

- an initial review, so everyone understands the high-level specification and functionality;
- a review before the architectural phase, confirming detailed functionality;
- at least one review before prototyping, confirming that the mapping of functions onto processors, FPGAs or ASICs is sound.

Formality can range from team members swapping drawings and code to detailed reviews with reviewers outside the project. Whatever the form, the review must be constructive — a good review yields a more robust product.

> Peckol s. 406–407

## Applying the Strategy—Module Debug and Test

When the first module prototypes are built they get a *smoke test*, and the *debugging* phase begins: identifying all the kinds of errors and faults that can have entered a new or modified circuit, software module or system.

> *Caution (Peckol's joke):* electronic circuits contain a "smoke demon" that keeps them working — once the smoke escapes, the circuit stops working.

On both hardware and software sides there are design errors and oversights that slipped past earlier analysis, modelling and reviews. Hardware additionally has implementation issues from the prototype build: wiring errors (often producing stuck-at faults) and wrong or wrongly installed parts.

Effective debugging requires knowing three things: what behaviour is being tested, how the necessary stimuli will be produced, and how the results will be analysed.

> Peckol s. 407 — kompendie-uddraget slutter her; bogens kapitel fortsætter.
