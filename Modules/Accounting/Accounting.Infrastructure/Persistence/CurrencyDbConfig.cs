using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Infrastructure.Persistence;

public class CurrencyDbConfig : BaseSettingEntityDbConfig<Currency>
{
    protected override EntityTypeBuilder<Currency> ApplyConfiguration(EntityTypeBuilder<Currency> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("Currencies");

        _ = builder.Property(e => e.Symbol).IsRequired().HasMaxLength(4).HasColumnOrder(columnNumber++);
        _ = builder.HasIndex(e => e.Symbol).IsUnique();
        _ = builder.Property(e => e.ExchangeRate).IsRequired().HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.IsDefault).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.IsActive).HasDefaultValue(true).HasColumnOrder(columnNumber++);
        return builder;
    }
}
