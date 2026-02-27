namespace TotemPrintService.Application.DTOs;

public class ComprovantePrintRequest
{
    public string? AppointmentDescription { get; set; }
    public string? ProviderName { get; set; }
    public string? AppointmentDateTime { get; set; }
    public string? UnitDescription { get; set; }
    public string? ResourceDescription { get; set; }
    public string? PatientName { get; set; }
    public string? EmissionDateTime { get; set; }
    public int AttendanceCode { get; set; }
}
