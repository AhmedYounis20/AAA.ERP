using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.FinancialPeriods
{
    public class FinancialPeriodDbConfig : BaseEntityDbConfig<FinancialPeriod>
    {

        protected override EntityTypeBuilder<FinancialPeriod> ApplyConfiguration(EntityTypeBuilder<FinancialPeriod> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("FinancialPeriods");

            builder.Property(e => e.YearNumber).HasMaxLength(50).IsRequired().HasColumnOrder(columnNumber++);
            builder.HasIndex(e => e.YearNumber).IsUnique();

            builder.Property(e => e.PeriodTypeByMonth).IsRequired().HasColumnOrder(columnNumber++);

            builder.Property(e => e.StartDate).IsRequired().HasColumnOrder(columnNumber++);

            builder.Property(e => e.EndDate).IsRequired().HasColumnOrder(columnNumber++);

            return builder;
        }
    }
}
