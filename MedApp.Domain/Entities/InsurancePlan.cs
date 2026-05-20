namespace MedApp.Domain.Entities;

public class InsurancePlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<DoctorInsurancePlan> DoctorInsurancePlans { get; set; } = [];
}