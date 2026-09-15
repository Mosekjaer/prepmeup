# Lab 01 – Hello MAUI (VS Code)

## Metadata

- **Lektion:** L01 – Introduktion til MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "Lab 01 Hello MAUI"
- **Emner dækket:**
  - Installation af Visual Studio Code
  - Installation af .NET og .NET MAUI workload
  - Android SDK-afhængigheder på Windows
  - Oprettelse af et .NET MAUI-projekt
  - Kørsel og debugging på Windows/Android/iOS
  - XAML hot reload og BackgroundColor
  - Ændring af event handler i code-behind

---

## Formål

At få installeret udviklerværktøjerne til MAUI samt opnå lidt erfaring med brugen af dem. Opgaven tager udgangspunkt i VS Code (Code), men du kan vælge at bruge andre værktøjer, så som Visual Studio eller JetBrains Rider.

## Forudsætninger

At du har læst kapitel 1 og 2 i *MAUI in Action*.

## Delopgave 1 – Installation

Installer Visual Studio Code (Code), hvis du ikke allerede har det.

- [Setting up Visual Studio Code](https://code.visualstudio.com/docs/setup/setup-overview)

Følg denne vejledning til at installere .NET og MAUI (ny link til installation):

- <https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?view=net-maui-10.0&tabs=visual-studio>

Du kan selv vælge udviklingsmiljø.

**HINT: dotnet version 10 anbefales, da den er Long Term Stable – det er version 9 ikke.**

Når du på en Windows PC kommer til trinnet "Using the InstallAndroidDependencies target", skal du være opmærksom på, at du selv skal indsætte nogle filstier i kommandoen:

```bash
dotnet build -t:InstallAndroidDependencies -f:net10.0-android -p:AndroidSdkDirectory="<AndroidSdkPath>" -p:JavaSdkDirectory="<JavaSdkPath>" -p:AcceptAndroidSDKLicenses=True
```

Den kan f.eks. se sådan ud (som på underviserens PC):

```bash
dotnet build -t:InstallAndroidDependencies -f:net10.0-android -p:AndroidSdkDirectory="C:\Program Files (x86)\Android\android-sdk" -p:JavaSdkDirectory="C:\Program Files\Microsoft" -p:AcceptAndroidSDKLicenses=True
```

## Delopgave 2 – Opret og kør projektet

Åbn Code og vælg "New window". Klik på Explorer og vælg "Create .NET Project". Vælg **.NET MAUI App …**, vælg en mappe og giv dit projekt et navn.

Når Code er færdig med at oprette projektet og installere dets afhængigheder, så prøv at køre programmet på din computer. Her er det vigtigt, at du først vælger en fil med extension `.cs` og så vælger Run – Debug.

Hvis du har tid og lyst, kan du også prøve at køre appen på Android-emulatoren, eller iOS-simulatoren hvis du har en Mac og har installeret Xcode. Læs evt. mere om kørsel på Android og iOS her:

- <https://learn.microsoft.com/en-us/dotnet/maui/get-started/first-app?view=net-maui-10.0&pivots=devices-android&tabs=vswin>

## Delopgave 3 – Layout og live opdatering

Åbn filen `MainPage.xaml` samtidig med at du kører (debugger) appen.

- Find elementet `ScrollView` og sæt dets `BackgroundColor`-property til en farve – f.eks. aqua. Iagttag at der er live opdatering, så baggrundsfarven på din app ændres, mens appen kører.
- Sæt `BackgroundColor` på `VerticalStackLayout` til en anden farve, så du kan se hvor dette layout er placeret, og hvor meget det fylder.
- Sæt ligeledes `BackgroundColor` på `Image`'et og de 2 labels.

## Delopgave 4 – Tæl 2 op

Når du klikker på knappen, tæller den 1 op. Prøv at ændre koden, så den tæller 2 op – altså 2, 4, 6 …

*Hint: se i filen `MainPage.xaml.cs`.*
