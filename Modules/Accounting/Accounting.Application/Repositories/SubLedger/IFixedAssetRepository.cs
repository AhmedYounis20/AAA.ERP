using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for fixed asset persistence.
/// </summary>
public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    /// <summary>Gets as no tracking.</summary>
    Task<FixedAsset?> GetAsNoTracking(Guid id);
}
