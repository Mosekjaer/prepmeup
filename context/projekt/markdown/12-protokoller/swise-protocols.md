# System Architectural Design — Protocols

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L20 — Protokoller |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `SWISE_Protocols.pdf` (30 slides; påtrykte slidenumre går til 31 — slide 10 mangler i PDF'en, så PNG p-10 = påtrykt 11 osv.) |
| **Type** | slides |
| **Emner dækket** | Protokol vs. interface, fysisk/logisk/software-lag, OSI 7-lagsmodel, TCP/IP-model, UDP/IP-indkapsling, Ethernet-frame, PRJ2-kommunikationsoplæg, Arduino Mega 2560-interfaces, UART (frame-format), SPI (signaler, master/slave), I2C (bus, adressering, frame-format), UART/SPI/I2C-sammenligning, duplex, Raspberry Pi-interfaces, Wi-Fi, HTTP GET |

Slidereferencer nedenfor er PDF-sidenumre (p-NN i PNG-renderingen).

---

## Protocols

- Protokoller er ét "trin op" fra det fysiske lag (signaler, navne, spændingsniveauer osv.).
- Protokoller definerer *hvordan* det fysiske interface bruges.
  - Fx: det *fysiske* interface er RS232 — 9 eller 25 ledere med data (Rx, Tx) og kontrolsignaler (RTS, RTR, CTS, …).
  - Protokollen definerer hvordan sendte/modtagne data skal fortolkes.
- Protokollen skal specificere interfacet entydigt.

> Slide 2

## Physical — logical — software

*Figur: To parter A og B. Venstre ("Physical – logical view"): kommunikation vises som lag — to logiske lag (røde pile, "Logisk") oven på ét fysisk lag (sort pil, "Fysisk"); data løber ned gennem lagene hos A, over den fysiske forbindelse, og op gennem lagene hos B. Højre ("Software Implementation Layered Boundary Classes"): samme struktur som en "Protocol Stack" af boundary-klasser, hvor hvert lag hos A taler logisk med det tilsvarende lag hos B (røde pile), mens data reelt går vertikalt gennem stakken.*

> Slide 3

## OSI 7-layer model (Open Systems Interconnection)

- OSI-modellen er abstrakte lag for computernetværk.

*Figur: To stakke (afsender/modtager) med lag 7–1. Ved hvert lag tilføjes en header: L7H+Data → L6H L7H Data → … → L2H L3H L4H L5H L6H L7H Data L2F (lag 2 har både header og footer). Lag 1 sender bitstrøm `1 0 0 1 0 1 1 0 1 1 0 0 1 0 1 1`.*

| Lag | Navn | Ansvar |
|---|---|---|
| 7 | Application Layer | Communication between applications; remote objects or functions (fx **HTTP** og web browsers) |
| 6 | Presentation Layer | Conversion between data representation |
| 5 | Session Layer | Handles connections like a phone call, TCP connection |
| 4 | Transport Layer | Splitting of data in packages/frames (segmentation/de-segmentation). Ex. Internet: **Transmission Control Protocol (TCP)**, **User Datagram Protocol (UDP)** |
| 3 | Network Layer | Translates logical to physical addresses; routing of messages through intermediate nodes; may deliver messages by splitting into several frames |
| 2 | Data Link Layer | Moves frames as a collection of bits; acknowledgement from receiver; ensure error-free transmission |
| 1 | Physical Layer | Mechanical and electrical interface; moves bits over a communication channel; concerns how the connection is established |

> Slide 4–5

## OSI Model to IP Model

| OSI Model | TCP/IP Model (or UDP IP model) | Eksempel |
|---|---|---|
| 7. Application Layer, 6. Presentation Layer, 5. Session Layer | Application Layer | HTTP |
| 4. Transport Layer | Transport Layer | TCP or UDP |
| 3. Network Layer | Internet Layer (or IP layer) | IPv4 or IPv6 |
| 2. Link Layer, 1. Physical Layer | Link Layer | Ethernet or Device Driver or.. |

"A system builds data packet through these model structure."

> Slide 6

## Example: IP model for UDP/IP

Indkapsling lag for lag (Data Packet / Frame structure):

| Lag | Indhold |
|---|---|
| Application | Data |
| Transport | UDP header \| UDP data (= Data) |
| Internet | IP header \| IP data (= UDP header + UDP data) |
| Link | Frame header \| Frame data (= IP header + IP data) \| Frame footer |

> Slide 7

## Example: Ethernet Packet (frame) — logical level

- Ethernet-protokollen bruges til dataudveksling mellem ethernet-kabel-netværksinterfaces.

Ethernet Packet (kilde: Vector, "Ethernet and IP – Ethernet Packet"):

| Felt | Størrelse | Indhold |
|---|---|---|
| Preamble | 7 Byte | 55₁₆ × 7 |
| SFD (Start Frame Delimiter) | 1 Byte | D5₁₆ |
| Receiver MAC Address (Destination) | 6 Byte | |
| Sender MAC Address (Source) | 6 Byte | |
| VLAN Tag (optional): TPID + TCI | 4 Byte | 81₁₆ 00₁₆ XX XX |
| Type Field | 2 Byte | |
| Data (Payload) | 0–1500 Byte | min. 46 Byte (42 Byte with VLAN tag) |
| PAD | variabel | Padding field — variable length to ensure minimum length of payload or Ethernet frame |
| CRC Checksum | 4 Byte | |

- Ethernet Frame = alt fra MAC-adresser til CRC (ekskl. Preamble og SFD). Min. length 64 Byte; max. length 1518 Byte (1522 Byte with VLAN tag).
- Preamble er et 7 Byte-felt der hjælper modtageren med at vide at det er reelle data (Ethernet Frame) og ikke tilfældig støj i transmissionsmediet.
- MAC-adresse bruges til at identificere fysisk kilde- og destinationsenhed.

> Slide 8

## Protocol vs. Interface

- **Protocol:**
  - "**Rule**"
  - En protokol definerer regler for udveksling af information på forbindelsespunktet.
  - Kaldes nogle gange også *Communication Protocols*
  - Bluetooth, Wi-Fi, Cellular, etc.
- **Interface:**
  - "Connecting point"
  - Interface er forbindelsespunktet mellem to entiteter
  - Interfaces kan være logiske (SW Interface) eller fysiske (HW Interface) eller begge
  - Når det er et HW-interface, kan det også kaldes *Serial Communication* (Interfaces)
  - HW Interfaces gennemgås i Lecture 12 (se `06-arkitektur-og-design/system-design-and-interfaces.md`)
  - Interface-eksempler: USB, RS232, HDMI, Ethernet, SPI, I2C, UART, etc.
    - Nogle gange kaldes interfaces også protokoller, men de passer bedst som interfaces.

> Slide 9

## Into the PRJ2 Semester

"You need to find the right interfaces between each step and you need to decide data packet structure for the interface."

> Slide 10

### Communications in the PRJ2 Project "oplæg" — new in F24

- Semester PRJ2 Gruppers egen valg

*Figur: Kæde af blokke. MUST-området (stiplet): Sensors & Actuators ↔ [choice 1?] ↔ Arduino Mega 2560 (or PSoC?) ↔ [choice 2?] ↔ UI (PC or RPI). Udenfor MUST: UI ↔ [Wi-Fi?] ↔ ?. Under hver blok og hvert valg: "Technical Analysis" => choice. Rød sky: "Communications (Protocols & Interfaces) Which? & How?". Grå sky: "1) PC terminal tekst-baseret UI eller 2) simple Web-baseret UI med et HTML siden".*

| Led | Status | Valg |
|---|---|---|
| Sensors & Actuators | MUST | Technical Analysis => choice |
| choice 1 (Sensor ↔ Arduino) | MUST | Technical Analysis => choice |
| Arduino Mega 2560 (or PSoC) | MUST | Technical Analysis => choice |
| choice 2 (Arduino ↔ UI) | MUST | Technical Analysis => choice |
| UI (PC or RPI) | MUST | 1) PC-terminal tekstbaseret UI eller 2) simpel webbaseret UI med en HTML-side |
| Wi-Fi → ? | optional | |

