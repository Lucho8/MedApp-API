namespace MedApp.Domain.Entities;

public class WaitingList : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Guid ClinicId { get; set; }
    public DateOnly? PreferredDateFrom { get; set; }
    public DateOnly? PreferredDateTo { get; set; }
    public string Status { get; set; } = "Waiting";

    public Doctor Doctor { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
}