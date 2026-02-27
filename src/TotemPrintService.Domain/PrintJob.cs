namespace TotemPrintService.Domain;

public class PrintJob
{
    public Guid Id { get; }
    public PrintJobType Type { get; }
    public string? Content { get; }
    public DateTime CreatedAt { get; }
    public PrintJobStatus Status { get; private set; }

    public PrintJob(PrintJobType type, string? content = null)
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        Status = PrintJobStatus.Pending;
    }

    public void MarkSent() => Status = PrintJobStatus.Sent;
    public void MarkFailed() => Status = PrintJobStatus.Failed;
}

public enum PrintJobStatus
{
    Pending,
    Sent,
    Failed
}
