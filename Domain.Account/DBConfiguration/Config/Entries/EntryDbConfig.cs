using Domain.Account.DBConfiguration.Config.BaseConfig;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Account.DBConfiguration.Config.GLSettings
{
    public class EntryDbConfig : BaseEntityDbConfig<Entry>
    {

        protected override EntityTypeBuilder<Entry> ApplyConfiguration(EntityTypeBuilder<Entry> builder)
        {
            base.ApplyConfiguration(builder);
            builder.ToTable("Entries");

            _ = builder.Property(e => e.EntryNumber).IsRequired().HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.EntryType).HasConversion<string>().HasColumnOrder(columnNumber++);
            _ = builder.HasIndex(e => e.EntryType);
            _ = builder.Property(e => e.DocumentNumber).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.EntryDate).IsRequired().HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.FinancialPeriodId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne<FinancialPeriod>().WithMany().HasForeignKey(e => e.FinancialPeriodId);

            _ = builder.Property(e => e.CurrencyId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne<Currency>().WithMany().HasForeignKey(e => e.CurrencyId);
            _ = builder.Property(e => e.ExchageRate).HasColumnOrder(columnNumber++);

            _ = builder.Property(e => e.BranchId).HasColumnOrder(columnNumber++);
            _ = builder.HasOne<Branch>().WithMany().HasForeignKey(e => e.BranchId);

            _ = builder.Property(e => e.ReceiverName).HasColumnOrder(columnNumber++);
            _ = builder.Property(e => e.Notes).HasColumnOrder(columnNumber++);

            _ = builder.HasMany<EntryAttachment>(e => e.EntryAttachments)
                .WithOne()
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany<FinancialTransaction>(e => e.FinancialTransactions)
                .WithOne(e => e.Entry)
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            _ = builder.HasMany<EntryCostCenter>(e => e.CostCenters)
                .WithOne(e => e.Entry)
                .HasForeignKey(e => e.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            return builder;
        }
    }
}