---
title: "UsingAndTestingEvents"
source: "csfiles/home_dir/Events/UsingAndTestingEvents.pdf"
modul: "C# materiale"
pages: 20
type: "slides"
vision: "done"
note: "delt mellem flere moduler"
---
# UsingAndTestingEvents

<!-- side 1 -->

DESIGN FOR TESTABILITY
- USING AND TESTING
C# EVENTS

AARHUS                           SWT    PETER HØGH MIKKELSEN
UNIVERSITY           05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
TECHNICAL SCIENCES

<!-- side 2 -->

DECOUPLING THE GENERAL “DATA-UPDATE”
PROBLEM
                           Let’s take a look at the general “data-update” problem.

                      Provider contains                                                             Consumer uses data
                      data which changes                                                            and would like to do
                      every now and them      How can we realize                                    some update based
                                                this coupling?                                      on changes in data


                             Provider                                                                           Consumer

                      - data: uint

                      + SetData(x: uint)




 AARHUS                                                   SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                    05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                           2
 TECHNICAL SCIENCES

<!-- side 3 -->

GOF OBSERVER
                      Subject is an abstract base                                                                 Observer is an interface that is implemented
                      class for all subjects (i.e. things                                                         by all Observers, i.e. objects that wish to be
                      that get updated)                                                                           informed of data changes
                                                                                                 *         « interface »
                                                         Subject
                                                                                                            Observer
                                                                         *
                                                  + Attach(Observer)                                 + Update()
                                                  + Detach(Observer)
                                                  + Notify()




                                                    ConcreteSubject                                    ConcreteObserver


                                                  - subjectState                                     + Update()

                                                  + GetState()


   ConcreteSubject inherits from Subject.                                                                         ConcreteObserver implements the Observer
   This is the actual class that must be                                                                          interface. This is the actual class that must
   monitored (in our case, Provider)                                                                              receive updates (in our case, Consumer)


 AARHUS                                                                           SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                            05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                                   3
 TECHNICAL SCIENCES

<!-- side 4 -->

                             GOF OBSERVER – PULL AND PUSH
                                      VARIANTS
                                                                                                                                                                                  o1:
                                                                                                                             : ConcreteSubject

          Pull variant
                                                                                                                                                                           ConcreteObserver

                                                                                                                                                            Attach(o1)
                                                                                           _observers.Add(o1)

          (Observer pulls state from
          subject)                                                                                                     SetData(2)


                                                                                                                                                 Notify()

                                                                                           Store new data                                                    Update()

                                                                                                                                                       GetSubjectState()

                                                                                                                                                            subjectState

          Push variant
          (Subject pushes state to
          observer)                                                                                                         : ConcreteSubject
                                                                                                                                                                                      o1:
                                                                                                                                                                               ConcreteObserver

                                                                                                                                                              Attach(o1)
                                                                                     _observers.Add(o1)
            This makes the Subject and Observer classes
            depend on the data exchanged for the
            standard implementation L                                                                               SetData(2)


                                                                                                                                                  Notify()
            This is the variant most often used with C#                              Store new data                                                    Update(subjectState)
            events, but fixes this problem
AARHUS                                                               SWT     PETER HØGH MIKKELSEN
UNIVERSITY                                                05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                                                              4
TECHNICAL SCIENCES

<!-- side 5 -->

GOF IMPLEMENTATION VS. C# EVENTS
The following slides will compare the standard implementation of GoF Observer
Pattern with the built in C# event feature

• C# event is not the same as the synchronization feature "event" used between
  threads and provided by the operating system, eg. AutoResetEvent
• C# events detaches the type of the data exchanged from the definition of the
  classes and makes it easier to implement many subjects for one observer and
  many subjects in one provider.
• The GoF standard implementation can encompass this through extension with
  generic interfaces and base classes



  AARHUS                                    SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                     05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                           5
  TECHNICAL SCIENCES

<!-- side 6 -->

GOF OBSERVER – PUSH VARIANTS

                                                                                    Step 1: Attach



                                                                                                                           o1:
                                                 : ConcreteSubject
                                                                                                                    ConcreteObserver

                                                                                       Attach(o1)
                      _observers.Add(o1)




                                           SetData(2)


                                                                          Notify()

                      Store new data                                            Update(subjectState)




                                                                                 Step 2: Update
 AARHUS                                                     SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                      05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                       6
 TECHNICAL SCIENCES

