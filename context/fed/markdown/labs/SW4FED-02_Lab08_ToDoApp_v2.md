# Lab 08-1 – ToDoApp v2

## Metadata

- **Lektion:** L08 – Lab 08-1 ToDoApp v2
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L08/FED Lab 08-1 ToDoApp-v2.pdf (1 side)
- **Emner dækket:**
  - MVVM i .NET MAUI
  - Binding to commands
  - `SwipeView` og dens `Command`-property
  - Fjernelse af eventhandlere fra code-behind
  - `CompleteTodoCommand` og `DeleteTodoCommand` i `MainViewModel`

---

## 1. Formål

At få erfaring med MVVM i MAUI.

## 2. Forudsætninger

At du har læst kapitel 9 i *MAUI in Action*.

## 3. Overordnet opgavebeskrivelse

At give funktionalitet til de eksisterende swipe gestures ved brug af MVVM-teknikken **binding to commands**. Som swipe er implementeret nu, vises der en besked, når brugeren swiper et ToDo-item. Dette gøres af `SwipeItem_Invoked`-eventhandleren.

## 4. Delopgave 1

Slet `SwipeItem_Invoked`-eventhandleren i code-behind-filen.

Brug `SwipeView`s `Command`-egenskab til at binde til `CompleteTodoCommand` i `MainViewModel`.

## 5. Delopgave 2

Tilføj en `DeleteTodoCommand` til `MainViewModel` og bind den til `SwipeView`s `Command`-property.

## 6. Forventet resultat

Når du har gennemført denne øvelse, vil du være i stand til at tilføje to-do-elementer, markere dem som fuldførte (enten ved at markere afkrydsningsfeltet eller bruge `SwipeView`) eller slette dem (ved hjælp af `SwipeView`).
