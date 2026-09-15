---
title: "FlashLight"
source: "csfiles/home_dir/FlashLight.zip"
modul: "Lektion 04.1: Test Types and Fake Types"
type: "kildekode"
files: 8
---
# FlashLight

Kildeprojekt udpakket fra `FlashLight.zip`.

## Filtrae

```
FlashLight.sln
FlashLight/App.config
FlashLight/FLControl.cs
FlashLight/FlashLight.csproj
FlashLight/ILed.cs
FlashLight/Led.cs
FlashLight/Program.cs
FlashLight/Properties/AssemblyInfo.cs
```

## `FlashLight.sln`

```text

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.27130.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "FlashLight", "FlashLight\FlashLight.csproj", "{23D3D52C-8965-4638-8EDC-AB4517D9494C}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{23D3D52C-8965-4638-8EDC-AB4517D9494C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{23D3D52C-8965-4638-8EDC-AB4517D9494C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{23D3D52C-8965-4638-8EDC-AB4517D9494C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{23D3D52C-8965-4638-8EDC-AB4517D9494C}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {5378D643-F0CE-497C-9AE9-126B95B407DC}
	EndGlobalSection
EndGlobal
```

## `FlashLight/App.config`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
    </startup>
</configuration>
```

## `FlashLight/FLControl.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashLight
{
    class FLControl
    {
        private enum FLState
        {
            Off,
            On
        }

        private FLState _state;
        private ILed _led;

        public FLControl(Led led)
        {
            this._led = led;
            _led.Off();
            _state = FLState.Off;
        }

        public void BtnPushed()
        {
            switch (_state)
            {
                case FLState.Off:
                    _led.On();
                    _state = FLState.On;
                    break;

                case FLState.On:
                    _led.Off();
                    _state = FLState.Off;
                    break;
            }
        }
    }
}
```

## `FlashLight/FlashLight.csproj`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{23D3D52C-8965-4638-8EDC-AB4517D9494C}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>FlashLight</RootNamespace>
    <AssemblyName>FlashLight</AssemblyName>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="FLControl.cs" />
    <Compile Include="ILed.cs" />
    <Compile Include="Led.cs" />
    <Compile Include="Program.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>
  <ItemGroup>
    <None Include="App.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

## `FlashLight/ILed.cs`

```csharp
namespace FlashLight
{
    internal interface ILed
    {
        void On();
        void Off();
    }
}
```

## `FlashLight/Led.cs`

```csharp
using System;

namespace FlashLight
{
    class Led : ILed
    {
        public void On()
        {
            Console.WriteLine("LED ON");
        }

        public void Off()
        {
            Console.WriteLine("LED OFF");
        }
    }
}
```

## `FlashLight/Program.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashLight
{
    class Program
    {
        static void Main(string[] args)
        {

            FLControl ctrl = new FLControl(new Led());

            ctrl.BtnPushed();
            ctrl.BtnPushed();
        }
    }
}
```

## `FlashLight/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("FlashLight")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("FlashLight")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("23d3d52c-8965-4638-8edc-ab4517d9494c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
```
