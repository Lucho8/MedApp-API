using MedApp.Domain.Enums;

namespace MedApp.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid ClinicId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime DateTime { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
    public Payment? Payment { get; set; }
}