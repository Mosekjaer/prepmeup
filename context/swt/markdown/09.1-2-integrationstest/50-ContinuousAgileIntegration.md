---
title: "ContinuousAgileIntegration"
source: "csfiles/home_dir/IntegrationTest/ContinuousAgileIntegration.pdf"
modul: "Lektion 09.1+2: Integrationstest"
pages: 12
type: "slides"
vision: "done"
---
# ContinuousAgileIntegration

<!-- side 1 -->

CONTINUOUS AND AGILE
SYSTEM INTEGRATION
I4SWT




AARHUS
UNIVERSITY                  SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
DEPARTMENT OF ENGINEERING

<!-- side 2 -->

TRADITIONAL INTEGRATION
  Still has its place:
       • You are not working in an iterative model
       • You have made a lot of (tested) units before putting them together
       • As risk mitigation in complex systems or when using new technology (planned!)
       • As a remedy when you have serious problems (unplanned!)
       • As general knowledge as a software engineer




  AARHUS
  UNIVERSITY                                SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING
                                                                                                 -2-

<!-- side 3 -->

INTEGRATION TEST IN AN ITERATIVE
PROCESS



                              IT   IT   IT   IT                IT                   IT            IT   IT




  AARHUS
  UNIVERSITY                                 SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING
                                                                                                            -3-

**Figur:** Iterationsdiagram som en savtakket kurve hen over otte iterationer. En rød pil kommer ind øverst til venstre mærket "Initial planning" og peger skråt ned i bunden af Iteration 1; en rød pil forlader diagrammet øverst til højre mærket "Final delivery". Imellem dem løber en grøn zigzag-kurve: hver iteration består af en nedadgående flanke (mærket Requirements analysis, Design, Implementation for iteration 1; Iteration planning, Requirements analysis, Design, Implementation for de følgende) og en opadgående flanke (mærket Testing) med toppen mærket "Deployment evaluation". To vandrette stiplede niveaulinjer krydser diagrammet: den nederste hedder "Internal deliveries", den øverste "External deliveries". De fleste iterationstoppe når kun op til Internal deliveries; toppene efter Iteration 3 og Iteration 6 rager op til External deliveries. Otte mørkeblå cirkler mærket "IT" (Integration Test) sidder på de opadgående flanker, én pr. iteration, netop hvor testfasen leder op mod leveringen — integrationstest sker altså i hver eneste iteration, ikke som én sluttelig fase. Nederst en tidsakse med dobbeltpile, der markerer Iteration 1 til Iteration 8.

<!-- side 4 -->

RELEVANT INTEGRATION STRATEGIES FOR
AN ITERATIVE PROCESS
  Big bang!
       • because the added functionality in an iteration is small, it can work
  Collaboration
       • The added functionality is typically a Use Case – using the classes collaborating for this
       • Like a mini (!) big bang!
  Top Down
       • because you can show the Product Owner and Customer the look and feel of the
         system early


  AARHUS
  UNIVERSITY                                  SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING
                                                                                                      -4-

<!-- side 5 -->

C4 MODEL BY SIMON BROWN




 AARHUS
 UNIVERSITY                  SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
 DEPARTMENT OF ENGINEERING

**Figur:** Fire miniaturediagrammer arrangeret diagonalt nedad fra venstre mod højre, forbundet af zoom-pile, med niveau-etiketter nederst: Level 1 Context, Level 2 Containers, Level 3 Components, Level 4 Code. I Level 1-diagrammet er ét element (Internet Banking System) rammet ind med grøn ramme, og en grøn pil mærket "Zoom in" fører til hele Level 2-diagrammet, som er indrammet grønt. I Level 2 er API Application rammet ind med gul ramme, og en gul "Zoom in"-pil fører til Level 3-diagrammet, indrammet gult. I Level 3 er én komponent (Mainframe Banking System Facade) rammet ind med rød ramme, og en rød "Zoom in"-pil fører til Level 4-klassediagrammet, indrammet rødt. Hvert niveau er altså en indzoomning på ét enkelt element fra niveauet over.

<!-- side 6 -->

