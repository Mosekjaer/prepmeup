# Network Architecture

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Peckol, *Embedded Systems Design, A Contemporary Design Tool*, chapter 16.7, pp. 627–637.*

## Implementing the Remote Device Model—A First Step

We now begin the message-exchange portion of the remote device model. Communication between electronic devices depends on a protocol for transmitting and receiving messages. This is the transport level we start with.

Distributed embedded applications may use a proprietary network or one built from a standard. In either case the transport topology is typically serial and the information flow is full duplex. A transport architecture is usually a hierarchy of virtual networks. Above the physical portion of the transport mechanism sits a stack of software layers, and each layer on one machine interacts with the corresponding layer on the other machine.

*Figur: An N Layer Network Architecture* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

At each level, a potentially different language is spoken. The language is the protocol for that level. The service at a level is provided to the level above, so the relationship between layers is that of a service provider and a service consumer. Typically, the lower levels are implemented in hardware and the upper levels in software.

In distributed systems the layers are often described through the OSI and TCP/IP protocol stacks. OSI was proposed and standardized by the International Organization for Standardization. TCP/IP is the practical protocol family that became the backbone of networked systems. Many embedded applications use one of these stacks, or a commercial derivative, rather than a proprietary design.

### The OSI and TCP/IP Protocol Stacks

The OSI model specifies seven layers. The TCP/IP model is usually represented as a five-layer hierarchy. The physical and data-link functions of OSI are combined into the host-to-network layer in TCP/IP. The transport layer appears in both models. The OSI session and presentation layers do not have direct counterparts in TCP/IP, while the application layer in TCP/IP absorbs the upper OSI responsibilities.

*Figur: Layers in the Network Architectures for the OSI and TCP/IP Models* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The following diagrams compare the two architectures. Notice that the network layer and everything below it are hardware-oriented, while the upper layers are software-oriented.

*Figur: The Network Architectures for the OSI and TCP/IP Models* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

##### OSI—Transport Layer

The transport layer accepts data from the session layer and subdivides it into packets that can be exchanged with a corresponding transport layer on another machine. The intent is to isolate the application from the underlying mechanics of the network.

##### TCP/IP—Transport Layer

The TCP/IP transport layer serves the same purpose as the OSI transport layer. The two transport protocols named here are TCP and UDP.

##### OSI—Session Layer

The session layer permits users on different machines to communicate. It supports dialog control, synchronization, and token management, and it can help reassemble a message if a transfer is interrupted.

##### TCP/IP—Host to Network

At this level the host system simply has to connect to the network and transmit or receive IP packets. The OSI network layer corresponds to the TCP/IP Internet layer.

##### OSI—Network Layer

The network layer handles routing of a transmission from source to destination. It must accommodate addressing, message size, and protocol differences between networks.

##### TCP/IP—Internet Layer

The Internet layer is the core element of TCP/IP. It defines the packet format and the Internet Protocol, IP. Its job is to move data between source and destination across the network.

##### OSI—Application Layer

The application layer is the software side of the OSI model. A number of incompatibilities that matter in embedded systems, such as file systems and remote procedure execution, show up here.

##### TCP/IP—Application Layer

The application layer in TCP/IP duplicates much of the responsibility already identified in OSI. In practice one usually selects a commercially available protocol stack rather than inventing a proprietary one.

### The Models

Most distributed embedded systems implement a message-based exchange using one of three communication patterns: client-server, peer-to-peer, and group multicast.

#### The Client-Server Model

The client-server model closely parallels the producer-consumer model studied earlier. A server, or a set of servers, exists to provide a service required by a client. The pattern uses request-reply messages.

*Figur: The Client-Server Model for Message-Based Exchange* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The client transmits a request to the server process and blocks. The server executes the request and returns the reply. In this model the server is aware of the message as soon as it arrives, while the sending process waits until the reply is received.

#### The Peer-to-Peer Model

The peer-to-peer model follows naturally from client-server. There is no predesignated server. Any member of the network may request a service from or provide one to any other member. Several clients may interact with a single server at the same time, and the peer-to-peer model permits several nodes to provide the requisite services.

*Figur: The Peer-to-Peer Model for Message-Based Exchange* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Synchronization is achieved through the message exchange itself, as in the client-server model, but the roles are distributed across peers.

#### The Group Multicast Model

The group multicast model comprises a single sender and multiple receivers. It is useful when information must be sent to a group of interested processes. The same task can also be used to discover services or announce presence. The approach can be fault tolerant because the same message can reach several servers.

*Figur: The Group Multicast Model for Message-Based Exchange* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

