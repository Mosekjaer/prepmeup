# Løsningsforslag: Access Control System — IBD

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L8 — SysML IBD (Brightspace "L8 IBD Løsningsforslag": "L8 SysML IBD: Access Control System IBD Løsning") |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `AccessControlSystem_IBD.pdf` (1 side, UMLet-tegning) |
| **Type** | løsningsforslag |
| **Emner dækket** | Internal Block Diagram (ibd), parts, flow ports, boundary ports, multiplicitet på port, konjugeret port (~), interface-typer (GPIO, string, Card, Force) |

Selve opgaveteksten til Access Control System findes ikke i zip 11 (kun løsningen er lagt op). Systemet er et adgangskontrolsystem med kortlæser og tastatur, der styrer en dør og giver feedback via to LED'er og en buzzer.

---

## `ibd Access Control System`

Diagramramme: `ibd Access Control System`. Rammen er blokken Access Control System.

### Parts

| Part | Type | Ports på part |
|---|---|---|
| (unavngivet) | Card Reader | `card:Card` (in, venstre), `id:string` (out, højre) |
| (unavngivet) | Keypad | `keyPressed[1..12]:Force` (in, venstre), `key:string` (out, højre) |
| (unavngivet) | Control | `id:string` (in, venstre øverst), `keyVal:string` (in, venstre nederst), `green:GPIO` (out, højre øverst), `red:GPIO` (out, højre midt), `buzzer:GPIO` (out, højre nederst), `doorCtrl:~DoorCtrl` (bidirektionel, bund — tegnet som port med dobbeltpil) |
| `green` | LED | `on:GPIO` (in) |
| `red` | LED | `on:GPIO` (in) |
| (unavngivet) | Buzzer | `ctrl:GPIO` (in) |

Parts uden instansnavn skrives `:Card Reader`, `:Keypad`, `:Control`, `:Buzzer`. De to LED'er har instansnavne: `green:LED` og `red:LED`.

### Boundary ports (på rammen)

| Side | Port | Retning |
|---|---|---|
| venstre, øverst | `card:Card` | in |
| venstre, nederst | `keyPressed[1..12]:Force` | in — multiplicitet `[1..12]` = tastaturets 12 taster |
| bund | `doorCtrl:~DoorCtrl` | bidirektionel (dobbeltpil-symbol), konjugeret |

### Connectors

| Fra | Til | Type/bemærkning |
|---|---|---|
| boundary `card:Card` | `:Card Reader`.`card` | kortet præsenteres for læseren |
| `:Card Reader`.`id:string` | `:Control` (in-port, øverst) | kort-id som streng |
| boundary `keyPressed[1..12]:Force` | `:Keypad` (in-port) | fysisk tryk på tast |
| `:Keypad`.`key:string` | `:Control`.`keyVal:string` | tastværdi som streng — bemærk at de to portnavne er forskellige (`key` / `keyVal`) men typen er ens |
| `:Control`.`green:GPIO` | `green:LED`.`on:GPIO` | |
| `:Control`.`red:GPIO` | `red:LED`.`on:GPIO` | |
| `:Control`.`buzzer:GPIO` | `:Buzzer`.`ctrl:GPIO` | |
| `:Control`.`doorCtrl:~DoorCtrl` | boundary `doorCtrl:~DoorCtrl` | Control's dørstyring føres direkte ud af systemgrænsen; døren/dørmotoren er uden for systemet |

Ingen item flows er tegnet; retning fremgår af port-pilene (alle simple flow ports med enkeltpil, undtagen `doorCtrl` som har dobbeltpil).

### Pointer

- `keyPressed[1..12]:Force`: multiplicitet på en port bruges til at udtrykke at der er 12 fysiske taster uden at tegne 12 ports.
- `~DoorCtrl` på både `:Control` og rammen: samme (konjugerede) port føres ud gennem rammen — den ikke-konjugerede `DoorCtrl` sidder i så fald på døren udenfor systemet.
- Alle output fra `:Control` til aktuatorer er af typen `GPIO` — LED'erne og buzzeren er simple digitale udgange.
