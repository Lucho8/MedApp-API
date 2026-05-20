using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).HasPrecision(10, 2);
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(10);
        builder.Property(p => p.Provider).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Status).IsRequired().HasMaxLength(50);
        builder.HasOne(p => p.Appointment)
               .WithOne(a => a.Payment)
               .HasForeignKey<Payment>(p => p.AppointmentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}