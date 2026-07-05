using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using ERP.BuildingBlocks.Persistence;

namespace Accounting.Infrastructure.Persistence;

public class FinancialPeriodDbConfig : BaseEntityDbConfig<FinancialPeriod>
{
    protected override EntityTypeBuilder<FinancialPeriod> ApplyConfiguration(EntityTypeBuilder<FinancialPeriod> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("FinancialPeriods", DbSchemaNames.Accounting);

        builder.Property(e => e.YearNumber).HasMaxLength(50).IsRequired().HasColumnOrder(columnNumber++);
        builder.HasIndex(e => e.YearNumber).IsUnique();

        builder.Property(e => e.PeriodTypeByMonth).IsRequired().HasColumnOrder(columnNumber++);
        builder.Property(e => e.StartDate).IsRequired().HasColumnOrder(columnNumber++);
        builder.Property(e => e.EndDate).IsRequired().HasColumnOrder(columnNumber++);

        return builder;
    }
}
