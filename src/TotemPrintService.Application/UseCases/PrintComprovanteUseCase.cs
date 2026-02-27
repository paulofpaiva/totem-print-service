using TotemPrintService.Application.DTOs;
using TotemPrintService.Application.Interfaces;
using TotemPrintService.Domain;

namespace TotemPrintService.Application.UseCases;

public class PrintComprovanteUseCase
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IPrinterService _printerService;

    public PrintComprovanteUseCase(IPdfGenerator pdfGenerator, IPrinterService printerService)
    {
        _pdfGenerator = pdfGenerator;
        _printerService = printerService;
    }

    public PrintResult Execute(ComprovantePrintRequest request)
    {
        var pdfPath = _pdfGenerator.GenerateComprovantePdf(request);
        if (string.IsNullOrEmpty(pdfPath))
            return new PrintResult(500, "Failed to generate PDF.");

        return _printerService.PrintFromPdf(pdfPath, PrintJobType.Comprovante);
    }
}
