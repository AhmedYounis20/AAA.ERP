using System.Security.Cryptography;
using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Attachments;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.SubLeadgers;

public class BranchDbConfig : BaseTreeSettingEntityDbConfig<Branch>
{
    protected override EntityTypeBuilder<Branch> ApplyConfiguration(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        base.ApplyConfiguration(builder);
        _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

        _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);
        _ = builder.HasOne(e => e.Attachment).WithMany().HasForeignKey(e => e.AttachmentId);
        return builder;
    }
}