using Domain.Account.Models.Entities.SubLeadgers;

namespace ERP.Application.Repositories.SubLeadgers;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    public Task<FixedAsset?> GetAsNoTracking(Guid id);
}