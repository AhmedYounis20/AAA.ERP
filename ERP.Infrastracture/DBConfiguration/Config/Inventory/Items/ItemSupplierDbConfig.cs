using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemSupplierDbConfig : BaseEntityDbConfig<ItemSupplier>
    {
        protected override EntityTypeBuilder<ItemSupplier> ApplyConfiguration(EntityTypeBuilder<ItemSupplier> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemSuppliers");

            _ = builder.HasIndex(e => new { e.ItemId, e.SupplierId }).IsUnique();

            _ = builder.HasOne(e => e.Supplier)
            .WithMany()
            .HasForeignKey(e => e.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}