# L03.1 – Controls i MAUI

## Metadata

- **Lektion:** L03 – Controls in MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L03/FED Controls.pdf (23 slides)
- **Emner dækket:**
  - Hvad controls er, og hvad "views" dækker over
  - Hierarkiet page / layout / control
  - Cross-platform controls som abstraktioner af native implementationer
  - Oversigt over controls til visning, input, kommandoer, grafik og lister
  - ImageSource og dens fire kilder
  - Height/Width, HeightRequest/WidthRequest og device-independent units
  - Clipping med geometrier
  - Borders, brushes og shadows
  - Gesture recognizers
  - RefreshView og SwipeView

---

## 1. Hvad er controls?

Controls er views, som enten direkte renderer noget på skærmen eller tager input fra en bruger.

Cirka 30 controls følger med .NET MAUI out of the box. Man kan downloade andre controls fra open source-projekter eller købe dem fra forskellige vendors.

## 2. Hvad menes med "views"?

Views i et UI er de ting, der vises på en skærm — i modsætning til andre dele af en app som en service, en model eller en viewmodel. Views er det, brugerne ser og interagerer med.

I en .NET MAUI-app er der tre typer views: controls, layouts og pages.

Hierarkiet i en MAUI-app:

- En page er noget, appen selv kan vise.
- En page ved, hvordan den renderer et layout.
- Layouts ved, hvordan de renderer controls og layouts.

Bemærk terminologien: i MAUI-dokumentationen kaldes controls for **Views**. I ikke-Microsoft-frameworks kaldes de ofte GUI widgets.

## 3. Cross-platform controls

MAUI-controls er abstraktioner af deres native platform-implementationer.

De er simple at bruge i sine apps, og der findes fuld dækning af dem alle i .NET MAUI-dokumentationen. Slidesene giver derfor kun et overblik og nogle eksempler.

## 4. Controls til visning af information

Disse controls accepterer ikke brugerinput.

| Control | Use case |
| --- | --- |
| Label | Viser tekst på skærmen |
| ProgressBar | Viser en værdi udtrykt som en brøkdel. Bruges typisk til at indikere progress, fx hvor meget af en fil der er downloadet |
| ActivityIndicator | Viser at noget er i gang |

## 5. Controls der accepterer input

| Control | Use case |
| --- | --- |
| Entry | Lader brugeren indtaste tekst |
| Editor | Lader brugeren indtaste tekst over flere linjer |
| CheckBox | Giver en yes/no-, true/false- eller on/off-mulighed |
| DatePicker | Til at vælge en dato |
| Slider | Til at vælge en værdi mellem et minimum og maksimum |
| Stepper | Øger eller mindsker et tal med et angivet beløb |
| TimePicker | Til at vælge et tidspunkt |
| RadioButton | Til at vælge én mulighed fra en gruppe viste muligheder ved at afkrydse en boks |
| Picker | Til at vælge én mulighed fra en gruppe listede muligheder ved at plukke den fra en liste |

## 6. Controls der accepterer kommandoer

Disse controls adskiller sig fra input-controls ved, at de udelukkende fortæller appen, at brugeren vil gøre noget — modsat at brugeren leverer en værdi.

| Control | Use case |
| --- | --- |
| Button | En simpel control brugeren kan tappe eller klikke for at initiere en handling. Bruger tekst til at formidle sin intent |
| ImageButton | Som Button, men bruger et billede i stedet for tekst til at formidle sin intent |
| SearchBar | Præsenterer en genkendelig søgeboks. Den har text input og et forstørrelsesglas-ikon |

## 7. Controls til grafik

MAUI leverer tre måder at præsentere grafik på.

| Control | Use case |
| --- | --- |
| Image | Viser billeder. `Source`-propertyen kan tage billeder fra en fil, en resource, en URL eller en `Stream`, og udfyldes automatisk med statiske constructors |
| Shapes | Lader dig tegne simple geometriske former på skærmen eller rendere komplekse billeder med en SVG-kompatibel `Path`-control. Nyttig til hurtigt at placere en simpel form på din page |
| GraphicsView | Eksponerer et canvas, der lader dig tegne simple eller komplekse billeder på skærmen med brushes og paths. `GraphicsView` er ny i .NET MAUI og en del af MAUI.Graphics-biblioteket |

