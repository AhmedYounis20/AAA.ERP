using Shared.BaseEntities;

namespace Domain.Account.Repositories.BaseRepositories.Interfaces;

public interface IBaseSettingRepository<TEntity> 
    : IBaseRepository<TEntity>, IDisposable where TEntity : BaseSettingEntity
{
    Task<IEnumerable<TEntity>> Search(string name);
    Task<bool> AnyByNames(string? name,string? nameSecondLanguage);
    Task<TEntity?> GetByNames(string? name, string? nameSecondLanguage);
}