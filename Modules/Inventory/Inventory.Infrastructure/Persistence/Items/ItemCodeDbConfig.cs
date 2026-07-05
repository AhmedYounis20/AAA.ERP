using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Items
{
    public class ItemCodeDbConfig : BaseEntityDbConfig<ItemCode>
    {
        protected override EntityTypeBuilder<ItemCode> ApplyConfiguration(EntityTypeBuilder<ItemCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemCodes");

            _ = builder.HasIndex(e=>e.Code).IsUnique();
            _ = builder.Property(e=>e.Code).IsRequired();
            _ = builder.HasIndex(e=>e.CodeType);
            _ = builder.Property(e=>e.CodeType).HasConversion<string>();

            return builder;
        }
    }
}