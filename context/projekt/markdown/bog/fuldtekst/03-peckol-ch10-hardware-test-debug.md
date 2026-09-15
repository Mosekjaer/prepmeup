# Hardware Test and Debug

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*James K. Peckol, “Embedded Systems Design, A Contemporary Design Tool”, Wiley, ISBN 978-0-471-72180-2.  
Chapter 10, Hardware Test and Debug, pp. 401–407.*

> **Things to look for…**
>
> - The vocabulary of testing.
>
> - The reasons for debugging, troubleshooting, and testing.
>
> - The need for planning, specifications, test procedures, and test cases.
>
> - Steps and heuristics for the debugging process.
>
> - Identification and isolation of common faults in combinational and sequential circuitry.
>
> - Tests for the designer and for the customer.

## Introduction

Formulating a testing, debugging, or troubleshooting strategy should occur early in the design and development process. Ideally, it should be concurrent with the hardware and software development.

Most often, when we come up with an idea for any new design, we begin with a plan. We think about what the system will do; we think about its features, its capabilities, and its functionality. Debugging, troubleshooting, and testing are no different. We must have a plan. Without a plan, we can’t be sure what we are looking for, or what it might look like if we find it, or when we are finished.

We will begin the study of test by introducing some of the relevant vocabulary. Understanding the terminology facilitates understanding the requirements of a test or test strategy, as well as the capabilities and limitations of the equipment utilized to execute the strategy. Earlier discussions of the effects of microprocessor word size on real numbers and subsequent computations now provide a basis for understanding, formulating, and interpreting measurements during the test process. We will then present a high-level model for a testing strategy; examine and motivate the need for planning, specifications, test procedures, and test cases; and examine several views, ranging from black to white box models, for approaching test. Finally, we will move into testing during the different stages of the product life cycle.

## Some Vocabulary

As a prelude to the study of debugging and testing, it is important to understand some of the vocabulary of the area. Making measurements or generating signals is rather straightforward. Doing so properly is more of a challenge. In part, the vocabulary will illustrate where the challenges lie. As we do so (philosophy aside), it is important to recognize that physical entities exist, they have attributes, and, based on fundamental physics, those attributes have values independent of any ability to measure them or to replicate them. Herein lie most of the challenges.

|  |  |
|:---|:---|
| *True Value* | The actual or inherent value of a physical quantity. |
| *UUT / DUT* | Unit or Device Under Test. |
| *Accuracy* | The measure of an instrument’s capability to approach a true or absolute value. |
| *Resolution* | Measure of ability to discern the value of a measurement. Expression of the value measurement to 1, 2, or 3 decimal places provides three levels of resolution of the true value of the measured value. |
| *Variance* | Has no unit of measure. Variance provides an indication of the relative degree of repeatability of a set of measurements; that is, how closely the values of series of repeated measurements agree with each other. |
| *Mean* | Measure of the central value of a set of measurements and is given by the following equation. $`m_i`$ is the value of an individual measurement. 
``` math
\text{mean} = \frac{1}{N}\sum_{i=0}^{N-1} m_i
``` |
| *Root Mean Square* | The square root of the average of the squares of a set of values and given by the following equation. $`m_i`$ is the value of an individual measurement. 
``` math
\text{rms} = \sqrt{\sum_N \frac{(y_i)^2}{N}}
``` |
| *Bias* | Measure of how closely the mean value in a series of repeated measurements approaches the true value. |
| *Residual* | Measured value minus the mean. |
| *Golden Unit* | Unit whose behavior is completely known used as a standard. |
| *Statistical Tolerance Interval* | Estimate of the amount of measurement variability due to the test system, excluding the UUT variability. Test limits must be outside the STI limits. |
| *Test Limits* | Upper and Lower physical limits of the measurement. |

With some of the basic vocabulary in hand, we begin by formulating a high-level strategy.

## Putting Together a Strategy

