---
title: SignalR
source: SignalR.pdf
course_week: 7
topic: Logging + SignalR
---

# SignalR

## What is SignalR?

- ASP.NET SignalR is a library for ASP.NET developers that simplifies the process of adding real-time web functionality to applications.
- SignalR allows bi-directional communication between server and client — the server can push to the client instead of only responding to requests.

Without real-time, the client repeatedly asks "Got data?" and mostly gets "No". With real-time, client and server negotiate a persistent connection and exchange data as it happens.

## Examples of real-time applications

- X (Twitter), Facebook, Mail — live searches/updates
- Stock streamers
- Auctions
- Interactive games
- Live scores
- Collaborative apps (Google Docs, Office web apps)
- Live user analytics (live graphs)

## How to do real-time on the web?

### Periodic polling

Poll from time to time using `fetch`. There is a delay in communication due to the polling interval, and it wastes bandwidth and latency.

```javascript
function updateInterface(){
    fetch(url, {
        // go to the server and ask for data
    });
    if (got_some_data)
        updateUI(data);
    setTimeout(10000, updateInterface);
}
setTimeout(10000, updateInterface);
```

**Pros:** works across all browsers.
**Cons:** more connections → more overhead; more overhead → more bandwidth; server strain.

### Long polling

Poll, but the server doesn't respond until there's data. Poll again after data is received or the connection times out. Consumes threads and connection resources on the server.

```javascript
function startListening(){
    // note: this call can take as long as 120 seconds
    // to return, depending on browser timeouts.
    fetch({
        url: "/blah",
        success: function (data){
            startListening();
        },
        failure: function (err){
            startListening();
        },
    });
}
```

**Pros:** works across all browsers; less server overhead and resource consumption than periodic polling.
**Cons:** we still have to make connections on a regular basis.

### WebSockets

- Extension to HTTP
- Provides raw sockets over HTTP
- Full-duplex
- Traverses proxies
- They are raw sockets!

```javascript
var connection = new WebSocket('ws://127.0.0.1');

connection.onmessage = function (message) {
    // handle incoming message
};

// send the message as ordinary text
connection.send(msg);
```

**Pros:** real bi-directional communication between the server and the client.
**Cons:** limited support on older browsers.

## Introducing SignalR

- Abstraction over transports. SignalR automatically chooses the best transport method within the capabilities of the server and client:
  1. WebSockets
  2. Server-Sent Events
  3. Long Polling
- Connection management
- Broadcast, or target a specific client or group of clients

## What does SignalR do?

- Client-to-server persistent connection over HTTP/HTTPS
- Easily build multi-user, real-time web applications
- Auto-negotiates transport
- Allows server-to-client push and RPC
- Built async to scale to 1000s of connections
- Open source on GitHub

## Hubs

- SignalR uses hubs to communicate between clients and servers.
- A hub is a high-level pipeline that allows a client and server to call methods on each other.
- SignalR handles the dispatching across machine boundaries automatically, allowing clients to call methods on the server and vice versa.
- You can pass strongly-typed parameters to methods, which enables model binding.
- SignalR provides two built-in hub protocols:
  - a text protocol based on JSON
  - a binary protocol based on MessagePack (generally creates smaller messages than JSON)
- Hubs call client-side code by sending messages that contain the name and parameters of the client-side method. Objects sent as method parameters are deserialized using the configured protocol.
- The client tries to match the name to a method in the client-side code; on a match, it calls the method with the deserialized parameter data.

## SignalR Hubs — server side

A hub is a .NET class that inherits from the SignalR `Hub` base class. The `Clients` property exposes properties for targeting specific clients, and virtual methods respond to connected/disconnected events:

```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
```

## Targeting clients

- `Clients.All.doWork()` — sends to all connected clients
- `Clients.Caller.doWork()` — sends to the calling client only
- `Clients.Others.doWork()` — sends to all other connected clients
- `Clients.Users("Brady").doWork()` — sends to specific users

## Groups

A group is a collection of connections associated with a name. Connections are added to or removed from groups via the `AddToGroupAsync` and `RemoveFromGroupAsync` methods.

