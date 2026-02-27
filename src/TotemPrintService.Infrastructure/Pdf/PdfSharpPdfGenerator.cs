using Microsoft.Extensions.Options;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TotemPrintService.Application.DTOs;
using TotemPrintService.Application.Interfaces;
using TotemPrintService.Infrastructure.Configuration;

namespace TotemPrintService.Infrastructure.Pdf;

public class PdfSharpPdfGenerator : IPdfGenerator
{
    private readonly PrintServiceOptions _options;

    public PdfSharpPdfGenerator(IOptions<PrintServiceOptions> options)
    {
        _options = options.Value;
    }

    public string? GenerateComprovantePdf(ComprovantePrintRequest request)
    {
        try
        {
            var pdfDir = _options.PdfOutputDirectory;
            EnsureDirectory(pdfDir);
            var pdfPath = Path.Combine(pdfDir, $"comprovante-{DateTime.Now.Ticks}.pdf");

            using var document = new PdfDocument();
            document.Info.Title = "Appointment Receipt";
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var pageWidth = page.Width;
            var boxWidth = pageWidth - 60;
            var startX = 30.0;
            var startY = 200.0;
            var lineHeight = 60.0;
            var smallerLineHeight = 30.0;
            var topLeftX = startX - 10;
            var topLeftY = startY;
            var contentHeight = 0.0;

            var fontBold = new XFont("Verdana", 25, XFontStyleEx.Bold);
            var fontRegular = new XFont("Verdana", 25, XFontStyleEx.Regular);

            DrawLogo(gfx, page, pageWidth);

            if (!string.IsNullOrEmpty(request.AppointmentDescription))
                DrawWrappedString(gfx, request.AppointmentDescription, fontBold, startX, ref startY, boxWidth, lineHeight, ref contentHeight);
            if (!string.IsNullOrEmpty(request.ProviderName))
                DrawWrappedString(gfx, $"Dr(a) {request.ProviderName}", fontBold, startX, ref startY, boxWidth, lineHeight, ref contentHeight);
            if (!string.IsNullOrEmpty(request.AppointmentDateTime))
                DrawWrappedString(gfx, request.AppointmentDateTime, fontRegular, startX, ref startY, boxWidth, lineHeight, ref contentHeight);
            if (!string.IsNullOrEmpty(request.UnitDescription))
                DrawWrappedString(gfx, request.UnitDescription, fontBold, startX, ref startY, boxWidth, lineHeight, ref contentHeight);
            if (!string.IsNullOrEmpty(request.ResourceDescription))
                DrawWrappedString(gfx, request.ResourceDescription, fontBold, startX, ref startY, boxWidth, lineHeight, ref contentHeight);
            if (!string.IsNullOrEmpty(request.PatientName))
                DrawWrappedString(gfx, request.PatientName, fontBold, startX, ref startY, boxWidth, lineHeight, ref contentHeight);

            var borderWidth = boxWidth + 20;
            gfx.DrawRectangle(new XPen(XColors.Black, 2), topLeftX, topLeftY, borderWidth, contentHeight);
            startY += 20;

            if (!string.IsNullOrEmpty(request.EmissionDateTime))
            {
                gfx.DrawString(request.EmissionDateTime, new XFont("Verdana", 23, XFontStyleEx.Regular), XBrushes.Black,
                    new XRect(0, startY, pageWidth - 30, smallerLineHeight), XStringFormats.TopRight);
                startY += smallerLineHeight + 10;
            }
            if (request.AttendanceCode != 0)
            {
                gfx.DrawString($"Attendance: {request.AttendanceCode}", new XFont("Verdana", 23, XFontStyleEx.Regular), XBrushes.Black,
                    new XRect(0, startY, pageWidth - 30, smallerLineHeight), XStringFormats.TopRight);
            }

            using (var stream = File.Create(pdfPath))
                document.Save(stream, false);
            return pdfPath;
        }
        catch
        {
            return null;
        }
    }

