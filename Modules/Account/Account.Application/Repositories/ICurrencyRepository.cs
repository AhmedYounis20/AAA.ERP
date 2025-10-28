using Account.Domain.Models.Entities.Currencies;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface ICurrencyRepository : IBaseSettingRepository<Currency>
{

    public Task<Currency?> GetDefaultCurrency();
    public Task<bool> IsExitedCurrencySymbol(string? symbol);
}
