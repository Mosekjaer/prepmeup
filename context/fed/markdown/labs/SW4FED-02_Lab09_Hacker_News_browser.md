# Lab 09 – Hacker News browser

## Metadata

- **Lektion:** L09 – MVVM Community Toolkit
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "Exercise 09 Hacker News browser"
- **Emner dækket:**
  - MVVM Toolkit i MAUI
  - Dependency injection i MAUI
  - Preferences (svarer til Settings i guiden)
  - HTTP-kald med Refit eller HttpClient
  - Deserialisering af JSON-respons
  - Hacker News (Algolia) søge-API

---

## Formål

At få erfaring med MVVM Toolkit i MAUI.

## Forudsætninger

At du har læst om MVVM Toolkit.

## Overordnet opgavebeskrivelse

At lave en simpel Hacker News browser ved brug af MAUI og MVVM Toolkit.

## Opgaven

Følg denne guide (delvist):

- <https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/puttingthingstogether>

Men med disse ændringer i forhold til guiden:

- Lav det som en MAUI app.
- Settings hedder **Preferences** i MAUI – se underpræsentationer.
- Brug MAUIs dependency injection system.
- Reddit har ændret deres API, så man nu skal have en api-key, og brugeren skal logge ind med OAuth2. I stedet laver vi en browser til Hacker News, som kan kaldes med denne url:

  ```text
  https://hn.algolia.com/api/v1/search?query=react
  ```

  Ordet efter `=` skal brugeren kunne vælge.
- Guiden bruger en NuGet package **Refit** til at lave type safe http-kald. Du kan læse om Refit til .NET her: [Using Refit to Consume APIs in C#](https://code-maze.com/using-refit-to-consume-apis-in-csharp/) – eller du kan vælge at bruge `HttpClient` i stedet.
- Du får en masse response data tilbage. Det vi er interesseret i er `title`, `author` og `url` for items i listen `hits`.
- Filen `HackerNewsResponse` kan bruges til deserialisering fra JSON – se [kode/SW4FED-02_Lab09_HackerNewsResponse.md](../kode/SW4FED-02_Lab09_HackerNewsResponse.md).
