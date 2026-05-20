using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class DoctorClinicConfiguration : IEntityTypeConfiguration<DoctorClinic>
{
    public void Configure(EntityTypeBuilder<DoctorClinic> builder)
    {
        builder.HasKey(dc => new { dc.DoctorId, dc.ClinicId });
        builder.HasOne(dc => dc.Doctor)
               .WithMany(d => d.DoctorClinics)
               .HasForeignKey(dc => dc.DoctorId);
        builder.HasOne(dc => dc.Clinic)
               .WithMany(c => c.DoctorClinics)
               .HasForeignKey(dc => dc.ClinicId);
    }
}