## 8. Lister og collections

Den mest effektive måde at vise en collection af data i din app er at definere en template for hvert item i collectionen og så lade appen rendere collectionen ud fra din template.

`CollectionView` er den, man oftest bruger i .NET MAUI.

| Control | Use case |
| --- | --- |
| CollectionView | Understøtter flere layout-typer: vertical list og horizontal list, vertical-grid og horizontal-grid |
| CarouselView | Bygger på CollectionView, men man kan vende tilbage til begyndelsen, når man når enden af collectionen |

Demo til dette emne: CellBoutique.

## 9. Templated views

Templated views tager en template og en collection af data og renderer hvert item i collectionen efter templaten.

## 10. ImageSource i .NET MAUI

`Image`-controlen har en property kaldet `Source` af typen `ImageSource`.

`ImageSource` har fire metoder, der lader dig vise et billede fra forskellige kilder:

- `FromFile`
- `FromUrl`
- `FromResource`
- `FromStream`

Når du bruger `Image`-controlen i XAML, er .NET MAUI smart nok til at vide, hvilken type ImageSource du bruger.

## 11. Height og Width

Alle views har `Height`- og `Width`-properties. Disse properties er read-only og kan bruges til at hente højden eller bredden af et view.

Man specificerer den ønskede størrelse med `HeightRequest` og `WidthRequest`. Disse værdier angives i device-independent units (DIUs).

## 12. Device-independent units, DIU

At arbejde direkte med pixels er upraktisk i moderne UI-udvikling, fordi hver skærm har en forskellig opløsning. Problemet er løst med indførelsen af device-independent units.

De fleste platforme følger den samme sizing-konvention, groft sagt svarende til 160 units pr. tomme eller 64 units pr. centimeter.

Det betyder, at hvis du giver en size-værdi på 2 for spacing i et grid, vil mellemrummet være ca. 1/32 centimeter eller 1/80 tomme, uanset skærmens størrelse eller opløsning.

Når du ser en height-, width- eller thickness-værdi i en .NET MAUI-app, er størrelsen i DIU'er — medmindre størrelsen er proportional, altså specificeret med en stjerne `*`.

## 13. Clipping

Man kan ændre formen på et view med `Clip`-propertyen.

Der findes tre prædefinerede "simple" geometrier i .NET MAUI:

- `EllipseGeometry`
- `LineGeometry`
- `RectangleGeometry`

Eller man kan bruge en composite geometry, hvor man kombinerer geometrier for at opnå den ønskede effekt.

```xml
<Image Source="{Binding Image}"
       WidthRequest="300"
       HeightRequest="400">
    <Image.Clip>
        <EllipseGeometry Center="150,200"
                          RadiusX="150"
                          RadiusY="200"/>
    </Image.Clip>
</Image>
```

Composite geometry:

```xml
<Image.Clip>
    <GeometryGroup>
        <EllipseGeometry Center="150,100“
                         RadiusX="100“
                         RadiusY="100"/>
        <EllipseGeometry Center="150,250"
                         RadiusX="150"
                         RadiusY="150"/>
    </GeometryGroup>
</Image.Clip>
```

Demoer: Clipping- og GeometryGroup-branches.

## 14. Borders

Man kan anvende en border på ethvert view i .NET MAUI ved at wrappe kontrollen i en `Border`.

På `Border`-controlen skal man specificere:

- `Stroke` — en `Brush`
- `StrokeThickness` — i DIU
- `StrokeShape` — enten `Ellipse`, `Rectangle` eller `RoundRectangle`

Der er tre typer `Brush`:

- SolidColor
- `LinearGradientBrush`
- `RadialGradientBrush`

