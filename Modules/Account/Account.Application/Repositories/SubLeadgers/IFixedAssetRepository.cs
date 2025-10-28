using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Repositories.SubLeadgers;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    public Task<FixedAsset?> GetAsNoTracking(Guid id);
}