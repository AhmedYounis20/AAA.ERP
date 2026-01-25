using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Products
{
    public class ProductAttachmentDbConfig : BaseEntityDbConfig<ProductAttachment>
    {
        protected override EntityTypeBuilder<ProductAttachment> ApplyConfiguration(EntityTypeBuilder<ProductAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductAttachments");

            _ = builder.Property(e => e.AttachmentType).HasConversion<string>();

            _ = builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductAttachments)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasOne(e => e.Attachment)
            .WithMany()
            .HasForeignKey(e => e.AttachmentId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}