| Method | Description |
|---|---|
| `Group` | Calls a method on all connections in the specified group |
| `GroupExcept` | Calls a method on all connections in the specified group, except the specified connections |
| `Groups` | Calls a method on multiple groups of connections |
| `OthersInGroup` | Calls a method on a group of connections, excluding the client that invoked the hub method |

```csharp
public Task SendMessageToGroup(string user, string message)
{
    return Clients.Group("Cat lovers").SendAsync("ReceiveMessage", user, message);
}
```

## SignalR clients

- The SignalR clients ship alongside the server components and are versioned to match. The client version must match the server version.
- Supported platforms:
  - JavaScript client: Node, Safari (incl. iOS), Chrome (incl. Android), Edge, Firefox
  - .NET client: any platform supported by ASP.NET Core (Xamarin/MAUI developers can use SignalR for Android and iOS apps)
  - Java client: Java 8 and later
  - Experimental/unofficial clients: C++, Swift

### JavaScript & .NET client parity

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
```

```csharp
HubConnection connection = new HubConnectionBuilder()
    .WithUrl(new Uri("http://127.0.0.1:5000/chatHub"))
    .Build();
```

## Using SignalR in ASP.NET web applications

Steps: install the SignalR and/or SignalR JS packages via NuGet → wire up in `Program.cs` → include SignalR.js in the HTML → create a SignalR Hub through which clients can communicate.

- SignalR is released with dotnet. The SignalR server library is included in the `Microsoft.AspNetCore.App` metapackage — no need to install.
- The JavaScript client library is not automatically included in the project.

## Create a Hub

```csharp
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRDemo.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
```

## Add SignalR in Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.Run();
```

## Add the SignalR client library

- Use Library Manager (LibMan) to get the client library from unpkg — or use npm.
- In Solution Explorer, right-click the project and select Add > Client-Side Library.
- For Provider select **unpkg**; for Library enter `@microsoft/signalr@latest`.
- Select "Choose specific files", expand the `dist/browser` folder, and select `signalr.js` and `signalr.min.js`.
- Set Target Location to `wwwroot/lib/microsoft/signalr/` and select Install.

## Connect the client to the Hub using JavaScript

In the HTML file:

```html
<script src="~/js/signalr/dist/browser/signalr.js"></script>
<script src="~/js/chat.js"></script>
```

In `chat.js`:

```javascript
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
```

## Calling a SignalR Hub method from a controller

Often you combine a WebAPI server and a SignalR server. Inject `IHubContext<T>`:

```csharp
[ApiController]
public class CountController : ControllerBase
{
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly counter _counter;

    public CountController(IHubContext<ChatHub> chatHubContext, counter counter)
    {
        _chatHubContext = chatHubContext;
        _counter = counter;
    }

    // GET: api/Count/inc
    [HttpGet("inc")]
    public async Task<IActionResult> Get()
    {
        await _chatHubContext.Clients.All.SendAsync("countUpdate", _counter.Inc());
        return Ok();
    }
}
```

## Strongly typed hubs

An alternative to using `SendAsync`: extract the client methods into an interface:

```csharp
public interface IChat
{
    Task ReceiveMessage(string user, string message);
}

public class StronglyTypedChatHub : Hub<IChat>
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message);
    }
}
```

## Strongly typed hubs from a controller

```csharp
[ApiController]
public class CountController : ControllerBase
{
    private readonly IHubContext<ChatHub, IChat> _chatHubContext;
    private readonly counter _counter;

    public CountController(IHubContext<ChatHub, IChat> chatHubContext, counter counter)
    {
        _chatHubContext = chatHubContext;
        _counter = counter;
    }

    // GET: api/Count/inc
    [HttpGet("inc")]
    public async Task<IActionResult> Get()
    {
        await _chatHubContext.Clients.All.CountUpdate(_counter.Inc());
        return Ok();
    }
}
```

## References & links

- Introduction to ASP.NET Core SignalR: https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction
- Tutorial: Get started with ASP.NET Core SignalR: https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr
- Send messages from outside a hub: https://docs.microsoft.com/en-us/aspnet/core/signalr/hubcontext
- YouTube video: https://www.youtube.com/watch?v=YwezzKWrFuo