Although the words *debugging*, *testing*, and *troubleshooting* all have the same general objectives, they represent three different tasks; they are undertaken at different times during the product’s lifetime and with different underlying assumptions. Debugging is done during the early phases; testing is done before delivery; and troubleshooting afterwards. Debugging does not assume that the design has ever worked. Debugging is a process utilized to identify the cause of problems that occur in the design and implementation of a system as it is incrementally made to work. Testing begins with the assumption that the design is correct. It is a process charged with identifying any faults that may have been introduced during the manufacture of the system and ensuring that a properly working product is delivered to the customer. Troubleshooting begins with the premise that the design is correct and that the product worked at one time. The troubleshooting process seeks to identify which hardware component(s) have failed. The focus is on the hardware because, as was stated earlier in the discussion of safety and reliability, software components do not wear out and fail during use.

During the early stages of product development, any debugging plan is likely to be less formal. Most of it will probably be in the designer’s head. Normally, the person who designed the circuit or the software should have a good idea of what to look for in terms of both Unit Under Test (UUT) functionality and the potential cause(s) of any anomalous or unintended behavior. Such is not always the case, however, so outlining a well-reasoned and orderly strategy is an excellent first step to help to focus the process.

As the design evolves and progresses through the development cycle, the need for a more formal approach becomes essential. In production, formal test procedures, based on a formal test plan, are required. In the field, troubleshooting procedures for the customer or the field service personnel are an essential part of a product’s deliverables.

In the ensuing discussion, the terms *test plan*, *test specification*, *test procedure*, and *test cases* are used in a generic sense. They apply equally to debug, test, and troubleshooting; only the initial presumptions and focus will differ for each. Although the main focus of the chapter is on the hardware side, the vocabulary, strategy, and philosophy apply equally well to the software side.

## Formulating a Plan

Testing in industry is not taken lightly. In companies that know what they are doing, it is a very serious task. For example, let’s consider the approach of a very large manufacturer of electronic test and measurement equipment. For many large companies, there is a full, highly qualified test group. For smaller companies, the approach may be to take several senior engineers off design projects and have them thoroughly test a (new) product with the intention of finding problems before the design is approved for release to production.

In some cases, the engineers have no knowledge of the specifics of the software or hardware inside of the box; they simply try to break it. Such testing is also called stress testing because the idea is to try to stress the software or hardware to break it or to find potential problems before they surface in a delivered product.

With a high-level overview, it is possible to begin to see how to put the high-level concepts to use. It is important to remember that testing is an integral part of all phases of product development, including design. Testing is done for four main reasons, as shown in Figure 1.1.

*Figur: Principal Reasons to Test* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Each of these reasons has a different objective and scope and tests different aspects of the design. All such requirements should be found in the *System Test Plan* and subsequent *Test Specification*. Remember: like the System Requirements Document that was discussed earlier in the context of original design, the Test Plan identifies what tests need to be carried out. It describes in general terms the following information:

- What is to be tested?

- The testing order within each type of test

- Assumptions made

- Algorithms that may be used

Testing formality increases as the system moves toward the latter stages of development. Testing to ensure functionality can be reasonably informal; we still should have some form of plan. As the system begins to come together, formality must increase. Today’s systems are becoming too complex. It is becoming too easy to miss critical, yet subtle, points.

A *Test Plan* begins the process. It describes in general terms *what* must be tested. Such a plan is based on the initial requirements captured in the *Requirements Specification*. It may specify *how* the testing will be carried out, the testing order within each general category of test (input, output, processing), as well as any assumptions that are made.

##### Example 10.0

Consider a simple AND gate as a Unit or (device) under test UUT/DUT (see Figure 1.2).

*Figur: Unit Under Test* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

A test plan might be similar to the following,

First verify the following static behavior of the device

1.  Ensure that the circuit functions as a logical AND gate according to its specified truth table.

2.  Verify the following signal values: $`V_{\mathrm{OHmin}}`$, $`V_{\mathrm{OLmax}}`$, $`V_{\mathrm{IHmin}}`$, $`V_{\mathrm{ILmax}}`$.

Then verify its dynamic behavior.

1.  Confirm the following parameters: $`\tau_{\mathrm{PDHL}}`$, $`\tau_{\mathrm{PDLH}}`$, $`\tau_{\mathrm{rise}}`$, $`\tau_{\mathrm{fall}}`$,

When putting an informal or formal test plan together, remember that the goal is to capture the essence of what must be tested. Keep things simple, precise, and to the point. The plan is not intended to be the next great novel.

