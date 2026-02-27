namespace TotemPrintService.Infrastructure.Configuration;

public class PrintServiceOptions
{
    public const string SectionName = "PrintService";
    public string PdfOutputDirectory { get; set; } = @"C:\temp";
    public string LogoPath { get; set; } = "Assets/logo-scristovao-comprovante.png";
}
