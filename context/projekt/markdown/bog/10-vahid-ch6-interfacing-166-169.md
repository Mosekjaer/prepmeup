# Interfacing (Vahid & Givargis, kap. 6, s. 166–169)

## Metadata

| Felt | Værdi |
|---|---|
| **Kursus** | SWISE-01 Indledende System Engineering (SW4PRJ4-02) |
| **Kilde** | Frank Vahid & Tony Givargis, *Embedded System Design: A Unified Hardware/Software Introduction*, Wiley — Chapter 6 *Interfacing*, pp. 166–169 |
| **Kilde-fil** | ISE Book-kompendium: `chapters/10_Vahid_Ch6_Interfacing_166-169.tex` |
| **Type** | lærebogskapitel |
| **Sprog** | engelsk (bogtekst ikke oversat; noter på dansk) |
| **Emner dækket** | Buffering (DMA-fragment), physical layer, parallel vs. serial vs. wireless communication, start/stop bit, IR og RF, layering, error detection (bit error, burst error), parity (odd/even), checksum (XOR), retransmission/acknowledgment, serial protocols (I²C, CAN, FireWire, USB — kun indledning) |

Note om transskriptionen: Kilden er en LaTeX-transskription af fotograferede bogsider. Uddraget begynder med et fragment fra afsnit 6.5 (DMA), hvor resten af sætningen ikke er med i fotoet, og slutter midt i en sætning i afsnit 6.9 om I²C. Ingen figurer i dette uddrag. Første side (venstre side af første foto) hører til kapitel 6 s. 137–153 og er placeret sidst i `09-vahid-ch6-interfacing-137-153.md`.

---

## 6.5 Microprocessor Interfacing: Direct Memory Access

Commonly, the data being accumulated in a peripheral should be first stored in memory before being processed by a program running on the microprocessor. Such temporary storage of data that is awaiting processing is called buffering. For example, packet data from an

*(transskription: resten af sætningen er ikke synlig i det fotograferede udsnit. Afsnittene 6.6 Arbitration og 6.7 Multilevel Bus Architectures er ikke med i uddraget.)*

## 6.8 Advanced Communication Principles

In the preceding sections, we discussed basic methods of interfacing. Those interfacing methods could be applied to interconnect components within an IC via on-chip buses, or to interconnect ICs via on-board buses. In the remainder of the chapter, we study more advanced interfacing concepts and look at communication from a more abstract point of view. In particular, we study parallel, serial, and wireless communication. We also describe some advanced concepts, such as layering and error detection, which are part of many communication protocols. Furthermore, we highlight some of the popular parallel, serial, and wireless communication protocols in use today.

Communication can take place over a number of different types of media, such as a single wire, a set of wires, radio waves, or infrared waves. We refer to the medium that is used to carry data from one device to another as the *physical layer*. Depending on the protocol, we may refer to an actor as a *device* or *node*. In either case, a device is simply a processor that uses the physical layer to send or receive data to and from another device.

In this section, we provide a general description of serial communication, parallel communication, and wireless communication. In addition, we describe communication principles such as layering, error detection and correction, data security, and plug and play.

### Parallel Communication

*Parallel communication* takes place when the physical layer is capable of carrying multiple bits of data from one device to another. This means that the data bus is composed of multiple data wires, in addition to control and possibly power wires, running in parallel from one device to another. Each wire carries one of the bits. Parallel communication has the advantage of high data throughput, if the length of the bus is short. The length of a parallel bus must be kept short because long parallel wires will result in high capacitance values, and transmitting a bit on a bus with a higher capacitance value will require more time to charge or discharge.

In addition, small variations in the length of the individual wires of a parallel bus can cause the received bits of the data word to arrive at different times. Such misalignment of data becomes more of a problem as the length of a parallel bus increases. Another problem with parallel buses is the fact that they are more costly to construct and may be bulky, especially when considering the insulation that must be used to prevent the noise from each wire from interfering with the other wires. For example, a 32-wire cable connecting two devices together will cost much more and be larger than a two-wire cable.

In general, parallel communication is used when connecting devices that reside on the same IC, or devices that reside on the same circuit board. Since the length of such buses is short, the capacitance load, data misalignment and cost problems mentioned earlier do not play an important role.

