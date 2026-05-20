using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class InsurancePlanConfiguration : IEntityTypeConfiguration<InsurancePlan>
{
    public void Configure(EntityTypeBuilder<InsurancePlan> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(150);
    }
}