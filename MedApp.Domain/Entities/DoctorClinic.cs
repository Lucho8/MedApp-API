namespace MedApp.Domain.Entities;

public class DoctorClinic
{
    public Guid DoctorId { get; set; }
    public Guid ClinicId { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
}