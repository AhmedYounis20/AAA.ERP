using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Shared.BaseRepositories.Interfaces;

public interface IBaseSettingRepository<TEntity,TContext> 
    : IBaseRepository<TEntity,TContext>, IDisposable where TEntity : BaseSettingEntity
    where TContext : IdentityDbContext<ApplicationUser>
{
    Task<IEnumerable<TEntity>> Search(string name);
    Task<bool> AnyByNames(string? name,string? nameSecondLanguage);
    Task<TEntity?> GetByNames(string? name, string? nameSecondLanguage);
}