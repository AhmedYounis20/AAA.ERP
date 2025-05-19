using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Entries;

public class EntryCostCenterDbConfig : BaseEntityDbConfig<EntryCostCenter>
{

    protected override EntityTypeBuilder<EntryCostCenter> ApplyConfiguration(EntityTypeBuilder<EntryCostCenter> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("EntryCostCenters");

        builder.HasOne(e => e.CostCenter)
        .WithMany()
        .HasForeignKey(e => e.CostCenterId)
        .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
}
