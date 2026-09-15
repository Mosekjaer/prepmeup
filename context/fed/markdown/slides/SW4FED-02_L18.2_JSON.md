# L18 – JSON (JavaScript Object Notation)

## Metadata

- **Lektion:** L18 – JSON
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L18/FED JSON.pdf (18 slides)
- **Emner dækket:**
  - Hvad JSON er, og hvorfor det bruges til data-interchange
  - Historik og standardisering (RFC 8259, ECMA-404, ISO/IEC 21778:2017)
  - JSON's to grundstrukturer og de seks datatyper
  - Native JSON i browseren: `JSON.parse()` og `JSON.stringify()`
  - Json.NET: serializer og LINQ to JSON (`JObject`, `JArray`)
  - `System.Text.Json` i .NET Core 3+
  - Name casing-konfiguration
  - Datoformater i JSON: ISO 8601 vs. unix timestamp
  - JSON5

---

## 1. Hvad er JSON?

JSON er beskrevet i **RFC 8259** (konsistent med ECMA-404 og ISO/IEC 21778:2017).

- Er et letvægts data-interchange format
- Er let for mennesker at læse og skrive
- Er let for maskiner at parse og generere
- Er baseret på et subset af JavaScript
- Er et tekstformat, der er fuldstændig sprogindependent — men bruger konventioner, som er velkendte for programmører fra C-familien af sprog, herunder C, C++, C#, Java, JavaScript, Perl, Python og mange andre

Disse egenskaber gør JSON til et ideelt data-interchange sprog. JSON bruges bredt i datakommunikation, og flere NoSQL-databaser bruger JSON (eller BSON) som lagringsformat. BSON er en binær repræsentation af JSON.

## 2. Historik

- Douglas Crockford var den første til at specificere og popularisere JSON-formatet.
- JSON blev brugt hos State Software, et firma medstiftet af Crockford, fra omkring 2001.
- JSON.org-hjemmesiden blev lanceret i 2002.
- I 2005 begyndte Yahoo at tilbyde nogle af sine web services i JSON.
- I 2006 begyndte Google at tilbyde JSON-feeds til sin GData-webprotokol.
- Formatet er beskrevet i RFC 8259.

## 3. JSON Structure

JSON er bygget på to strukturer:

- **En samling af name/value-par.** I forskellige sprog realiseres dette som et object, record, struct, dictionary, hash table, keyed list eller associative array.
- **En ordnet liste af værdier.** I de fleste sprog realiseres dette som et array, vector, list eller sequence.

### Kodeeksempel

```json
{
    "name" : "10gen HQ",
    "address" : "578 Broadway 7th Floor",
    "city" : "New York",
    "zip" : "10011",
    "tags" : [ "business", "tech" ]
}
```

## 4. Data types

**Number**
Et signed decimaltal, der kan indeholde en fraktionsdel og kan bruge eksponentiel E-notation, men kan ikke indeholde ikke-tal som `NaN`. Formatet skelner **ikke** mellem integer og floating-point.

**String**
En sekvens af nul eller flere Unicode-tegn. Strings afgrænses med dobbelte anførselstegn og understøtter backslash escaping-syntaks.

**Boolean**
Enten værdien `true` eller `false`.

**Array**
En ordnet liste af nul eller flere værdier, som hver kan være af en vilkårlig type. Arrays bruger kantparentes-notation med kommaseparerede elementer.

**Object**
En uordnet samling af name–value-par, hvor navnene (også kaldet keys) er strings. Objects er tænkt til at repræsentere associative arrays, hvor hver key er unik inden for et object. Objects afgrænses med krøllede parenteser og bruger kommaer til at separere hvert par, mens kolon-tegnet `:` inden for hvert par separerer key/navn fra dens værdi.

**Null**
En tom værdi, angivet med ordet `null`.

## 5. Eksempel med alle typer

```json
{
  "name": "John Doe",
  "isAlive": true,
  "age": 27,
  "height": 1.90,
  "address": {
    "streetAddress": "21 2nd Street",
    "city": "New York",
    "state": "NY",
    "postalCode": "10021-3100"
  },
  "phoneNumbers": [
    {
      "type": "home",
      "number": "212 555-1234"
    },
    {
      "type": "office",
      "number": "646 555-4567"
    },
    {
      "type": "mobile",
      "number": "123 456-7890"
    }
  ],
  "children": [],
  "spouse": null
}
```

Her ses `"name"` som String, `"isAlive"` som Boolean, `"age"`/`"height"` som Number, `"address"` som Object, `"phoneNumbers"` som Array og `"spouse"` som Null.

## 6. Native JSON

Alle moderne browsere har native JSON-understøttelse via:

- `JSON.parse()`
- `JSON.stringify()`

De blev tilføjet i femte udgave af ECMAScript-standarden.

## 7. Json.NET

Json.NET er et populært high-performance JSON-framework til .NET.

Features:

- Fleksibel JSON serializer til at konvertere mellem .NET-objekter og JSON
- LINQ to JSON til manuelt at læse og skrive JSON
- Skriv indented, letlæselig JSON
- Konvertér JSON til og fra XML

Serializeren er et godt valg, når den JSON du læser eller skriver mapper tæt til en .NET-class.

LINQ to JSON er godt i situationer, hvor du kun er interesseret i at hente værdier ud af JSON, hvor du ikke har en class at serialisere/deserialisere til, eller hvor JSON'en er radikalt forskellig fra din class og du manuelt skal læse og skrive fra dine objekter.

## 8. Serialization Example (Json.NET)

### Kodeeksempel

