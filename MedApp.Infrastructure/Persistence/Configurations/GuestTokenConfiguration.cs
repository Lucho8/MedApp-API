using MedApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedApp.Infrastructure.Persistence.Configurations;

public class GuestTokenConfiguration : IEntityTypeConfiguration<GuestToken>
{
    public void Configure(EntityTypeBuilder<GuestToken> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Email).IsRequired().HasMaxLength(256);
        builder.Property(g => g.Token).IsRequired();
        builder.HasIndex(g => g.Token).IsUnique();
    }
}