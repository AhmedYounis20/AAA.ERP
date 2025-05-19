using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Repositories.SubLeadgers;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    public Task<FixedAsset?> GetAsNoTracking(Guid id);
}