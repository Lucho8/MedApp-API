using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class DoctorInsurancePlanConfiguration : IEntityTypeConfiguration<DoctorInsurancePlan>
{
    public void Configure(EntityTypeBuilder<DoctorInsurancePlan> builder)
    {
        builder.HasKey(di => new { di.DoctorId, di.InsurancePlanId });
        builder.HasOne(di => di.Doctor)
               .WithMany(d => d.DoctorInsurancePlans)
               .HasForeignKey(di => di.DoctorId);
        builder.HasOne(di => di.InsurancePlan)
               .WithMany(i => i.DoctorInsurancePlans)
               .HasForeignKey(di => di.InsurancePlanId);
    }
}