> Slide 11

### Between Sensors and Arduino (or PSoC)

- Fra sensorer til Arduino (or PSoC):
  - Der skal et interface til **mellem Sensor og Arduino** (eller Sensor og PSoC)
  - Arduino Mega 2560 understøtter UART, SPI, I2C (se næste slide)
  - I skal tjekke hvilke af disse interfaces jeres sensor understøtter

> Slide 12

### Arduino Interfaces

| Board name | Onboard connectivity | GPIO pins | Analog input | USB ports | ICSP header | Other hardware interfaces |
|---|---|---|---|---|---|---|
| Uno | – | 14 | 6 | 1 | 1 | SPI, UART, I2C/TWI |
| **Mega2560** (MUST in PRJ2, or PSoC) | – | 54 | 16 | 1 | 1 | SPI, 4 UART, I2C/TWI |
| Due | – | 54 | 12 | 2 | 1 | SPI, 4 UART, I2C, 2 TWI |
| Yún | Ethernet, Wi-Fi | 20 | 12 | 2 | 1 | SPI, UART, I2C/TWI |
| 101 | BLE | 14 | 6 | 1 | 1 | SPI, UART, I2C/TWI |
| MKR1000 | IEEE 802.11b/g/n | 8 | 7 | 1 | – | SPI, UART, I2C |

