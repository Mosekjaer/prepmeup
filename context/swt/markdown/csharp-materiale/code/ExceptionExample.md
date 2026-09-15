---
title: "ExceptionExample"
source: "csfiles/home_dir/Exceptions/ExceptionExample.zip"
modul: "C# materiale"
type: "kildekode"
files: 12
---
# ExceptionExample

Kildeprojekt udpakket fra `ExceptionExample.zip`.

## Filtrae

```
ExceptionExample.Test.Unit/ExceptionExample.Test.Unit.csproj
ExceptionExample.Test.Unit/Properties/AssemblyInfo.cs
ExceptionExample.Test.Unit/ShipTest.cs
ExceptionExample.Test.Unit/packages.config
ExceptionExample.sln
ExceptionExample/App.config
ExceptionExample/CourseException.cs
ExceptionExample/ExceptionExample.csproj
ExceptionExample/Program.cs
ExceptionExample/Properties/AssemblyInfo.cs
ExceptionExample/Ship.cs
ExceptionExample/SpeedException.cs
```

## `ExceptionExample.Test.Unit/ExceptionExample.Test.Unit.csproj`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="..\packages\NUnit.3.12.0\build\NUnit.props" Condition="Exists('..\packages\NUnit.3.12.0\build\NUnit.props')" />
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{9818B35E-CE24-4468-AB11-F1AFD640E9D6}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ExceptionExample.Test.Unit</RootNamespace>
    <AssemblyName>ExceptionExample.Test.Unit</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <Deterministic>true</Deterministic>
    <NuGetPackageImportStamp>
    </NuGetPackageImportStamp>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="nunit.framework, Version=3.12.0.0, Culture=neutral, PublicKeyToken=2638cd05610744eb, processorArchitecture=MSIL">
      <HintPath>..\packages\NUnit.3.12.0\lib\net45\nunit.framework.dll</HintPath>
    </Reference>
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
    <Compile Include="ShipTest.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>
  <ItemGroup>
    <None Include="packages.config" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\ExceptionExample\ExceptionExample.csproj">
      <Project>{fa1ff14b-7969-4f24-bf6c-3014a726b531}</Project>
      <Name>ExceptionExample</Name>
    </ProjectReference>
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <Target Name="EnsureNuGetPackageBuildImports" BeforeTargets="PrepareForBuild">
    <PropertyGroup>
      <ErrorText>This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition="!Exists('..\packages\NUnit.3.12.0\build\NUnit.props')" Text="$([System.String]::Format('$(ErrorText)', '..\packages\NUnit.3.12.0\build\NUnit.props'))" />
  </Target>
</Project>
```

## `ExceptionExample.Test.Unit/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExceptionExample.Test.Unit")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ExceptionExample.Test.Unit")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9818b35e-ce24-4468-ab11-f1afd640e9d6")]

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

## `ExceptionExample.Test.Unit/ShipTest.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ExceptionExample.Test.Unit
{
    [TestFixture]
    public class ShipTest
    {

        [Test]
        public void ctor_CorrectParameters_DoesNotThrow()
        {
            Ship uut;
            Assert.DoesNotThrow(() => uut = new Ship(300, 12));
        }

        [Test]
        public void ctor_WrongCourse_Throws()
        {
            Ship uut;
            Assert.Throws<CourseException>(() => uut = new Ship(400, 12));
        }

        [Test]
        public void ctor_WrongSpeed_Throws()
        {
            Ship uut;
            Assert.Throws<SpeedException>(() => uut = new Ship(300, 150));
        }

        [Test]
        public void ctor_WrongSpeed_ThrowsCorrectValue()
        {
            // One method to test the content of the thrown exception
            Ship uut;
            Assert.That(
                () => uut = new Ship(250, 150),
                Throws.TypeOf<SpeedException>().With.Property("Speed").EqualTo(150));
        }

        [Test]
        public void ctor_WrongCourse_ThrowsCorrectValue()
        {
            // Another method to test the content of the thrown exception
            var exc = Assert.Catch<CourseException>(() => new Ship(500, 10));
            Assert.That(exc.Course, Is.EqualTo(500));
        }
    }

}
```

## `ExceptionExample.Test.Unit/packages.config`

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="NUnit" version="3.12.0" targetFramework="net472" />
</packages>
```

