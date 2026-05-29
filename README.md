# MatrixProject

MatrixProject is a small .NET client/server socket sample with two console applications:

- `./ServerProject` hosts a TCP server.
- `./ClientProject` connects to the server and exchanges console messages.

## What the project does

- Starts a TCP server on a configurable port.
- Accepts client connections in dedicated threads.
- Sends a welcome message to each client.
- Lets the client send text messages to the server.
- Logs each major step with timestamps, level, and step name.

## Project structure

- `ServerProject/Program.cs` starts the server.
- `ServerProject/Server.cs` configures the listening socket and accepts clients.
- `ServerProject/ClientCommunication.cs` handles each connected client session.
- `ClientProject/Program.cs` starts the client with optional address and port arguments.
- `ClientProject/CommunicationHandler.cs` manages connection, sending, receiving, and shutdown.

## Requirements

- .NET 8 SDK

## Run the server

```bash
dotnet run --project ./ServerProject/ServerProject.csproj -- 2510
```

The port argument is optional. If omitted, the server uses `2510`.

## Run the client

```bash
dotnet run --project ./ClientProject/ClientProject.csproj -- 127.0.0.1 2510
```

Arguments are optional:

- argument 1: server address, default `127.0.0.1`
- argument 2: server port, default `2510`

Type messages in the client console and press Enter to send them. Type `exit` to close the session.

## Logging

Both applications now log important steps such as:

- startup
- connect / accept
- send / receive
- shutdown
- client session lifecycle

Each log line includes:

- UTC timestamp
- log level
- step name
- message

## Missing parts completed

- Added the missing client call that actually starts communication.
- Replaced the hard-coded remote IPv6 endpoint with configurable host/port arguments.
- Fixed the server so each client handler runs on its own thread.
- Completed the message flow so the client can send input and the server can respond.
- Added graceful shutdown behavior and basic error handling.
- Moved the projects to a supported .NET target.

## Proposed enhancements

- Add automated tests around message handling and connection lifecycle.
- Add a shared library for reusable networking and logging components.
- Introduce cancellation tokens and coordinated shutdown for the server.
- Add configuration files or environment-variable support for host, port, and backlog.
- Replace console logging with `Microsoft.Extensions.Logging` and configurable sinks.
- Add message framing or a simple protocol to support larger payloads reliably.
- Add connection limits, retries, and health checks for stronger production behavior.