ABSTRACTIONS IN C4

                                                                                                       A software system is made up of
                                                                                                       one or more
                                                                                                       containers (web applications,
                                                                                                       mobile apps, desktop applications,
                                                                                                       databases, file systems, etc), each
                                                                                                       of which contains one or more
                                                                                                       components, which in turn are
                                                                                                       implemented by one or more
                                                                                                       code elements (e.g. classes,
                                                                                                       interfaces, objects, functions, etc).



                   Source: https://c4model.com/




  AARHUS
  UNIVERSITY                                      SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

**Figur:** Hierarkisk diagram i tre lag med stiplede forbindelseslinjer. Øverst én bred mørkeblå boks "Software System". Under den tre bokse i rækken "Container (e.g. client-side web app, server-side web app, console application, mobile app, microservice, database schema, file system, etc)" — den midterste er fremhævet mørkeblå, de to andre nedtonede. Under hver container tre "Component"-bokse (den midterste under den fremhævede container er mørkere blå). Under hver komponent tre "Code"-bokse (igen én fremhævet). En prikket lodret linje løber gennem den fremhævede sti Software System → Container → Component → Code, mens de øvrige forbindelser er stiplede skrå linjer — indeholdelseshierarkiet er ét-til-mange på hvert niveau.

<!-- side 7 -->

LEVEL 1: SYSTEM CONTEXT DIAGRAM
                                                                           A zoomed out view showing a big
                                                                           picture of the system landscape.
                                                                           The focus should be on people
                                                                           (actors, roles, personas, etc) and
                                                                           software systems rather than
                                                                           technologies, protocols and other
                                                                           low-level details.
                                                                           It's the sort of diagram that you
                                                                           could show to non-technical
                                                                           people.
                                                                           Similar to a UML context or UC
                                                                           diagram

                          https://c4model.com/
  AARHUS
  UNIVERSITY                                     SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

**Figur:** C4 Level 1-eksempel (Internet Banking System). Tre elementer plus én person, forbundet af stiplede pile med tekst:

<!-- side 8 -->

LEVEL 2: CONTAINER DIAGRAM
                                                                                   A container is a separately
                                                                                   runnable/deployable unit (e.g.
                                                                                   a separate process space) that
                                                                                   executes code or stores data.
                                                                                   The Container diagram shows
                                                                                   the high-level shape of the
                                                                                   software architecture and how
                                                                                   responsibilities are distributed
                                                                                   across it.
                                                                                   The interfaces internally and
                                                                                   externally between systems will
                                                                                   be visible at this level (red)




  AARHUS
  UNIVERSITY                  SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

**Figur:** C4 Level 2-eksempel. Personen "Personal Banking Customer" står øverst. En stiplet systemgrænse omkranser Internet Banking System [Software System] og indeholder fem grønne containere:

<!-- side 9 -->

LEVEL 3: COMPONENT DIAGRAM
                                                                                           Design your
                                                                                           components such that
                                                                                           automatic
                                                                                           unit/component
                                                                                           testing can cover as
                                                                                           much as possible
                                                                                           (green).
                                                                                           And system
                                                                                           integration can be
                                                                                           done manually or with
API Application
                                                                                           less work (red)


          AARHUS
          UNIVERSITY                  SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
          DEPARTMENT OF ENGINEERING

**Figur:** C4 Level 3-eksempel: indmaden i API Application. Øverst de to grønne containere Single-Page Application og Mobile App. En stiplet grænse mærket "API Application [Container]" omslutter seks komponenter i to rækker.

<!-- side 10 -->

SYSTEM LEVEL INTEGRATION
  Prepare your system integration at the unit level
       • Use testable design with well defined interfaces
       • Defer dependencies to external systems and interfaces by encapsulating at a low level
       • Test your logic first in unit tests
       • Test the interface encapsulations using fakes for external systems – or wait until the
         system integration phase




  AARHUS
  UNIVERSITY                                   SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

<!-- side 11 -->

RELEVANT DESIGN PATTERNS
  Façade pattern
  Repository pattern
  Layered architectural pattern
  Message bus architectural pattern
  Dependency injection at system level
  and other decoupling design and architectural patterns




  AARHUS
  UNIVERSITY                      SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

<!-- side 12 -->

RELEVANT TOOLS AT SYSTEM LEVEL
  Graphical user interface test tools
  In memory database mocks
  Custom made test databases
  Custom made test systems
  Web service simulators
  Web service testers




  AARHUS
  UNIVERSITY                        SPRING 2024   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  DEPARTMENT OF ENGINEERING

