---
title: "InterfacesRecap"
source: "InterfacesRecap.pdf"
modul: "Lektion 03.1+2: Design For Testability"
pages: 14
type: "slides"
vision: "done"
---
# InterfacesRecap

<!-- side 1 -->

  Interfaces
In C++ and C#

Software Test




     Slide 1

<!-- side 2 -->

                   Tags
• Inheritance
• Polymorfi
• Abstract class
• Interface




                   Slide 2

<!-- side 3 -->

                    Primitive example in C++
// GraphicsApp.hpp                                 // Circle.hpp
class GraphicsApp()                                class Circle
{                                                  {
private:                                             private:
  vector<Circle> circles;                              double radius;
public:                                                Coord center;
  void AddCircle(Circle* c)                          public:
  {                                                    void setCenter(Coord c) { center = c;}
    circles.push_back(c);                              double getArea() { return …; }
  }                                                }

    double getTotalArea()
    {
      double area = 0;
      for(…)                                       // Rectangle.hpp
        area += circles[i]->getArea();             class Rectangle
      return area;                                 {
    }                                                private:
}                                                      double width;
                                                       double length;
                                                       Coord center;

                                                       public:
                                                         void setCenter(Coord c) { center = c;}
                                                         double getArea() { return …; }
                                                   }
                                         Slide 3

**Figur:** Fire kodebokse i to kolonner. Venstre kolonne: én stor boks `// GraphicsApp.hpp` med klassen `GraphicsApp()`, der har `private: vector<Circle> circles;` og `public: void AddCircle(Circle* c) { circles.push_back(c); }` samt `double getTotalArea() { double area = 0; for(…) area += circles[i]->getArea(); return area; }`. Højre kolonne: to mindre bokse over hinanden — øverst `// Circle.hpp` (`class Circle` med `private: double radius; Coord center;` og `public: void setCenter(Coord c) { center = c;}` samt `double getArea() { return …; }`), nederst `// Rectangle.hpp` (`class Rectangle` med `private: double width; double length; Coord center;` og samme to public-metoder). Pointen i opstillingen: `GraphicsApp` er hårdt bundet til den konkrete type `Circle` — `Rectangle` har identisk grænseflade, men kan ikke bruges. Ingen pile mellem boksene.

<!-- side 4 -->

          Polymorphy should be used
• The GraphicsApp really does not care if the shapes are of a
  particular implementation                // GraphicsApp.hpp
                                              class GraphicsApp()
   – Circle, Rectangle, etc.                  {
                                              public:
                                                void AddCircle(Circle* c)
                                                {
• They only care about what they                }
                                                  circles.push_back(c);

  can do with them                                double getTotalArea()
   – Get the area, set the center, etc.           {
                                                    double area = 0;
                                                    for(…)
                                                       area += circles[i]->getArea();
                                                    return area;
                                                  }
                                              }

• So, by using the shapes’ interface instead of their
  implementation, we can lower the coupling between client
  and dependency

                                    Slide 4

**Figur:** Tekst i venstre halvdel, én kodeboks i højre halvdel på højde med de to første punkter. Kodeboksen `// GraphicsApp.hpp` er den samme som på foregående side, men med `private`-medlemmet fjernet: kun `public: void AddCircle(Circle* c) { circles.push_back(c); }` og `double getTotalArea() { double area = 0; for(…) area += circles[i]->getArea(); return area; }`. Parametertypen er stadig `Circle*`, dvs. koblingen til den konkrete klasse er endnu ikke brudt.

<!-- side 5 -->

             Interfaces – what’s the fuss?
• In C++, we can use abstract classes to define classes without
  any data or code - what’s their use?


Circle implements IShape
                                                       // IShape.hpp
                                                       class IShape
// Circle.hpp                                          {
class Circle : public IShape                             public:
{                                                          virtual void setCenter(Coord c)=0;
  private:                                                 virtual double getArea()=0;
    double radius;                                     }
    Coord center;
  public:
    void setCenter(Coord c) { center = c;}
    double getArea() { return …; }
}


                                    IShape defines the interface of Circle



                                             Slide 5

**Figur:** To kodebokse forbundet med to callout-bokse (lyseblå med sort kant) og sorte pile. Nederst til venstre boksen `// Circle.hpp` med `class Circle : public IShape`, `private: double radius; Coord center;`, `public: void setCenter(Coord c) { center = c;}` og `double getArea() { return …; }`. Til højre boksen `// IShape.hpp` med `class IShape { public: virtual void setCenter(Coord c)=0; virtual double getArea()=0; }`. Callout'en «Circle implements IShape» sidder over Circle-boksen og har en pil ned mod arve-linjen `class Circle : public IShape`. Callout'en «IShape defines the interface of Circle» sidder nederst i midten og har en pil op mod IShape-boksen.

<!-- side 6 -->

                    Interfaces to the rescue!
