using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.GLSettings
{
    public class FinancialTransactionDbConfig : BaseEntityDbConfig<FinancialTransaction>
    {

        protected override EntityTypeBuilder<FinancialTransaction> ApplyConfiguration(EntityTypeBuilder<FinancialTransaction> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("FinancialTransactions");

            _ = builder.Property(e => e.EntryId).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ChartOfAccountId).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.AccountNature).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Amount).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.OrderNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.OrderNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.OrderNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.OrderNumber).HasColumnOrder(columnNumber++);
            
            _ = builder.HasOne<Bank>(e=>e.ChequeBank).WithMany().HasForeignKey(e => e.ChequeBankId);
            _ = builder.HasOne<FinancialTransaction>(e=>e.ComplementTransaction).WithMany().HasForeignKey(e => e.ComplementTransactionId);
       
            _ = builder.Property(e => e.Notes).HasColumnOrder(columnNumber++);

            return builder;
        }
    }
}