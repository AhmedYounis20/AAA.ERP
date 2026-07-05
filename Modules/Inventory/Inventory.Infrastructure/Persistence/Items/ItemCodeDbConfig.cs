using Inventory.Domain.Models.Entities.Items;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence.Items
{
    public class ItemCodeDbConfig : BaseEntityDbConfig<ItemCode>
    {
        protected override EntityTypeBuilder<ItemCode> ApplyConfiguration(EntityTypeBuilder<ItemCode> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("ItemCodes", DbSchemaNames.Inventory);

            _ = builder.HasIndex(e=>e.Code).IsUnique();
            _ = builder.Property(e=>e.Code).IsRequired();
            _ = builder.HasIndex(e=>e.CodeType);
            _ = builder.Property(e=>e.CodeType).HasConversion<string>();

            return builder;
        }
    }
}