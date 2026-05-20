namespace MedApp.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid? UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? DNI { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public User? User { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<WaitingList> WaitingLists { get; set; } = [];
}