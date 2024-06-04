using AAA.ERP.DBConfiguration.Config.BaseConfig;
using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AAA.ERP.DBConfiguration.Config.Currencies
{
    public class CashInBoxDbConfig : BaseTreeSettingEntityDbConfig<CashInBox>
    {

        protected override EntityTypeBuilder<CashInBox> ApplyConfiguration(EntityTypeBuilder<CashInBox> builder)
        {
            builder.ToTable("CashInBox");
            base.ApplyConfiguration(builder);
            _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++).IsRequired();
            _ = builder.HasOne<ChartOfAccount>().WithMany().HasForeignKey(e => e.ChartOfAccountId);

            return builder;
        }
    }
}
