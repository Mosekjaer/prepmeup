# Interfacing

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Frank Vahid and Tony Givargis, *Embedded System Design: A Unified Hardware/Software Introduction*, Wiley, Chapter 6, Interfacing, pp. 137–153.*

|      |                                                  |
|-----:|:-------------------------------------------------|
|  6.1 | Introduction                                     |
|  6.2 | Communication Basics                             |
|  6.3 | Microprocessor Interfacing: I/O Addressing       |
|  6.4 | Microprocessor Interfacing: Interrupts           |
|  6.5 | Microprocessor Interfacing: Direct Memory Access |
|  6.6 | Arbitration                                      |
|  6.7 | Multilevel Bus Architectures                     |
|  6.8 | Advanced Communication Principles                |
|  6.9 | Serial Protocols                                 |
| 6.10 | Parallel Protocols                               |
| 6.11 | Wireless Protocols                               |
| 6.12 | Summary                                          |
| 6.13 | References and Further Reading                   |
| 6.14 | Exercises                                        |

## Introduction

As stated in Chapter 5, we use processors to implement processing, memory to implement storage, and buses to implement communication. The earlier chapters described processors and memory. This chapter describes implementing communication with buses, known as interfacing. Communication is the transfer of data among processors and memories. For example, a general-purpose processor reading or writing a memory is a common form of communication. A general-purpose processor reading or writing a peripheral’s register is another common form.

We begin by defining some basic communication concepts. We then introduce several issues relating to the common task of interfacing to a general-purpose processor: addressing, interrupts, and direct memory access. We also describe several schemes for arbitrating among multiple processors attempting to access a single bus or memory simultaneously. We show that many systems may include several hierarchically organized buses. We then discuss some more advanced communication principles and survey several common serial, parallel, and wireless communication protocols.

## Communication Basics

### Basic Terminology

We begin by introducing a very basic communication example between a processor and a memory, shown in Figure 1.1. Figure 1.1(a) shows the bus structure, or the wires connecting the processor and the memory. A line *rd’/wr* indicates whether the processor is reading or writing. An *enable* line is used by the processor to carry out the read or write. Twelve address lines *addr* indicate the memory address that the processor wishes to read or write. Eight data lines *data* are set by the processor when writing or set by the memory when the processor is reading. Figure 1.1(b) describes the read protocol over these wires: the processor sets *rd’/wr* to 0, places a valid address on *addr*, and strobes *enable*, after which the memory will place valid data on the *data* lines. Figure 1.1(c) shows a write protocol: the processor sets *rd’/wr* to 1, places a valid address on *addr*, places data on *data*, and strobes *enable*, causing the memory to store the data.

*Figur: A simple bus example: (a) bus structure, (b) read protocol, (c) write protocol.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Wires may be unidirectional, meaning they transmit in only one direction, as did *rd’/wr*, *enable*, and *addr*; or they may be bidirectional, meaning they transmit in two directions, though only in one direction at a time, as did *data*. A set of wires with the same function is typically drawn as a thick line and/or as a line with a small angled line drawn through it, as was the case with *addr* and *data*.

The term *bus* can refer to a set of wires with a single function within a communication. For example, we can refer to the “address bus” and the “data bus” in the above example. The term bus can also refer to the entire collection of wires used for the communication along with the communication protocol over those wires. Both uses are common and are often used together.

The bus connects to ports of a processor or memory. A *port* is the actual conducting device, like metal, on the periphery of a processor, through which a signal is input to or output from the processor. A port may refer to a single wire, or to a set of wires with a single function, such as an address port consisting of twelve wires. A related term is *pin*. When a processor is packaged as its own IC, there are actual pins extending from the package. Today, however, a processor commonly coexists on a single IC with other processors and memories. Such a processor may not have actual pins on its periphery, but rather pads of metal in the IC. Even so, the term pin is still commonly used for connections to a port.

The distinction between a bus and a port is similar to the distinction between a street and a driveway. The bus is like the street, which connects various driveways. A processor’s port is like a house’s driveway, which provides access between the house and the street.

The most common method for describing a hardware protocol is a timing diagram. Time proceeds to the right along the horizontal axis. Control lines are shown as either high or low. A single control line may be asserted or deasserted depending on whether active high or active low logic is used. Data and address values are drawn only as valid windows, because the exact value is often irrelevant when describing the protocol.

