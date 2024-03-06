using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;

namespace AAA.ERP.Repositories.Impelementation;

public class CurrencyRepository : BaseSettingRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Currency?> GetDefaultCurrency()
    {
        return await dbSet.FirstOrDefaultAsync(e => e.IsDefault);
    }

    public async Task<bool> IsExitedCurrencySymbol(string? symbol)
    {
        string? trimmedSymbol = symbol?.Trim().ToUpper();

        return await dbSet.AnyAsync(e => e.Symbol != null && e.Symbol.Trim().ToUpper() == trimmedSymbol);
    }
}