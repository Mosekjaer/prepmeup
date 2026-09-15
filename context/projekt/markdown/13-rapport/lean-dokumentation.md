# Lean Documentation (Tomas Björkholm)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Materiale til SW4PRJ4-02 Projekt 4 (Brightspace "Materiale") — ikke knyttet til én bestemt lektion |
| **Kursus** | SWISE-01 Indledende System Engineering / SW4PRJ4-02 Projekt 4 |
| **Kilde** | `LeanDokumentation.pdf` (5 sider; OneNote-udskrift "Dokumentation", dateret 14. april 2015) |
| **Type** | artikel |
| **Original** | Tomas Björkholm, "Practices for Lean Documentation", InfoQ: http://www.infoq.com/articles/practices-lean-documentation |
| **Emner dækket** | Regler for god dokumentation; seks praksisser: identificér målgrupper, strukturér som Google Earth (drill-down-hierarki), hold det småt, gør teksten indbydende, brug visuelle elementer, gør det nemt at vedligeholde; resultatet |

---

My amateur research has given me the insight that the three most important things for greater effectiveness and good quality are knowledge, knowledge and knowledge. Knowledge is best acquired through a dialog but a dialog is only efficient if it includes someone with knowledge. Unfortunately, there are situations when such a person is not around.

This article will help you write effective and useful documentation for those situations where documentation is the only available source of knowledge. The practices presented in this article are based on my experiences working on a project at a big multinational company.

It all started when a developer said he had an idea for improving the project documentation. We gathered a team of people who were interested in improving the documentation and agreed to a few rules.

## The rules for great documentation

The documentation should:

- **Be quick and easy both to create and update.** Information that is out of date can be worse than no information at all.
- **Easily provide correct answers.** If it's not easy to find no one will use it anyway.
- **Not replace human interaction.** Individuals and interaction over processes and tools, right?

To reach the goal of fulfilling the rules, we set up six practices:

## Practice 1 – Identify your consumers and their reasons for using the documentation

While this sounds obvious, it is rarely done. For our project, our improvement team identified four target groups.

- Managers who need a short summary of what we're working on.
- New developers who need a quick introduction.
- Former developers of the system who come back to the project after a few years of doing something else.
- Trouble shooters who help customers with their problems.

The first target group was pleasantly surprised when we asked them about their documentation needs. First of all, no one had ever asked them before. Wow! Secondly, they never even used the massive documents that they had. This target group only needed the answer to three things. In total, three lines of documentation was all that was needed. The rest was waste both for the reader and the writer. OMG!

> Side 1–2

## Practice 2 – Structure it like Google Earth

People use documentation to find answers to the questions they have. The quality of the documentation can be measured by the time it takes to find the answers. We used Google Earth as a model.

Have you ever tried to find your house on Google Earth (drilling down, not searching on address)? How long did it take? Maybe 30 to 60 seconds? Finding your house on the surface of the Earth is like finding 1 answer among 1,5 trillion (1,5 · 10^12) answers. If you are looking for an answer it shouldn't take more than 60 seconds, even if your system is complex and huge.

How does this apply to documentation? We followed a hierarchy analogous to moving through the levels in Google Earth: moon level, satellite level, airplane level, helicopter level and so on. Each level has a short description, which we called an elevator pitch, and up to nine possibilities to continue down the next level.

*Figur: En pyramide (trekant) af noder i et træ. Toppen er mærket "Overview", midten "Details", bunden "Comments in code", og under pyramiden står "Links to clean code". Hver node har pile ned til næste niveaus noder. Banner under figuren: "Every level should link to the next".*

Keep in mind that not all documentation tools are well suited for a drill down approach. A directory structure with Word documents is probably not a good idea. A wiki with links to next levels is much better.

> Side 2

## Practice 3 – Keep it small

We discussed the reason for documentation and come up with the following principles to minimize the size:

- Documentation should be communication without constraints in time or location. It's not a replacement for communication in real time.
- We should only document results not requirements. That means we update or replace the documents when we launch new functionality instead of adding new documents when we get the requirements. This approach ensures that the documentation accurately reflects the current state of the system.
- The benefit of having documentation must be greater than the cost of creating and maintaining it. Don't waste time on documenting what is already written as code. The documentation should provide a big picture overview, and enough information to help the reader quickly find the necessary code.

