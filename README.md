# gRPC_Coding_Test

# Description

This project is a .NET client-server application for addition, subtraction, multiplication, and division. The WPF client accepts two numbers and an arithmetic operation, then sends the request to the server using gRPC. The server calculates the result and returns either the result or an appropriate gRPC error status.

Calculation logic is separated from gRPC transport. The client uses an MVC-oriented structure:

```text
WPF View
  -> CalculatorController
  -> GrpcCalculatorClient
  -> ArithmeticService
  -> Calculator
  <- Calculation result / validation error
  <- gRPC response
  <- CalculationResult
  <- Result or error message
```

# Technologies

- .NET 8 and C#
- WPF for the desktop client
- ASP.NET Core gRPC for the server
- Protocol Buffers for the shared contract
- `Grpc.Net.Client` for gRPC calls in the client
- xUnit and ASP.NET Core TestHost for unit and integration tests

# Quickstart

Clone the repository and switch to the solution directory:

```powershell
git clone https://github.com/BalderNordmann/gRPC_Coding_Test.git
cd gRPC_Coding_Test/gRPC_Coding_Test
```

# Installation

Restore dependencies and build the solution:

```powershell
dotnet restore
dotnet build
```

Run the tests:

```powershell
dotnet test
```

## Start the application

Start the server first:

```powershell
dotnet run --project gRPC_Server --launch-profile https
```

Then start the client in a second terminal:

```powershell
dotnet run --project gRPC_Client
```

By default, the server runs at `https://localhost:7042`.

## Requirements

- Windows 10 or Windows 11 for the WPF client
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git to clone the repository
- Optional: Visual Studio 2022 with the **.NET Desktop Development** and **ASP.NET and web development** workloads

## Recommended settings

For the local HTTPS connection, install and trust the development certificate:

```powershell
dotnet dev-certs https --trust
```

The server address is configured as `https://localhost:7042` in both `gRPC_Server/Properties/launchSettings.json` and `gRPC_Client/Services/GrpcCalculatorClient.cs`. If the port is changed, update both locations.