The test plan establishes a strategy. During debugging, an informal test plan guides the process. During this phase, the behavior of the design is examined from a high level. Does a 5-volt signal appear on this pin? Does the counter work? Is the proper sequence of control signals being generated? Once again, informality, yet precision, is appropriate. As the design matures, testing must be based on concrete values and tolerances.

## Formalizing the Plan—Writing a Specification

The *Test Specification* evolves from and formalizes the test plan in a manner analogous to the relationship between the *Requirements Specification* and *Design Specification* studied earlier. The test specification includes a description of and specification for each test. As with the test plan, its focus remains on *what* is being tested. Analogous to the design specification studied earlier, the test specification also begins to establish *how* the tests are to be carried out and what the appropriate test stimuli and test limits should be. The test specification assigns specific values, limits, and tolerances to all of the parameters to be tested based on what was stated in the design specification. These values ultimately lead to constraints, requirements, and specifications for the test equipment to be utilized in creating and conducting the tests.

The test specification that is used during the design phase will often serve as a base on which to build the production test strategy. The full complement of tests utilized during the design phase to ensure compliance with the system specifications is generally not needed once the product is in production. Upon release to production, the design is assumed to be correct. A specific subset to confirm continued compliance is definitely appropriate.

The test specification for the UUT will now take on a more formal appearance as we see in the next example.

##### Example 10.1

Verify the following static behavior of the device.

1.  Ensure that the circuit functions as a logical AND gate according to its specified truth table.

| IN1 | IN2 | OUT1 |
|:---:|:---:|:----:|
|  0  |  0  |  0   |
|  0  |  1  |  0   |
|  1  |  0  |  0   |
|  1  |  1  |  1   |

1.  Verify the following signal values and limits:

|                        |                                   |
|:-----------------------|:----------------------------------|
| $`V_{\mathrm{OHmin}}`$ | $`2.4 \pm 0.003~V_{\mathrm{DC}}`$ |
| $`V_{\mathrm{OLmax}}`$ | $`0.4 \pm 0.001~V_{\mathrm{DC}}`$ |
| $`V_{\mathrm{IHmin}}`$ | $`2.0 \pm 0.003~V_{\mathrm{DC}}`$ |
| $`V_{\mathrm{ILmax}}`$ | $`0.8 \pm 0.001~V_{\mathrm{DC}}`$ |

Next, we must verify its dynamic behavior.

Confirm the following parameters:

|                          |                               |
|:-------------------------|:------------------------------|
| $`\tau_{\mathrm{PDHL}}`$ | $`15.0 \pm 0.05~\mathrm{ns}`$ |
| $`\tau_{\mathrm{PDLH}}`$ | $`15.0 \pm 0.05~\mathrm{ns}`$ |
| $`\tau_{\mathrm{rise}}`$ | $`5.0 \pm 0.001~\mathrm{ns}`$ |
| $`\tau_{\mathrm{fall}}`$ | $`5.0 \pm 0.001~\mathrm{ns}`$ |

The test specification provided a quantified description of what must be tested. Now, those tests must be carried out. During the debugging phase, the procedures and approaches may be based primarily on heuristic experience. In production, such an approach gives way to more formal methods.

## Executing the Plan—The Test Procedure and Test Cases

The *Test Procedure* and *Test Cases* specify *how* the test plan and specification are to be implemented and must provide the detailed lists of the necessary equipment and the steps for each test. These documents first decompose the plan into a series of blocks in much the same way we first decomposed the overall design into functional modules. Each block has a specific behavior, parameter, or set of related parameters in the system that it is testing. It gives the order of the test steps, values, and ranges of stimuli to be applied to the UUT during each test step. It specifies the values and ranges of the resulting measurements for each step. A series of (related) test cases is called a *test suite*.

Test case design is essential for testing at any level. The content of test cases will of course vary with the specific nature and intent of each individual test. During the early stages of test, one must test the design for behavior with following three kinds of values:

- Expected values

- Unexpected values

- The boundaries of expected values, inside, outside, and at the boundary

Recall the earlier comments in the chapter on safety and reliability.

