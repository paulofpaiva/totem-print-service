# Totem Print Service

A C# print service I built while working at Hospital São Cristóvão to support thermal receipt printing via USB-connected printers on kiosk machines.

## Context

I developed the React frontend and REST API for the hospital totem (self-service kiosk), but needed a way to print the receipts that the web app generated. Since the React app runs in the browser on each local machine, direct printer access wasn't possible. The solution was a lightweight C# service that:

- Runs as a Windows service on each kiosk machine
- Listens for HTTP requests on a local port (default: 3000)
- Receives print jobs from the totem app and sends them to the USB thermal printer

## Run

```bash
dotnet run --project src/TotemPrintService.Api/
```

Listens on `http://localhost:3000`.

## Usage

The service exposes REST endpoints for different print types (appointment receipts, queue tickets, protocols, etc.). The totem frontend calls these endpoints when the user requests a print.

**Endpoints:**
- `POST /totem-print-service/print-comprovante-confirmacao` – appointment receipt
- `POST /totem-print-service/print-comprovante-senha` – queue ticket
- `POST /totem-print-service/print-comprovante-protocolo` – ID card protocol

## Tech Stack

- .NET 8
- PDFsharp, PdfiumViewer
- Minimal API / ASP.NET Core
- Windows System.Drawing for thermal printer support
