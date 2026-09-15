# Lab 07 – El spot priser

## Metadata

- **Lektion:** L07 – Navigation og HTTP i MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "El spot priser"
- **Emner dækket:**
  - Udskiftning af Shell-navigation med hierarkisk navigation (NavigationPage)
  - Flere ContentPages og navigation mellem dem
  - Modelklasser til deserialisering af JSON
  - HttpClient med baseAddress
  - `OnAppearing` lifecycle event
  - Visning af data på tabelform
  - Energi Data Service API

---

## Trin 1 – Nyt projekt med hierarkisk navigation

Lav en ny MAUI app med navnet `Elspotpriser` og udskift derefter Shell-navigation med hierarkisk navigation (`NavigationPage`), som viser `MainPage` som default.

## Trin 2 – Sider og navigation

Tilføj 2 sider af typen `ContentPage`. Den ene skal hedde `ElSpotPricesPage` og den anden `CO2EmissionsPage`. Tilføj knapper på `MainPage`, som navigerer brugeren til henholdsvis `ElSpotPricesPage` og `CO2EmissionsPage`.

## Trin 3 – Modelklasser

Tilføj disse modelklasser:

```csharp
public class PriceEntry
{
    public DateTime HourDK { get; set; }
    public string? PriceArea { get; set; }
    public double SpotPriceDKK { get; set; }
}
```

```csharp
public class ElSpotPrices
{
    public long total { get; set; }
    public string? filters { get; set; }
    public int limit { get; set; }
    public string? dataset { get; set; }
    public PriceEntry[]? records { get; set; }
}
```

## Trin 4 – Hent data

Tilføj kode i en `OnAppearing` lifecycle event i `ElSpotPricesPage`, som henter elspotpriserne fra energidata.

Opret en `HttpClient` med denne baseAddress:

```csharp
"https://api.energidataservice.dk/"
```

Brug denne relative url til at hente dataene:

```csharp
"dataset/Elspotprices?filter={\"PriceArea\":[\"DK1\"]}&columns=HourDK,PriceArea,SpotPriceDKK&limit=24"
```

## Trin 5 – Vis data

Vis dataene (dem i `records`) på tabelform – pænt i rækker under hinanden.

## Trin 6 – CO2-emissioner (hvis du kan nå det)

Gør det samme for CO2-emissioner. Læs her hvordan du kan gøre det:

- [Energi Data Service | API Guide](https://www.energidataservice.dk/guides/api-guides)
