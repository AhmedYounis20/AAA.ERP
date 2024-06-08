using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.SubLeadgers
{
    public class CustomerDbConfig : BaseTreeSettingEntityDbConfig<Customer>
    {
        protected override EntityTypeBuilder<Customer> ApplyConfiguration(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            base.ApplyConfiguration(builder);
            _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne(e=>e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);
            
            _ = builder.Property(e => e.CustomerType).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Phone).HasMaxLength(300).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Mobile).HasMaxLength(300).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Email).HasMaxLength(300).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.TaxNumber).HasMaxLength(300).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Notes).HasMaxLength(1000).HasColumnOrder(columnNumber++);
            return builder;
        }
    }
}
