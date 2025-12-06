using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.BaseEntities;

namespace ERP.Infrastracture.DBConfiguration.Config.Audit;

public class AuditLogDbConfig : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.EntityId)
            .IsRequired();

        builder.Property(e => e.Action)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.OldValues)
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.NewValues)
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.ChangedProperties)
            .HasMaxLength(1000);

        builder.Property(e => e.UserName)
            .HasMaxLength(256);

        builder.Property(e => e.IpAddress)
            .HasMaxLength(50);

        builder.Property(e => e.UserAgent)
            .HasMaxLength(500);

        builder.Property(e => e.AdditionalInfo)
            .HasMaxLength(1000);

        // Indexes for common queries
        builder.HasIndex(e => e.EntityType);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.Action);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Timestamp);
        builder.HasIndex(e => new { e.EntityType, e.EntityId });
    }
}

