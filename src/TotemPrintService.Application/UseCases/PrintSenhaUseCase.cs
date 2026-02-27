using TotemPrintService.Application.DTOs;
using TotemPrintService.Application.Interfaces;
using TotemPrintService.Domain;

namespace TotemPrintService.Application.UseCases;

public class PrintSenhaUseCase
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IPrinterService _printerService;

    public PrintSenhaUseCase(IPdfGenerator pdfGenerator, IPrinterService printerService)
    {
        _pdfGenerator = pdfGenerator;
        _printerService = printerService;
    }

    public PrintResult Execute(SenhaPrintRequest request)
    {
        var pdfPath = _pdfGenerator.GenerateSenhaPdf(request);
        if (string.IsNullOrEmpty(pdfPath))
            return new PrintResult(500, "Failed to generate PDF.");

        return _printerService.PrintFromPdf(pdfPath, PrintJobType.Senha);
    }
}
