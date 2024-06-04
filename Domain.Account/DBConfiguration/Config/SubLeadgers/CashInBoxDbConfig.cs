﻿using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.SubLeadgers
{
    public class CashInBoxDbConfig : BaseTreeSettingEntityDbConfig<CashInBox>
    {

        protected override EntityTypeBuilder<CashInBox> ApplyConfiguration(EntityTypeBuilder<CashInBox> builder)
        {
            builder.ToTable("CashInBox");
            base.ApplyConfiguration(builder);
            _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++).IsRequired();
            _ = builder.HasOne(e=>e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

            return builder;
        }
    }
}
