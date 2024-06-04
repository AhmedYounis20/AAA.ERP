using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class CurrencyRepository : BaseSettingRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Currency?> GetDefaultCurrency()
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.IsDefault);
    }

    public async Task<bool> IsExitedCurrencySymbol(string? symbol)
    {
        string? trimmedSymbol = symbol?.Trim().ToUpper();

        return await _dbSet.AnyAsync(e => e.Symbol != null && e.Symbol.Trim().ToUpper() == trimmedSymbol);
    }
}