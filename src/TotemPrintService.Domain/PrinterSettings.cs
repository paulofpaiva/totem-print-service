namespace TotemPrintService.Domain;

public class PrinterSettings
{
    public string PrinterName { get; }
    public PaperSize PaperSize { get; }
    public bool IsLandscape { get; }

    public PrinterSettings(string printerName, PaperSize paperSize, bool isLandscape = false)
    {
        if (string.IsNullOrWhiteSpace(printerName))
            throw new ArgumentException("Printer name cannot be empty.", nameof(printerName));

        PrinterName = printerName;
        PaperSize = paperSize;
        IsLandscape = isLandscape;
    }
}