### Serial Communication

*Serial communication* involves a physical layer that carries one bit of data at a time. This means that the data bus is composed of a single data wire, along with control and possibly power wires, running from one device to another. In serial communication, a word of data is transmitted one bit at a time. Serial buses are capable of higher throughputs than parallel buses when used to connect two physically distant devices. The reason for this is that a serial bus will have less average capacitance, enabling it to send more bits per unit of time. In addition, a serial bus cable is cheaper to build because it has fewer wires. The disadvantage of a serial bus is that the interfacing logic and communication protocol will be more complex. On the sending side, a transmitter must decompose data words into bits and on the receiving side, and the receiver must compose bits into words.

Most serial bus protocols eliminate the need for extra control signals, such as read and write signals, by using the same wire that carries data for this purpose. This is performed as follows. When data is to be sent, the sender first transmits a bit called a *start bit*. A start bit merely signals the receiver to wake up and start receiving data. The start bit is then followed by *N* data bits, where *N* is the size of the word, and a *stop bit*. The stop bit signals to the receiver the end of the transmission. Often, both the transmitter and the receiver agree on the transmission speed used to send and receive data. After sending a start bit, the transmitter sends all *N* bits at the predetermined transmission speed. Likewise, on seeing a start bit, a receiver simply starts sampling the data at a predetermined frequency until all *N* bits are assembled. Another common synchronization technique is to use an additional wire for clocking purposes (see the I²C bus protocol). Here, the transmitter and receiver devices use this clock line to determine when to send or sample the data.

### Wireless Communication

Wireless communication eliminates the need for devices to be physically connected in order to communicate. The physical layer used in wireless communication is typically either an infrared (IR) channel or a radio frequency (RF) channel.

*Infrared* uses electromagnetic wave frequencies that are just below the visible light spectrum, thus undetectable by the human eye. These waves can be generated by using an infrared diode and detected by using an infrared transistor. An infrared diode is similar to a red or green diode except that it emits infrared light. An infrared transistor is a transistor that conducts (i.e., allows current to flow from its source to its drain), when exposed to infrared light. A simple transmitter can send 1s by turning on its infrared diode and can send 0s by turning off its infrared diode. Correspondingly, a receiver will detect 1s when current flows through its infrared transistor and 0s otherwise. The advantage of using infrared communication is that it is relatively cheap to build transmitters and receivers. The disadvantage of using infrared is the need for line of sight between the transmitter and receiver, resulting in a very restricted communication range.

*Radio frequency* (RF) uses electromagnetic wave frequencies in the radio spectrum. A transmitter here will need to use analog circuitry and an antenna to transmit data. Likewise, a receiver will need to use an antenna and analog circuitry to receive data. One advantage of using RF is that, generally, a line of sight is not necessary and thus longer distance communication is possible. The range of communication is, of course, dependent on the transmission power used by the transmitter.

### Layering

*Layering* is a hierarchical organization of a communication protocol where lower levels of the protocol provide services to the higher levels. We have already discussed the physical layer. The physical layer provides the basic service of sending and receiving bits or words of data. The next higher-level protocol uses the physical layer to send and receive packets of data, where a packet of data is composed of possibly multiple bytes. The next higher level uses the packet transmission service of its lower level to perhaps send different type of data such as acknowledgments, special requests, and so on. Typically, the lowest level consists of the physical layer and the highest level consists of the application layer. The application layer provides abstract services to the application such as ftp or http.

Layering is a way to break the complexity of a communication protocol into independent pieces, thus making it easier to design and understand, much like a programmer abstracting away complexities of a program by creating objects or libraries of routines. In communication and networking, the concept of layering is very fundamental.

### Error Detection and Correction

*Error detection* is the ability of a receiver to detect errors that may occur during the transmission of a data word or packet. The most common types of errors are bit errors and burst of bit errors. A *bit error* occurs when a single bit in a word or data packet is received as its inverted value. A *burst of bit error* occurs when consecutive bits of a word or data packet are received incorrectly. Given that an error is detected, *error correction* is the ability of a receiver and transmitter to cooperate in order to correct the problem. The ability to detect and correct errors is often part of a bus protocol. We will next discuss parity and checksum error detection algorithms, which are commonly used in bus protocols.

