using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class CurrencyRepository : BaseSettingRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(IApplicationDbContext context) : base(context) { }

    public async Task<Currency?> GetDefaultCurrency()
        => await _dbSet.FirstOrDefaultAsync(e => e.IsDefault);

    public async Task<bool> IsExitedCurrencySymbol(string? symbol)
    {
        var trimmedSymbol = symbol?.Trim().ToUpper();
        return await _dbSet.AnyAsync(e => e.Symbol != null && e.Symbol.Trim().ToUpper() == trimmedSymbol);
    }
}