<!-- side 7 -->

ATTACH – SUBJECT SIDE
                      GoF Observer pattern standard implementation (Subject)
                      private List<PulseObserver> observers = new List<PulseObserver>();
                      private PulseData currentPulse;

                      public void Attach(PulseObserver observer)
                      {
                          observers.Add(observer);
                      }

                      C# built in event mechanism
                      public class PulseDataEventArgs : EventArgs
                      {
                          public PulseData pulseData {get; set; }                                      A provider can have many
                      }                                                                                different subjects
                      private PulseData currentPulse;

                      public event EventHandler<PulseDataEventArgs>? PulseValueEvent;

                      // No code needed for attach! Event is nullable (null = no subscribers)
 AARHUS                                                      SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                       05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                  7
 TECHNICAL SCIENCES

**Figur:** To sammenstillede kodebokse med blå overskriftsbjælker: øverst "GoF Observer pattern standard implementation (Subject)", nederst "C# built in event mechanism". Callout-boksen "A provider can have many different subjects" peger med pil ned på linjen `public event EventHandler<PulseDataEventArgs>? PulseValueEvent;` — dvs. hvert event-felt udgør ét selvstændigt subject. Kommentarlinjen om at attach ikke kræver kode er fremhævet med rød skrift.

<!-- side 8 -->

