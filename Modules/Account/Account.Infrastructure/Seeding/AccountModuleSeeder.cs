using Account.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Seeding;
using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Domain.Models.Entities.AccountGuides;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Account.Domain.Models.Entities.CollectionBooks;
using Account.Domain.Models.Entities.CostCenters;
using Account.Domain.Models.Entities.Currencies;
using Account.Domain.Models.Entities.FinancialPeriods;
using Account.Domain.Models.Entities.GLSettings;
using Account.Domain.Models.Entities.Roles;
using Account.Domain.Models.Entities.SubLeadgers;
using Shared.Domain.Attachments;
using Newtonsoft.Json;

namespace Account.Infrastructure.Seeding;

public class AccountModuleSeeder : IModuleSeeder
{
    private readonly AccountDbContext _db;
    private readonly ILogger<AccountModuleSeeder> _logger;
    public string Name => "Account";

    public AccountModuleSeeder(AccountDbContext db, ILogger<AccountModuleSeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
        var basePath = Path.Combine(AppContext.BaseDirectory, "Seeding", "Data");

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            await ImportTable<AccountGuide>(basePath, "AccountGuides.json", cancellationToken);
            await ImportTable<Role>(basePath, "Roles.json", cancellationToken);
            await ImportTable<ChartOfAccount>(basePath, "ChartOfAccounts.json", cancellationToken);
            await ImportTable<GLSetting>(basePath, "GLSettings.json", cancellationToken);
            await ImportTable<Currency>(basePath, "Currencies.json", cancellationToken);
            await ImportTable<FinancialPeriod>(basePath, "FinancialPeriods.json", cancellationToken);
            await ImportTable<Bank>(basePath, "Banks.json", cancellationToken);
            await ImportTable<Customer>(basePath, "Customers.json", cancellationToken);
            await ImportTable<Supplier>(basePath, "Suppliers.json", cancellationToken);
            await ImportTable<CashInBox>(basePath, "CashInBoxes.json", cancellationToken);
            await ImportTable<FixedAsset>(basePath, "FixedAssets.json", cancellationToken);
            await ImportTable<Attachment>(basePath, "Attachments.json", cancellationToken);
            await ImportTable<Branch>(basePath, "Branches.json", cancellationToken);
            await ImportTable<CostCenter>(basePath, "CostCenters.json", cancellationToken);
            await ImportTable<CostCenterChartOfAccount>(basePath, "CostCenterChartOfAccounts.json", cancellationToken);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {

            await transaction.RollbackAsync();
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportTable<TEntity>(string basePath, string jsonFile, CancellationToken ct) where TEntity : BaseEntity
    {
        var fullPath = Path.Combine(basePath, jsonFile);
        if (!File.Exists(fullPath))
            return; 
        var json = await File.ReadAllTextAsync(fullPath);
        List<TEntity> entities = JsonConvert.DeserializeObject<List<TEntity>>(json) ?? new();

        try
        {
            foreach (TEntity entity in entities)
            {
                TEntity? dbEntity = _db.Set<TEntity>().Where(e => e.Id == entity.Id).FirstOrDefault();
                if (dbEntity != null)
                    _db.Update(entity);
                else
                    _db.Add(entity);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}