```csharp
Product product = new Product();
product.Name = "Apple";
product.Expiry = new DateTime(2008, 12, 28);
product.Price = 3.99M;
product.Sizes = new string[] { "Small", "Medium", "Large" };
string json = JsonConvert.SerializeObject(product);
// . . .

Product deserializedProduct =
                JsonConvert.DeserializeObject<Product>(json);
```

Resultatet:

```json
{
  "Name": "Apple",
  "Expiry": "2008-12-28T00:00:00",
  "Price": 3.99,
  "Sizes": [
    "Small",
    "Medium",
    "Large"
  ]
}
```

## 9. Getting JSON Values med JObject

### Kodeeksempel

```csharp
string json = @"{
  ""Name"": ""Apple"",
  ""Expiry"": "2008-12-28T00:00:00",
  ""Price"": 3.99,
  ""Sizes"": [
    ""Small"",
    ""Medium"",
    ""Large"”
  ]}";

JObject o = JObject.Parse(json);

string name = (string)o["Name"];
// Apple

sizes = (JArray)o["Sizes"];
string smallest = (string)sizes[0];
// Small
```

<!-- Kilden har inkonsistente anførselstegn i denne C#-streng (bl.a. et typografisk citationstegn) — gengivet som i slidesene. -->

`JObject` er godt i situationer, hvor:

- du kun er interesseret i at hente værdier ud af JSON
- du ikke har en class at serialisere eller deserialisere til
- eller JSON'en er radikalt forskellig fra din class, og du skal læse og skrive manuelt fra dine objekter

## 10. LINQ to JSON

`JObject`/`JArray` kan også queries med LINQ. `Children()` returnerer child-værdierne af et `JObject`/`JArray` som en `IEnumerable<JToken>`, som derefter kan queries med de standard LINQ-operatorer Where/OrderBy/Select.

### Kodeeksempel

```csharp
string json = @"{. . .}";
JObject rss = JObject.Parse(json);

var postTitles =
        from p in rss["channel"]["item"]
        select (string)p["title"];

foreach (var item in postTitles)
{
  Console.WriteLine(item);
}
```

## 11. System.Text.Json

.NET Core 3 bragte en helt ny JSON-(de)serializer med sig: `System.Text.Json`.

## 12. Customize JSON name casing

```csharp
class WeatherForecast {
  public DateTimeOffset Date { get; set; }
}
```

For at bruge camel case til alle JSON property-navne:

```csharp
var options = new JsonSerializerOptions
{
  PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var json = JsonSerializer.Serialize(weatherForecast, options);
```

Case-insensitive property matching:

```csharp
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
};
var wf = JsonSerializer.Deserialize<WeatherForecast>(weatherForecast, options);
```

## 13. Det "rigtige" JSON-datoformat

JSON specificerer ikke selv, hvordan datoer skal repræsenteres — men det gør JavaScript. Du bør bruge det format, som `Date`'s `toJSON`-metode udsender:

```text
2012-04-23T18:25:43.511Z
```

Begrundelsen:

- Det er human readable, men også kortfattet
- Det sorterer korrekt
- Det inkluderer fraktioner af sekunder, hvilket kan hjælpe med at genetablere kronologi
- Det følger ISO 8601
- ISO 8601 har været veletableret internationalt i mere end et årti

Når det er sagt: ethvert datobibliotek nogensinde skrevet kan forstå "millisekunder siden 1970" (unix timestamp). Det kan dog kun udtrykke datoer mellem 1. januar 1970 og 2038 (32 bit word).

### Kodeeksempel — C#

```csharp
Date().toISOString();

/// <summary>
/// Gateway rx UTC time, ISO 8601, up to nanosecond precision
/// </summary>
public string gwtime { get; set; }
```

### Kodeeksempel — JavaScript

```javascript
const json = JSON.stringify(new Date());
const parsed = JSON.parse(json); //2015-10-26T07:46:36.611Z
const date = new Date(parsed); // Back to date object
```

## 14. Brug af unix timestamp

### Kodeeksempel — C#

```csharp
gwts = (DateTimeOffset.Now - TimeSpan.FromDays(1.0)).ToUnixTimeMilliseconds();

/// Gateway timestamp as number (miliseconds from Unix epoch)
/// </summary>
public long gwts { get; set; }      // 43424140,
```

### Kodeeksempel — JavaScript

JavaScript arbejder med antallet af **millisekunder** siden epoch, hvorimod de fleste andre sprog arbejder med sekunder. Derfor divisionen med 1000:

```javascript
var unix = Math.round(+new Date() / 1000);
// or
var unix = Math.round(new Date().getTime() / 1000);
// or
var unix = Math.round(Date.now() / 1000);
```

## 15. JSON5

JSON5 er en udvidelse af JSON-syntaksen, som — ligesom JSON — også er gyldig JavaScript-syntaks.

De væsentligste forskelle fra JSON-syntaks:

- Valgfrie trailing commas
- Unquoted object keys
- Single quoted og multiline strings
- Yderligere talformater
- Kommentarer

JSON5-syntaks understøttes i noget software, men ikke alle frameworks og libraries understøtter det.

## 16. References & Links

- How to serialize and deserialize JSON in .NET: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
- Json.NET: https://www.newtonsoft.com/json
- The "right" JSON date format: https://stackoverflow.com/questions/10286204/the-right-json-date-format
- https://michaelscodingspot.com/the-battle-of-c-to-json-serializers-in-net-core-3/
- RFC 8259
- JSON.org
- https://en.wikipedia.org/wiki/JSON
