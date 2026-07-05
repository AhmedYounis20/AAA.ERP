using Accounting.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Repositories;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    Task<FixedAsset?> GetAsNoTracking(Guid id);
}
