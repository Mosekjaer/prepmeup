# HttpClient

## Metadata

- **Lektion:** L05.3 – HttpClient
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L05/Httpclient.pdf (11 slides)
- **Emner dækket:**
  - Hvad `HttpClient` er
  - Grundlæggende GET med exception handling
  - Kald af et REST API fra en MAUI-side
  - Genbrug af en static HttpClient
  - `DefaultRequestHeaders`
  - API-oversigt over HTTP-metoder
  - `SendAsync` med `HttpRequestMessage`
  - `HttpContent`-subklasser
  - Cleartext traffic på Android

---

## 1. Hvad er HttpClient

`HttpClient` er en klasse til at sende HTTP requests og modtage HTTP responses fra en resource identificeret ved en URI.

## 2. Grundlæggende eksempel

Asynkrone netværkskald placeres i en try/catch-blok for at håndtere exceptions:

```csharp
HttpClient client = new();
// Call asynchronous network methods in a try/catch block to handle exceptions.
try
{
  using HttpResponseMessage response = await client.GetAsync("http://www.dr.dk/");
  response.EnsureSuccessStatusCode();
  string responseBody = await response.Content.ReadAsStringAsync();

  // Above three lines can be replaced with the helper method below
  // string responseBody = await client.GetStringAsync(uri);

  Console.WriteLine(responseBody);
}
catch (HttpRequestException e)
{
  Console.WriteLine("\nException Caught!");
  Console.WriteLine("Message :{0} ", e.Message);
}
```

De tre første linjer i try-blokken kan erstattes af hjælpemetoden `GetStringAsync`.

## 3. Kald af et REST API

Her sættes `BaseAddress` i constructoren, og selve datahentningen sker i `OnAppearing`-lifecycle-metoden med `GetFromJsonAsync<T>`:

```csharp
public partial class MainPage : ContentPage
{
    string _apiKey = "31abcccf28ac2806f17a1b56d67215b3";
    string _baseUri = "https://api.themoviedb.org/3/";
    private readonly HttpClient _httpClient;
    public ObservableCollection<Genre> Genres { get; set; } = new();

    public MainPage()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUri) };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _genres = await _httpClient.GetFromJsonAsync<GenreList>(
                  $"genre/movie/list?api_key={_apiKey}&language=en-US");
```

Den tilhørende DTO:

```csharp
public class GenreList
{
    public List<Genre> genres { get; set; }
}
```

## 4. Opret en static HttpClient

```csharp
private static HttpClient sharedClient = new()
{
  BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
}
```

En `HttpClient` er tænkt til at blive instantieret én gang og genbrugt gennem hele applikationens levetid.

## 5. Sæt DefaultRequestHeaders

Headers, der skal med på alle requests fra klienten, sættes på `DefaultRequestHeaders`:

```csharp
// The GitHub API requires two headers.
_httpClient.DefaultRequestHeaders.Add(
    HeaderNames.Accept, "application/vnd.github.v3+json");
_httpClient.DefaultRequestHeaders.Add(
    HeaderNames.UserAgent, "HttpRequestsSample");
```

## 6. Lav et HTTP request

| HTTP-metode | API |
|---|---|
| GET | `HttpClient.GetAsync` |
| GET | `HttpClient.GetByteArrayAsync` |
| GET | `HttpClient.GetStreamAsync` |
| GET | `HttpClient.GetStringAsync` |
| POST | `HttpClient.PostAsync` |
| PUT | `HttpClient.PutAsync` |
| PATCH | `HttpClient.PatchAsync` |
| DELETE | `HttpClient.DeleteAsync` |
| enhver gyldig `HttpMethod` | `HttpClient.SendAsync` |

## 7. HttpClient.SendAsync

Man kan bruge et `HttpRequestMessage` til at sætte headers på det enkelte request:

- `request.Headers.Add()`
- `request.Headers.Accept.Add()`

```csharp
var request = new HttpRequestMessage() {
    RequestUri = new Uri("http://www.someURI.com"),
    Method = HttpMethod.Get,
};
request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
var result = await client.SendAsync(request);
var content = await result.Content.ReadAsStringAsync();
Console.WriteLine(content);
```

## 8. HTTP content

Typen `HttpContent` bruges til at repræsentere en HTTP entity body og response body.

De fleste eksempler viser, hvordan man forbereder subklassen `StringContent` med en JSON-payload, men der findes flere subklasser:

- `ByteArrayContent`
- `FormUrlEncodedContent`
- `JsonContent`
- `MultipartContent`
- `MultipartFormDataContent`
- `ReadOnlyMemoryContent`
- `ReadOnlyMemory<T>`
- `StreamContent`
- `StringContent`

## 9. For Android

Hvis man targeter Android-enheder, skal man tilføje `android:usesCleartextTraffic="true"` til `<application></application>` i `AndroidManifest.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
  <application android:usesCleartextTraffic="true"></application>
  <!-- omitted for brevity -->
</manifest>
```

## 10. Referencer og links

- Make HTTP requests with the HttpClient class — https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
- HttpClient Class (reference) — https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-8.0
