namespace MedApp.Domain.Entities;

public class Clinic : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<DoctorClinic> DoctorClinics { get; set; } = [];
    public ICollection<Availability> Availabilities { get; set; } = [];
    public ICollection<BlockedSlot> BlockedSlots { get; set; } = [];
}