    public string? GenerateSenhaPdf(SenhaPrintRequest request)
    {
        try
        {
            var pdfDir = _options.PdfOutputDirectory;
            EnsureDirectory(pdfDir);
            var pdfPath = Path.Combine(pdfDir, $"queue-ticket-{DateTime.Now.Ticks}.pdf");

            using var document = new PdfDocument();
            document.Info.Title = "Queue Ticket";
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var pageWidth = page.Width;
            var margin = 20.0;
            var lineHeight = 40.0;
            var maxWidth = pageWidth - 40;

            var fontRegular = new XFont("Verdana", 35, XFontStyleEx.Regular);
            var fontBold = new XFont("Verdana", 120, XFontStyleEx.Bold);

            DrawWrappedStringSimple(gfx, request.Company, fontRegular, 20, margin, maxWidth, lineHeight);
            margin += lineHeight + 80;
            DrawWrappedStringSimple(gfx, request.Queue, fontRegular, 20, margin, maxWidth, lineHeight);
            margin += lineHeight + 80;
            DrawWrappedStringSimple(gfx, request.Password, fontBold, 20, margin, maxWidth, 85);
            margin += 85 + 80;
            DrawWrappedStringSimple(gfx, request.EmissionDateTime, fontRegular, 20, margin, maxWidth, lineHeight);

            using (var stream = File.Create(pdfPath))
                document.Save(stream, false);
            return pdfPath;
        }
        catch
        {
            return null;
        }
    }

    public string? GenerateProtocoloPdf(ProtocoloPrintRequest request)
    {
        try
        {
            var pdfDir = _options.PdfOutputDirectory;
            EnsureDirectory(pdfDir);
            var pdfPath = Path.Combine(pdfDir, $"protocol-{DateTime.Now.Ticks}.pdf");

            using var document = new PdfDocument();
            document.Info.Title = "ID Card Protocol";
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var pageWidth = page.Width;
            var margin = 20.0;
            var lineHeight = 40.0;
            var maxWidth = pageWidth - 40;

            var fontRegular = new XFont("Verdana", 35, XFontStyleEx.Regular);
            var fontBold = new XFont("Verdana", 45, XFontStyleEx.Bold);

            DrawWrappedStringSimple(gfx, "HOSPITAL E MATERNIDADE SÃO CRISTÓVÃO", fontRegular, 20, margin, maxWidth, lineHeight);
            margin += lineHeight + 120;
            DrawWrappedStringSimple(gfx, "ID CARD ISSUANCE REQUEST", fontRegular, 20, margin, maxWidth, lineHeight);
            margin += lineHeight + 130;
            DrawWrappedStringSimple(gfx, $"Protocol {request.ProtocolNumber}", fontBold, 20, margin, maxWidth, 85);
            margin += 85 + 120;
            DrawWrappedStringSimple(gfx, request.EmissionDateTime, fontRegular, 20, margin, maxWidth, lineHeight);

            using (var stream = File.Create(pdfPath))
                document.Save(stream, false);
            return pdfPath;
        }
        catch
        {
            return null;
        }
    }

    private void DrawLogo(XGraphics gfx, PdfPage page, double pageWidth)
    {
        var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.LogoPath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(imagePath)) return;

        var imageWidth = 260.0;
        var x = (pageWidth - imageWidth) / 2;
        var logo = XImage.FromFile(imagePath);
        gfx.DrawImage(logo, x, 50, imageWidth, logo.PixelHeight * imageWidth / logo.PixelWidth);
    }

    private static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static void DrawWrappedString(XGraphics gfx, string text, XFont font, double x, ref double y, double maxWidth, double lineHeight, ref double contentHeight)
    {
        var remainingText = text;
        while (!string.IsNullOrEmpty(remainingText))
        {
            var textToFit = remainingText;
            while (gfx.MeasureString(textToFit, font).Width > maxWidth && textToFit.Length > 0)
            {
                var lastSpace = textToFit.LastIndexOf(' ');
                if (lastSpace == -1) break;
                textToFit = textToFit[..lastSpace];
            }
            gfx.DrawString(textToFit, font, XBrushes.Black, new XRect(x, y, maxWidth, lineHeight), XStringFormats.Center);
            remainingText = remainingText[textToFit.Length..].TrimStart();
            y += lineHeight;
            contentHeight += lineHeight;
        }
    }

    private static void DrawWrappedStringSimple(XGraphics gfx, string text, XFont font, double x, double y, double maxWidth, double lineHeight)
    {
        var remainingText = text;
        var currentY = y;

        while (!string.IsNullOrEmpty(remainingText))
        {
            var textToFit = remainingText;
            while (gfx.MeasureString(textToFit, font).Width > maxWidth && textToFit.Length > 0)
            {
                var lastSpace = textToFit.LastIndexOf(' ');
                if (lastSpace == -1) break;
                textToFit = textToFit[..lastSpace];
            }
            gfx.DrawString(textToFit, font, XBrushes.Black, new XRect(x, currentY, maxWidth, lineHeight), XStringFormats.Center);
            remainingText = remainingText[textToFit.Length..].TrimStart();
            currentY += lineHeight;
        }
    }
}
