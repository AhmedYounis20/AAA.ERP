using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Products
{
    public class ProductAttachmentDbConfig : BaseEntityDbConfig<ProductAttachment>
    {
        protected override EntityTypeBuilder<ProductAttachment> ApplyConfiguration(EntityTypeBuilder<ProductAttachment> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ProductAttachments", DbSchemaNames.Inventory);

            _ = builder.Property(e => e.AttachmentType).HasConversion<string>();

            // Indexes for frequently queried columns
            // Note: ProductId index already exists from foreign key
            _ = builder.HasIndex(e => e.IsPrimary); // For filtering primary attachments
            _ = builder.HasIndex(e => new { e.ProductId, e.IsPrimary }); // Composite index for finding primary picture efficiently

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


