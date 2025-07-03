using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Inventory.Colors;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Inventory.Colors
{
    public class ColorDbConfig : BaseSettingEntityDbConfig<Color>
    {

        protected override EntityTypeBuilder<Color> ApplyConfiguration(EntityTypeBuilder<Color> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Colors");

            _ = builder.HasIndex(e => e.Code).IsUnique();
            _ = builder.Property(e => e.Code).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.HasIndex(e => e.ColorValue).IsUnique();
            _ = builder.Property(e => e.ColorValue).IsRequired().HasColumnOrder(columnNumber++);
            return builder;
        }
    }
}