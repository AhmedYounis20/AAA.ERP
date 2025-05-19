using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastracture.DBConfiguration.Config.Account.Entries
{
    public class FinancialTransactionDbConfig : BaseEntityDbConfig<FinancialTransaction>
    {

        protected override EntityTypeBuilder<FinancialTransaction> ApplyConfiguration(EntityTypeBuilder<FinancialTransaction> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("FinancialTransactions");

            _ = builder.Property(e => e.EntryId).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ChartOfAccountId).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.AccountNature).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Amount).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.OrderNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.PaymentType).HasConversion<string>().HasDefaultValue(PaymentType.Cash).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.CollectionBookId).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.CashAgentName).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.CashPhoneNumber).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.ChequeBankId).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ChequeNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ChequeIssueDate).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.ChequeCollectionDate).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.PromissoryName).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.PromissoryNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.PromissoryCollectionDate).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.PromissoryIdentityCard).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.WireTransferReferenceNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.AtmReferenceNumber).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.CreditCardLastDigits).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.IsPaymentTransaction).HasDefaultValue(false).HasColumnOrder(columnNumber++);

            _ = builder.HasOne(e => e.ChequeBank).WithMany().HasForeignKey(e => e.ChequeBankId);
            _ = builder.HasOne(e => e.CollectionBook).WithMany().HasForeignKey(e => e.CollectionBookId);
            _ = builder.Property(e => e.Notes).HasColumnOrder(columnNumber++);

            return builder;
        }
    }
}