ATTACH – OBSERVER SIDE

                      GoF Observer pattern standard implementation (Observer)

                      Display(ISubject pulseSubject)
                      {
                          pulseSubject.Attach(this);
                      . . .



                      C# built in event mechanism

                                                                                                            A consumer can call its
                      Display(IPulseMeter pulseMeter)                                                       update method any name
                      {                                                                                     it wants
                          pulseMeter.PulseValueEvent += HandleNewPulse;
                      . . .
                                                                                                            A consumer can attach to
 AARHUS
 UNIVERSITY
                                                            SWT
                                                 05 FEBRUARY 2024
                                                                    PETER HØGH MIKKELSEN
                                                                                                            many subjects
                                                                    DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                       8
 TECHNICAL SCIENCES

**Figur:** Samme to-boks-opstilling (blå bjælker: GoF Observer-implementation øverst, C#-event-mekanisme nederst). To callouts hænger sammen lodret i højre side og peger begge ind i den nederste kodeboks: "A consumer can call its update method any name it wants" ved `pulseMeter.PulseValueEvent += HandleNewPulse;` og under den "A consumer can attach to many subjects". Pointen er, at `+=`-tilmeldingen ikke stiller krav til metodenavnet, modsat GoF hvor observeren skal implementere en fast interface-metode.

<!-- side 9 -->

UPDATE – SUBJECT SIDE

                      GoF Observer pattern standard implementation
                                                                                                            A provider doesn't need to
                      public void Notify()                                                                  know the consumers but
                      {
                          foreach (var observer in observers)                                               Update is assumed to be
                          {                                                                                 the method name
                              observer.Update(currentPulse);
                          }
                      }


                      C# built in event mechanism
                                                                                                            A provider doesn't need to
                      private OnNewPulseData()                                                              know the consumers or the
                      {                                                                                     method names
                          PulseValueEvent?.invoke(this,
                                                  new PulseDataEventArgs()
                                                      {pulseData = currentPulse});

                         // Only one statement needed for update, and only invoke if
 AARHUS                  // PulseValueEvent is not    nullSWT (?)PETER(=
                                                                       HØGH has
                                                                              MIKKELSENsubscribers)
 UNIVERSITY                                      05 FEBRUARY 2024 DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                    9
 TECHNICAL SCIENCES

**Figur:** To-boks-sammenligning. De to callouts er her røde (fremhævet som kernepointe) og peger ind i hver sin kodeboks: den øverste røde boks "A provider doesn't need to know the consumers but Update is assumed to be the method name" hører til `Notify()`/`observer.Update(currentPulse)`, den nederste røde "A provider doesn't need to know the consumers or the method names" hører til `PulseValueEvent?.invoke(...)`. Kontrasten mellem de to callouts er slidets budskab: event-mekanismen fjerner også koblingen til metodenavnet. Kommentarlinjerne nederst er røde.

<!-- side 10 -->

UPDATE – OBSERVER SIDE

                      GoF Observer pattern standard implementation

                      public void Update(PulseData pulseData)
                      {
                          // Handle pulse data




                      C# built in event mechanism

                      private HandleNewPulse(object? sender, PulseDataEventArgs e)
                      {                                                                                                                    A consumer can identify
                          PulseData = e.PulseData;
                                                                                                                                           the provider by checking
                            // Handle pulse data                                                                                           the sender or by using
                                                                                                                                           different methods
                      https://learn.microsoft.com/en-us/dotnet/api/system.eventhandler?view=net-8.0

 AARHUS                                                                     SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                      05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                              10
 TECHNICAL SCIENCES

**Figur:** To-boks-sammenligning som de foregående slides. Rød callout "A consumer can identify the provider by checking the sender or by using different methods" peger ind i den nederste kodeboks ved handler-signaturen `private HandleNewPulse(object? sender, PulseDataEventArgs e)` — pointen knytter sig specifikt til `sender`-parameteren. Under kodeboksen står et understreget hyperlink til Microsofts EventHandler-dokumentation.

<!-- side 11 -->

EXAMPLE

                                                                                  TempChangedEventArgs                           This is the data to be
                                                                                                                                 transferred. This class
                                                                             + Temp : int <<property>>
ITempSensor declares the event                                                                                                   must be known by all
                                                                                                                                 other classes involved
                                        ITempSensor
                          + TempChangedEvent : event




                                        TempSensor                                                                Control

                      + TempChangedEvent : event                                       - HandleTempChangedEvent(
                                                                                            s : object,
                      + SetTemp(newTemp : int) : void                                       e : TempChangedEventArgs) : void
                      - OnTempChanged(e: TempChangedEventArgs)



TempSensor raises an event when the                                                                       Control responds to the “temperature
temperature changes                                                                                       changed”-event


 AARHUS                                                              SWT         PETER HØGH MIKKELSEN
 UNIVERSITY                                               05 FEBRUARY 2024       DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                           11
 TECHNICAL SCIENCES

**Figur:** UML-klassediagram med fire kasser.

<!-- side 12 -->

                     EVENT SOURCE: THE TEMPSENSOR
                     public class TempChangedEventArgs : EventArgs
                     {                                                                 Data type to be transferred via the
                         public int Temp { get; set; }
                     }                                                                 event

                     public interface ITempSensor
                     {                                                               Event: The connection point for
                         event EventHandler<TempChangedEventArgs>? TempChangedEvent; Observers
                     }


                     public class TempSensor : ITempSensor
                     {
                         private int _oldTemp;

                         public event EventHandler<TempChangedEventArgs>? TempChangedEvent;

                         public void SetTemp(int newTemp)
                         {
                             if (newTemp != _oldTemp)                                A new instance of the data
                             {
                                 OnTempChanged(new TempChangedEventArgs { Temp = newTemp});
                                 _oldTemp = newTemp;
                             }
                         }

                         protected virtual void OnTempChanged(TempChangedEventArgs e)
                         {
AARHUS
                             TempChangedEvent?.Invoke(this, e);
                                                                         SWT
                                                                                             Invoke: Sending an instance of the
                                                                              PETER HØGH MIKKELSEN
                         }
UNIVERSITY
TECHNICAL SCIENCES   }
                                                             05 FEBRUARY 2024          data to all connected Observers
                                                                              DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                     12

**Figur:** Tre stablede kodebokse med syntaksfarvning (nøgleord blå, typenavne lyseblå). Fire callouts med pile ind i koden:
- "Data type to be transferred via the event" peger på klassedeklarationen `public class TempChangedEventArgs : EventArgs`.
- "Event: The connection point for Observers" peger på interface-linjen `event EventHandler<TempChangedEventArgs>? TempChangedEvent;` og har desuden en lang pil videre ned til den tilsvarende felt-erklæring `public event EventHandler<TempChangedEventArgs>? TempChangedEvent;` inde i `TempSensor` — de to er samme forbindelsespunkt, deklaration og implementering.
- "A new instance of the data" peger på `OnTempChanged(new TempChangedEventArgs { Temp = newTemp});`.
- "Invoke: Sending an instance of the data to all connected Observers" peger på `TempChangedEvent?.Invoke(this, e);`.
Bemærk at `public event ... TempChangedEvent;` i `TempSensor` er sat i fed, ligesom det bemærkes at `OnTempChanged` er `protected virtual`.

<!-- side 13 -->

                     EVENT RECEIVER: CONTROL
                     public class Control
                     {
                         public int CurrentTemperature { get; set; }
                                                                                              Connecting this to the event
                         public Control(ITempSensor tempSensor)
                         {
                             tempSensor.TempChangedEvent += HandleTempChangedEvent;
                         }

                         private void HandleTempChangedEvent(object? sender, TempChangedEventArgs e)
                         {
                             CurrentTemperature = e.Temp;
                             Regulate();
                         }                                                                                                     The Data will arrive in this parameter
                     }


                                         The method that will be called when new data are ready.
                                         It must have this prototype/signature – actually it is a hidden
                                         interface definition.
                                         It should normally never be public, see last slide.




AARHUS                                                                   SWT     PETER HØGH MIKKELSEN
UNIVERSITY                                                    05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                               13
TECHNICAL SCIENCES

**Figur:** Én kodeboks med syntaksfarvning. Tre callouts med pile:
- "Connecting this to the event" peger på `tempSensor.TempChangedEvent += HandleTempChangedEvent;` (linjen er sat i fed).
- "The Data will arrive in this parameter" peger på parameteren `TempChangedEventArgs e` i handler-signaturen.
- Den store boks nederst ("The method that will be called when new data are ready … hidden interface definition … never be public") har to pile: én til selve `+=`-linjen og én til metodedeklarationen `private void HandleTempChangedEvent(object? sender, TempChangedEventArgs e)` — altså at signaturen er et implicit interface pålagt af event-typen.

<!-- side 14 -->

THINGS TO TEST

When an event is involved between to classes, there are two classes to test (if you provide them
both)


1.    The source – the class which have the event property
2.    The receiver – the class that connects to the event property




     AARHUS                                           SWT     PETER HØGH MIKKELSEN
     UNIVERSITY                            05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                     14
     TECHNICAL SCIENCES

<!-- side 15 -->

TESTING THE EVENT SOURCE
To test the event source, we must ensure that..
  • the UUT raises the expected event …
  • … under the expected circumstances.
  • … with the expected data…
  • AND the UUT doesn't raise the event …
  • … under the circumstances when it should NOT

To do this, we arrange to let the test fixture subscribe to the event from the UUT using a lambda
expression as the receiving method

Then we act by stimulating the UUT to raise the event and assert by investigating the received
data


   AARHUS                                            SWT     PETER HØGH MIKKELSEN
   UNIVERSITY                             05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                    15
   TECHNICAL SCIENCES

<!-- side 16 -->

TEST THE EVENT SOURCE: TEMPSENSOR
                      public class TempSensorUnitTest
                      {
                          private TempSensor _uut;
                          private TempChangedEventArgs? _receivedEventArgs;

                          [SetUp]
                          public void Setup()
                          {                                                                                     Arrange: Test subscribes to event with
                              _uut = new TempSensor();
                              _uut.SetTemp(20);                                                                 a lambda expression
                              // Set up an event listener to check the event occurrence and event data
                              _uut.TempChangedEvent +=
                                  (o, args) =>
                                  {                                                                             It will store the occurrence and data
                                      _receivedEventArgs = args;
                                  };                                                                            when it is called
                          }

                          [Test]
                          public void SetTemp_TempSetToNewValue_EventFired()
                          {
                              _uut.SetTemp(25);                                                                 Act: Make the source raise the event
                              Assert.That(_receivedEventArgs, Is.Not.Null);
                          }

                          [Test]
                          public void SetTemp_TempSetToNewValue_CorrectNewTempReceived()                        Test cases assert data from the event
                          {                                                                                     as stored by the lambda for
                              _uut.SetTemp(25);
                              Assert.That(_receivedEventArgs?.Temp, Is.EqualTo(25));                            occurrence and correctness
                          }
                      }


 AARHUS                                                                      SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                       05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                         16
 TECHNICAL SCIENCES

**Figur:** Én kodeboks med syntaksfarvning ([SetUp]/[Test]-attributter i lyseblåt, kommentar i grønt). Fire callouts med pile ind i præcise kodelinjer:
- "Arrange: Test subscribes to event with a lambda expression" peger på `_uut.TempChangedEvent +=`.
- "It will store the occurrence and data when it is called" peger på lambda-kroppen `_receivedEventArgs = args;`.
- "Act: Make the source raise the event" peger på `_uut.SetTemp(25);` i den første testmetode.
- "Test cases assert data from the event as stored by the lambda for occurrence and correctness" har to pile, én til hver af de to `Assert.That(...)`-linjer (`Is.Not.Null` og `Is.EqualTo(25)`), dvs. den dækker begge testcases.
Ordene *Arrange* og *Act* står i kursiv, hvilket markerer AAA-strukturen.

<!-- side 17 -->

TESTING THE EVENT RECIPIENT
To test the event recipient (UUT), we must ensure that…
  • The UUT subscribes to the event
  • The UUT handles the event correctly


Thus, the test fixture must arrange that a fake event source is injected into the UUT and act
such that the event is raised and assert that it received the event and handled it correctly

Several strategies:
  • Use Nsubstitute to create the fake AND raise the event
  • Define a helper class – a fake – that implements the interface, through which the test
    fixture can raise the event, similar to using Nsubstitute – not shown
  • Let the test fixture itself implement the same interface as the event source and inject
    itself to the UUT – not shown

  AARHUS                                             SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                              05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                    17
  TECHNICAL SCIENCES

<!-- side 18 -->

TEST THE EVENT RECIPIENT USING NSUBSTITUTE

                      [TestFixture]
                      class ControlUnitTests
                      {
                                                                                                                           Arrange: Create a fake of the Interface
                          private Control _uut;                                                                            which has an event for the data
                          private TestTemperatureSource _tempSource;

                          [SetUp]
                          public void Setup()                                                                              Arrange: inject the fake (event source)
                          {                                                                                                in the UUT
                              _tempSource = Substitute.For<ITempSensor>();
                              _uut = new Control(_tempSource);
                          }
                                                                                                                           Act: Make the fake raise the event –
                          [TestCase(25)]                                                              special NSubstitute syntax
                          [TestCase(20)]
                          [TestCase(30)]
                          public void TemperatureChanged_DifferentArguments_CurrentTemperatureIsCorrect(int newTemp)
                          {
                              _tempSource. TempChangedEvent += Raise.EventWith(new TempChangedEventArgs {Temp = newTemp});
                              Assert.That(_uut.CurrentTemperature, Is.EqualTo(newTemp));
                          }
                      }                                                                                                    Assert that UUT is in expected state
                                                                                                                           after event is raised


 AARHUS                                                                   SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                    05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                                                                              18
 TECHNICAL SCIENCES

**Figur:** Én kodeboks med syntaksfarvning; felt-, subscribe- og assert-linjer er sat i fed. Fire callouts med pile:
- "Arrange: Create a fake of the Interface which has an event for the data" peger på `_tempSource = Substitute.For<ITempSensor>();`.
- "Arrange: inject the fake (event source) in the UUT" peger på `_uut = new Control(_tempSource);`.
- "Act: Make the fake raise the event – special NSubstitute syntax" peger på `_tempSource. TempChangedEvent += Raise.EventWith(new TempChangedEventArgs {Temp = newTemp});`.
- "Assert that UUT is in expected state after event is raised" peger på `Assert.That(_uut.CurrentTemperature, Is.EqualTo(newTemp));`.
De tre `[TestCase]`-attributter (25, 20, 30) fodrer parameteren `newTemp`, så samme test køres tre gange. *Arrange*, *Act* og *Assert* står i kursiv.

<!-- side 19 -->

TESTING THE EVENT RECIPIENT
It is NOT enough to call the receiving method directly because:
   • It will not test that the UUT has attached to the event!
   • It is a white box test, which will break, if the UUT is changed such that the
     name of the receiving method is changed!

Therefore: always use a fake class (your own or one created with NSubstitute) to
trigger the event!




   AARHUS                                       SWT     PETER HØGH MIKKELSEN
   UNIVERSITY                        05 FEBRUARY 2024   DESIGN FOR TESTABILITY - USING AND TESTING C# EVENTS
                                                                                                               19
   TECHNICAL SCIENCES

<!-- side 20 -->

AARHUS
UNIVERSITY

