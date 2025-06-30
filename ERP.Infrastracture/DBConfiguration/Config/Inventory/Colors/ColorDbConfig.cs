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

            _ = builder.HasIndex(e => e.ColorCode).IsUnique();
            _ = builder.Property(e => e.ColorCode).IsRequired().HasColumnOrder(columnNumber++);
            return builder;
        }
    }
}