SolidColor er default, hvilket er grunden til, at man kan specificere en enkelt farve med en af de tilgængelige metoder — enum, hex eller ved at referere en resource.

### Border-eksempel

```xml
<DataTemplate>
    <Border Stroke="{StaticResource Primary}"
            StrokeThickness="3"
            Padding="5"
            Margin="0,10">
        <Border.StrokeShape>
            <RoundRectangle CornerRadius="10"/>
        </Border.StrokeShape>
        <Grid WidthRequest="350"
```

## 15. Shadows

`Shadow` kræver fire properties:

- `Brush` — specificerer skyggens farve og kan sættes med `Colors`-enummet
- `Opacity` — specificerer hvor opak skyggen er, angives med en værdi mellem 0 og 1
- `Radius` — definerer skyggens radius, angives med et tal i DIU'er
- `Offset` — specificerer skyggens offset. Er af typen `Point` og kræver derfor et par værdier til x- og y-koordinaterne

```xml
<Button Grid.Row="3"
        >
    <Button.Shadow>
        <Shadow Brush="Black"
                Offset="5,5"
                Radius="10"
                Opacity="0.8" />
    </Button.Shadow>
</Button>
```

## 16. Gesture recognizers

På touchscreens understøtter MAUI touch gestures. MAUI understøtter følgende fem gesture recognizers:

- Tap
- Pan
- Swipe
- Pinch
- Drag and drop

Disse gestures kan tilføjes til ethvert view i MAUI.

```xml
<Label Text="Drag">
  <Label.GestureRecognizers>
    <DragGestureRecognizer/>
  </Label.GestureRecognizers>
</Label>
```

```xml
<Frame>
  <Frame.GestureRecognizers>
    <DropGestureRecognizer/>
  </Frame.GestureRecognizers>
</Frame>
```

Reference: https://medium.com/nerd-for-tech/add-drag-and-drop-gesture-recognizers-in-net-maui-9caef4083347

## 17. RefreshView

Pull-to-refresh er endnu et allestedsnærværende UI-paradigme, der er blevet populært med touchscreens. Man trækker ned fra toppen af skærmen og slipper for at opdatere indholdet.

.NET MAUI gør det let at implementere med `RefreshView`. `RefreshView` er en wrapper, der kan placeres omkring ethvert andet view for at tilføje pull-to-refresh-funktionaliteten.

## 18. SwipeView

Et andet allestedsnærværende UI-paradigme er brugen af swipe. Brug `SwipeView` til at tilføje denne funktionalitet til ethvert view. `SwipeView` wrapper andre views — det er sådan man tilføjer SwipeView-funktionaliteten til dem.

`SwipeView` har fire collections af `SwipeItems`:

- `LeftItems`
- `RightItems`
- `TopItems`
- `BottomItems`

Man kan tilføje `SwipeItem`s til disse collections.

En property kaldet `Mode` har to muligheder, `Execute` og `Reveal`. Den bestemmer, hvad der sker, når brugeren swiper på `SwipeView`'et.

### SwipeView-eksempel

Vi tilføjer `SwipeView` til MauiTodo. Vi tilføjer et `SwipeItem` til `LeftItems`-collectionen for at slette to-do-item'et og et `SwipeItem` til `RightItems`, der markerer item'et som done.

```xml
<DataTemplate>
  <SwipeView>
    <SwipeView.LeftItems>
      <SwipeItems Mode="Execute">
        <SwipeItem Text="Delete" IconImageSource="delete" BackgroundColor="Tomato"/>
      </SwipeItems>
    </SwipeView.LeftItems>
    <SwipeView.RightItems>
      <SwipeItems Mode="Execute">
        <SwipeItem Text="Done" IconImageSource="check" BackgroundColor="LimeGreen"/>
      </SwipeItems>
    </SwipeView.RightItems>
    <Border Stroke="{StaticResource Primary}"
```

## 19. References & Links

- MAUI documentation: https://learn.microsoft.com/en-us/dotnet/maui/
- Controls: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/
