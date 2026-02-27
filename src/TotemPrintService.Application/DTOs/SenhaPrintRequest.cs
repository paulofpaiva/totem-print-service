namespace TotemPrintService.Application.DTOs;

public class SenhaPrintRequest
{
    public string Password { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string EmissionDateTime { get; set; } = string.Empty;
}
