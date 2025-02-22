using EventManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations;

public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.Property(e => e.Message)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(e => e.StackTrace)
            .HasMaxLength(4000);

        builder.Property(e => e.Source)
            .HasMaxLength(256);

        builder.Property(e => e.RequestPath)
            .HasMaxLength(256);

        builder.Property(e => e.RequestMethod)
            .HasMaxLength(16);

        builder.Property(e => e.UserAgent)
            .HasMaxLength(512);

        builder.Property(e => e.IpAddress)
            .HasMaxLength(50);

        builder.Property(e => e.ErrorType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.AdditionalInfo)
            .HasMaxLength(4000);

        // Create index on Timestamp for better query performance
        builder.HasIndex(e => e.Timestamp);
    }
}
