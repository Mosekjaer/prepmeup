# Lab 06 – MauiShell App

## Metadata

- **Lektion:** L06 – Lab 06, FED MAUI Lab 06 MauiShell App
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L06/Lab/FED Lab 06 MauiShell app.pdf
- **Emner dækket:**
  - Multipage-apps og Shell Navigation
  - Projektstruktur med Models- og Views/Pages-mapper
  - Animal-model
  - Oprettelse af flere ContentPages
  - AppShell.xaml og navigationsstruktur
  - FlyoutItem med FlyoutDisplayOptions="AsMultipleItems"
  - Tab og ShellContent med Icon og Title
  - Binding af sider med ContentTemplate og DataTemplate

---

## Formål

At få erfaring med MultiPages og Shell Navigation.

Lav MauiShell-appen for at øve og demonstrere forståelse af, hvordan man bygger og navigerer multipage, og hvordan man bruger Shell Navigation i .NET MAUI.

## Forudsætninger

Du har læst kapitel 7 i *MAUI in Action*.

## ToDo

### 1. Opret et nyt .NET MAUI-projekt

### 2. Download ikoner og billeder

- Download billederne fra Brightspace og kopiér dem ind i mappen `Resources/Images`.

### 3. Opret mapperne

- `Models`-mappen: til `Animal.cs`
- `Views` (eller `Pages`)-mappen: til dine XAML-sider

### 4. Definér Animal-modellen

- Opret `Animals.cs` i `Models`-mappen.
- Sæt properties `Name`, `Location`, `Details` og `ImageUrl` med string-typer.

### 5. Opret Views eller Pages (afhængigt af mappenavnet)

- Opret XAML-sider: `AboutPage.xaml`, `BearsPage.xaml`, `CatsPage.xaml`, `ElephantsPage.xaml`, `MonkeyPage.xaml`.
- Disse sider bruges til at vise dyredetaljer.

### 6. Tilføj navigation

- Åbn `AppShell.xaml`.
- Definér din navigationsstruktur med `<FlyoutItem FlyoutDisplayOptions="AsMultipleItems">`.
  - `FlyoutDisplayOptions="AsMultipleItems"` viser hvert `ShellContent` eller `Tab` som individuelle items.
- Inde i `<FlyoutItem FlyoutDisplayOptions="AsMultipleItems">` strukturerer du sidenavigationen med flyout og tabs:
  - Lav noget sidenavigation med Flyout (`FlyoutItem`) — du bestemmer selv, hvilket dyre-item der skal i flyout'en.
  - Brug `Tab` med `ShellContent` for hver dyreside.
  - Download ikoner og sæt `Icon` og `Title` i `ShellContent` for hver dyreside.
  - Bind siderne i `ShellContent` med `ContentTemplate` og `DataTemplate`.

### Kodeeksempel

```xml
ContentTemplate="{DataTemplate views:DogsPage}"
```
