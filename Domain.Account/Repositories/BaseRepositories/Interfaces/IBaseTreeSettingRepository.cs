using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Shared.BaseRepositories.Interfaces;

public interface IBaseTreeSettingRepository<TEntity,TContext> 
    : IBaseSettingRepository<TEntity,TContext>, 
        IDisposable where TEntity : BaseTreeSettingEntity<TEntity>
    where TContext : IdentityDbContext<ApplicationUser>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
    Task<bool> HasChildren(Guid id);
}