## `ExceptionExample.sln`

```text

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 16
VisualStudioVersion = 16.0.29209.62
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "ExceptionExample", "ExceptionExample\ExceptionExample.csproj", "{FA1FF14B-7969-4F24-BF6C-3014A726B531}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "ExceptionExample.Test.Unit", "ExceptionExample.Test.Unit\ExceptionExample.Test.Unit.csproj", "{9818B35E-CE24-4468-AB11-F1AFD640E9D6}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{FA1FF14B-7969-4F24-BF6C-3014A726B531}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FA1FF14B-7969-4F24-BF6C-3014A726B531}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FA1FF14B-7969-4F24-BF6C-3014A726B531}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FA1FF14B-7969-4F24-BF6C-3014A726B531}.Release|Any CPU.Build.0 = Release|Any CPU
		{9818B35E-CE24-4468-AB11-F1AFD640E9D6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9818B35E-CE24-4468-AB11-F1AFD640E9D6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9818B35E-CE24-4468-AB11-F1AFD640E9D6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9818B35E-CE24-4468-AB11-F1AFD640E9D6}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {3A71EB59-E7B3-4AAB-A502-B8478987ED91}
	EndGlobalSection
EndGlobal
```

## `ExceptionExample/App.config`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
```

## `ExceptionExample/CourseException.cs`

```csharp
using System;

namespace ExceptionExample
{
    public class CourseException : Exception
    {
        public uint Course { get; private set; }
        public CourseException(uint course)
        {
            Course = course;
        }
    }
}
```

## `ExceptionExample/ExceptionExample.csproj`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="12.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{FA1FF14B-7969-4F24-BF6C-3014A726B531}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ExceptionExample</RootNamespace>
    <AssemblyName>ExceptionExample</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
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
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Program.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <Compile Include="Ship.cs" />
    <Compile Include="CourseException.cs" />
    <Compile Include="SpeedException.cs" />
  </ItemGroup>
  <ItemGroup>
    <None Include="App.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>
```

## `ExceptionExample/Program.cs`

```csharp
using System;

namespace ExceptionExample
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Write("Course for new ship? ");
            var course = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("");

            Console.Write("Speed for new ship? ");
            var speed = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("");


            try
            {
                Console.WriteLine("Creating ship...");
                var ship = new Ship(course, speed); // Should throw exception if values are wrong
                Console.WriteLine("Done. New Ship created, heading course {0} at {1} knots", ship.Course, ship.Speed);
            }
            catch (CourseException ce)
            {
                Console.WriteLine("Error in course. Is {0}, should be in [0;360[", ce.Course);
            }

            catch (SpeedException se)
            {
                Console.WriteLine("Error in speed. Is {0}, should be in [0;100[", se.Speed);
            }
            finally
            {
                Console.WriteLine("In finally{}");
            }

        }
    }
}
```

## `ExceptionExample/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExceptionExample")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("IHA")]
[assembly: AssemblyProduct("ExceptionExample")]
[assembly: AssemblyCopyright("Copyright © IHA 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9e704e61-06d8-423e-8d1b-cced27355b2f")]

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

## `ExceptionExample/Ship.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionExample
{
    public class Ship
    {
        public uint Course { get; private set; }
        public uint Speed { get; private set; }

        public Ship(uint course, uint speed)
        {
            if (speed < 100)    
                Speed = speed;
            else 
                throw new SpeedException(speed);
            
            if (course < 360)
                Course = course;
            else throw new CourseException(course);
        }
    }
}
```

## `ExceptionExample/SpeedException.cs`

```csharp
using System;

namespace ExceptionExample
{
    public class SpeedException : Exception
    {
        public uint Speed { get; private set; }
        public SpeedException(uint spd)
        {
            Speed = spd;
        }
    }
}
```
