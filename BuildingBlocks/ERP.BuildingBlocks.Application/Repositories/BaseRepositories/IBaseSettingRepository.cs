using Shared.BaseEntities;

namespace ERP.Application.Repositories.BaseRepositories;

public interface IBaseSettingRepository<TEntity>
    : IBaseRepository<TEntity>, IDisposable where TEntity : BaseSettingEntity
{
    Task<IEnumerable<TEntity>> Search(string name);
    Task<bool> AnyByNames(string? name, string? nameSecondLanguage);
    Task<TEntity?> GetByNames(string? name, string? nameSecondLanguage);
}