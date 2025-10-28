using Account.Application.Data;
using Account.Domain.Models.Entities.AccountGuides;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Account.Domain.Models.Entities.CollectionBooks;
using Account.Domain.Models.Entities.CostCenters;
using Account.Domain.Models.Entities.Currencies;
using Account.Domain.Models.Entities.Entries;
using Account.Domain.Models.Entities.FinancialPeriods;
using Account.Domain.Models.Entities.GLSettings;
using Account.Domain.Models.Entities.Roles;
using Account.Domain.Models.Entities.SubLeadgers;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Attachments;
using Shared.Domain.BaseEntities;

namespace Account.Infrastructure.Data;

public class AccountDbContext :  DbContext, IAccountDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure schema
        builder.HasDefaultSchema("Account");
        
        // Apply configurations
        builder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);
    }

    // Account Entities
    public DbSet<AccountGuide> AccountGuides { get; set; }
    public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
    public DbSet<CollectionBook> CollectionBooks { get; set; }
    public DbSet<CostCenter> CostCenters { get; set; }
    public DbSet<CostCenterChartOfAccount> CostCenterChartOfAccounts { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntryAttachment> EntryAttachments { get; set; }
    public DbSet<EntryCostCenter> EntryCostCenters { get; set; }
    public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
    public DbSet<FinancialPeriod> FinancialPeriods { get; set; }
    public DbSet<GLSetting> GLSettings { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<CashInBox> CashInBoxes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<FixedAsset> FixedAssets { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}
