using AAA.ERP.DBConfiguration.Config.BaseConfig;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.FinancialPeriods;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AAA.ERP.DBConfiguration.Config.Currencies
{
    public class FinancialPeriodDbConfig : BaseEntityDbConfig<FinancialPeriod>
    {

        protected override EntityTypeBuilder<FinancialPeriod> ApplyConfiguration(EntityTypeBuilder<FinancialPeriod> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("FinancialPeriods");
            
            builder.Property(e => e.YearNumber).HasMaxLength(50).IsRequired().HasColumnOrder(columnNumber++);
            builder.HasIndex(e=>e.YearNumber).IsUnique();

            builder.Property(e => e.PeriodTypeByMonth).IsRequired().HasColumnOrder(columnNumber++);

            builder.Property(e=>e.StartDate).IsRequired().HasColumnOrder(columnNumber++);
            
            builder.Property(e=>e.EndDate).IsRequired().HasColumnOrder(columnNumber++);

            return builder;
        }
    }
}
