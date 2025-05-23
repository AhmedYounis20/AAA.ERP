using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.SubLeadgers
{
    public class CashInBoxDbConfig : BaseTreeSettingEntityDbConfig<CashInBox>
    {
        protected override EntityTypeBuilder<CashInBox> ApplyConfiguration(EntityTypeBuilder<CashInBox> builder)
        {
            builder.ToTable("CashInBoxes");
            base.ApplyConfiguration(builder);
            _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

            _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);
            return builder;
        }
    }
}