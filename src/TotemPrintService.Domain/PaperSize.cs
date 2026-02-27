namespace TotemPrintService.Domain;

public readonly record struct PaperSize(int Width, int Height)
{
    public static PaperSize Comprovante => new(280, 400);
    public static PaperSize Senha => new(280, 250);
    public static PaperSize Protocolo => new(280, 250);
    public static PaperSize Default => new(280, 300);
}
