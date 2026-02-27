using TotemPrintService.Application.Interfaces;

namespace TotemPrintService.Infrastructure.Logging;

public class TraceLogger : ITraceLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {message}");
    }
}
