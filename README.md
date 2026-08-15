# gRPC-Rechner – Client/Server

Eine einfache .NET-Client-Server-Anwendung für Addition, Subtraktion, Multiplikation und Division. Der WPF-Client überträgt die Anfrage per gRPC an den ASP.NET-Core-Server, der die Berechnung ausführt und das Ergebnis oder einen gRPC-Fehler zurückgibt.

## Architektur und Projektstruktur

```text
gRPC_Coding_Test/
├── Protos/calculator.proto                 Gemeinsamer gRPC-Vertrag
├── gRPC_Contracts/                         Generierte gRPC-Vertragstypen
├── gRPC_Client/                            WPF-Client und Oberfläche
├── gRPC_Server/                            ASP.NET-Core-gRPC-Server
│   ├── Logic/Calculator.cs                 Reine Berechnungslogik
│   └── Services/ArithmeticService.cs       gRPC-Transport und Statuszuordnung
└── Tests/                                  Unit- und gRPC-Integrationstests
```

Die Proto-Datei liegt ausschließlich im obersten Ordner `gRPC_Coding_Test/Protos`. Nur `gRPC_Contracts` bindet sie ein und generiert daraus die Vertragstypen; Client, Server und Tests verwenden dieses gemeinsame Projekt. Es gibt keine kopierten Proto-Dateien oder eingecheckten generierten Klassen.

`CalculationOperation` ist ein Proto-Enum. Zulaessig sind nur `Addition`, `Subtraction`, `Multiplication` und `Division`; ein nicht gesetzter oder unbekannter Enumwert wird vom Server als `InvalidArgument` abgelehnt.

Der WPF-Client folgt einer MVC-Struktur:

```text
gRPC_Client/
├── Controllers/CalculatorController.cs     Validierung und Ablaufsteuerung
├── Models/                                 Eingabe- und Ergebnisdaten
├── Services/GrpcCalculatorClient.cs        gRPC-Transport
└── Views/MainWindow.xaml                   WPF-Oberflaeche
```

## Voraussetzungen

- Windows 10/11 für den WPF-Client
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional: Visual Studio 2022 mit Workload **.NET-Desktopentwicklung** und **ASP.NET- und Webentwicklung**

## Installation, Wiederherstellen und Build

Im Verzeichnis `gRPC_Coding_Test/gRPC_Coding_Test` ausführen:

```powershell
dotnet restore
dotnet build
```

## Server starten

```powershell
dotnet run --project gRPC_Server
```

Das HTTPS-Profil lauscht standardmäßig auf `https://localhost:7042`. Falls das lokale Entwicklungszertifikat fehlt, einmalig ausführen:

```powershell
dotnet dev-certs https --trust
```

## Client starten

Den Server gestartet lassen und in einem zweiten Terminal ausführen:

```powershell
dotnet run --project gRPC_Client
```

## Bedienungsbeispiel

1. `12,5` (oder `12.5`) als erste Zahl eingeben.
2. **Division** auswählen.
3. `2,5` als zweite Zahl eingeben und **Absenden** klicken.
4. Das nicht editierbare Antwortfeld zeigt `5`.

Bei leeren oder ungültigen Eingaben zeigt der Client eine Validierungsmeldung. Division durch null, unbekannte Operationen und fehlerhafte Anfragen liefert der Server mit `InvalidArgument`. Ist der Server nicht erreichbar, zeigt der Client eine verständliche Verbindungsfehlermeldung. Unerwartete Serverfehler werden als `Internal` behandelt.

## Tests

```powershell
dotnet test
```

Die Unit-Tests decken Addition, Subtraktion, Multiplikation, Division, Division durch null, negative Zahlen, Dezimalzahlen mit Toleranz sowie NaN und Infinity ab. Zusaetzliche Controller-Tests pruefen ungueltige Eingaben, fehlende Operationen und nicht erreichbare Server. Die Integrationstests starten einen Testserver, senden Anfragen mit dem generierten gRPC-Client und pruefen Ergebnis sowie gRPC-Fehlerstatus einschliesslich unbekannter und nicht gesetzter Enumwerte.

## Verwendete Technologien

- .NET 8, C# und WPF
- ASP.NET Core gRPC, `Grpc.Net.Client` und Protocol Buffers
- xUnit und ASP.NET Core TestHost

## Annahmen und Einschränkungen

- Der Client verwendet die lokale Serveradresse `https://localhost:7042`; sie ist als Konstante in `GrpcCalculatorClient.cs` hinterlegt.
- Die Eingabefelder akzeptieren die Dezimaltrennzeichen der aktuellen Kultur und zusätzlich den Punkt.
- Die gRPC-Clients/-Serverstubs werden während des Builds aus der gemeinsamen Proto-Datei erzeugt und nicht im Repository abgelegt.
