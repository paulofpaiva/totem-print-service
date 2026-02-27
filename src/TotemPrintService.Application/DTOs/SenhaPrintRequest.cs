using System.Text.Json.Serialization;

namespace TotemPrintService.Application.DTOs;

public class SenhaPrintRequest
{
    [JsonPropertyName("dsSenha")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("dsFila")]
    public string Queue { get; set; } = string.Empty;
    [JsonPropertyName("dsEmpresa")]
    public string Company { get; set; } = string.Empty;
    [JsonPropertyName("dhEmissao")]
    public string EmissionDateTime { get; set; } = string.Empty;
}