// GraphicsApp.hpp                                // Shape.hpp
class GraphicsApp()                               class IShape
{                                                 {
public:                                             public:
  void AddShape(IShape* c)                            virtual void setCenter(Coord c) = 0;
  {                                                   virtual double getArea() = 0;
    shapes.push_back(c);                          }
  }

    double getTotalArea()
    {
      double area = 0;                            // Circle.hpp
      for(…)                                      class Circle : public IShape
        area += shapes[i]->getArea();             {
      return area;                                   // Rectangle.hpp
                                                    private:
    }                                                class  Rectangle
                                                       double  radius; : public IShape
}                                                    { Coord center;
                                                       private:
                                                    public:
                                                          double
                                                       void       width;
                                                             setCenter(Coord  c) { center = c;}
                                                          double
                                                       double     length; { return …; }
                                                               getArea()
                                                  }       Coord center;

Polymorphy kicks in here                                public:
                                                          void setCenter(Coord c) { center = c;}
                                                          double getArea() { return …; }
                                                    }
                                        Slide 6

**Figur:** Tre kodebokse plus en callout. Venstre: `// GraphicsApp.hpp` med `class GraphicsApp()`, `public: void AddShape(IShape* c) { shapes.push_back(c); }` og `double getTotalArea() { double area = 0; for(…) area += shapes[i]->getArea(); return area; }`. Øverst til højre: `// Shape.hpp` med `class IShape { public: virtual void setCenter(Coord c) = 0; virtual double getArea() = 0; }`. Nederst til højre: to overlappende bokse i kortstak — bagerst `// Circle.hpp` med `class Circle : public IShape`, forrest `// Rectangle.hpp` med `class Rectangle : public IShape`, `private: double width; double length; Coord center;`, `public: void setCenter(Coord c) { center = c;}` og `double getArea() { return …; }`. Ordene `IShape` i `AddShape(IShape* c)`, i `class IShape`, i `class Circle : public IShape` og i `class Rectangle : public IShape` er understreget med rødt for at markere det fælles type-bånd. En lyseblå callout «Polymorphy kicks in here» nederst til venstre peger med en pil op mod linjen `area += shapes[i]->getArea();`.

<!-- side 7 -->

                    Interfaces in a nutshell
• Basically, implementing an interface is like signing a contract:
  “I promise to implement…”
• When this promise is fulfilled, the implementor (Circle) can
  be used anywhere the interface type (IShape) is referenced.
                                                       // Shape.hpp
                                                       class IShape
// Circle.hpp                                          {
class Circle : public IShape                             public:
{                                                          virtual void setCenter(Coord c)=0;
  private:                                                 virtual double getArea()=0;
    double radius;                                     }
    Coord center;
  public:
    void setCenter(Coord c) { center = c;}
    double getArea() { return …; }
}


                                    Circle promises to implement
                                    getArea() and setCenter()
                                             Slide 7

**Figur:** To kodebokse med én fælles callout. Venstre boks `// Circle.hpp`: `class Circle : public IShape`, `private: double radius; Coord center;`, `public: void setCenter(Coord c) { center = c;}`, `double getArea() { return …; }`. Højre boks `// Shape.hpp`: `class IShape { public: virtual void setCenter(Coord c)=0; virtual double getArea()=0; }`. Den lyseblå callout «Circle promises to implement getArea() and setCenter()» står nederst i midten og har to pile: én skråt op til venstre mod `class Circle : public IShape` og én skråt op til højre mod IShape-boksen — kontrakten binder de to sammen.

<!-- side 8 -->

             What is an interface?
• An interface is…
   – A contract – “I promise to provide…”
   – A list of interaction points (“points of connection”)
   – A focus on functionality instead of implementation
   – Something that separates and hides implementation from
     users
   – An interface is a type definition.




                            Slide 8

<!-- side 9 -->

               Interfaces in C++ vs. C#
• Thus, in C++ we have (somewhat awkward ) language support
  for defining interfaces
    // Shape.hpp                               All methods are pure virtual.
    class IShape                               There are no member variables.
    {                                          That makes the class an interface
      public:
        virtual void setCenter(Coord c)=0;
        virtual double getArea()=0;
    }


• In C#, the interface is a built-in construct – but the idea is the
  same: To define the interface of all implementing classes
    // Shape.cs
    interface IShape
    {
      void setCenter(Coord c);
      double getArea();
    }
                                     Slide 9

**Figur:** To kodebokse plus en fremhævet kommentarboks. Øverste boks `// Shape.hpp`: `class IShape { public: virtual void setCenter(Coord c)=0; virtual double getArea()=0; }`; ved siden af den en lyseblå boks med monospace-teksten «All methods are pure virtual. There are no member variables. That makes the class an interface» (nøgleordene fremhævet med fed). Nederste boks `// Shape.cs`: `interface IShape { void setCenter(Coord c); double getArea(); }` — samme grænseflade uden `virtual`/`= 0`-syntaksen.

<!-- side 10 -->

          Interfaces in C# - definition
• An interface declares methods etc., but contains no
  implementation of them