### The ISA Bus Protocol – Memory Access

The Industry Standard Architecture bus protocol shows how a processor can access a memory or a peripheral using a conventional set of bus signals. Figure 1.2 shows a read operation. The processor places the address and read command on the bus, waits until the addressed device returns data, and then completes the bus cycle. The same bus lines are shared by many devices, so the protocol defines which device is responsible for driving each signal during each interval.

*Figur: The ISA bus protocol: (a) timing diagram for a read operation, (b) interface schematic.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave] [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

During the bus cycle, address and data may share the same physical pins. A latch captures the address when *ALE* is asserted; later in the cycle the same pins carry data. The *CHRDY* signal can stretch the cycle when a memory or peripheral is not ready.

### Time-Multiplexed Data Transfer

A bus may reduce the number of wires by using time multiplexing. In one form, a narrow bus serializes a wider data value over several cycles. In another form, the same set of physical wires carries address information during one part of a transaction and data during another part.

*Figur: Time-multiplexed data transfer: (a) data serializing, (b) address/data muxing.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Basic Protocol Concepts

The processor-memory protocol above is a simple one. Hardware protocols can be much more complex. However, several basic protocol concepts recur: actors, data direction, addresses, time multiplexing, control methods, and clock cycles.

An *actor* is a processor or memory involved in a data transfer. A protocol typically involves two actors: a master and a servant. A master initiates the data transfer. A servant responds to the initiation request. In the example of Figure 1.1, the processor is the master and the memory is the servant. The servant could also be another processor. Masters are usually general-purpose processors, and servants are usually peripherals and memories.

Data direction denotes the direction that the transferred data moves between actors. Some transfers are independent of direction, but most specify whether the master writes data to, or reads data from, the servant. Addresses select which internal register, memory location, or peripheral location is the target of the transfer.

Control methods define how transfer timing is controlled. Figure 1.4 shows two common methods. In a *strobe* method, a single control line indicates when the data is valid. In a *handshake* method, the master and servant exchange request and acknowledge signals so that transfer timing can adapt to a slow servant or to variable latency.

*Figur: Two protocol control methods: (a) strobe, (b) handshake. The main difference is undefined, fixed-time access versus acknowledged access.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Another protocol concept is time multiplexing. To multiplex means to share a single set of wires for multiple pieces of data. In time multiplexing, pieces of data are sent over the shared wires, one at a time. This reduces pins and wires at the cost of a longer transfer and often more complex control.

*Figur: A strobe/handshake compromise: (a) fast-response, (b) slow-response.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

## Microprocessor Interfacing: I/O Addressing

### Port-Based I/O

A microprocessor may have tens or hundreds of pins, many of which are control pins for selecting the microprocessor, resetting the microprocessor, clock input and output, and other functions. Other pins communicate data to and from the microprocessor. Two common methods for using pins to support I/O are port-based I/O and bus-based I/O.

In *port-based I/O*, also known as *parallel I/O*, a peripheral is connected to a dedicated port of the microprocessor. For example, a processor can have ports A, B, and C, each of which is a set of pins mapped to registers. The program reads and writes those registers in order to read and write the external pins.

*Figur: Parallel I/O: (a) adding parallel I/O to a bus-based I/O processor, (b) standard parallel I/O.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

In *bus-based I/O*, the microprocessor has address, data, and control ports corresponding to bus lines, and it uses the bus to access memory as well as peripherals. The microprocessor has the bus protocol built into its hardware. Specifically, the software does not implement the bus protocol but merely executes a single instruction that in turn causes the bus access to occur. We normally consider the access to the peripheral as a peer of the access to memory, since both are carried out by the microprocessor.

### Memory-Mapped I/O and Standard I/O

In bus-based I/O, there are two methods for a microprocessor to communicate with peripherals, known as *memory-mapped I/O* and *standard I/O*. In memory-mapped I/O, peripherals occupy specific addresses in the existing address space. For example, consider a bus with a 16-bit address. The lower 32K addresses may correspond to memory addresses, while the upper 32K may correspond to I/O addresses.

*Figur: A basic memory protocol: (a) timing diagram for read operation, (b) interface schematic.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Standard I/O includes an additional pin, *M/IO*, which indicates whether the access is to memory or to a peripheral. Thus a microprocessor can preserve its memory address space while still supporting I/O addresses. In some processors, special instructions such as `MOV` or `IN`/`OUT` are used for I/O transfers.

## Microprocessor Interfacing: Interrupts

Another microprocessor I/O issue is that of interrupt-driven I/O. To introduce this issue, suppose a program running on a microprocessor must, among other tasks, read and process data from a peripheral. If the program continuously checks the peripheral, the technique is called *polling*. Polling wastes many cycles when new data arrives infrequently. The processor may instead continue with other work and be notified only when the peripheral needs service.

An *interrupt* is a signal that indicates a peripheral has new data or requires service. The processor suspends its current execution, saves its state, and jumps to an interrupt service routine (ISR). The ISR performs the required service, restores the state, and returns to the interrupted program.

*Figur: Interrupt-driven I/O using fixed ISR locations: summary of flow of actions.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

One method for associating an interrupt with an ISR is to use fixed locations. After an interrupt, the microprocessor jumps to a location reserved for that interrupt source. The ISR at that fixed location performs the service and then returns. This approach is simple, but it requires the processor to assign known addresses for interrupt routines.

Whenever a peripheral has new data, such processing is called servicing. The peripheral can indicate new data at unpredictable intervals. A straightforward approach is checking the peripheral repeatedly in a register bit; this repeated checking is called polling. Polling may be acceptable in small systems, but often wastes cycles in systems with several peripherals or infrequent events.

Interrupts solve the problem with polling by allowing the peripheral to notify the processor. Most microprocessors have a pin, often called *Int*. If a future called interrupt exists at a particular address, the controller checks *Int*. If *Int* is asserted, a subroutine that services the interrupt is called an interrupt service routine, or ISR. Such I/O is called interrupt-driven I/O.

One method is to use a fixed ISR location for each interrupt source. Another method is vectored interrupts, in which the interrupting peripheral supplies an address or vector that identifies the ISR.

*Figur: Interrupt-driven I/O using vector-driven locations: summary of flow of actions.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Interrupt processing, like any peripheral transaction, must also decide what happens while an ISR is executing. In many systems interrupts are nonmaskable, meaning they cannot be disabled, or maskable, meaning they can be temporarily disabled. Nested interrupts can allow a higher priority interrupt to interrupt a lower priority ISR, but this requires careful handling of saved state and priorities.

## Microprocessor Interfacing: Direct Memory Access

Direct memory access (DMA) allows a peripheral or DMA controller to move a block of data directly between a peripheral and memory without forcing the processor to execute an instruction for every byte or word. DMA is useful for high-throughput devices such as disks, displays, and communication interfaces.

In a typical DMA transfer, the processor initializes a DMA controller with the source address, destination address, transfer size, and direction. The DMA controller requests the bus, becomes the bus master after arbitration, performs the transfers, and then interrupts the processor when the transfer is complete.

*Figur: Interrupt-driven I/O using vectored interrupt: summary of flow of actions.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

programmer may include an instruction that disables interrupts at the beginning of the routine, and another instruction reenabling interrupts at the end of the routine. *Nonmaskable interrupt*s cannot be masked by the programmer. It requires a pin distinct from maskable interrupts. It is typically used for very drastic situations, such as power failure. In this case, if power is failing, a nonmaskable interrupt can cause a jump to a subroutine that stores critical data in nonvolatile memory, before power is completely gone.

In some microprocessors, the jump to an ISR is handled just like the jump to any other subroutine, meaning that the state of the microprocessor is stored on a stack, including contents of the program counter, datapath status register, and all other registers. The state is then restored upon completion of the ISR. In other microprocessors, only a few registers are stored, like just the program counter and status registers. The assembly programmer must be aware of what registers have been stored, so as not to overwrite nonstored register data with the ISR. These microprocessors need two types of assembly instructions for subroutine return.

A regular return instruction returns from a regular subroutine, which was called using a subroutine call instruction. A return from interrupt instruction returns from an ISR, which was jumped to not by a call instruction but by the hardware itself, and which restores only those registers that were stored at the beginning of the interrupt. The C programmer is freed from having to worry about such considerations, as the C compiler handles them.

The reason we used the term *external interrupt* is to distinguish this type of interrupt from internal interrupts, also called *traps*. An internal interrupt results from an exceptional condition, such as divide-by-0, or execution of an invalid opcode. Internal interrupts, like external ones, result in a jump to an ISR. A third type of interrupt, called *software interrupts*, can be initiated by executing a special assembly instruction.
