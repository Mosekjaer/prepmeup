# Lab 03 – Select Images

## Metadata

- **Lektion:** L03 – Lab 03 Select Images
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L03/FED Lab 03 SelectImages.pdf (6 sider)
- **Emner dækket:**
  - Nyt MAUI-projekt med sqlite-net-pcl og Android-specifik csproj-tilføjelse
  - ImageInfo-model med SQLite-attributter
  - Database-klasse med CRUD og fejlhåndtering
  - UI med Label, Buttons, Entry, Editor og Image-kontrol
  - FilePicker til at vælge en billedfil fra enheden
  - ObservableCollection og CollectionView med BindingContext
  - Persistering til databasen og indlæsning ved opstart

---

## Formål

At få erfaring med brug af kontroller i MAUI.

## Forudsætninger

At du har læst kapitel 4 i *MAUI in Action*.

## Overordnet opgavebeskrivelse

At lave en app, hvor brugeren kan tilføje billeder fra sin enhed (computer) til en database sammen med noget beskrivende tekst.

Senere kan man udbygge programmet med en billedkarrusel, som automatisk skifter til næste billede efter et tidsrum, men det er ikke en del af denne opgave.

## Delopgave 1: Projekt, model og database

Lav en ny MAUI app.

Appen skal lagre (persistere) filstierne til de af brugeren tilføjede billeder i en database, så installer NuGet-pakken `sqlite-net-pcl`.

Hvis du vil køre appen på Android, skal følgende tilføjes csproj-filen:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier(
    '$(TargetFramework)')) == 'android'">
    <PackageReference Include="SQLitePCLRaw.provider.dynamic_cdecl"
                      Version="2.1.10" />
</ItemGroup>
```

Opret en mappe `Models` og en mappe `Data`.

I mappen `Models` tilføjes en C#-fil med navnet `ImageInfo.cs`:

```csharp
using SQLite;

namespace SelectImages.Models
{
    public class ImageInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }

    }
}
```

Og i mappen `Data` tilføjes en C#-fil med navnet `Database.cs`:

```csharp
using SelectImages.Models;
using SQLite;