• An interface is declared by the keyword interface.
• Anything defined in an interface is automatically public
• The interface may hold
   – method declarations,
                                               // IShape.cs
   – properties,                               public interface IShape
                                               {
   – events                                      // Property
   – (and indexers, not used in this course)     Coord Center {get; };
                                                 // Method
                                                 double GetArea();
                                               }




                                   Slide 10

**Figur:** Kodeboks nederst til højre ud for punktlisten: `// IShape.cs` med `public interface IShape { // Property\n  Coord Center {get; };\n  // Method\n  double GetArea(); }`. Kommentarerne i koden mapper de to medlemmer til punkterne «properties» og «method declarations» i listen til venstre.

<!-- side 11 -->

                      Interfaces in C# -
              Definition and implementation
• Implementation is declared in the class definition.                     // IShape.cs
• A class may implement several interfaces                                interface IShape
                                                                          {
• The class must implement at least the methods                             Coord Center {get; };
                                                                            double GetArea();
  and properties defined in the interface(s)                              }

• The class can also implement other elements,
  specific for the class
• When implemented, the class can be considered of
  the interface’s type

// Square.cs                                            // Circle.cs
class Square : IShape                                   class Circle : IShape
{                                                       {
  public double SideLength { get; private set;}           public double Radius { get; private set;}
  public Coord Center { get; private set;}                public Coord Center { get; private set;}
  public double GetArea(){…}                              public double GetArea(){…}
}                                                       }



                                             Slide 11

**Figur:** Tre kodebokse. Øverst til højre `// IShape.cs`: `interface IShape { Coord Center {get; }; double GetArea(); }`. Nederst side om side to implementeringer: venstre `// Square.cs` med `class Square : IShape { public double SideLength { get; private set;} public Coord Center { get; private set;} public double GetArea(){…} }`, højre `// Circle.cs` med `class Circle : IShape { public double Radius { get; private set;} public Coord Center { get; private set;} public double GetArea(){…} }`. I begge klasser er de to medlemmer, der stammer fra interfacet (`Center` og `GetArea()`), sat med fed, mens den klassespecifikke property (`SideLength` hhv. `Radius`) står med normal vægt — visuel markering af «at least the methods and properties defined in the interface» plus egne tilføjelser.

<!-- side 12 -->

                         Interfaces in C# -
                        Reference and use
• The interface is used as a type                                 // IShape.cs
                                                                  interface IShape
• All implementors of the interface can be used as                {
                                                                    Coord Center {get; };
  type of the interface.                                            double GetArea();
                                                                  }




                                             Objects of any class that implements
                                             IShape may be stored in _shapes
    // Shape.cs
    class GraphicsApp
    {
      private list<IShape> _shapes;
      public void AddShape(IShape s){ _shapes.Add(s);}
      public double GetTotalArea()
      {
        double res = 0;                             Objects of any class that implements
        for each s in shapes:                       IShape may be used as argument for
          res += s.GetArea();
        return res;                                   AddShape()
    }
                                        Slide 12

**Figur:** Kodeboks øverst til højre `// IShape.cs`: `interface IShape { Coord Center {get; }; double GetArea(); }`. Nederst en bred kodeboks `// Shape.cs` med `class GraphicsApp { private list<IShape> _shapes; public void AddShape(IShape s){ _shapes.Add(s);} public double GetTotalArea() { double res = 0; for each s in shapes: res += s.GetArea(); return res; }`. To grønne callout-bokse med sorte pile peger ind i koden: «Objects of any class that implements IShape may be stored in _shapes» peger ned mod feltet `private list<IShape> _shapes;`, og «Objects of any class that implements IShape may be used as argument for AddShape()» peger op mod parameteren i `public void AddShape(IShape s)`.

<!-- side 13 -->

                 Some properties of C# interfaces
• A C# interface is like an abstract base class: Any non-abstract type that implements
  the interface must implement all members.
• An interface cannot be instantiated directly.
• Interfaces can contain events, indexers, methods, and properties.
• Interfaces cannot contain implementation of methods.
• Interfaces cannot contain member variables, only properties
• Classes and structs can implement multiple interfaces.
• An interface itself can itself inherit from one or multiple interfaces.




                                           Slide 13

<!-- side 14 -->

                            Interfaces in UML

                           «interface»
                             IShape
GraphicsApp
                       + GetArea (): double
                       + Center : Coord {get}




              Square                            Circle




                                                         Slide 14

**Figur:** To UML-notationer for samme forhold side om side. Venstre — klassediagram i «ball-and-socket»-fri form: boksen `GraphicsApp` med en åben pil (afhængighed/association) mod interface-boksen, der har tre rum: stereotypen «interface» og navnet *IShape* i kursiv øverst, derunder operationerne `+ GetArea (): double` og `+ Center : Coord {get}`. Fra `IShape` går to stiplede linjer med hul trekantsspids (realisering) ned til to bokse, `Square` til venstre og `Circle` til højre. Højre — samme relation i lollipop-notation: boksen `Square` og boksen `Circle` har hver en lille cirkel på en pind ovenpå, begge mærket `IShape`.

