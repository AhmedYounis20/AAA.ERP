using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Items
{
    public class ItemManufacturerCompanyDbConfig : BaseEntityDbConfig<ItemManufacturerCompany>
    {
        protected override EntityTypeBuilder<ItemManufacturerCompany> ApplyConfiguration(EntityTypeBuilder<ItemManufacturerCompany> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemManufacturerCompanies");

            _ = builder.HasIndex(e => new { e.ItemId, e.ManufacturerCompanyId }).IsUnique();

            _ = builder.HasOne(e => e.ManufacturerCompany)
            .WithMany()
            .HasForeignKey(e => e.ManufacturerCompanyId)
            .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
    }
}