---
title: "ExampleStatemachine"
source: "csfiles/home_dir/BB-Cou-UUVA-69276/ExampleStatemachine.zip"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
type: "kildekode"
files: 12
---
# ExampleStatemachine

Kildeprojekt udpakket fra `ExampleStatemachine.zip`.

## Filtrae

```
ExampleStatemachine.Test.Unit/ExampleStatemachine.Test.Unit.csproj
ExampleStatemachine.Test.Unit/FlashLightUnitTest.cs
ExampleStatemachine.Test.Unit/Properties/AssemblyInfo.cs
ExampleStatemachine.Test.Unit/Settings.aiis
ExampleStatemachine.Test.Unit/packages.config
ExampleStatemachine.sln
ExampleStatemachine/ExampleStatemachine.csproj
ExampleStatemachine/FlashLightCtrl.cs
ExampleStatemachine/IFlashLightCtrl.cs
ExampleStatemachine/ILamp.cs
ExampleStatemachine/Properties/AssemblyInfo.cs
ExampleStatemachine/Settings.aiis
```

## `ExampleStatemachine.Test.Unit/ExampleStatemachine.Test.Unit.csproj`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{CA7FE082-7D4B-489B-9091-B8B2D43951AD}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ExampleStatemachine.Test.Unit</RootNamespace>
    <AssemblyName>ExampleStatemachine.Test.Unit</AssemblyName>
    <TargetFrameworkVersion>v4.5.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
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
    <Reference Include="NSubstitute, Version=1.10.0.0, Culture=neutral, PublicKeyToken=92dd2e9066daa5ca, processorArchitecture=MSIL">
      <HintPath>..\packages\NSubstitute.1.10.0.0\lib\net45\NSubstitute.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Reference Include="nunit.framework, Version=3.4.1.0, Culture=neutral, PublicKeyToken=2638cd05610744eb, processorArchitecture=MSIL">
      <HintPath>..\packages\NUnit.3.4.1\lib\net45\nunit.framework.dll</HintPath>
      <Private>True</Private>
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
    <Compile Include="FlashLightUnitTest.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>
  <ItemGroup>
    <None Include="packages.config" />
    <None Include="Settings.aiis" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\ExampleStatemachine\ExampleStatemachine.csproj">
      <Project>{3dfd71bd-32f5-46de-bb4c-2dced7a91267}</Project>
      <Name>ExampleStatemachine</Name>
    </ProjectReference>
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <ProjectExtensions>
    <VisualStudio>
      <UserProperties Name="ExampleStatemachine.Test.Unit" />
    </VisualStudio>
  </ProjectExtensions>
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>
```

## `ExampleStatemachine.Test.Unit/FlashLightUnitTest.cs`

```csharp
using NSubstitute;
using NUnit.Framework;

namespace ExampleStatemachine.Test.Unit
{
    [TestFixture]
    public class FlashLightUnitTest
    {
        private ILamp _lamp;
        private IFlashLightCtrl _uut;

        [SetUp]
        public void SetUp()
        {
            _lamp = Substitute.For<ILamp>();
            _uut = new FlashLightCtrl(_lamp);
        }

        [Test]
        public void PwrPressedOnce_Initial_LowBeamSet()
        {
            _uut.PwrPressed();
            _lamp.Received().Low();
        }

        [Test]
        public void PwrPressedTwice_Initial_HighBeamSet()
        {
            _uut.PwrPressed();
            _uut.PwrPressed();
            _lamp.Received().High();
        }

        [Test]
        public void PwrPressedThrice_Initial_LampOff()
        {
            _uut.PwrPressed();
            _uut.PwrPressed();
            _uut.PwrPressed();
            _lamp.Received().Off();
        }



        [Test]
        public void LowBattery_LowBatteryInLampOff_LampDidNotReceiveOff()
        {
            _uut.LowBattery();
            _uut.PwrPressed();

            _lamp.DidNotReceive().Off();
        }

        [Test]
        public void LowBattery_LowBatteryInLampOff_LampDidNotReceiveLow()
        {
            _uut.LowBattery();
            _uut.PwrPressed();

            _lamp.DidNotReceive().Low();
        }

        [Test]
        public void LowBattery_LowBatteryInLampOff_LampDidNotReceivehigh()
        {
            _uut.LowBattery();
            _uut.PwrPressed();

            _lamp.DidNotReceive().High();
        }

        [Test]
        public void LowBattery_LowBatteryInLowBeam_NoReaction()
        {
            _uut.PwrPressed();
            _uut.LowBattery();
            _uut.PwrPressed();
            _lamp.Received().High();
        }

