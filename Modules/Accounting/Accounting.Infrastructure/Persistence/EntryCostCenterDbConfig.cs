using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence;

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
