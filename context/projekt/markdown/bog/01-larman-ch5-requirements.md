# Requirements (Larman, kap. 5)

## Metadata

| Felt | Værdi |
|---|---|
| **Kilde** | Craig Larman, *Applying UML and Patterns*, 3. udg., Prentice Hall PTR — kap. 5 "Requirements", s. 54–57 |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Type** | lærebogskapitel (kondenserede studienoter; tex-transskriptionen `01_Larman_Ch5_Requirements.tex` er den fulde kilde) |
| **Sprog** | engelsk |
| **Emner dækket** | requirements-definition, manage requirements (UP), waterfall vs. evolutionary requirements, requirements elicitation, FURPS+, quality attributes, functional vs. non-functional |

> Note: This file is a structured, condensed rendering of the chapter — all sections, definitions, figures and tables are covered, but the prose is paraphrased rather than reproduced word for word.

---

## Definition: Requirements

> Larman s. 54

- **Requirements** = capabilities and conditions the system (and more broadly the project) must conform to [JBR99].
- UP best practice: **manage requirements**. Not the waterfall attitude of fully defining and freezing requirements in a first phase, but — in the context of changing, unclear stakeholder wishes — "a systematic approach to finding, documenting, organizing, and tracking the *changing* requirements of a system" [RUP].
- Short version: do it iteratively and skillfully, not sloppily.
- Core challenge of requirements analysis: find, communicate and remember (usually: write down) what is really needed, in a form that speaks clearly to both client and development team.

## Evolutionary vs. Waterfall Requirements

> Larman s. 54–56

- The word *changing* in the definition is key. UP treats requirements change as a fundamental driver on projects — this is the heart of waterfall vs. iterative/evolutionary thinking.
- In UP and other evolutionary methods (Scrum, XP, FDD, ...) production-quality programming and testing starts long before most requirements are analysed — perhaps when only 10–20 % of the most architecturally significant, risky and high-business-value requirements are specified.
- Process details: see "How to do Iterative and Evolutionary Analysis and Design?" (Larman s. 25) and "Process: How to Work With Use Cases in Iterative Methods?" (Larman s. 95).

> **Caution!** A so-called UP/iterative project that tries to specify most or all requirements (use cases etc.) before programming and testing starts reflects a profound misunderstanding — it is not a healthy UP or iterative project.

Historical argument:

- 1960s–70s: common belief in full, early requirements analysis (waterfall). From the 1980s: evidence that this was unskillful and caused many failures. The belief was rooted in the wrong paradigm — treating software like predictable mass manufacturing with low change rates. Software is new product development: high change rates, high novelty and discovery.
- Key statistic: on average **25 % of requirements change** on software projects (cf. change research, Larman s. 24). Any method that tries to freeze requirements at the start is fundamentally flawed.
- Study of failure factors in **1,027 software projects [Thomas01]**: attempting waterfall practices (incl. detailed up-front requirements) was the single largest contributing factor to failure, cited in **82 %** of projects as the number one problem. The study concludes that full requirements definition followed by a long gap before delivery is no longer appropriate, and that assuming little requirements change after documentation is fundamentally flawed.
- Study of thousands of projects **[Johnson02]**: of features specified early in waterfall style, **45 % were never used** and a further **19 % rarely used** — almost 65 % of little or no value.
- Conclusion is *not* to start coding on day one and skip requirements. The middle way: iterative and evolutionary requirements analysis combined with early timeboxed iterative development and frequent stakeholder participation, evaluation and feedback on partial results.

*Figur 5.1: Actual use of waterfall-specified features* (pie chart):

| Usage | Share |
|---|---|
| always | 7 % |
| often | 13 % |
| sometimes | 16 % |
| rarely | 19 % |
| never | 45 % |

## What are Skillful Means to Find Requirements?

> Larman s. 56

- The UP best practice *manage requirements* again: "a systematic approach to finding, documenting, organizing, and tracking the changing requirements of a system" [RUP].
- Besides *changing*, the word **finding** matters: UP encourages skillful elicitation — writing use cases with customers, requirements workshops with both developers and customers, focus groups with proxy customers, and a demo of each iteration's results to customers for feedback.
- UP welcomes any elicitation method that adds value and increases user participation. Even XP "story cards" are acceptable on a UP project if they can be made to work (requires a full-time customer-expert in the project room — excellent, but often hard to achieve).

## What are the Types and Categories of Requirements?

> Larman s. 56–57

In UP, requirements are categorised by the **FURPS+** model [Grady92]:

| Letter | Category | Covers |
|---|---|---|
| F | **Functional** | features, capabilities, security |
| U | **Usability** | human factors, help, documentation |
| R | **Reliability** | frequency of failure, recoverability, predictability |
| P | **Performance** | response times, throughput, accuracy, availability, resource usage |
| S | **Supportability** | adaptability, maintainability, internationalization, configurability |

The "+" denotes ancillary and sub-factors:

| Factor | Covers |
|---|---|
| **Implementation** | resource limitations, languages and tools, hardware, ... |
| **Interface** | constraints imposed by interfacing with external systems |
| **Operations** | system management in its operational setting |
| **Packaging** | e.g. a physical box |
| **Legal** | licensing etc. |

Footnote: other categorisation schemes exist (ISO 9126 — similar to FURPS+ — and several from SEI); any can be used on a UP project.

- Use FURPS+ (or another scheme) as a **checklist for requirements coverage**, to reduce the risk of overlooking an important facet of the system.
- Usability, reliability, performance and supportability are collectively the **quality attributes**, **quality requirements** or the "-ilities".
- Common usage splits requirements into **functional** (behavioral) and **non-functional** (everything else). Some dislike this broad generalisation [BCK98], but it is very widely used.
- Quality attributes strongly influence the **architecture** (cf. architectural analysis, Larman s. 541): e.g. high-performance/high-reliability requirements shape the choice and configuration of software and hardware components.