Having the right amount of documentation is a balance between too short to be useful and too long to read. If you don't use it, you will not update it and then it's worthless or even worse harmful if it's old and misleading.

Here is the advice I got from Henrik Kniberg:

- Have as few documents as possible – but not fewer
- Have as short documents as possible – but not shorter

It's that easy :-).

> Side 2–3

## Practice 4 – Make the text inviting to read

Long, irrelevant text is boring. How can you make your text relevant?

We tried this: We asked a potential consumer to interview someone with knowledge. The reader knows what they want to know and how to structure the text better than the expert. The expert can only guess what the reader wants to know and how it should be structured.

*Figur: To personer — en ekspert (med "!" over hovedet) til venstre og en læser med bærbar computer (med "?" over hovedet) til højre. Pile: "Q" (spørgsmål) fra læser til ekspert, "A" (svar) fra ekspert til læser.*

A typical anti pattern is when someone is leaving the company and their manager asks them to spend their remaining time at the company documenting everything they know. For most people that is more of a punishment than value adding work.

> Side 3

## Practice 5 – Incorporate visuals

Even though the text should be short, further down in the hierarchy, on leaf level, the text might need to be longer and contain more detail. How can this be done without losing the reader? Popular Science to the rescue! Rather than writing your documentation like a scientific report, use the approachable style that Popular Science magazine uses, with lots of visual elements and short easy to read paragraphs.

Adding visuals helps your documentation consumers find what they need quickly. The reader's eyes will go from picture to picture just like when you read a newspaper.

> Side 3

## Practice 6 – Make it easy to maintain

The biggest challenge in writing good documentation is to keep it up to date.

This requires some discipline and a simple Boy Scout rule, "always leave the campground cleaner than you found it." In documentation this means: if the document does not give you the information you need – update it with the correct information as soon as you have figured it out.

The likelihood of maintaining up-to-date documentation decreases as the distance between what you change and where to document increases. Comments in code are closer to the change so they are more likely to be updated than a document in another tool. If you use a wiki for your documentation you can easily integrate comments in code.

If the documentation is difficult to update, or can't be updated as soon as an issue is detected, it's less likely to be updated. Use a tool for easy updates or even simultaneous updates. The same goes for images so make sure to use a tool that creates and updates images directly in the tool. MediaWiki with PlantUML and Confluence with Gliffy are two examples.

> Side 4

## The result

Our documentation came to a test when a new group of people working in another city needed to change code that is usually maintained by our department. A short introduction and an e-mail with a link to the documentation area was the only contact we had and much to our surprise that was also all that was needed. We had reached our goal. We had documentation that was quick and easy to create and maintain, and that effectively helped users find the answers they needed.

I hope our rules and practices can help you create better documentation.

Good luck!

*Disclaimer: Lean in the title has nothing to do with the car manufacturer Toyota. It is the adjective lean (meaning: efficient and with no wastage) I'm referring to.*

Thanks to Ellen Gottesdiener for her support and help. Thanks to Jonas Boegård, Henrik Rasmussen and Igor Soloviev for all their good ideas. Thanks also to Mia Starck and Yassal Sundman for their help with the language.

**Note:** the "knowledge, knowledge and knowledge" mentioned in the first paragraph refer to: domain knowledge (knowing the business), system knowledge (knowing the domain and architecture of the system) and immediate product need knowledge (knowledge about what we are building right now). The documentation methods described in this article are best suited to domain and system knowledge, and to bridging the gap between the two.

## About the Author

Tomas Björkholm works as an Agile coach and trainer at Crisp in Stockholm, Sweden. He has a great passion for growing teams especially within large enterprises. His mission is to make Agile easy to understand and to adapt. Tomas has written two books, a guide to Agile in Swedish and the soon to come "Kanban development in 30 days".

Fra: http://www.infoq.com/articles/practices-lean-documentation

> Side 4–5
