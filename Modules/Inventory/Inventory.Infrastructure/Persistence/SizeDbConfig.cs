using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Persistence;

public class SizeDbConfig : BaseSettingEntityDbConfig<Size>
{
    protected override EntityTypeBuilder<Size> ApplyConfiguration(EntityTypeBuilder<Size> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Sizes");
        _ = builder.HasIndex(e => e.Code).IsUnique();
        _ = builder.Property(e => e.Code).IsRequired().HasColumnOrder(columnNumber++);
        return builder;
    }
}
