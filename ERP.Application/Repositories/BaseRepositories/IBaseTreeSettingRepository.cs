using Shared.BaseEntities;

namespace ERP.Application.Repositories.BaseRepositories;

public interface IBaseTreeSettingRepository<TEntity>
    : IBaseSettingRepository<TEntity>,
        IDisposable where TEntity : BaseTreeSettingEntity<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
    Task<bool> HasChildren(Guid id);
}