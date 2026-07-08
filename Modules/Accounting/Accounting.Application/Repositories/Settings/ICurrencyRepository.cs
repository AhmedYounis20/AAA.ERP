using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for currency persistence.
/// </summary>
public interface ICurrencyRepository : IBaseSettingRepository<Currency>
{
    /// <summary>Gets default currency.</summary>
    Task<Currency?> GetDefaultCurrency();
    /// <summary>is exited currency symbol operation.</summary>
    Task<bool> IsExitedCurrencySymbol(string? symbol);
    /// <summary>Checks whether currency is referenced by entries.</summary>
    Task<bool> IsUsedInEntries(Guid id);
}
