using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace ERP.Application.Repositories.Account;

public interface ICurrencyRepository : IBaseSettingRepository<Currency>
{

    public Task<Currency?> GetDefaultCurrency();
    public Task<bool> IsExitedCurrencySymbol(string? symbol);
}
