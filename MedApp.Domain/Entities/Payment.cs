namespace MedApp.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ARS";
    public string Provider { get; set; } = "MercadoPago";
    public string? ExternalPaymentId { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? UpdatedAt { get; set; }

    public Appointment Appointment { get; set; } = null!;
}