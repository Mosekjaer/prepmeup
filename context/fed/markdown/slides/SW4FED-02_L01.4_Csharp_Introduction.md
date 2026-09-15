# L01.4 – C# Introduction

## Metadata

- **Lektion:** L01 – C# Introduction
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L01/Csharp-Introduction.pdf (17 slides)
- **Emner dækket:**
  - Baggrund: Anders Hejlsberg og Mads Torgersen
  - Motivationen bag sproget C#
  - Forskelle fra C++
  - Klasser i C# og nye keywords (internal, readonly, sealed, override, new)
  - foreach-statement over arrays og IEnumerable
  - Exception handling med finally
  - Properties og automatically implemented properties
  - Object initializers
  - Interfaces: definition, implementering og de tre måder at referere til dem (cast, `as`, `is`)

---

## 1. Hvem har lavet C#?

**Anders Hejlsberg** er chief architect of C#. Dansk ingeniør (næsten) fra DIA (DTU). Skaberen af Turbo Pascal og Delphi mens han arbejdede for Borland.

**Mads Torgersen** er Principal Program Manager, VS Managed Languages. Før han kom til Microsoft, arbejdede Mads som lektor i datalogi på Aarhus Universitet, hvor han var en del af gruppen, der udviklede wildcards til Java generics.

## 2. Hvorfor lave et nyt sprog?

Målene var:

- At skabe et sprog, hvor alting virkelig *er* et objekt.
- At muliggøre konstruktion af robust og durable software.
- At forenkle C++, men samtidig bevare de skills og investeringer, programmørerne allerede har.

Syntaksen for C# ligger tæt op ad C++ og Java. I C# findes der ikke `delete`, ikke multiple inheritance og ikke header files.

## 3. C#'s forskelle fra C++

C# kompilerer til machine-independent kode, som kører i et managed execution environment kaldet CLR.

Garbage Collection (GC) kombineret med afskaffelsen af pointers — i C# er begrænset brug af pointers tilladt inde i kode markeret `unsafe`.

Kraftfulde reflection-muligheder.

Ingen header files. Al kode er scoped til packages eller assemblies. Ingen problemer med at deklarere én klasse før en anden med circular dependencies inden for samme assembly.

Alle klasser nedstammer fra `Object` og skal allokeres på heapen med `new`-keywordet.

Videre:

- Har interfaces med multiple inheritance af interfaces, men single inheritance af implementationer.
- Inner classes.
- Intet koncept om at nedarve en klasse med et specificeret access level.
- Ingen globale funktioner eller konstanter — alt tilhører en klasse.
- Arrays og strings med indbygget længde og bounds checking.
- `.`-operatoren bruges altid; ingen `->` eller `::`.
- `null` og `bool` er keywords.
- Alle værdier initialiseres før brug.
- Man kan ikke bruge integers til at styre `if`-statements.
- `try`-blokke kan have en `finally`-clause.

## 4. Klasser i C#

Klasser virker næsten som i C++, men introducerer nogle nye begreber:

- `internal` — kan kun kaldes fra samme assembly
- `readonly` — datafeltets værdi kan ikke ændres efter instantiering
- `properties` — nyt begreb (kommer fra VB), forklares senere
- `base` — kald af basisklassens konstruktør/metode
- `sealed` — der kan ikke nedarves fra denne klasse
- `abstract` — nyt keyword til kendt begreb
- `override`, `new` — til versionering
- og andre

Bemærk at access modifiers skal skrives hver gang; default er `private`.

```csharp
public class myClass
{
    private int x1;
    private int x2;
    public int GetX1 ()
    {
        return x1;
    }
}
```

## 5. foreach-statement

Iteration af arrays:

```csharp
public static void Main(string[] args)
{
   foreach (string s in args)
     Console.WriteLine(s);
}
```

Iteration af `IEnumerable`-collections:

```csharp
List<Account> accounts = Bank.GetAccounts(...);
foreach (var a in accounts)
{
   if (a.Balance < 0) Console.WriteLine(a.CustName);
}
```

## 6. Exception handling

Næsten som i C++, men har også mulighed for en `finally`-blok.