Kolonnen "Other hardware interfaces" er markeret som "Interfaces".

> Slide 13

## Interfaces: UART, SPI, I2C (Wired)

> Slide 14

### UART Interface

- En simpel seriel kommunikationsprotokol der lader host kommunikere med en hjælpeenhed.
- UART understøtter bi-directional, asynchronous og serial datatransmission.
- To datalinjer: én til at sende (TX) og én til at modtage (RX), som kommunikerer via digital pin 0 og digital pin 1.
- TX og RX forbindes mellem to enheder (fx USB og computer).
- UART kan også håndtere synkroniseringsproblemer mellem computere og eksterne serielle enheder.
- **UART** er en **seriel, asynkron, full-duplex kommunikationsprotokol** der er udbredt i embedded-feltet.

> Slide 15

### UART — Data Communication format

- Start bit
- Data bit
- Parity bit
- Stop bit

Frame-format:

| start | Data bit (5–9 bits) | Parity (optional) | stop |
|---|---|---|---|
| START (logic 0) | D0 D1 D2 D3 D4 D5 D6 D7 | PB | STOP (logic 1) |

*Figur 1 (UART Data Communication format [2], ref. Electricimp): Start ved detektion af overgang fra logic 1 til logic 0; indkommende data samples i midten af bit-pulsen; stop bit samples til sidst.*

*Figur: Device 1 og Device 2 krydsforbundet: Device 1 TX → Device 2 RX, Device 2 TX → Device 1 RX, fælles GND.*

> Slide 16

### Serial Peripheral Interface (SPI)

- Et andet almindeligt interface er serial peripheral interface (SPI).
- SPI lader data blive sendt og modtaget samtidigt.
- Derfor kræver SPI to ledninger mere end I2C — i alt seks forbindelser for et SPI-interface.
- SPI kører med højere datarater = 8 Mbits eller mere sammenlignet med UART og I2C.
- Der er ofte stor variation i hvordan sensorproducenter mærker SPI-forbindelser, men et hyppigt format er:
  - **SDO**: Serial data output
  - **CS**: Chip select — vælger hvilken enhed i et par der sender og hvilken der modtager
  - **SCK**: Serial clock signal
  - **SDI**: Serial data input
  - **3V3**: Power connection to 3.3V
  - **GND**: Ground connection

> Slide 17

### SPI — signaler og topologi

*Figur "SPI Interface": SPI MASTER ↔ SPI SLAVE med fire linjer:*

| Signal | Retning |
|---|---|
| SCLK | Master → Slave |
| MOSI | Master → Slave |
| MISO | Slave → Master |
| SS | Master → Slave |

*Figur "SPI Data Transmission": SCLK vist som clock-pulstog fra master til slave; SS trukket som linje fra master til slave.*

*Figur (flere slaves): SPI MASTER med SCLK, MOSI, MISO delt til SPI SLAVE 1 … SPI SLAVE N; separate select-linjer SS1, SS2, … SSn — én pr. slave.*

> Slide 18

### I2C (Inter-integrated Circuit) Interface

- I2C er en seriel kommunikationsbus med multi-**master-slave**-arkitektur.
- I2C understøtter flere slaves.
- De forskellige I2C-slaveenheder har forskellige device addresses.
- I2C-masteren tilgår en bestemt enhed via dens device address.

