using EventManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations;

public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        builder.Property(o => o.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