*Parity* is a single bit of information that is sent along with a word of data by the transmitter to give the receiver some additional knowledge about the data word. This additional knowledge is used by the receiver to detect, to some degree, a bit or burst of bit error in receiving a word. Common types of parity are odd or even. Odd parity is a bit that if set indicates to the receiver that the data word bits plus parity bit contain an odd number of 1s. Even parity is a bit that if set indicates to the receiver that the data word bits plus parity bit contain an even number of 1s. Prior to sending a word of data, the transmitter will compute the parity and send that along with the data word to the receiver. On reception of the data word and parity bit, the receiver will compute the parity of the data and make sure that it agrees with the parity bit received from the transmitter. If a parity check fails, it indicates with certainty that there was at least one transmission error. Parity checks will always detect a single bit error. However, burst bit errors may or may not be detected by parity checking — an even number of errors, for example, will not be detected.

As an example of parity-based error checking, consider wanting to transmit the following 7-bit word: 0101010. Assuming even parity, we would actually transmit the 8-bit word: 01010101, where the least-significant bit is the parity bit. Now, suppose during transmission, a bit gets flipped, so that a receiver receives the following 8-bit word: 11010101. The receiver detects that this word has odd parity; knowing that the word was supposed to have even parity, he receiver determines that this word has an error. Instead, suppose the receiver receives: 11110101. This word has even parity, and so the receiver thinks the word is correct, even though it contains two errors.

| Eksempel (even parity) | Ord | Antal 1'ere | Paritet | Resultat |
|---|---|---|---|---|
| Sendt (7-bit data + paritetsbit som LSB) | `01010101` | 4 | even | OK |
| Modtaget, 1 bit flippet | `11010101` | 5 | odd | fejl detekteret |
| Modtaget, 2 bit flippet | `11110101` | 6 | even | fejl **ikke** detekteret |

*Checksum* is a stronger form of error checking that is applied to a packet of data. A packet of data will contain multiple words of data. Using parity checking, we used one extra bit per word to help us detect errors. Using checksum, we use an extra word per packet for the same purpose. For example, we may compute the XOR sum of all the data words in a packet and send this value along with the data packet. Upon receiving the data packet words and the checksum word, the receiver will compute the XOR sum of all the data words it received. If the computed checksum word equals the received checksum word, the data packet is assumed to be correct. Otherwise, it is assumed to be incorrect. Again, not all error combinations can be detected. We can of course use both parity and checksum for stronger error checking.

As an example, suppose a packet consists of four words: 00000000, 0101010, 1010101, and 00000000. The XOR checksum of these four words is 1111111. A transmitter can thus send that checksum word at the end of the packet. Now, suppose the receiver receives 1000001, 0101010, 1010101, and 0000000. Note that two bits have switched in the first word, and that parity-based error checking would not detect this error. The receiver computes the checksum of this packet and obtains 0111110. This differs from the received checksum of 1111111, and thus the receiver determines that an error has occurred.

| | Ord 1 | Ord 2 | Ord 3 | Ord 4 | XOR-checksum |
|---|---|---|---|---|---|
| Sendt | `00000000` | `0101010` | `1010101` | `00000000` | `1111111` |
| Modtaget | `1000001` | `0101010` | `1010101` | `0000000` | `0111110` (≠ `1111111` → fejl) |

*(Ordbredderne 7/8 bit er gengivet som i kilden.)*

Note that errors can also occur in the parity bit or the checksum word itself. When using parity or checksum error detection, error correction is typically done by a retransmission and acknowledgment protocol. Here, the transmitter sends a data packet and expects to receive an acknowledgment from the receiver indicating that the data packet was received correctly. If an acknowledgment is not received, the transmitter retransmits the data packet and waits for a second acknowledgment.

## 6.9 Serial Protocols

In this section, we describe four popular serial protocols, namely the I²C protocol, the CAN protocol, the FireWire protocol, and the USB protocol.

### I²C

Philips Semiconductors developed the inter-I²C, or I²C, bus nearly 20 years ago. I²C is a two-wire serial bus protocol. This protocol stabilizes peripheral ICs in electronic systems to

*(transskription: uddraget slutter her, midt i sætningen. Resten af I²C-afsnittet samt CAN, FireWire og USB er ikke med.)*
