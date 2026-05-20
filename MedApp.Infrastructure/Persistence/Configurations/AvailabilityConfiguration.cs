using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Doctor)
               .WithMany(d => d.Availabilities)
               .HasForeignKey(a => a.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Clinic)
               .WithMany(c => c.Availabilities)
               .HasForeignKey(a => a.ClinicId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}