        [Test]
        public void LowBattery_LowBatteryInHighBeam_NoReaction()
        {
            _uut.PwrPressed();
            _uut.PwrPressed();
            _uut.LowBattery();
            _uut.PwrPressed();
            _lamp.Received().Off();
        }

        [Test]
        public void BatteryOk_LowBattery_OperationBackToNormal()
        {
            _uut.LowBattery();
            _uut.BatteryOk();
            _uut.PwrPressed();
            _lamp.Received().Low();
        }


    }
}
```

## `ExampleStatemachine.Test.Unit/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExampleStatemachine.Test.Unit")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ExampleStatemachine.Test.Unit")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ca7fe082-7d4b-489b-9091-b8b2d43951ad")]

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

## `ExampleStatemachine.Test.Unit/Settings.aiis`

```text
{
  "__type": "ArtOfTest.WebAii.Design.UserSettings",
  "__value": {
    "UseHttpProxy": false,
    "RecordjQueryInDescriptorsIfPossible": true,
    "ShouldDeleteElement": false,
    "SkipDeleteElementPrompt": false,
    "HideFindExpressionWelcome": false,
    "MenuHoldTime": 1.0,
    "SilverlightConnectTimeout": 60000,
    "BaseClassName": "BaseWebAiiTest",
    "UrlRecordMode": 2,
    "HighlightBorderColor": -65536,
    "HighlightBorderSize": 1,
    "CodeGenerationElementIdentificationType": 1,
    "UrlHistory": [],
    "AbsoluteDragDropRecording": true,
    "ImageScalePercentage": 75,
    "SelectedIdentificaitonScheme": "Html",
    "IdentificationSchemes": {
      "Html": {
        "__type": "ArtOfTest.Common.Design.Translation.IdentificationOptionsScheme",
        "__value": {
          "CheckFindParamUniqueness": true,
          "AutoDetectTestRegions": true,
          "TryAttributeCombinations": true,
          "AlwaysAssertTagName": true,
          "IdentificationsPerTag": {
            "All Elements": {
              "__type": "ArtOfTest.Common.Design.Translation.IdentificationDescriptorList",
              "__value": [
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "id"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "name"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "src"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "href"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "value"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "alt"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 1,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 8,
                    "AttributeName": null
                  }
                }
              ]
            }
          },
          "TechnologyType": 1
        }
      },
      "Silverlight": {
        "__type": "ArtOfTest.Common.Design.Translation.IdentificationOptionsScheme",
        "__value": {
          "CheckFindParamUniqueness": true,
          "AutoDetectTestRegions": true,
          "TryAttributeCombinations": true,
          "AlwaysAssertTagName": true,
          "IdentificationsPerTag": {
            "All Elements": {
              "__type": "ArtOfTest.Common.Design.Translation.IdentificationDescriptorList",
              "__value": [
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 5,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 1,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 6,
                    "AttributeName": null
                  }
                }
              ]
            }
          },
          "TechnologyType": 2
        }
      }
    },
    "UnitTypeTypeGeneration": 1,
    "RecorderBaseUrl": "",
    "IsStoryBoardCapturingEnabled": true,
    "SimulateRealClickByDefault": false,
    "SimulateRealTypingByDefault": false,
    "SimulateRealSilverlightUserByDefault": true,
    "DefaultDropDownSelection": 1,
    "QuickExecutionElementWaitTimeout": 15000,
    "QuickExecutionClientReadyTimeout": 60000,
    "QcServerUrl": null,
    "QcUserName": "",
    "QcPassword": "",
    "QcSkipAuthDialog": false,
    "QcDomain": null,
    "QcProject": null,
    "TfsUserName": "",
    "TfsPassword": "",
    "TfsSkipAuthDialog": true,
    "TfsDomain": "",
    "TeamPulseServerUrl": "",
    "TpUserName": "",
    "TpPassword": "",
    "TpUseWindowsAuth": false,
    "TpSkipAuthDialog": false,
    "TpDefaultProjectId": -1,
    "RecordWpfWindowStateChanged": false,
    "PromptNameOnAddElement": true,
    "DefaultWPFApplication": null,
    "UseLegacySilverlightFindLogic": false,
    "ProjectGuid": "38473801-74d3-4891-9016-5ac658d0f73b",
    "SourceControlRepository": null,
    "IsOnline": false,
    "ProjectLanguage": 1,
    "ProjectReferences": [
      "System",
      "System.Core",
      "ArtOfTest.WebAii, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=4fd5f65be123776c",
      "ArtOfTest.WebAii.Design, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=4fc62bbc3827ab1d",
      "Telerik.WebAii.Controls.Html, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.WebAii.Controls.Xaml, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.WebAii.Controls.Xaml.Wpf, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.TestingFramework.Controls.KendoUI, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45"
    ],
    "ScheduleServerUrl": "http://localhost:8009",
    "IsScheduleServerRemote": false,
    "ProjectVersion": "2016.2.610.0",
    "Namespace": "ExampleStatemachine.Test.Unit",
    "AssemblyName": "ExampleStatemachine.Test.Unit",
    "OutputFolder": "bin",
    "ExcludedFiles": [],
    "DisabledTranslators": [],
    "SkipFindExpressionSetDataDrivenWarning": false,
    "BugTrackerPersistableSettings": {},
    "ActiveBugTrackers": [],
    "BugTitleMask": "{Step name} step on {Test name} test failed.",
    "BugAddAttachment": true,
    "BugAutoSubmit": false,
    "BugDescriptionMask": "{Description}",
    "SelectedBrowserOption": 4
  }
}
```

