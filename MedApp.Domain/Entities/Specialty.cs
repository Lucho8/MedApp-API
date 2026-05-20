namespace MedApp.Domain.Entities;

public class Specialty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; } = [];
}