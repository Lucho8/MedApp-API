namespace MedApp.Domain.Entities;

public class DoctorInsurancePlan
{
    public Guid DoctorId { get; set; }
    public Guid InsurancePlanId { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public InsurancePlan InsurancePlan { get; set; } = null!;
}