## `ExampleStatemachine.Test.Unit/packages.config`

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="NSubstitute" version="1.10.0.0" targetFramework="net452" />
  <package id="NUnit" version="3.4.1" targetFramework="net452" />
</packages>
```

## `ExampleStatemachine.sln`

```text

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.24720.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "ExampleStatemachine", "ExampleStatemachine\ExampleStatemachine.csproj", "{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "ExampleStatemachine.Test.Unit", "ExampleStatemachine.Test.Unit\ExampleStatemachine.Test.Unit.csproj", "{CA7FE082-7D4B-489B-9091-B8B2D43951AD}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}.Release|Any CPU.Build.0 = Release|Any CPU
		{CA7FE082-7D4B-489B-9091-B8B2D43951AD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CA7FE082-7D4B-489B-9091-B8B2D43951AD}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{CA7FE082-7D4B-489B-9091-B8B2D43951AD}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CA7FE082-7D4B-489B-9091-B8B2D43951AD}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
```

## `ExampleStatemachine/ExampleStatemachine.csproj`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{3DFD71BD-32F5-46DE-BB4C-2DCED7A91267}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>ExampleStatemachine</RootNamespace>
    <AssemblyName>ExampleStatemachine</AssemblyName>
    <TargetFrameworkVersion>v4.5.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
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
    <Compile Include="FlashLightCtrl.cs" />
    <Compile Include="IFlashLightCtrl.cs" />
    <Compile Include="ILamp.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
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

## `ExampleStatemachine/FlashLightCtrl.cs`

```csharp
using System;

namespace ExampleStatemachine
{
    public class FlashLightCtrl : IFlashLightCtrl
    {
        enum FlState
        {
            Off, Low, High, PowerSave
        }

        private FlState _curState;
        private readonly ILamp _lamp;

        public FlashLightCtrl(ILamp lamp)
        {
            _lamp = lamp;
            _curState = FlState.Off;
        }

