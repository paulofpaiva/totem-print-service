using TotemPrintService.Application.DTOs;
using TotemPrintService.Domain;

namespace TotemPrintService.Application.Interfaces;

public interface IPrinterService
{
    PrintResult PrintFromPdf(string pdfPath, PrintJobType jobType);
}