## Implementing the Remote Device Model—A Second Step

We now examine the client-server and group multicast communication schemes in more detail. Peer-to-peer follows naturally from client-server. We begin with the client-server model by looking at the fundamental components of such systems, including the data structures and the messages.

### The Messages

When designing distributed embedded systems, one quickly discovers that remote operations make up a substantial proportion of the interactions between processes. Such operations are initiated by one process sending a request message to another process. The receiving process responds with an acknowledgment or a reply indicating that the operation has been or will be carried out.

At the most abstract level, a message is a collection of bits, and the exchange is the movement of those bits from one place to another. To make message-based communication tractable, rules are applied to the exchange and to the interpretation of the bits.

*Figur: An Abstraction of the Basic Message* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### The Message Structure

One interpretation views some of the bits as data or payload and other bits as header information. The payload is the information being transported. Moving the payload from one place to another is the ultimate objective of sending the message. The header information is added by the communication driver to provide addressing, control, and synchronization information.

Not all messages are the same size. They may be simple and occupy only a few words of memory, or they may be complex and comprise a large number of blocks of data. The design of the exchange process is cleaner and more robust if the bits are organized so that fixed-size groupings are always transferred. Such groupings are often referred to as frames, datagrams, or packets.

*Figur: A Header Added to the Basic Message* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

*Figur: The Basic Message Decomposed into Packets* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Message Control and Synchronization

There are actually two kinds of control and synchronization in a message-based exchange: the header information and the data-transfer scheme.

#### The Header

The header information is overhead that is necessary for getting the data from one place to another. It is added at each level within the protocol stack by the communication driver, based on the requirements of the exchange protocol at that level. Potential header elements include the destination address or a message identifier. At minimum, the header identifies the destination for the message. In a networked context it may also provide routing information and identify both sender and receiver.

The header may indicate the size of the message in several ways. There may be unique start and end identifiers, or a start identifier and a length field. The header generally includes information about the message type or structure. It may be useful to distinguish between data-type messages and command-type messages, for example.

#### The Transfer Scheme

The physical information transfer may follow a proprietary protocol or one of the standards, or a derivative of one of those standards. Two kinds of service are commonly identified: connection-oriented and connectionless.

A connection-oriented service establishes a specific connection between the source and destination before the exchange of any data. The exchange then follows, and the connection is terminated afterward. Messages enter one end and are extracted from the other end, and ordering is preserved. The exchange is designated as reliable because the reply is effectively an acknowledgment. This kind of exchange is often described as circuit switching, and each packet uses the same physical path.

A connectionless service does not establish a specific connection before the exchange begins. Each packet carries full address information, and packets may arrive through different routes and need not arrive in the same order in which they were sent. Such a scheme is referred to as packet switching. A connectionless exchange is best effort.

Messages may be sent as a datagram service, in which the receiver does not acknowledge; as an acknowledged datagram service, in which the receiver acknowledges; or as a request-reply service, in which the sender transmits a datagram containing a request and the receiver returns a datagram containing the answer. The last scheme is commonly used in the client-server model.

*Figur: The Basic Message Decomposed into Packets* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

*Figur: Address Information Added to Each Packet* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

*Figur: A High-Level View of a Client-Server Model* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

## Working with Remote Tasks

The next step in examining the inner workings of message-based remote operations begins with the client-server model. In this model, processes, whether local or remote, either provide a service or request one. The former are servers and the latter are clients. Any process may play either or both roles.

### Preliminary Thoughts on Working with Remote Tasks

Before proceeding with the development of remote functionality, it is important to keep several major differences between local and remote tasks in mind. The initial view of distributed client-server interaction is shown in Figure 1.12. The client and the server each assume direct communication with the other, but the communication link actually passes through the local kernel to the network and then to the remote node, where the message is interpreted and passed to the local process.

One important issue is the possibility that the complete message may not be received, or that the remote request may be interrupted and rejected. Consequently, the remote procedure may never be executed, may be only partially executed, or may be completely executed. These alternatives can create serious safety problems. Even when a requested remote procedure is completed, a failure to confirm that completion may cause additional requests to be made.

#### Repeated Task Execution

The repetitive nature of request and reply traffic makes it necessary to consider what happens if a task is restarted, duplicated, or only partially completed. The design has to anticipate message loss, retries, and uncertain completion.

If all components in an external system speak the same language, there may be no need for conversion. More often, however, the outside world is made up of a heterogeneous collection of devices, communication methods, and data representations. The interface must therefore cope with differences in size, endianness, and the structure of exchanged data.
