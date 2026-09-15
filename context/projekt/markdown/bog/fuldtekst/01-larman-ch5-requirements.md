# Requirements (Larman, kap. 5)

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Craig Larman, “Applying UML and Patterns”, Prentice Hall PTR, 3. ed.  
Chapter 5, Requirements, pp. 54–57.*

## Definition: Requirements

**Requirements** are capabilities and conditions to which the system—and more broadly, the project—must conform \[JBR99\].

The UP promotes a set of best practices, one of which is *manage requirements*. This does not mean the waterfall attitude of attempting to fully define and stabilize the requirements in the first phase of a project before programming, but rather—in the context of inevitably changing and unclear stakeholder’s wishes, this means—“a systematic approach to finding, documenting, organizing, and tracking the *changing* requirements of a system” \[RUP\].

In short, doing it iteratively and skillfully, not being sloppy.

A prime challenge of requirements analysis is to find, communicate, and remember (that usually means write down) what is really needed, in a form that clearly speaks to the client and development team members.

## Evolutionary vs. Waterfall Requirements

Notice the word *changing* in the definition of what it means to manage requirements. The UP embraces change in requirements as a fundamental driver on projects. That’s incredibly important and at the heart of waterfall versus iterative and evolutionary thinking.

In the UP and other evolutionary methods (Scrum, XP, FDD, and so on), we start production-quality programming and testing long before most of the requirements have been analyzed or specified—perhaps when only 10% or 20% of the most architecturally significant, risky, and high-business-value requirements have been specified.

What are the process details? How to do partial, evolutionary requirements analysis combined with early design and programming, in iterations? See “How to do Iterative and Evolutionary Analysis and Design?” on page 25. It provides a brief description and a picture to help explain the process. See “Process: How to Work With Use Cases in Iterative Methods?” on page 95. It has more detailed discussion.

> **Caution!**  
> If you find yourself on a so-called UP or iterative project that attempts to specify most or all of the requirements (use cases, and so forth) before starting to program and test, there is a profound misunderstanding—it is not a healthy UP or iterative project.

In the 1960s and 1970s (when I started work as a developer) there was still a common speculative belief in the efficacy of full, early requirements analysis for software projects (i.e., the waterfall). Starting in the 1980s, there arose evidence this was unskillful and led to many failures; the old belief was rooted in the wrong paradigm of viewing a software project as similar to predictable mass manufacturing, with low change rates. But software is in the domain of new product development, with high change ranges and high degrees of novelty and discovery.

Recall the key statistic that, on average, 25% of the requirements change on software projects. Any method that therefore attempts to freeze or fully define requirements at the start is fundamentally flawed, based on a false assumption, and fighting or denying the inevitable change.

Underlining this point, for example, was a study of failure factors in 1,027 software projects \[Thomas01\]. The findings? Attempting waterfall practices (including detailed up-front requirements) was the single largest contributing factor for failure, being cited in 82% of the projects as the number one problem. To quote the conclusion:

> *…the approach of full requirements definition followed by a long gap before those requirements are delivered is no longer appropriate.*
>
> The high ranking of changing business requirements suggests that any assumption that there will be little significant change to requirements once they have been documented is fundamentally flawed, and that spending significant time and effort defining them to the maximum level is inappropriate.

Another relevant research result answers this question: When waterfall requirements analysis is attempted, how many of the prematurely early specified features are actually useful in the final software product? In a study \[Johnson02\] of thousands of projects, the results are quite revealing—45% of such features were never used, and an additional 19% were “rarely” used. See Figure 1.1. Almost 65% of the waterfall-specified features were of little or no value!

These results don’t imply that the solution is to start pounding away at the code near Day One of the project, and forget about requirements analysis or recording requirements. There is a middle way: iterative and evolutionary requirements analysis combined with early timeboxed iterative development and frequent stakeholder participation, evaluation, and feedback on partial results.

*Figur: Actual use of waterfall-specified features.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

## What are Skillful Means to Find Requirements?

To review the UP best practice *manage requirements*:

> *…a systematic approach to finding, documenting, organizing, and tracking the changing requirements of a system. \[RUP\]*

Besides *changing*, the word *finding* is important; that is, the UP encourages skillful elicitation via techniques such as writing use cases with customers, requirements workshops that include both developers and customers, focus groups with proxy customers, and a demo of the results of each iteration to the customers, to solicit feedback.

The UP welcomes any requirements elicitation method that can add value and that increases user participation. Even the simple XP “story card” practice is acceptable on a UP project, if it can be made to work effectively (it requires the presence of a full-time customer-expert in the project room—an excellent practice but often difficult to achieve).

## What are the Types and Categories of Requirements?

In the UP, requirements are categorized according to the FURPS+ model \[Grady92\], a useful mnemonic with the following meaning:[^1]

- **Functional**—features, capabilities, security.

- **Usability**—human factors, help, documentation.

- **Reliability**—frequency of failure, recoverability, predictability.

- **Performance**—response times, throughput, accuracy, availability, resource usage.

- **Supportability**—adaptability, maintainability, internationalization, configurability.

The “+” in FURPS+ indicates ancillary and sub-factors, such as:

- **Implementation**—resource limitations, languages and tools, hardware, …

- **Interface**—constraints imposed by interfacing with external systems.

- **Operations**—system management in its operational setting.

- **Packaging**—for example, a physical box.

- **Legal**—licensing and so forth.

It is helpful to use FURPS+ categories (or some categorization scheme) as a checklist for requirements coverage, to reduce the risk of not considering some important facet of the system.

Some of these requirements are collectively called the **quality attributes**, **quality requirements**, or the “-ilities” of a system. These include usability, reliability, performance, and supportability. In common usage, requirements are categorized as **functional** (behavioral) or **non-functional** (everything else); some dislike this broad generalization \[BCK98\], but it is very widely used.

As we shall see when exploring architectural analysis, the quality attributes have a strong influence on the architecture of a system. For example, a high-performance, high-reliability requirement will influence the choice of software and hardware components, and their configuration.

[^1]: There are several systems of requirements categorization and quality attributes published in books and by standards organizations, such as ISO 9126 (which is similar to the FURPS+ list), and several from the Software Engineering Institute (SEI); any can be used on a UP project.
