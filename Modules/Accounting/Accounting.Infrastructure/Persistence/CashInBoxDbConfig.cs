using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Configurations;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class CashInBoxDbConfig : BaseTreeSettingEntityDbConfig<CashInBox>
{
    protected override EntityTypeBuilder<CashInBox> ApplyConfiguration(EntityTypeBuilder<CashInBox> builder)
    {
        builder.ToTable("CashInBoxes", DbSchemaNames.Accounting);
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);
        return builder;
    }
}
