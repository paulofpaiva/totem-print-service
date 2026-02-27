using System.Text.Json.Serialization;

namespace TotemPrintService.Application.DTOs;

public class ProtocoloPrintRequest
{
    [JsonPropertyName("nProtocolo")]
    public string ProtocolNumber { get; set; } = string.Empty;
    [JsonPropertyName("dhEmissao")]
    public string EmissionDateTime { get; set; } = string.Empty;
}
