# Kode: Minutur som WPF-applikation (C#, .NET Framework)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L23 — Application Model og Implementation II |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | Visual Studio solution `MinuturWPF` (projekt `MinutUrWPF`, 7 kildefiler; `Properties/`, `App.config` og projektfiler udeladt) |
| **Type** | eksempel (kode) |
| **Emner dækket** | Applikationsmodel implementeret i GUI, boundary-klasser der wrapper WPF-kontroller (`TextBox`, `DispatcherTimer`), event-drevet i stedet for polling, mapper Boundary/Control |

---

## Mapping fra applikationsmodel til C#-klasser

Svarer til klassediagrammet i `ImplementationFinalGUI.pdf` (se `implementation-minutur-gui-wpf.md`). Mapperne `Boundary/` og `Control/` afspejler stereotyperne direkte.

| Applikationsmodel-klasse | Stereotype | C#-klasse (fil) | Rolle |
|---|---|---|---|
| Hovedprogram / MainWindow | — | `MainWindow` (`MainWindow.xaml` + `MainWindow.xaml.cs`) | Ejer `_display`, `_timer`, `_ur`; kobler dem i constructoren; click-handlers `Start_Click`, `Stop_Click`, `Reset_Click` sender hændelser til Ur |
| KnapPanel | «boundary» | 3 × `Button` i XAML (`Start`, `Stop`, `Reset`) | WPF's egne kontroller erstatter KnapPanel; ingen egen klasse |
| Display | «boundary» | `DisplayBoundary` (`Boundary/DisplayBoundary.cs`) | Wrapper om `TextBox Display`; `visTid(min, sek)` sætter `Text = "mm:ss"` |
| Timer | «boundary» | `TimerBoundary` (`Boundary/TimerBoundary.cs`) | Wrapper om `DispatcherTimer` (1 s); `HandleTimerTick` kalder `MitUr.TimeOut()`; `Start()`, `Stop()` |
| Ur | «controller» | `Ur` (`Control/Ur.cs`) | State machine (`Tilstand.STOPPET`/`STARTET`), `Start()`, `Stop()`, `Reset()`, `TimeOut()`; private `TaelOp()`, `Nulstil()` |

Bemærkninger:
- Modellens `vis(min, sec)` hedder `visTid(minutter, sekunder)` i koden; `timeout()` hedder `TimeOut()`.
- Associationen Timer→Ur (så timeren kan levere `timeout`) er property `MitUr` på `TimerBoundary`, sat af `MainWindow` efter `Ur` er oprettet — der er en cirkulær reference Ur↔TimerBoundary.
- I `TimeOut()` kaldes `_timer.Start()` ikke igen (som i Arduino-versionerne), fordi `DispatcherTimer` er periodisk og bliver ved med at ticke, indtil `Stop()`.
- Ingen polling: hændelserne `start`/`stop`/`reset` kommer fra WPF's `Click`-events, `timeout` fra `DispatcherTimer.Tick`.

## Kildefiler

### `App.xaml`

```xml
<Application x:Class="MinutUrWPF.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:MinutUrWPF"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
         
    </Application.Resources>
</Application>

```

### `App.xaml.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MinutUrWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}

```

### `MainWindow.xaml`

```xml
<Window x:Class="MinutUrWPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:MinutUrWPF"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <TextBox x:Name="Display" Height="78" Margin="142,74,0,0" TextWrapping="Wrap" Text="TextBox" VerticalAlignment="Top" Width="491" FontSize="48" FontWeight="Bold" HorizontalAlignment="Left" TextAlignment="Center"/>
        <Button x:Name="Start" Content="Start" HorizontalAlignment="Left" Margin="72,233,0,0" VerticalAlignment="Top" Width="133" Height="53" FontSize="22" FontWeight="Bold" Click="Start_Click"/>
        <Button x:Name="Stop" Content="Stop" HorizontalAlignment="Left" Margin="300,233,0,0" VerticalAlignment="Top" Width="133" Height="53" FontSize="22" FontWeight="Bold" Click="Stop_Click"/>
        <Button x:Name="Reset" Content="Reset" HorizontalAlignment="Left" Margin="552,233,0,0" VerticalAlignment="Top" Width="133" Height="53" Click="Reset_Click" FontSize="22" FontWeight="Bold"/>

    </Grid>
</Window>

```

### `MainWindow.xaml.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MinutUrWPF.Boundary;
using MinutUrWPF.Control;

namespace MinutUrWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DisplayBoundary _display;
        private TimerBoundary _timer;

        private Ur _ur;

        public MainWindow()
        {
            InitializeComponent();

            _display = new DisplayBoundary(Display);
            _timer = new TimerBoundary();

            _ur = new Ur(_timer, _display);
            _timer.MitUr = _ur;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _ur.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _ur.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _ur.Reset();
        }

    }
}

```

### `Boundary/DisplayBoundary.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MinutUrWPF.Boundary
{
    public class DisplayBoundary
    {
        private TextBox _textBox;
        public DisplayBoundary(TextBox textBox)
        {
            _textBox = textBox;
            visTid(0,0);
        }

        public void visTid(int minutter, int sekunder)
        {
            _textBox.Text = $"{minutter:D2}:{sekunder:D2}";
        }
    }
}

```

### `Boundary/TimerBoundary.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using MinutUrWPF.Control;

namespace MinutUrWPF.Boundary
{
    public class TimerBoundary
    {
        public Ur MitUr { get; set; }

        private DispatcherTimer _timer;
        public TimerBoundary()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += HandleTimerTick;

        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            MitUr.TimeOut();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

    }
}

```

### `Control/Ur.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinutUrWPF.Boundary;

namespace MinutUrWPF.Control
{
    public class Ur
    {
        private TimerBoundary _timer;
        private DisplayBoundary _display;

        private enum Tilstand
        {
            STOPPET,
            STARTET
        };

        private Tilstand _tilstand;

        private int _minutter;
        private int _sekunder;

        public Ur(TimerBoundary timer, DisplayBoundary display)
        {
            _timer = timer;
            _display = display;

            _tilstand = Tilstand.STOPPET;
            Nulstil();
        }

        public void Start()
        {
            switch (_tilstand)
            {
                case Tilstand.STARTET:
                    break;

                case Tilstand.STOPPET:
                    _timer.Start();
                    _tilstand = Tilstand.STARTET;
                    break;
            }
        }

        public void Stop()
        {
            switch (_tilstand)
            {
                case Tilstand.STARTET:
                    _timer.Stop();
                    _tilstand = Tilstand.STOPPET;
                    break;

                case Tilstand.STOPPET:
                    break;
            }

        }

        public void TimeOut()
        {
            switch (_tilstand)
            {
                case Tilstand.STARTET:
                    TaelOp();
                    _display.visTid(_minutter, _sekunder);
                    break;

                case Tilstand.STOPPET:
                    break;
            }

        }

        private void TaelOp()
        {
            _sekunder++;
            if (_sekunder >= 60)
            {
                _minutter++;
                _sekunder = 0;
            }

        }

        public void Reset()
        {
            switch (_tilstand)
            {
                case Tilstand.STARTET:
                    break;

                case Tilstand.STOPPET:
                    Nulstil();
                    break;
            }

        }

        private void Nulstil()
        {
            _minutter = _sekunder = 0;
            _display.visTid(_minutter, _sekunder);
        }
    }
}

```

