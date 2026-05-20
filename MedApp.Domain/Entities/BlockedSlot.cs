namespace MedApp.Domain.Entities;

public class BlockedSlot : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid ClinicId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Reason { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
}