namespace SelectImages.Data
{
    internal class Database
    {
        private readonly SQLiteAsyncConnection? _connection;
        public Database()
        {
            try
            {
                  var dataDir = FileSystem.AppDataDirectory;
                  var databasePath = Path.Combine(dataDir, "ImageCarousel.db");
                  var dbOptions = new SQLiteConnectionString(databasePath, true);
                  _connection = new SQLiteAsyncConnection(dbOptions);
                  _ = Initialise();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         }
         private async Task Initialise()
         {
             try
             {
                 if (_connection != null)
                 {
                     await _connection.CreateTableAsync<ImageInfo>();
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
         }

         public async Task<List<ImageInfo>> GetImageInfos()
         {
             try
             {
                 if (_connection != null)
                 {
                     return await _connection.Table<ImageInfo>().ToListAsync();
                 }
                 return new List<ImageInfo>();
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return new List<ImageInfo>();
             }
         }

         public async Task<ImageInfo> GetImageInfo(int id)
         {
             try
             {
                 if (_connection != null)
                 {
                     var query = _connection.Table<ImageInfo>().Where(t => t.Id == id);
                     return await query.FirstOrDefaultAsync();
                 }
                 return new ImageInfo();
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return new ImageInfo();
             }
         }
         public async Task<int> AddImageInfo(ImageInfo item)
         {
             try
             {
                 if (_connection != null)
                 {
                     return await _connection.InsertAsync(item);
                 }
                 return 0;
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return 0;
             }
         }
         public async Task<int> DeleteImageInfo(ImageInfo item)
         {
             try
             {
                 if (_connection != null)
                 {
                     return await _connection.DeleteAsync(item);
                 }
                 return 0;
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return 0;
             }
         }
         public async Task<int> UpdateImageInfo(ImageInfo item)
         {
             try
             {
                 if (_connection != null)
                 {
                     return await _connection.UpdateAsync(item);
                 }
                 return 0;
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return 0;
             }
         }
     }
}
```

Du bruger endnu ikke den kode, du har tilføjet. Men prøv alligevel at bygge projektet for at sikre, at alt virker som det skal.

## Delopgave 2: Grundlæggende UI

I filen `MainPage.xaml` slettes alt indhold i `VerticalStackLayout`. Der tilføjes 1 Label, 2 Buttons, 1 Entry og 1 Editor-kontrol, så udseendet bliver ca. som vist på opgavens skærmbillede: en titel-label øverst, en "Select Image"-knap, et Entry til titel, et Editor-felt til beskrivelse og en "Add"-knap.

I filen `MainPage.xaml.cs` slettes eventhandleren `OnCounterClicked` samt datamedlemmet `count`.

Du skulle nu kunne køre programmet, men det har stadig ingen funktionalitet.

## Delopgave 3: Vælg et billede med FilePicker

Tilføj en `Image`-kontrol til højre for de kontroller, du tilføjede i delopgave 2, og giv den navnet `selectedImage`. Det kan du gøre ved at bruge et `HorizontalStackLayout` og et `VerticalStackLayout`, som omfatter alle de kontroller, der skal ligge til venstre for image-kontrollen.

Tilføj så en eventhandler til knappen "Select Image", som viser en fildialog, hvor brugeren kan vælge en billedfil. Du kan bruge denne kode — det kræver, at du tilføjer et datamedlem `_imagePath` af typen `string`:

```csharp
var image = await FilePicker.Default.PickAsync(new PickOptions
{
    PickerTitle = "Pick Image",
    FileTypes = FilePickerFileType.Images
});
if (image != null)
{
    _imagePath = image.FullPath.ToString();
    selectedImage.Source = _imagePath;
}
```

Kør programmet. Du skal nu kunne vælge et billede på din computer og få det vist i din app.

## Delopgave 4: Collection af ImageInfo

Næste trin er at koble billedet sammen med en titel og en beskrivelse og så lagre disse i en collection i programmet.

Tilføj en collection af `ImageInfo` til `MainPage.xaml.cs` med denne kode:

```csharp
ObservableCollection<ImageInfo> Images { get; set; } = new();
```

Tilføj så en eventhandler til Add-knappen, som opretter et nyt `ImageInfo`-objekt og sætter dets properties, hvorefter det tilføjes `Images`-collectionen. Når dette er gjort, nulstilles diverse kontroller, så de er klar til at tilføje et nyt billede.

Kør programmet. Du skal nu kunne vælge et billede på din computer og få det vist i din app. Ved tryk på Add nulstilles alle kontroller, men du vil ikke kunne se det tilføjede billede.

## Delopgave 5: CollectionView

Vi vil gerne kunne se de billeder, vi har tilføjet appen, samt deres titel og beskrivelse. Du skal derfor tilføje en `CollectionView`-control, hvis `ItemsSource` bindes til `Images` — husk `BindingContext = this;` i `MainPage`s constructor.

Du kan selv vælge, om du vil have det nye `CollectionView` over eller under de andre kontroller. Et `CollectionView` har meget til fælles med et `CarouselView`, så du kan kopiere meget fra demoen CellBoutique.

## Delopgave 6: Persistering

Det sidste, der mangler, er at kunne persistere dataene, så de også er der efter en genstart af programmet.

Tilføj noget kode til eventhandleren for Add-knappen, som tilføjer det nye `ImageInfo`-objekt til databasen. Se evt. i ToDo-appen, hvordan du gør det.

Når appen starter, skal programmet vise de billeder, der er i databasen. Så tilføj noget kode i `MainPage`s constructor, som indlæser alle billeder i databasen og tilføjer dem til `Images`-collectionen. Du kan også få inspiration til dette i ToDo-appen.