*Figur 2 ("SPI SLA and SDA with multiple slaves [3]", ref. Robot Electronics — figurteksten siger "SPI" men viser I2C): +5V med to pull-up-modstande Rp til SCL og SDA; Device 1, Device 2, Device 3 hænger alle på de to fælles linjer SCL og SDA.*

> Slide 19

### Inter-integrated Circuit (I2C) Interface — signaler og strøm

- I2C-formatet bruges ofte til at sende bytes mellem elektroniske enheder.
- Disse bytes kan være et tal der angiver en målt størrelse eller en sensors status.
- I2C består af signaler over to ledninger: clock line (SCL) og data line (SDA).
  - Dvs. én lodning/ledning til clock og én til data
  - **Serial clock line (SCL)**: en stabil frekvens som reference for hvornår information sendes på datalinjen; sender CLK-signaler, normalt fra master til slave
  - **Serial data (SDA)**: seriel datalinje, overfører kommunikationsdata
- To yderligere forbindelser til en I2C-sensor: power (PWR) og ground (GND).
  - PWR: "5V" eller "3.3V" — kan give forvirring om hvad der skal tilsluttes.
  - Arduino-boardet har to spændingsudgange (5V og 3.3V). Vigtigt ikke at blande dem sammen.
  - Sensoren skal tilsluttes den Arduino-udgang der matcher dens krævede indgangsspænding.
    - Nogle gange er spændingen printet på sensorens print.
    - Er sensoren kun mærket "PWR", må databladet konsulteres for 5V vs. 3.3V. [1]

> Slide 20

### I2C Data Format for transmission

- Seriel overførsel af 0 og 1 mellem master og slave over SDA-linjen.
- Strukturen af en I2C seriel datasekvens:
  - Start Bit
  - Address Bits (7 bit or 10 bit)
  - Read and Write Bit (1 bit)
  - Response Bit (1 bit)
  - Data bit + response bit (data bit 8 bit; response bit 1 bit)
    - data + response kan gentages mange gange indtil stop bit
  - Stop Bit

Frame-format (Figure 4 [4]):

| START CONDITION | 7 Address Bits | R/W Bit | ACK/NACK Bit | 8 Data Bits | ACK/NACK Bit | STOP CONDITION |
|---|---|---|---|---|---|---|
| | Contains address of slave device to be communicated | | | Repeated until all the data bits are transferred | | |

Hele sekvensen fra START til STOP kaldes en *Transaction*.

> Slide 21

### UART vs. SPI vs. I2C

| Protocol | Complexity | Speed | # of Devices | # of Wires | Duplex | No. of master and slave |
|---|---|---|---|---|---|---|
| UART | Simple | Slowest | Up to 2 devices | 1 | Full Duplex | Single to Single |
| I2C | Easy to chain multiple devices | Faster than UART | Up to 127, but gets complex | 2 | Half Duplex | Multiple slaves and master |
| SPI | Complex as device increases | Fastest | Many, but gets complex | 4 | Full Duplex | 1 master, multiple slaves |

(Figure 5: Comparison between UART, SPI and I2C [2]. Sliden skriver "# of Wries".)

> Slide 22

### Duplex: Communication mode (Types of transmission)

- **Full-duplex**
  - Data kan sendes og modtages samtidigt.
  - Data kan sendes fra master til slave og fra slave til master på samme tid.
- **Half-duplex**
  - Data kan sendes eller modtages, men ikke samtidigt.
  - Datatransmission kan kun foregå i én retning ad gangen.

*Figur 6 [1]: Simplex — Transmitter → Receiver (én retning). Half Duplex — Transmitter/Receiver i hver ende, skiftevis retning. Full Duplex — to separate kanaler, Transmitter → Receiver i begge retninger samtidigt.*

> Slide 23

### What to choose

- Sensoren skal understøtte interfacet — tjek sensorens datablad.
- Studér interfacet — referencer er givet i sliderne.
- Vurdér kompleksitet vs. den funktionalitet I har brug for vs. den performance systemet kræver osv. — vælg det der passer til jeres system.
- Angiv referencer når I bruger dokumenterne.
- **Lav Technical Analysis for interfaces.**
- (Optional, kun hvis I bruger RPI som ekstra) De samme interfaces kan bruges mellem Arduino og RPI / mellem PSoC og RPI.

> Slide 24

## (optional) Raspberry Pi

### Protocols and Interfaces on Raspberry Pi models

