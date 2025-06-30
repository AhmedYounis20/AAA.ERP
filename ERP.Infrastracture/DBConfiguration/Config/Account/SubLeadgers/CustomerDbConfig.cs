using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.SubLeadgers
{
    public class CustomerDbConfig : BaseTreeSettingEntityDbConfig<Customer>
    {
        protected override EntityTypeBuilder<Customer> ApplyConfiguration(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            base.ApplyConfiguration(builder);
            _ = builder.Property(e => e.ChartOfAccountId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne(e => e.ChartOfAccount).WithMany().HasForeignKey(e => e.ChartOfAccountId);

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
