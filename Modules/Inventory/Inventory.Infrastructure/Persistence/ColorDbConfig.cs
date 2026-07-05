using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Persistence;

namespace Inventory.Infrastructure.Persistence;

public class ColorDbConfig : BaseSettingEntityDbConfig<Color>
{
    protected override EntityTypeBuilder<Color> ApplyConfiguration(EntityTypeBuilder<Color> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Colors", DbSchemaNames.Inventory);

        _ = builder.HasIndex(e => e.Code).IsUnique();
        _ = builder.Property(e => e.Code).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.HasIndex(e => e.ColorValue).IsUnique();
        _ = builder.Property(e => e.ColorValue).IsRequired().HasColumnOrder(columnNumber++);
        return builder;
    }
}
