using TotemPrintService.Application.DTOs;

namespace TotemPrintService.Application.Interfaces;

public interface IPdfGenerator
{
    string? GenerateComprovantePdf(ComprovantePrintRequest request);
    string? GenerateSenhaPdf(SenhaPrintRequest request);
    string? GenerateProtocoloPdf(ProtocoloPrintRequest request);
}
