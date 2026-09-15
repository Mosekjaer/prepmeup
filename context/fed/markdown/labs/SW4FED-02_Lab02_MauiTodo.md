# Lab 02 – MauiTodo App

## Metadata

- **Lektion:** L02 – Lab 02 MauiTodo
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L02/Lab/FED Lab 02 MauiTodo.pdf
- **Emner dækket:**
  - Oprettelse af MAUI-projekt med SQLite-net
  - Grid-layout med Label, Entry, DatePicker, Button og ScrollView
  - Code-behind med database-instans og async-initialisering
  - Event handler til Add-knappen
  - Data binding med CollectionView, ItemTemplate og DataTemplate
  - ObservableCollection som items source
  - Refaktorering: flyt ItemsSource-binding fra code-behind til XAML

---

## Formål

At få erfaring med data binding.

Der skal laves en MauiTodo App for at øve og demonstrere en forståelse af, hvordan data binding fungerer i .NET MAUI. Det er en simpel .NET MauiTodo App til at oprette en liste over opgaver.

## Forudsætninger

Du har læst kapitel 3 i *MAUI in Action*.

## Delopgave 1: Todo List

Opret et nyt .NET MAUI-projekt kaldet **MauiTodo** i Visual Studio. Brug pakken SQLite-net. Du skal installere NuGet-pakkerne `sqlite-net-pcl` og `sqlite-net-cipher` i Visual Studio.

Tilføj en mappe kaldet `Models`, og du skal bruge en `TodoItem`-klasse med properties, der repræsenterer to-do-items. `TodoItem`-klassen og `Database`-klassen er implementeret i filerne `TodoItem.cs` og `Database.cs` — download fra Brightspace.

### I XAML-filen (MainPage.xaml)

Slet det præ-genererede `ScrollView` og alt indeni. Det burde efterlade kun `<ContentPage...> </ContentPage>`-taggene.

Inde i `<ContentPage...> </ContentPage>`-taggene erstattes med et nyt layout, et Grid og child-elementer (Label, Entry, Date picker, Button, ScrollView).

**1.** Tilføj et åbnende og lukkende Grid-tag med fem rækker.

```xml
<Grid RowDefinitions="1*, 1*, 1*, 1*, 8*"
      MaximumWidthRequest="400"
      Padding="20">

</Grid>
```

Note: Label, Entry, Date picker, Button og ScrollView-layoutet skal ligge mellem Grid'ets åbnende og lukkende tags.

Inde i Grid'et:

**2.** Tilføj et `Label` med sidetitlen "Maui Todo". Brug et `Label` med `Text`-propertyen.

```xml
<Label ...
       Text="Maui Todo"
       .../>
```

**3.** Tilføj en tekst-`Entry`, hvor brugeren kan indtaste titlen på nye to-do-items, og giv den navnet `TodoTitleEntry`.

```xml
<Entry ...
       ...
       x:Name="TodoTitleEntry" />
```

**4.** Tilføj en `DatePicker` til due date, og giv den navnet `DueDatePicker`.

```xml
<DatePicker ...
            ...
            x:Name="DueDatepicker" />
```

**5.** Tilføj en `Button` til at bekræfte tilføjelsen af nye to-do-items, med et `Clicked`-event og en event handler ved navn fx `Button_Clicked`.

```xml
<Button ..
        Text="Add"
            ...
        Clicked="Button_Clicked" />
```

**6.** Tilføj et `ScrollView` på femte række (row 4), så listen kan udvides og scrolles, når den strækker sig ud over siden. Note: du skal bruge et `Label` med navnet `TodosLabel` inde i `<ScrollView ..> ... </ScrollView>`.

```xml
<ScrollView Grid.Row="4">
    <Label ...
           x:Name="TodosLabel" />
</ScrollView>
```

### I code-behind (MainPage.xaml.cs)

Slet metoden `OnCounterClicked` — den erstattes med den nye logik.

Tilføj kode til code-behind for at koble funktionaliteten sammen (se forelæsningsslidesene):

**1.** Tilføj en instans af databasen og værdierne af to-do'erne i databasen til at holde brugerens nye to-do-item.

```csharp
string _todoListData = string.Empty; //Values of the to-do items
readonly Database _database; //Stores an instance of the database class
```

