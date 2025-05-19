using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.SubLeadgers;

public class FixedAssetDbConfig : BaseTreeSettingEntityDbConfig<FixedAsset>
{
    protected override EntityTypeBuilder<FixedAsset> ApplyConfiguration(EntityTypeBuilder<FixedAsset> builder)
    {
        builder.ToTable("FixedAssets");
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);
        _ = builder.HasOne(e => e.AccumlatedAccount).WithMany().HasForeignKey(e => e.AccumlatedAccountId);
        _ = builder.HasOne(e => e.ExpensesAccount).WithMany().HasForeignKey(e => e.ExpensesAccountId);

        _ = builder.Property(e => e.Serial).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Model).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Version).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.ManufactureCompany).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);

        return builder;
    }
}