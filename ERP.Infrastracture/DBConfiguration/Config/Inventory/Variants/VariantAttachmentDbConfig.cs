using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Variants
{
    public class VariantAttachmentDbConfig : BaseEntityDbConfig<VariantAttachment>
    {
        protected override EntityTypeBuilder<VariantAttachment> ApplyConfiguration(EntityTypeBuilder<VariantAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("VariantAttachments");

            _ = builder.Property(e => e.AttachmentType).HasConversion<string>();

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


