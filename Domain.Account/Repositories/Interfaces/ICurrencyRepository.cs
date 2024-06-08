using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces;

public interface ICurrencyRepository: IBaseSettingRepository<Currency> {

    public Task<Currency?> GetDefaultCurrency();
    public Task<bool> IsExitedCurrencySymbol(string? symbol);
}
