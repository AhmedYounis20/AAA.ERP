using Account.Domain.Models.Entities.Currencies;
using Account.Domain.Models.Entities.Entries;
using Account.Domain.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Infrastructure.Data.DbConfig.BaseConfig;

namespace Account.Infrastructure.Data.DbConfig.Entries;

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