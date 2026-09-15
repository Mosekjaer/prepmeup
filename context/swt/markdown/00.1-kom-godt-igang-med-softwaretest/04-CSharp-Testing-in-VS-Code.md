---
title: "C# Testing in VS Code"
source: "Using VS Code.html"
modul: "Lektion 00.1: Kom godt igang med Softwaretest"
type: "brightspace-side"
kind: "indhold"
---
# C# Testing in VS Code

Visual Studio Code can be used instead of Visual Studio, although integration is less streamlined when compared with Visual Studio. If you use Mac or Linux, VS Code is a free way to go. Alternatively you can use Rider from Jetbrains (Use license from Resharper).

To use VS Code do:

Install the .NET10 **SDK**: [https://dotnet.microsoft.com/en-us/download/dotnet/10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Install VS Code: [https://code.visualstudio.com/download](https://code.visualstudio.com/download)

Install these packages in VS Code:

- C# Devkit

- Nuget Package Manager GUI (Allows you to update all packages in one go)

*Note! If C# Devkit fails to connect to the Microsoft server when loading a project, there is a bugfix [here](https://github.com/microsoft/vscode-dotnettools/issues/391). *

More details here: [https://code.visualstudio.com/docs/csharp/get-started](https://code.visualstudio.com/docs/csharp/get-started) (DON'T install the C# Coding pack!!! It will install a new vs code, .NET and extensions, but not the most recent versions and will break your existing installation)

To perform testing you can use the test explorer in VS Code. We'll use NUnit, and Test projects must be created as such. How to use the tools is shown here: [https://code.visualstudio.com/docs/csharp/testing](https://code.visualstudio.com/docs/csharp/testing)

At [https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools](https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools) you can find bash/PowerShell scripts to create a project (solution) consisting of a Class library, a console application, and a Test project can be found. Details about creating projects with code can be found here: [https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-10-0](https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-10-0)

To build, run and test your code, you can use the tools in VS Code or you can use CLI:

dotnet build
dotnet test
cd myApp
dotnet run

If you experience problems with the VSCode og C# Devkit not finding the correct .NET installation, then open the Command Palette and type "Open User Settings (JSON) and add the following (enter the correct path for dotnet, you can find it with: dotnet --list-sdks):

"dotnetAcquisitionExtension.existingDotnetPath": [
{
"extensionId": "ms-dotnettools.csharp",
"path": "/usr/local/share/dotnet/dotnet"
},
{
"extensionId": "ms-dotnettools.csdevkit",
"path": "/usr/local/share/dotnet/dotnet"
},

{
"extensionId": "ms-dotnettools.vscode-dotnet-runtime",
"path": "/usr/local/share/dotnet/dotnet"
}

]
