using ERP.Infrastructure.DBConfiguration.DbContext;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Currencies;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Domain.Models.Entities.GLSettings;
using Accounting.Domain.Models.Entities.Roles;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Inventory.Domain.Models.Entities.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Identity.Domain;

namespace ERP.Infrastructure.Utilities;

public class ImportDataToSeed
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ImportDataToSeed> _logger;

    public ImportDataToSeed(IConfiguration configuration, ApplicationDbContext context,
        ILogger<ImportDataToSeed> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Import(string folder = "account")
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await ImportBussinessData(folder);
            await ImportIdentityData(folder);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Account seed import failed for folder {Folder}", folder);
            throw;
        }
    }

    private async Task ImportBussinessData(string folder)
    {
        await ImportTable<AttributeDefinition>(folder, "AttributeDefinitions.json");
        await ImportTable<AttributeValue>(folder, "AttributeValues.json");
        await ImportTable<AccountGuide>(folder, "AccountGuides.json");
        await ImportTable<Role>(folder, "Roles.json");
        await ImportTable<ChartOfAccount>(folder, "ChartOfAccounts.json");
        await ImportTable<SubLedgerBaseAccount>(folder, "SubLedgerBaseAccounts.json");
        await ImportTable<GLSetting>(folder, "GLSettings.json");
        await ImportTable<Currency>(folder, "Currencies.json");
        await ImportTable<FinancialPeriod>(folder, "FinancialPeriods.json");
        await ImportTable<Bank>(folder, "Banks.json");
        await ImportTable<Customer>(folder, "Customers.json");
        await ImportTable<Supplier>(folder, "Suppliers.json");
        await ImportTable<CashInBox>(folder, "CashInBoxes.json");
        await ImportTable<FixedAsset>(folder, "FixedAssets.json");
        //await ImportTable<Attachment>(folder, "Attachments.json");
        await ImportTable<Branch>(folder, "Branches.json");
    }

    private async Task ImportTable<TEntity>(string folder, string jsonFile) where TEntity : BaseEntity
    {
        var path = ResolveSeedFile(folder, jsonFile);
        var json = await File.ReadAllTextAsync(path);
        List<TEntity> entities = JsonConvert.DeserializeObject<List<TEntity>>(json) ?? new();

        foreach (TEntity entity in entities)
        {
            TEntity? dbEntity = _context.Set<TEntity>().Where(e => e.Id == entity.Id).FirstOrDefault();
            if (dbEntity != null)
                _context.Update(entity);
            else
                _context.Add(entity);
        }
    }

    private async Task ImportIdentityData(string folder)
    {
        await ImportUsers(folder);
        await ImportRoles(folder);
        await ImportUserRoles(folder);
    }

    private async Task ImportUserRoles(string folder)
    {
        var json = await File.ReadAllTextAsync(ResolveSeedFile(folder, "AspNetUserRoles.json"));
        List<IdentityUserRole<string>> entities =
            JsonConvert.DeserializeObject<List<IdentityUserRole<string>>>(json) ?? new();
        _context.Set<IdentityUserRole<string>>()
            .RemoveRange(_context.Set<IdentityUserRole<string>>().AsNoTracking().ToList());
        await _context.Set<IdentityUserRole<string>>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    private async Task ImportRoles(string folder)
    {
        var json = await File.ReadAllTextAsync(ResolveSeedFile(folder, "AspNetRoles.json"));
        List<IdentityRole> entities = JsonConvert.DeserializeObject<List<IdentityRole>>(json) ?? new();
        _context.Set<IdentityRole>().RemoveRange(_context.Set<IdentityRole>().AsNoTracking().ToList());
        await _context.Set<IdentityRole>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    private async Task ImportUsers(string folder)
    {
        var json = await File.ReadAllTextAsync(ResolveSeedFile(folder, "AspNetUsers.json"));
        List<ApplicationUser> entities = JsonConvert.DeserializeObject<List<ApplicationUser>>(json) ?? new();
        _context.Set<ApplicationUser>().RemoveRange(_context.Set<ApplicationUser>().AsNoTracking().ToList());
        await _context.Set<ApplicationUser>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    private static string ResolveSeedFile(string folder, string jsonFile)
    {
        var relative = Path.Combine("seeding", folder, jsonFile);
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, relative),
            Path.GetFullPath(relative),
        };

        foreach (var candidate in candidates)
        {
            if (File.Exists(candidate))
                return candidate;
        }

        throw new FileNotFoundException(
            $"Seed file not found: {relative}. Ensure seeding/{folder} is copied next to the host binaries.",
            candidates[0]);
    }
}
