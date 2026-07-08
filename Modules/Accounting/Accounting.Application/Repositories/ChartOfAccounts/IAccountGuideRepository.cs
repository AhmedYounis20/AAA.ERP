using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for account guide persistence.
/// </summary>
public interface IAccountGuideRepository : IBaseSettingRepository<AccountGuide>
{
    /// <summary>Checks whether the guide is referenced by chart of accounts.</summary>
    Task<bool> IsUsedInChartOfAccounts(Guid id);
}
