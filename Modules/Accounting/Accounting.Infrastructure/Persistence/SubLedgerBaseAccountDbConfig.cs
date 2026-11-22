using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Persistence;
using ERP.BuildingBlocks.Persistence.Configurations;

namespace Accounting.Infrastructure.Persistence;

public class SubLedgerBaseAccountDbConfig : BaseEntityDbConfig<SubLedgerBaseAccount>
{
    private const int EnumColumnLength = 50;

    protected override EntityTypeBuilder<SubLedgerBaseAccount> ApplyConfiguration(
        EntityTypeBuilder<SubLedgerBaseAccount> builder)
    {
        base.ApplyConfiguration(builder);
        builder.ToTable("SubLedgerBaseAccounts", DbSchemaNames.Accounting);

        _ = builder.Property(e => e.ChartOfAccountId).IsRequired();
        _ = builder.Property(e => e.SubLedgerType).HasConversion<string>().HasMaxLength(EnumColumnLength).IsRequired();
        _ = builder.Property(e => e.Role).HasConversion<string>().HasMaxLength(EnumColumnLength).IsRequired();
        _ = builder.Property(e => e.FixedAssetType).HasConversion<string>().HasMaxLength(EnumColumnLength);

        _ = builder.HasOne(e => e.ChartOfAccount)
            .WithMany()
            .HasForeignKey(e => e.ChartOfAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.HasIndex(e => e.ChartOfAccountId).IsUnique();
        _ = builder.HasIndex(e => new { e.SubLedgerType, e.Role, e.FixedAssetType })
            .IsUnique()
            .HasFilter("[FixedAssetType] IS NOT NULL");
        _ = builder.HasIndex(e => new { e.SubLedgerType, e.Role })
            .IsUnique()
            .HasFilter("[FixedAssetType] IS NULL");

        return builder;
    }
}
