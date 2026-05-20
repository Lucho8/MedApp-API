namespace MedApp.Domain.Entities;

public class SecretaryDoctor
{
    public Guid SecretaryUserId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public User Secretary { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}