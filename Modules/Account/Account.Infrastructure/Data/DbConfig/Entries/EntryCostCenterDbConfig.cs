using Account.Domain.Models.Entities.Entries;

namespace Account.Infrastructure.Data.DbConfig.Entries;

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
