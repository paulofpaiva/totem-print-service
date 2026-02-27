using System.Text.Json.Serialization;

namespace TotemPrintService.Application.DTOs;

public class ComprovantePrintRequest
{
    [JsonPropertyName("dsAgendamento")]
    public string? AppointmentDescription { get; set; }
    [JsonPropertyName("nmPrestador")]
    public string? ProviderName { get; set; }
    [JsonPropertyName("dhAgendamento")]
    public string? AppointmentDateTime { get; set; }
    [JsonPropertyName("dsUnidade")]
    public string? UnitDescription { get; set; }
    [JsonPropertyName("dsRecursoCentral")]
    public string? ResourceDescription { get; set; }
    [JsonPropertyName("nomePaciente")]
    public string? PatientName { get; set; }
    [JsonPropertyName("dhEmissao")]
    public string? EmissionDateTime { get; set; }
    [JsonPropertyName("cdAtendimento")]
    public int AttendanceCode { get; set; }
}
