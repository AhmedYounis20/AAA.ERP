using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.ChartOfAccountDbConfig
{
    public class ChartOfAccountDbConfig : BaseTreeSettingEntityDbConfig<ChartOfAccount>
    {
        protected override EntityTypeBuilder<ChartOfAccount> ApplyConfiguration(EntityTypeBuilder<ChartOfAccount> builder)
        {
            builder.ToTable("ChartOfAccounts");
            
            _ = builder.Property(e => e.AccountGuidId).HasColumnOrder(columnNumber++).IsRequired();
            _ = builder.HasOne<AccountGuide>().WithMany().HasForeignKey(e => e.AccountGuidId);
            
            _ = builder.Property(e => e.Code).HasColumnOrder(columnNumber++).HasMaxLength(300).IsRequired();
            _ = builder.HasIndex(e => e.Code).IsUnique();
            _ = builder.Property(e => e.Code).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.AccountNature).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsDepreciable).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsPostedAccount).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsStopDealing).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsActiveAccount).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsCreatedFromSubLeadger).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsSubLeadgerBaseAccount).HasColumnOrder(columnNumber++);

            base.ApplyConfiguration(builder);

            return builder;
        }
    }
}