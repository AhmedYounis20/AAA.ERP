using Accounting.Domain.Entities;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;

namespace Accounting.Infrastructure.Persistence;

public class TaxDbConfig : BaseTreeSettingEntityDbConfig<Tax>
{
    protected override EntityTypeBuilder<Tax> ApplyConfiguration(EntityTypeBuilder<Tax> builder)
    {
        builder.ToTable("Taxes");
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Percent).HasColumnType("decimal(18,4)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);

        return builder;
    }
}
