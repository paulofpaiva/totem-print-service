using System.Drawing;
using System.Drawing.Printing;
using PdfiumViewer;
using TotemPrintService.Application.DTOs;
using TotemPrintService.Application.Interfaces;
using TotemPrintService.Domain;

namespace TotemPrintService.Infrastructure.Printing;

public class ThermalPrinterService : IPrinterService
{
    private static readonly string[] AllowedPrinters =
    {
        "Bematech MP-4200 HS", "MP-4200 HS", "Bematech MP-4200 TH",
        "CUSTOM TG2480-H", "TG2480-H", "MP-4200 TH", "MP-4200TH",
        "EPSON TM-T20 Receipt", "EPSON TM-T20"
    };

    public PrintResult PrintFromPdf(string pdfPath, PrintJobType jobType)
    {
        using var image = ConvertPdfToImage(pdfPath);
        if (image == null)
            return new PrintResult(500, "Failed to convert PDF to image.");

        return PrintImage(image, jobType);
    }

    private static Image? ConvertPdfToImage(string pdfPath, int pageNumber = 0)
    {
        try
        {
            using var pdfDocument = PdfDocument.Load(pdfPath);
            var image = pdfDocument.Render(pageNumber, 300, 300, PdfRenderFlags.Annotations);
            return image;
        }
        catch
        {
            return null;
        }
    }

    private static PrintResult PrintImage(Image image, PrintJobType jobType)
    {
        var imageType = jobType.ToString().ToUpperInvariant();
        var selectedPrinter = FindThermalPrinter();

        if (selectedPrinter == null)
            return new PrintResult(500, "No allowed thermal printer found.");

        var (paperWidth, paperHeight) = GetPaperSize(selectedPrinter, imageType);

        using var printDocument = new PrintDocument { PrinterSettings = { PrinterName = selectedPrinter } };
        var paperSize = new System.Drawing.Printing.PaperSize("Custom", paperWidth, paperHeight);

        printDocument.DefaultPageSettings!.Landscape = false;
        printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        printDocument.DefaultPageSettings.PaperSize = paperSize;

        printDocument.PrintPage += (_, e) =>
        {
            e.Graphics!.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            var imageAspectRatio = (float)image.Width / image.Height;
            var paperAspectRatio = (float)paperSize.Width / paperSize.Height;

            int imgWidth, imgHeight;
            if (imageAspectRatio > paperAspectRatio)
            {
                imgWidth = paperSize.Width;
                imgHeight = (int)(paperSize.Width / imageAspectRatio);
            }
            else
            {
                imgHeight = paperSize.Height;
                imgWidth = (int)(paperSize.Height * imageAspectRatio);
            }

            var x = (paperSize.Width - imgWidth) / 2;
            var y = (paperSize.Height - imgHeight) / 2;
            e.Graphics.DrawImage(image, new Rectangle(x, y, imgWidth, imgHeight));
        };

        try
        {
            printDocument.Print();
            return new PrintResult(200, "Print sent successfully.");
        }
        catch (Exception ex)
        {
            return new PrintResult(500, $"Print error: {ex.Message}");
        }
    }

    private static string? FindThermalPrinter()
    {
        foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
        {
            foreach (var allowed in AllowedPrinters)
            {
                if (printer.IndexOf(allowed, StringComparison.OrdinalIgnoreCase) < 0) continue;

                var ps = new System.Drawing.Printing.PrinterSettings { PrinterName = printer };
                if (ps.IsDefaultPrinter)
                    return printer;
            }
        }
        return null;
    }

    private static (int width, int height) GetPaperSize(string printer, string imageType)
    {
        const int paperWidth = 280;
        var isTg2480 = printer.Contains("TG2480-H", StringComparison.OrdinalIgnoreCase);

        var paperHeight = imageType switch
        {
            "SENHA" or "PROTOCOLO" => isTg2480 ? 300 : 250,
            "COMPROVANTE" => isTg2480 ? 300 : 400,
            _ => 300
        };
        return (paperWidth, paperHeight);
    }
}
