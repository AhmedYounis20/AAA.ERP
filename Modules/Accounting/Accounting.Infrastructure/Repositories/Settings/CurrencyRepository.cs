using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Currencies;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Repositories;

public class CurrencyRepository : BaseSettingRepository<Currency>, ICurrencyRepository
{
    private readonly IApplicationDbContext _context;

    public CurrencyRepository(IApplicationDbContext context) : base(context)
        => _context = context;

    public async Task<Currency?> GetDefaultCurrency()
        => await _dbSet.FirstOrDefaultAsync(e => e.IsDefault);

    public async Task<bool> IsExitedCurrencySymbol(string? symbol)
    {
        var trimmedSymbol = symbol?.Trim().ToUpper();
        return await _dbSet.AnyAsync(e => e.Symbol != null && e.Symbol.Trim().ToUpper() == trimmedSymbol);
    }

    public Task<bool> IsUsedInEntries(Guid id)
        => _context.Set<Entry>().AnyAsync(e => e.CurrencyId == id);
}
