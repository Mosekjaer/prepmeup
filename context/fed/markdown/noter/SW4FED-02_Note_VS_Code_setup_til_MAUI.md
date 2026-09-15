# Note – VS Code setup til MAUI (i stedet for Visual Studio)

## Metadata

- **Lektion:** L01 – Værktøjsopsætning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Jung Min Kim (Jenny)
- **Kilde:** Brightspace-note om VS Code-opsætning (eksporteret som "Untitled - Copy (2).html")
- **Emner dækket:**
  - Visual Studio vs. VS Code til .NET MAUI
  - Installation af MAUI workload på Windows
  - Installation af .NET MAUI-extensions i VS Code
  - Oprettelse af et .NET-projekt fra VS Code
  - Valg af C#-debugger ved Run and Debug
  - Installation af SQLite-pakker fra terminalen
  - Kontrol af .NET-version (version 10 anbefales)

---

## 1. Visual Studio eller VS Code?

*(Note) You can always use Visual Studio instead of VS Code.*

- **Microsoft made the most optimal to fit in Visual Studio** with .NET MAUI app.
- But if you really want to work on VS Code instead of Visual Studio:
  - First, you must install .NET MAUI if it is your first time with .NET MAUI.
  - After installing .NET MAUI, then also the extension later.
- Follow the steps to install .NET MAUI **before** installing extensions in VS Code.

## 2. How to install .NET MAUI in Visual Studio Code (Windows)

*You only need to do this the very first time, and afterward you won't need to do it again.*

**1) Open Visual Studio Code and go to Terminal in VS Code**

**2) In Terminal:**

```bash
dotnet workload install maui
```

![Terminalkommando: dotnet workload install maui](assets/image_20240828222321850.png)

- It will start to install.

*… (it takes time, and when it is completed, it will show like this) …*

> **Successfully installed workload(s) maui.**

- Done.

**3) Go to this icon, then install .NET MAUI extensions**

![Extensions-ikonet i VS Codes activity bar, med .NET MAUI-extension i marketplace-listen](assets/PastedImage_c821151917d24eada5c34df4009658dc_image.png)

Next:

![Explorer-panelet i VS Code med knappen "Create .NET Project" markeret nederst](assets/image_20240828222321865.png)

**(If you can't see this button – Create .NET Project – then press "Ctrl+Shift+P" (this can always be used to create a new .NET project), then you can see a pull-down list that looks like the next image)**

Next:

![Templatelisten "Create a new .NET project" med ".NET MAUI App" øverst](assets/image_20240828222321868.png)

Next:

![Run and Debug-panelet i VS Code med knappen "Run and Debug" markeret](assets/image_20240828222501851.png)

Done. (You need to choose **C#** – it will appear – when you press "Run and Debug":)

![Select debugger-listen hvor C# vælges efter klik på Run and Debug](assets/image_20240903103052435.png)

## 3. How to install local database SQLite in Visual Studio Code (Windows)

Go to Terminal (command line) in Visual Studio Code and write:

```bash
dotnet add package sqlite-net-pcl
dotnet add package sqlite-net-sqlcipher
```

### Troubleshooting

If these commands are not working, then go to the Extensions symbol in VS Code:

![Extensions-ikonet i VS Codes activity bar](assets/image_20250128130021037.png)

Search **SQLite** and press the install button:

![SQLite-extension i VS Codes extension marketplace](assets/image_20250128130100527.png)

## 4. Tjek din .NET-version

***If you continuously face problems with .NET MAUI in VS Code, check your .NET version in the terminal in VS Code:***

![Terminalkommando: dotnet --version](assets/image_20250205213551716.png)

```bash
dotnet --version
```

- We recommend that you use .NET version **10**.
- If you need to change .NET version, you can prompt ChatGPT "How to set .net10 in visual studio code"; it provides a detailed guide. Follow that and then check in your terminal, `dotnet --version` again.