        public void PwrPressed()
        {
            switch (_curState)
            {
                case FlState.Off:
                    _lamp.Low();
                    _curState = FlState.Low;
                    break;

                case FlState.Low:
                    _lamp.High();
                    _curState = FlState.High;
                    break;
                    
                case FlState.High:
                    _lamp.Off();
                    _curState = FlState.Low;
                    break;

                case FlState.PowerSave:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }

        public void LowBattery()
        {
            switch(_curState)
            {
                case FlState.Off:
                    _curState = FlState.PowerSave;
                    break;

                case FlState.Low:
                case FlState.High:
                case FlState.PowerSave:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void BatteryOk()
        {
            switch(_curState)
            {
                case FlState.Off:
                case FlState.Low:
                case FlState.High:
                    break;
                case FlState.PowerSave:
                    _curState = FlState.Off;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
```

## `ExampleStatemachine/IFlashLightCtrl.cs`

```csharp
namespace ExampleStatemachine
{
    public interface IFlashLightCtrl
    {
        void PwrPressed();
        void LowBattery();
        void BatteryOk();
    }
}
```

## `ExampleStatemachine/ILamp.cs`

```csharp
namespace ExampleStatemachine
{
    public interface ILamp
    {
        void Low();
        void High();
        void Off();
    }
}
```

## `ExampleStatemachine/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExampleStatemachine")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ExampleStatemachine")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3dfd71bd-32f5-46de-bb4c-2dced7a91267")]

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

## `ExampleStatemachine/Settings.aiis`

```text
{
  "__type": "ArtOfTest.WebAii.Design.UserSettings",
  "__value": {
    "UseHttpProxy": false,
    "RecordjQueryInDescriptorsIfPossible": true,
    "ShouldDeleteElement": false,
    "SkipDeleteElementPrompt": false,
    "HideFindExpressionWelcome": false,
    "MenuHoldTime": 1.0,
    "SilverlightConnectTimeout": 60000,
    "BaseClassName": "BaseWebAiiTest",
    "UrlRecordMode": 2,
    "HighlightBorderColor": -65536,
    "HighlightBorderSize": 1,
    "CodeGenerationElementIdentificationType": 1,
    "UrlHistory": [],
    "AbsoluteDragDropRecording": true,
    "ImageScalePercentage": 75,
    "SelectedIdentificaitonScheme": "Html",
    "IdentificationSchemes": {
      "Html": {
        "__type": "ArtOfTest.Common.Design.Translation.IdentificationOptionsScheme",
        "__value": {
          "CheckFindParamUniqueness": true,
          "AutoDetectTestRegions": true,
          "TryAttributeCombinations": true,
          "AlwaysAssertTagName": true,
          "IdentificationsPerTag": {
            "All Elements": {
              "__type": "ArtOfTest.Common.Design.Translation.IdentificationDescriptorList",
              "__value": [
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "id"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "name"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "src"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "href"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "value"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": "alt"
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 1,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.HtmlIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 8,
                    "AttributeName": null
                  }
                }
              ]
            }
          },
          "TechnologyType": 1
        }
      },
      "Silverlight": {
        "__type": "ArtOfTest.Common.Design.Translation.IdentificationOptionsScheme",
        "__value": {
          "CheckFindParamUniqueness": true,
          "AutoDetectTestRegions": true,
          "TryAttributeCombinations": true,
          "AlwaysAssertTagName": true,
          "IdentificationsPerTag": {
            "All Elements": {
              "__type": "ArtOfTest.Common.Design.Translation.IdentificationDescriptorList",
              "__value": [
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 0,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 5,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": false,
                    "SearchType": 1,
                    "AttributeName": null
                  }
                },
                {
                  "__type": "ArtOfTest.WebAii.Design.Translation.Silverlight.SilverlightIdentificationDescriptor",
                  "__value": {
                    "IsLocked": true,
                    "SearchType": 6,
                    "AttributeName": null
                  }
                }
              ]
            }
          },
          "TechnologyType": 2
        }
      }
    },
    "UnitTypeTypeGeneration": 1,
    "RecorderBaseUrl": "",
    "IsStoryBoardCapturingEnabled": true,
    "SimulateRealClickByDefault": false,
    "SimulateRealTypingByDefault": false,
    "SimulateRealSilverlightUserByDefault": true,
    "DefaultDropDownSelection": 1,
    "QuickExecutionElementWaitTimeout": 15000,
    "QuickExecutionClientReadyTimeout": 60000,
    "QcServerUrl": null,
    "QcUserName": "",
    "QcPassword": "",
    "QcSkipAuthDialog": false,
    "QcDomain": null,
    "QcProject": null,
    "TfsUserName": "",
    "TfsPassword": "",
    "TfsSkipAuthDialog": true,
    "TfsDomain": "",
    "TeamPulseServerUrl": "",
    "TpUserName": "",
    "TpPassword": "",
    "TpUseWindowsAuth": false,
    "TpSkipAuthDialog": false,
    "TpDefaultProjectId": -1,
    "RecordWpfWindowStateChanged": false,
    "PromptNameOnAddElement": true,
    "DefaultWPFApplication": null,
    "UseLegacySilverlightFindLogic": false,
    "ProjectGuid": "4a4a6a11-82df-4d6f-afa7-61b5fb73398b",
    "SourceControlRepository": null,
    "IsOnline": false,
    "ProjectLanguage": 2,
    "ProjectReferences": [
      "System",
      "System.Core",
      "ArtOfTest.WebAii, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=4fd5f65be123776c",
      "ArtOfTest.WebAii.Design, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=4fc62bbc3827ab1d",
      "Telerik.WebAii.Controls.Html, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.WebAii.Controls.Xaml, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.WebAii.Controls.Xaml.Wpf, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45",
      "Telerik.TestingFramework.Controls.KendoUI, Version=2016.2.630.0, Culture=neutral, PublicKeyToken=528163f3e645de45"
    ],
    "ScheduleServerUrl": "http://localhost:8009",
    "IsScheduleServerRemote": false,
    "ProjectVersion": "2016.2.610.0",
    "Namespace": "ExampleStatemachine",
    "AssemblyName": "ExampleStatemachine",
    "OutputFolder": "bin",
    "ExcludedFiles": [],
    "DisabledTranslators": [],
    "SkipFindExpressionSetDataDrivenWarning": false,
    "BugTrackerPersistableSettings": {},
    "ActiveBugTrackers": [],
    "BugTitleMask": "{Step name} step on {Test name} test failed.",
    "BugAddAttachment": true,
    "BugAutoSubmit": false,
    "BugDescriptionMask": "{Description}",
    "SelectedBrowserOption": 4
  }
}
```
