using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces.SubLeadgers;

public interface IFixedAssetRepository : IBaseSubLeadgerRepository<FixedAsset>
{
    public  Task<FixedAsset?> GetAsNoTracking(Guid id);
}