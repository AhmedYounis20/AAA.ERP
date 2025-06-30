using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.SubLeadgers;

public class BranchDbConfig : BaseTreeSettingEntityDbConfig<Branch>
{
    protected override EntityTypeBuilder<Branch> ApplyConfiguration(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.Attachment).WithMany().HasForeignKey(e => e.AttachmentId).OnDelete(DeleteBehavior.SetNull);
        return builder;
    }
}