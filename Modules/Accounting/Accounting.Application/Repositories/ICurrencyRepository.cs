using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace Accounting.Application.Repositories;

public interface ICurrencyRepository : IBaseSettingRepository<Currency>
{
    Task<Currency?> GetDefaultCurrency();
    Task<bool> IsExitedCurrencySymbol(string? symbol);
}
