using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.GLSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Domain.Account.Utility;

public class ImportDataToSeed
{
    private readonly ApplicationDbContext _context;
    private ILogger<ImportDataToSeed> _logger;

    public ImportDataToSeed(IConfiguration configuration, ApplicationDbContext context,
        ILogger<ImportDataToSeed> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Import(string folder = "account")
    {
        var transaction = _context.Database.BeginTransaction();
        try
        {
            await ImportBussinessData(folder);
            await ImportIdentityData(folder);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportBussinessData(string folder)
    {
        await ImportTable<AccountGuide>(folder, "AccountGuides.json");
        await ImportTable<ChartOfAccount>(folder, "ChartOfAccounts.json");
        await ImportTable<GLSetting>(folder, "GLSettings.json");
        await ImportTable<Currency>(folder, "Currencies.json");
        await ImportTable<FinancialPeriod>(folder, "FinancialPeriods.json");
    }

    private async Task ImportTable<TEntity>(string folder, string jsonFile) where TEntity : BaseEntity
    {
        var json = await File.ReadAllTextAsync(Path.Combine($"seeding/{folder}", jsonFile));
        List<TEntity> entities = JsonConvert.DeserializeObject<List<TEntity>>(json) ?? new();

        try
        {
            foreach(TEntity entity in entities)
            {
                TEntity? dbEntity = _context.Set<TEntity>().Where(e => e.Id == entity.Id).FirstOrDefault();
                if (dbEntity != null)
                    _context.Update(entity);
                else
                    _context.Add(entity);
            }
            await _context.SaveChangesAsync();
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
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
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine($"seeding/{folder}", "AspNetUserRoles.json"));
            List<IdentityUserRole<string>> entities =
                JsonConvert.DeserializeObject<List<IdentityUserRole<string>>>(json) ?? new();
            _context.Set<IdentityUserRole<string>>()
                .RemoveRange(_context.Set<IdentityUserRole<string>>().AsNoTracking().ToList());
            await _context.Set<IdentityUserRole<string>>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportRoles(string folder)
    {
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine($"seeding/{folder}", "AspNetRoles.json"));
            List<IdentityRole> entities = JsonConvert.DeserializeObject<List<IdentityRole>>(json) ?? new();
            _context.Set<IdentityRole>().RemoveRange(_context.Set<IdentityRole>().AsNoTracking().ToList());
            await _context.Set<IdentityRole>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

    private async Task ImportUsers(string folder)
    {
        try
        {
            var json = await File.ReadAllTextAsync(Path.Combine($"seeding/{folder}", "AspNetUsers.json"));
            List<ApplicationUser> entities = JsonConvert.DeserializeObject<List<ApplicationUser>>(json) ?? new();
            _context.Set<ApplicationUser>().RemoveRange(_context.Set<ApplicationUser>().AsNoTracking().ToList());
            await _context.Set<ApplicationUser>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}