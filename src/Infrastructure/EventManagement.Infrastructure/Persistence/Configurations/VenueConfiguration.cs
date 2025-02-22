using EventManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.Property(v => v.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Address)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.Description)
            .HasMaxLength(1000);

        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
