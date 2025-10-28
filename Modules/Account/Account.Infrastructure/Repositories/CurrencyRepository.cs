using Account.Domain.Models.Entities.Currencies;

namespace Account.Infrastructure.Repositories;

public class CurrencyRepository : BaseSettingRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(IAccountDbContext context) : base(context) { }

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
