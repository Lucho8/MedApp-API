namespace MedApp.Domain.Entities;

public class Availability : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid ClinicId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
}