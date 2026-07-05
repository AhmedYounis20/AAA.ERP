using ERP.Infrastructure.DBConfiguration.DbContext;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Currencies;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Models.Entities.PackingUnits;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Models.Entities.Variants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.IntegrationTests.Infrastructure;

public static class InventoryTestDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, decimal initialStockBalance = 0)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Set<Currency>().AnyAsync())
            return;

        var accountGuide = new AccountGuide
        {
            Id = InventoryTestIds.AccountGuideId,
            Name = "Test Account Guide",
            NameSecondLanguage = "Test Account Guide"
        };

        var currency = new Currency
        {
            Id = InventoryTestIds.CurrencyId,
            Name = "EGP",
            NameSecondLanguage = "EGP",
            Symbol = "EGP",
            ExchangeRate = 1,
            IsDefault = true,
            IsActive = true
        };

        var financialPeriod = new FinancialPeriod
        {
            Id = InventoryTestIds.FinancialPeriodId,
            YearNumber = "2026",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2027, 1, 1)
        };

        var partyAccount = CreateChartOfAccount(InventoryTestIds.PartyAccountId, "Party", "1000", AccountNature.Debit);
        var branch1Coa = CreateChartOfAccount(InventoryTestIds.Branch1CoaId, "Branch 1 Inventory", "1100", AccountNature.Debit);
        var branch2Coa = CreateChartOfAccount(InventoryTestIds.Branch2CoaId, "Branch 2 Inventory", "1200", AccountNature.Debit);
        var stockUnderTransferCoa = CreateChartOfAccount(
            InventoryTestIds.StockUnderTransferCoaId,
            "Stock Under Transfer",
            "1300",
            AccountNature.Debit);

        var branch1 = CreateBranch(InventoryTestIds.Branch1Id, "Branch 1", InventoryTestIds.Branch1CoaId);
        var branch2 = CreateBranch(InventoryTestIds.Branch2Id, "Branch 2", InventoryTestIds.Branch2CoaId);
        var stockUnderTransferBranch = CreateBranch(
            InventoryTestIds.StockUnderTransferBranchId,
            "Stock Under Transfer",
            InventoryTestIds.StockUnderTransferCoaId);

        var packingUnit = new PackingUnit
        {
            Id = InventoryTestIds.PackingUnitId,
            Name = "Piece",
            NameSecondLanguage = "Piece"
        };

        var product = new Product
        {
            Id = InventoryTestIds.ProductId,
            Name = "Test Product",
            NameSecondLanguage = "Test Product",
            NodeType = NodeType.Domain
        };

        var variant = new Variant
        {
            Id = InventoryTestIds.VariantId,
            ProductId = InventoryTestIds.ProductId,
            IsSales = true,
            IsPurchase = true
        };

        var variantPackingUnit = new VariantPackingUnit
        {
            Id = InventoryTestIds.VariantPackingUnitId,
            VariantId = InventoryTestIds.VariantId,
            PackingUnitId = InventoryTestIds.PackingUnitId,
            PartsCount = 1,
            IsDefaultPackingUnit = true,
            AverageCostPrice = 10,
            LastCostPrice = 10
        };

        db.AddRange(
            accountGuide,
            currency,
            financialPeriod,
            partyAccount,
            branch1Coa,
            branch2Coa,
            stockUnderTransferCoa,
            branch1,
            branch2,
            stockUnderTransferBranch,
            packingUnit,
            product,
            variant,
            variantPackingUnit);

        if (initialStockBalance > 0)
        {
            db.Add(new StockBalance
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
                VariantId = InventoryTestIds.VariantId,
                PackingUnitId = InventoryTestIds.PackingUnitId,
                BranchId = InventoryTestIds.Branch1Id,
                CurrentBalance = initialStockBalance,
                UnitCost = 10,
                TotalCost = initialStockBalance * 10
            });
        }

        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
    }

    public static async Task<int> CountEntriesAsync(IServiceProvider services)
        => await services.GetRequiredService<IntegrationTestDbContext>().Set<Entry>().CountAsync();

    public static async Task<int> CountTransactionsAsync(IServiceProvider services)
        => await services.GetRequiredService<IntegrationTestDbContext>().Set<InventoryTransaction>().CountAsync();

    public static async Task<decimal> GetStockBalanceAsync(
        IServiceProvider services,
        Guid branchId,
        Guid variantId = default,
        Guid packingUnitId = default)
    {
        variantId = variantId == default ? InventoryTestIds.VariantId : variantId;
        packingUnitId = packingUnitId == default ? InventoryTestIds.PackingUnitId : packingUnitId;

        var balance = await services.GetRequiredService<IntegrationTestDbContext>()
            .Set<StockBalance>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.BranchId == branchId &&
                e.VariantId == variantId &&
                e.PackingUnitId == packingUnitId);

        return balance?.CurrentBalance ?? 0;
    }

    public static async Task<Entry?> GetLatestEntryWithLinesAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        var entry = await db.Set<Entry>()
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();

        if (entry is null)
            return null;

        entry.FinancialTransactions = await db.Set<FinancialTransaction>()
            .Where(t => t.EntryId == entry.Id)
            .ToListAsync();

        return entry;
    }

    public static async Task<int> CountFinancialTransactionsAsync(IServiceProvider services)
        => await services.GetRequiredService<IntegrationTestDbContext>().Set<FinancialTransaction>().CountAsync();

    private static ChartOfAccount CreateChartOfAccount(Guid id, string name, string code, AccountNature nature)
        => new()
        {
            Id = id,
            Name = name,
            NameSecondLanguage = name,
            Code = code,
            NodeType = NodeType.Domain,
            AccountNature = nature,
            IsActiveAccount = true,
            AccountGuidId = InventoryTestIds.AccountGuideId
        };

    private static Branch CreateBranch(Guid id, string name, Guid chartOfAccountId)
        => new()
        {
            Id = id,
            Name = name,
            NameSecondLanguage = name,
            NodeType = NodeType.Domain,
            ChartOfAccountId = chartOfAccountId
        };
}
