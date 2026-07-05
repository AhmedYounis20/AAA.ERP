using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Repositories;

public interface ICurrencyRepository : IBaseSettingRepository<Currency>
{
    Task<Currency?> GetDefaultCurrency();
    Task<bool> IsExitedCurrencySymbol(string? symbol);
}
