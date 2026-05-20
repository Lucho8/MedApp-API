using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class DoctorSpecialtyConfiguration : IEntityTypeConfiguration<DoctorSpecialty>
{
    public void Configure(EntityTypeBuilder<DoctorSpecialty> builder)
    {
        builder.HasKey(ds => new { ds.DoctorId, ds.SpecialtyId });
        builder.HasOne(ds => ds.Doctor)
               .WithMany(d => d.DoctorSpecialties)
               .HasForeignKey(ds => ds.DoctorId);
        builder.HasOne(ds => ds.Specialty)
               .WithMany(s => s.DoctorSpecialties)
               .HasForeignKey(ds => ds.SpecialtyId);
    }
}