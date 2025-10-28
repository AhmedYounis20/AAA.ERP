using Account.Domain.Models.Entities.GLSettings;

namespace Account.Infrastructure.Data.DbConfig.GLSettings;

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
