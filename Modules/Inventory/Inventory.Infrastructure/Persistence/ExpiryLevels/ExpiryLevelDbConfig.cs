using ERP.Domain.Models.Entities.Inventory.ExpiryLevels;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.ExpiryLevels;

public class ExpiryLevelDbConfig : BaseEntityDbConfig<ExpiryLevel>
{
    protected override EntityTypeBuilder<ExpiryLevel> ApplyConfiguration(EntityTypeBuilder<ExpiryLevel> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("ExpiryLevels");

        _ = builder.Property(e => e.Level)
            .IsRequired();
        _ = builder.Property(e => e.DaysFrom)
            .IsRequired();
        _ = builder.Property(e => e.DaysTo)
            .IsRequired();
        _ = builder.Property(e => e.OwnerType)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        _ = builder.Property(e => e.EnableNotification)
            .IsRequired();

        // NO ACTION to avoid multiple cascade paths (Product -> Variants -> ExpiryLevels and Product -> ExpiryLevels)
        _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ExpiryLevels)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.ExpiryLevels)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.HasIndex(e => new { e.ProductId });
        _ = builder.HasIndex(e => new { e.VariantId });

        return builder;
    }
}
