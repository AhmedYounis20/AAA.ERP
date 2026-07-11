using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Persistence.Configurations;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class CostCenterDbConfig : BaseTreeSettingEntityDbConfig<CostCenter>
{
    protected override EntityTypeBuilder<CostCenter> ApplyConfiguration(EntityTypeBuilder<CostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CostCenters", DbSchemaNames.Accounting);
        builder.HasMany(e => e.ChartOfAccounts).WithOne().HasForeignKey(e => e.CostCenterId).OnDelete(DeleteBehavior.Cascade);
        return builder;
    }
}
