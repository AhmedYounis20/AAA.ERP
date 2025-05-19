using Domain.Account.DBConfiguration.Config.BaseConfig;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.SubLeadgers;

public class BankDbConfig : BaseTreeSettingEntityDbConfig<Bank>
{
    protected override EntityTypeBuilder<Bank> ApplyConfiguration(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks");
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Phone).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Email).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.BankAccount).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.BankAddress).HasMaxLength(300).HasColumnOrder(columnNumber++);
        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);

        return builder;
    }
}