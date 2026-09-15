# Lab 09 – HackerNewsResponse (JSON-model)

## Metadata

- **Lektion:** L09 – MVVM Community Toolkit og HTTP
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing
- **Kilde:** `L09/HackerNewsResponse.cs` (udleveret kode til lab 09)
- **Emner dækket:**
  - Deserialisering af JSON til C#-klasser
  - Nullable reference types i en response-model
  - Nested objekter og arrays i JSON
  - Algolia-søge-API'ets responsformat

---

## Kontekst

Denne fil hører til **lab 09 (Hacker News browser)** – se [labs/SW4FED-02_Lab09_Hacker_News_browser.md](../labs/SW4FED-02_Lab09_Hacker_News_browser.md). Den indeholder de klasser, du deserialiserer JSON-svaret fra Hacker News' Algolia-søge-API til:

```text
https://hn.algolia.com/api/v1/search?query=react
```

`HackerNewsResponse` er rodobjektet. Det interessante felt er `Hits` – et array af `Hit`, hvor hver `Hit` har `Title`, `Author` og `Url`, som er de tre ting opgaven beder dig vise. Resten af felterne (paginering i `Page`/`NbPages`/`HitsPerPage`, samt `_highlightResult` og timing-objekterne) er metadata fra API'et. `NbPages` er den, du får brug for i lab 11, når du skal lave en pagination control.

Næsten alle properties er nullable (`string?`, `int?`), fordi API'et ikke garanterer at levere dem alle for hvert hit. Arrays initialiseres til tomme (`= []`), så man kan iterere over dem uden null-tjek.

Bemærk at namespacet stadig hedder `RedditBrowser.Models`, fordi opgaven oprindeligt var en Reddit-browser.

## HackerNewsResponse.cs

```csharp
namespace RedditBrowser.Models
{
    public class HackerNewsResponse
    {
        public ExhaustiveType? Exhaustive { get; set; }
        public bool ExhaustiveNbHits { get; set; }
        public bool ExhaustiveTypo { get; set; }
        public Hit[] Hits { get; set; } = [];
        public int HitsPerPage { get; set; }
        public int NbHits { get; set; }
        public int NbPages { get; set; }
        public int Page { get; set; }
        public string? Params { get; set; }
        public int ProcessingTimeMS { get; set; }
        public ProcessingTiming? ProcessingTimingsMS { get; set; } 
        public string? Query { get; set; }
        public int ServerTimeMS { get; set; }
    }

    public class ExhaustiveType
    {
        public bool NbHits { get; set; }
        public bool ExhaustiveTypo { get; set; }
    }

    public class Hit
    {
        public string? Author { get; set; }
        public long[]? Children { get; set; }
        public DateTime? Created_at { get; set; }
        public long? Updated_at_i { get; set; }
        public int? Num_comments { get; set; }
        public string? ObjectID { get; set; }
        public int? Points { get; set; }
        public long? Story_id { get; set; }
        public string? Title { get; set; }
        public DateTime? Updated_at { get; set; }
        public string? Url { get; set; }
        public HighlightResult? _highlightResult { get; set; }
        public string[]? _tags { get; set; }
    }

    public class HighlightResult
    {
        public Details? Author { get; set; }
        public TitleDetails? Title { get; set; }
        public Details? Url { get; set; }
    }

    public class Details
    {
        public string? MatchLevel { get; set; }
        public string[] MatchedWords { get; set; } = [];
        public string? Value { get; set; }
    }

    public class TitleDetails : Details
    {
        public bool FullyHighlighted { get; set; }
    }

    public class ProcessingTiming
    {
        public FetchType? Fetch { get; set; }
        public int Total { get; set; } = 0;
        public Request? _request { get; set; }
    }

    public class FetchType
    {
        public int Query { get; set; }
        public int Scanning { get; set; }
        public int Total { get; set; }
    }

    public class Request
    {
        public int RoundTrip { get; set; }
    }
}
```
