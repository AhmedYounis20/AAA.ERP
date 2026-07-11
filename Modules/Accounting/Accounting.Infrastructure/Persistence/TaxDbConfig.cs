using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Configurations;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class TaxDbConfig : BaseTreeSettingEntityDbConfig<Tax>
{
    protected override EntityTypeBuilder<Tax> ApplyConfiguration(EntityTypeBuilder<Tax> builder)
    {
        builder.ToTable("Taxes", DbSchemaNames.Accounting);
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Percent).HasColumnType("decimal(18,4)").HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);

        return builder;
    }
}
