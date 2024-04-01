using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Repositories.BaseRepositories.Interfaces;

public interface IBaseTreeSettingRepository<TEntity> : IBaseTreeRepository<TEntity>, IDisposable where TEntity : BaseTreeSettingEntity
{
    Task<IEnumerable<TEntity>> Search(string name);
    Task<bool> AnyByNames(string? name,string? nameSecondLanguage);
    Task<TEntity?> GetByNames(string? name, string? nameSecondLanguage);
}