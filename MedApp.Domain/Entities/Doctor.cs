namespace MedApp.Domain.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public int AppointmentDurationMinutes { get; set; } = 30;
    public decimal ConsultationPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; } = [];
    public ICollection<DoctorClinic> DoctorClinics { get; set; } = [];
    public ICollection<DoctorInsurancePlan> DoctorInsurancePlans { get; set; } = [];
    public ICollection<Availability> Availabilities { get; set; } = [];
    public ICollection<BlockedSlot> BlockedSlots { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}