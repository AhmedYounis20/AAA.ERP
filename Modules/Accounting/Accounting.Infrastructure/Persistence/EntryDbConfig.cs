using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.DBConfiguration.Config.BaseConfig;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence
{
    public class EntryDbConfig : BaseEntityDbConfig<Entry>
    {

        protected override EntityTypeBuilder<Entry> ApplyConfiguration(EntityTypeBuilder<Entry> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Entries");

            _ = builder.Property(e => e.EntryNumber).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.EntryType)
                .HasConversion(EntryTypeValueConverter.Instance)
                .HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.DocumentNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.EntryDate).IsRequired().HasColumnOrder(columnNumber++);
            
            // Indexes for frequently queried columns
            _ = builder.HasIndex(e => e.EntryType);
            _ = builder.HasIndex(e => e.EntryNumber);
            _ = builder.HasIndex(e => e.EntryDate);
            _ = builder.HasIndex(e => e.FinancialPeriodId);
            _ = builder.HasIndex(e => new { e.FinancialPeriodId, e.EntryNumber }).IsUnique();

            _ = builder.Property(e => e.SourceDocumentId).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.SourceDocumentType).HasMaxLength(64).HasColumnOrder(columnNumber++);
            _ = builder.HasIndex(e => new { e.SourceDocumentType, e.SourceDocumentId })
                .IsUnique()
                .HasFilter("[SourceDocumentId] IS NOT NULL");

            _ = builder.Property(e => e.FinancialPeriodId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne(e => e.FinancialPeriod).WithMany().HasForeignKey(e => e.FinancialPeriodId);

            _ = builder.Property(e => e.CurrencyId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne<Currency>().WithMany().HasForeignKey(e => e.CurrencyId);
            _ = builder.Property(e => e.ExchangeRate).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.BranchId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne<Branch>().WithMany().HasForeignKey(e => e.BranchId);

            _ = builder.Property(e => e.ReceiverName).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Notes).HasColumnOrder(columnNumber++);

            _ = builder.HasMany(e => e.EntryAttachments)
                .WithOne()
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.FinancialTransactions)
                .WithOne(e => e.Entry)
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany(e => e.CostCenters)
                .WithOne(e => e.Entry)
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}