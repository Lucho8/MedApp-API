using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class WaitingListConfiguration : IEntityTypeConfiguration<WaitingList>
{
    public void Configure(EntityTypeBuilder<WaitingList> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Status).IsRequired().HasMaxLength(50);
        builder.HasOne(w => w.Doctor)
               .WithMany()
               .HasForeignKey(w => w.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(w => w.Patient)
               .WithMany(p => p.WaitingLists)
               .HasForeignKey(w => w.PatientId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(w => w.Clinic)
               .WithMany()
               .HasForeignKey(w => w.ClinicId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}