using System.Text.Json;
using TotemPrintService.Application.DTOs;
using TotemPrintService.Application.UseCases;
using TotemPrintService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<PrintComprovanteUseCase>();
builder.Services.AddScoped<PrintSenhaUseCase>();
builder.Services.AddScoped<PrintProtocoloUseCase>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Totem Print Service");

app.MapPost("/totem-print-service/print-comprovante-confirmacao", async (HttpContext context, PrintComprovanteUseCase useCase) =>
{
    context.Request.EnableBuffering();
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;

    if (string.IsNullOrEmpty(body))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Request body is empty."));
        return;
    }

    var request = JsonSerializer.Deserialize<ComprovantePrintRequest>(body);
    if (request == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Failed to deserialize request body."));
        return;
    }

    var result = useCase.Execute(request);
    context.Response.StatusCode = result.StatusCode;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/totem-print-service/print-comprovante-senha", async (HttpContext context, PrintSenhaUseCase useCase) =>
{
    context.Request.EnableBuffering();
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;

    if (string.IsNullOrEmpty(body))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Request body is empty."));
        return;
    }

    var request = JsonSerializer.Deserialize<SenhaPrintRequest>(body);
    if (request == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Failed to deserialize request body."));
        return;
    }

    var result = useCase.Execute(request);
    context.Response.StatusCode = result.StatusCode;
    await context.Response.WriteAsJsonAsync(result);
});

app.MapPost("/totem-print-service/print-comprovante-protocolo", async (HttpContext context, PrintProtocoloUseCase useCase) =>
{
    context.Request.EnableBuffering();
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;

    if (string.IsNullOrEmpty(body))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Request body is empty."));
        return;
    }

    var request = JsonSerializer.Deserialize<ProtocoloPrintRequest>(body);
    if (request == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new PrintResult(400, "Failed to deserialize request body."));
        return;
    }

    var result = useCase.Execute(request);
    context.Response.StatusCode = result.StatusCode;
    await context.Response.WriteAsJsonAsync(result);
});

app.Run("http://localhost:3000");
