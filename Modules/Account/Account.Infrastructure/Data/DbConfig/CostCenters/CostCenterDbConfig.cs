using Account.Domain.Models.Entities.CostCenters;

namespace Account.Infrastructure.Data.DbConfig.CostCenters;

public class CostCenterDbConfig : BaseTreeSettingEntityDbConfig<CostCenter>
{
    protected override EntityTypeBuilder<CostCenter> ApplyConfiguration(EntityTypeBuilder<CostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("CostCenters");
        builder.HasMany(e => e.ChartOfAccounts).WithOne().HasForeignKey(e => e.CostCenterId).OnDelete(DeleteBehavior.Cascade);
        return builder;
    }
}