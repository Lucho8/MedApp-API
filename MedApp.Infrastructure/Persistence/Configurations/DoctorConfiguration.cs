using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.LastName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.ConsultationPrice).HasPrecision(10, 2);
        builder.HasOne(d => d.User)
               .WithOne(u => u.Doctor)
               .HasForeignKey<Doctor>(d => d.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}