| Raspberry Pi | Onboard connectivity (Protocols) | GPIO pins | USB ports | Display ports/interfaces | Camera port | Other hardware interfaces (Interfaces) |
|---|---|---|---|---|---|---|
| Zero | – | 40 | 1 mini | Mini-HDMI | CSI | UART, SPI, I2C |
| Zero W | Wi-Fi, Bluetooth 4.1, BLE | 40 | 1 mini | Mini-HDMI | CSI | UART, SPI, I2C |
| 1 B+ | Ethernet | 40 | 4 | HDMI, DSI, 3.5 mm Video Jack | CSI | UART, SPI, I2C |
| 2 B | Ethernet | 40 | 4 | HDMI, DSI, 3.5 mm Video Jack | CSI | UART, SPI, I2C |
| 3 B | Ethernet, Wi-Fi, Bluetooth 4.1, BLE | 40 | 4 | HDMI, DSI, 3.5 mm Video Jack | CSI | UART, SPI, I2C |

Zero W og 3 B er fremhævet med pile (de modeller med Wi-Fi). Kolonnen "Onboard connectivity" er markeret "Protocols", "Other hardware interfaces" er markeret "Interfaces".

> Slide 25–26

## (Bonus Optional) Protocols for Web: Wi-Fi, HTTP. Markup language: HTML

- Web-protokoller gennemgås ikke i dybden — det er 3.-semesterstof.
- Det er en valgfri del af PRJ2-"oplægget".
- Brug ikke det meste af tiden på dette i PRJ2 — brug tiden fornuftigt.
- Ved tidspres: fokusér på "MUST", ikke på det valgfri.
  - I lærer det på 3. semester i et andet kursus.

> Slide 27

### Wi-Fi

- Wi-Fi er en kommunikationsprotokol — én af de trådløse netværksprotokoller baseret på IEEE 802.11-standardfamilien.
- Wi-Fi er kortrækkende kommunikation, typisk brugt til *local area networking* af enheder og internetadgang.
- Wi-Fi er en *forbindelsesprotokol* for trådløs kommunikation.
- Den laver ikke jeres webside og får ikke data på internettet — Wi-Fi etablerer kun forbindelsen.
- Derfor kræves en anden protokol for at få data vist på en webside (hvis I vil have web-baseret UI).

> Slide 28

### HTTP Protocol

- Forbindelsen til web går via Wi-Fi.
- Men Wi-Fi er kun en forbindelse — den henter ikke automatisk data fra sensoren.
- Data skal **hentes (get/fetch)** fra websiden til det embedded system (sensor – Arduino/PSoC):
  - Det gøres af en anden protokol: HTTP.
- **HTTP** (**H**yper**t**ext **T**ransfer **P**rotocol)
  - HTTP specificerer hvordan hypertekst (dvs. linkede webdokumenter) overføres mellem to endpoints (fx computere, maskiner) via internetforbindelse (fx via Wi-Fi).
- **HTTP Request**
  - For at hente data skal websiden sende et HTTP Request til Pi eller PC via Wi-Fi; derefter skal Pi/PC-koden levere data fra sensoren.
- HTTP Request er **HTTP GET method**
  - GET-metoden implementeres i websidens kode.
- Et HTTP request skal indeholde:
  - En HTTP-metode (fx GET)
  - En host URL: I skal definere jeres Pi-adresse
  - En endpoint path (fx `/tempsensor/`): I skal definere jeres sensornavn

Protokoldetaljer læres på 3. sem (SW), 4. sem (HW) — obligatorisk fag — og 6. sem (ST) — valgfag. Valgfag (6. semester): et kursus åbent for SW, ST*, HW med avancerede protokolemner.

> Slide 29

## References & Links

- [1] Serial Communication Protocols and Standards; RS232/485, UART/USART, SPI, USB, INSTEON, Wi-Fi and WiMAX, A. Jamlipour, J. Zhang, M. Ruggieri, 2020
- [2] UART, https://www.seeedstudio.com/blog/2022/09/08/uart-communication-protocol-and-how-it-works/
- [3] SPI, https://www.seeedstudio.com/blog/2019/11/22/spi-introduction-to-serial-peripheral-interface/
- [4] I2C, https://www.seeedstudio.com/blog/2022/09/02/i2c-communication-protocol-and-how-it-works/

> Slide 30
