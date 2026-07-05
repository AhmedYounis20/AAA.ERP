using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    Task<FixedAsset?> GetAsNoTracking(Guid id);
}
