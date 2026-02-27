using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TotemPrintService.Application.Interfaces;
using TotemPrintService.Infrastructure.Configuration;
using TotemPrintService.Infrastructure.FileSystem;
using TotemPrintService.Infrastructure.Logging;
using TotemPrintService.Infrastructure.Pdf;
using TotemPrintService.Infrastructure.Printing;

namespace TotemPrintService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PrintServiceOptions>(configuration.GetSection(PrintServiceOptions.SectionName));

        services.AddSingleton<IPdfGenerator, PdfSharpPdfGenerator>();
        services.AddSingleton<IPrinterService, ThermalPrinterService>();
        services.AddSingleton<IFileSystemService, FileSystemService>();
        services.AddSingleton<ITraceLogger, TraceLogger>();

        return services;
    }
}