```csharp
try {
// Læg beslag på en ressource, f.eks. en fil
}
catch (SomeExceptionType e) {
// Håndter den konkrete fejlsituation
}
finally {
// Frigiv ressourcen uanset hvad
}
```

Exceptions virker på tværs af alle sprogene på .NET-platformen.

## 7. Properties

Properties er "smart fields" — naturlig syntaks, accessors og inlining.

```csharp
public class Button: Control
{
   private string m_caption;

   public string Caption {
      get {
         return m_caption;
      }
      set {
         m_caption = value;
         Repaint();
      }
   }
}
```

Anvendelse:

```csharp
Button b = new Button();
b.Caption = "OK";
String s = b.Caption;
```

## 8. Automatically implemented properties

Tillader at udtrykke trivielle properties med mindre kode. Det man skriver:

```csharp
public class Person
{
  public string Name { get; set; }
}
```

kompileres som:

```csharp
public class Person
{
  private string <Name>k__BackingField;
  public string Name
  {
    get { return <Name>k__BackingField; }
    set { <Name>k__BackingField = value; }
  }
}
```

## 9. Object initializers

Med object initializers kan man oprette et objekt og initialisere alle relevante properties i ét udtryk.

```csharp
public class Person
{
  public int Age { get; set; }
  public string Name { get; set; }

  public Person() { }
  public Person(string name)
  {
    Name = name;
  }
}
```

```csharp
Person tom1 = new Person();
tom1.Name = "Tom";
tom1.Age = 6;

Person tom2 = new Person("Tom");
tom2.Age = 6;

Person tom3 = new Person() { Name="Tom", Age = 6 };
Person tom4 = new Person { Name="Tom", Age = 6 };
Person tom5 = new Person("Tom") { Age = 6 };
```

De tre sidste linjer bruger object initializer-syntaksen.

## 10. Datatypen Interface

Et interface er en samling semantisk relaterede abstrakte medlemmer — funktioner, properties og events.

Et interface kan bestå af blot én medlemsfunktion, eller det kan indeholde mange.

Begrebet interface findes ikke i C++, men svarer til en abstrakt basisklasse uden datamedlemmer, og hvor alle medlemsfunktionerne er rent virtuelle.

### Eksempel på definition

Medlemmerne er implicit abstract.

```csharp
// The pointy behavior.
public interface IPointy
{
    // an interface contains functions
    byte GetNumberOfPoints();

    // an interface can also have properties...
    byte Points{get; set;}
    // but can't have datamembers
}
```

### Implementering af et interface

```csharp
public class Triangle : Shape, IPointy
{
    public Triangle(string name): base(name)
    {}
    public Triangle()
    {}
    public override void Draw()
    {
         Console.WriteLine("Drawing " + PetName + " the Triangle");
    }
    // IPointy interface
    public byte GetNumberOfPoints()
    {
         return 3;
    }
}
```

## 11. Interface references — tre måder

### 1. Typecast med exception handling (den langsomme)

```csharp
Triangle t = new Triangle();
IPointy itfPt;
try
{
    itfPt = (IPointy)t;
    ...
}
Catch(InvalidCastException e)
{Console.WriteLine("OOPS!    Not pointy...");}
```

### 2. `as`-operatoren

```csharp
Triangle t = new Triangle();
IPointy itfPt;
itfPt = t as IPointy;
if (itfPt != null)
      Console.WriteLine("Got interface using as keyword");
else
      Console.WriteLine("OOPS! Not pointy...");
```

`as`-operatoren kan kun bruges på reference types. Det er best practice at bruge `as`, når man skal typecaste en reference type — når man kan.

### 3. `is`-operatoren

```csharp
Triangle t = new Triangle();
IPointy itfPt;

if(t is IPointy)
  {
   itfPt = (IPointy)t;
    ...
  }
else
 Console.WriteLine("OOPS!    Not pointy...");
```

`is`-operatoren kan også bruges på value types. Det er best practice at bruge `is` sammen med normal type cast på value types.

## 12. References & Links

- C# programming guide: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/
- C# naming guideline: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions og https://csharpcodingguidelines.com/
- C# reference: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/
- C# Language Specification: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/introduction
