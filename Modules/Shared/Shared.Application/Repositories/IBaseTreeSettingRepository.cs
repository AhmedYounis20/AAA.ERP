using Shared.Domain.BaseEntities;

namespace Shared.Application.Repositories;

public interface IBaseTreeSettingRepository<TEntity>
    : IBaseSettingRepository<TEntity>,
        IDisposable where TEntity : BaseTreeSettingEntity<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
    Task<bool> HasChildren(Guid id);
}