**2.** Opdatér klassens constructor. Inde i constructoren oprettes en instans af `Database`-klassen, som tildeles `_database`-feltet. Vigtigt: slet **ikke** `InitializeComponent();`.

```csharp
_database = new Database(); //create an instance of the database class & assign it to the _database field.
_ = Initialize(); //Uses the discard variable to call our Initialize method
```

**3.** Tilføj en ny `Task`-metode til at initialisere siden ved load (brug async/await) — se forelæsningsslidesene.

**4.** Tilføj en event handler-metode (`Button_Clicked`) til at respondere på button clicks og tilføje en ny to-do til databasen (brug async/await) — se forelæsningsslidesene.

### Resultat

Kør appen. Når du klikker på Add-knappen, henter en event handler værdierne fra `Entry` og `DatePicker` og bruger dem til at oprette et nyt to-do-item med de værdier som henholdsvis titel og due date. Derefter tilføjes de til listen af to-do-items på skærmen.

## Delopgave 2: Bindings

Anvend data binding i MauiTodo-appen.

### I XAML-filen (MainPage.xaml)

**1.** Erstat `ScrollView` med et `CollectionView`. Brug `CollectionView`, `ItemTemplate` og `DataTemplate` til at anvende data binding i XAML (se forelæsningsslidesene).

Giv `CollectionView`'et et navn, fx `TodosCollection`, så vi kan referere til det i kode.

```xml
<CollectionView ...
    x:Name="TodosCollection">
```

Inde i `CollectionView`'et tilføjes et element, der definerer `ItemTemplate`-propertyen:

```xml
<CollectionView.ItemTemplate>
```

Brug en `DataTemplate` til at definere, hvordan hvert item i collectionen præsenteres. Den tildeles `ItemTemplate`-propertyen ved at være nested som direkte child:

```xml
<DataTemplate>
```

**2.** Til ekstra layout kan du tilføje et Grid, en Checkbox og to Labels.

Grid'et kan have:

- To rækker: én med en højde der tilpasser sig indholdet, og én med en højde på fx 50.
- To kolonner: én på 2/7 af bredden, og én på 5/7 af bredden.

Checkbox'en placeres i første kolonne, første række i item-layoutet.

De to Labels:

- Tilføj ét `Label` i anden kolonne, første række, og bind `Text`-propertyen på labelen til `Title`-propertyen på item'et.
- Tilføj det andet `Label` i anden kolonne, anden række, og bind `Text`-propertyen til `Due`-propertyen på item'et. Da `Due` er en `DateTime`, angives en formateringsregel.

### I code-behind

**1.** Tilføj en `ObservableCollection` af `TodoItem`s helt øverst i klassen (før de private member-definitioner). Husk at tilføje det nødvendige `using`-statement øverst i filen.

**2.** I constructoren sættes `ItemsSource`-propertyen på `CollectionView`'et til `ObservableCollection`'en.

**3.** Slet den private `_todoListData`-string og hver linje, der refererer til den, for at komme af med den kode, der ikke længere bruges.

**4.** Tilføj ny logik to steder: til at håndtere initialisering af collectionen og til at opdatere den, når en bruger tilføjer et nyt to-do-item.

- Tilføj to-do-item'et i loopet til `ObservableCollection`'en i `foreach`-loopet i `Initialise`-metoden.
- Tilføj den samme logik i `Button_Clicked`-metoden i `if`-statementet.

```csharp
Todos.Add(todo);
```

## Delopgave 3: Bindings refaktoreret med ItemsSource i XAML

Refaktorér MauiTodo-appen, så al binding foregår i XAML — for at se, hvordan man sætter `ItemsSource` for vores `CollectionView` i XAML.

**1.** Åbn code-behind-filen `MainPage.xaml.cs`, og fjern i constructoren den linje, vi tilføjede i forrige afsnit til at sætte items source:

```csharp
TodosCollection.ItemsSource = Todos;
```

**2.** Sæt i stedet denne binding op i XAML. Åbn `MainPage.xaml`.

**3.** I det åbnende `<ContentPage...>`-tag tilføjes et navn (så den kan refereres) og en binding context.

**4.** I `CollectionView`'et bindes `ItemsSource`-propertyen til `Todos`-`ObservableCollection`'en i binding contexten.

Note: kør appen — outputtet vil være det samme, men koden er refaktoreret mere effektivt.