The test values may be randomly generated test vectors or statistically based patterns. Such an approach is reasonable for combinational logic; however, it falls down on sequential types of relationships. See the earlier discussions of testing sequential circuits.

To describe the efficacy of a test, the phrase *test coverage* is used. Test coverage provides the percentage of hardware, software, or system tested in a specific test or series of tests. When putting the test cases together, one must ensure that every path through the system is traversed at least once with signals of both polarities for the hardware cases and with variables of nominal and extreme values for the software cases.

During test, the emphasis is primarily on the system’s behavior as manifest through various hardware signals. The purpose of the underlying firmware is to produce the intended or specified hardware behavior. Access to such hardware signals is gained through test points and/or test connectors. Access to software-driven results comes indirectly through those same signals. These are a signal or sets of signals, internal to the UUT, that can be observed directly via a probe point or connector that is incorporated into the circuit or system during design. When direct access to signals (inside of a complex component) is not available, boundary scan or similar techniques are used.

The test suite must evolve with testing. As faults are found and fixed, further tests are often suggested that may find similar faults that were not in the original test design.

The details of the test procedures strongly depend on the test system being used. Often such systems are a combination of commercial instruments such as power supplies, function generators, or digital word generators and proprietary circuits designed specifically for the test system. During debugging, one may use more sophisticated analysis equipment such as data generators and logic analyzers or oscilloscopes to help to verify the design. In production, it is assumed that the design is good; testing serves to identify defects introduced during manufacturing.

## Applying the Strategy—Egoless Design

Let’s now follow the test process from the initial stages of testing prior to release to manufacture. The process commences with testing for ourselves and then moves to testing for our customer.

A key element of debug and testing during the early phases of the development life cycle is *egoless design*. What does design have to do with test? At this stage, testing begins with the initial specification as the basis for evaluating the preliminary designs. As we discussed in Chapter 8, such evaluation occurs through design reviews, code walkthroughs, and code inspections. One cannot let the belief that the best widget ever witnessed by humankind has just been designed to get in the way of an unbiased assessment of that design to determine if it is really the case. It is essential that ego not be part of the review process.

Design reviews, code walkthroughs, and code inspections must be done by someone else. As is often the case when proofreading one’s own writing, schematics, or code, our brain ensures that they appear exactly the way we want them to rather than as it is actually written or designed.

## Applying the Strategy—Design Reviews

During the early stages of product design, we are really testing for ourselves. That testing does not start when the first few pieces are on a lab bench or a couple of high-level algorithms have been coded up. In reality, it must begin much earlier during the design process.

A good first step in this direction is to hold design reviews as the design of the system progresses. Initially, such a review can ensure that everyone understands the high-level specifications and functionality of the system. Thereafter, a design review preceding the architectural phase can confirm detailed functionality. As the design progresses, at least one review prior to moving to prototype can ensure that the mapping from function to processors, FPGAs, or ASICs is sound. The formality of such reviews can vary with need. They can range from a simple exchange of drawings and code among the team members to detailed reviews with reviewers who are not directly involved with the project.

No matter how one chooses to proceed, it is important that the review be conducted in a constructive manner. All participants should recognize that a good review helps to ensure a more robust product at the end of the day.

## Applying the Strategy—Module Debug and Test

As the design moves along the development cycle, the first prototypes of the individual modules are built and ready for a *smoke test* as the jargon goes. The phase now entered is called *debugging*. During this phase, the goal is to identify all of the different kinds of errors and faults that might have occurred in the development of a new or modified circuit, software module, or system.

> ***Caution:*** When running a smoke test. There is strong scientific evidence that most electronic circuits have an embedded smoke demon whose presence is essential to keeping the circuit working. Empirical evidence seems to support the theory since we can show that once the smoke demon is released from the circuit, it no longer works.

On both sides, we have design errors and oversights that have escaped earlier analysis, modeling, and reviews. On the hardware side we have implementation issues such as wiring errors (which often lead to stuck-at types of faults) and incorrect or incorrectly installed parts that have occurred during the prototype build.

To effectively debug the design, it is essential to know what behavior is being tested, how the necessary and appropriate stimuli are going to be produced, and how the results are going to be analyzed.
