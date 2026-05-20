using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class BlockedSlotConfiguration : IEntityTypeConfiguration<BlockedSlot>
{
    public void Configure(EntityTypeBuilder<BlockedSlot> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Reason).HasMaxLength(300);
        builder.HasOne(b => b.Doctor)
               .WithMany(d => d.BlockedSlots)
               .HasForeignKey(b => b.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.Clinic)
               .WithMany(c => c.BlockedSlots)
               .HasForeignKey(b => b.ClinicId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}