using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.GLSettings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.GLSettings
{
    public class GLSettingDbConfig : BaseEntityDbConfig<GLSetting>
    {

        protected override EntityTypeBuilder<GLSetting> ApplyConfiguration(EntityTypeBuilder<GLSetting> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("GLSettings");

            _ = builder.Property(e => e.IsAllowingEditVoucher).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsAllowingDeleteVoucher).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsAllowingNegativeBalances).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.DecimalDigitsNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.DepreciationApplication).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.MonthDays).HasColumnOrder(columnNumber++);
            return builder;
        }
    }
}
