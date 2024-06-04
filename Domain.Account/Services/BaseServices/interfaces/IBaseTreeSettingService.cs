using Shared.BaseEntities;

namespace Domain.Account.Services.BaseServices.interfaces;

public interface IBaseTreeSettingService<TEntity> : IBaseSettingService<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
}