using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Variants
{
    public class VariantAttachmentDbConfig : BaseEntityDbConfig<VariantAttachment>
    {
        protected override EntityTypeBuilder<VariantAttachment> ApplyConfiguration(EntityTypeBuilder<VariantAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantAttachments");

            _ = builder.Property(e => e.AttachmentType).HasConversion<string>();

            // Indexes for frequently queried columns
            // Note: VariantId index already exists from foreign key
            _ = builder.HasIndex(e => e.IsPrimary); // For filtering primary attachments
            _ = builder.HasIndex(e => new { e.VariantId, e.IsPrimary }); // Composite index for finding primary picture efficiently

            _ = builder.HasOne(e => e.Variant)
            .WithMany(e => e.VariantAttachments)
            .HasForeignKey(e => e.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.Attachment)
            .WithMany()
            .HasForeignKey(e => e.AttachmentId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}


