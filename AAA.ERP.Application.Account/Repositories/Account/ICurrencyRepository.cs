using Domain.Account.Models.Entities.Currencies;
using ERP.Application.Repositories.BaseRepositories;

namespace ERP.Application.Repositories;

public interface ICurrencyRepository: IBaseSettingRepository<Currency> {

    public Task<Currency?> GetDefaultCurrency();
    public Task<bool> IsExitedCurrencySymbol(string? symbol);
}
