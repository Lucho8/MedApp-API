using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class SecretaryDoctorConfiguration : IEntityTypeConfiguration<SecretaryDoctor>
{
    public void Configure(EntityTypeBuilder<SecretaryDoctor> builder)
    {
        builder.HasKey(sd => new { sd.SecretaryUserId, sd.DoctorId });
        builder.HasOne(sd => sd.Secretary)
               .WithMany()
               .HasForeignKey(sd => sd.SecretaryUserId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sd => sd.Doctor)
               .WithMany()
               .HasForeignKey(sd => sd.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}