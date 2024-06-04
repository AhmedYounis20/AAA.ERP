using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;
using Shared.BaseRepositories.Interfaces;

namespace Shared.BaseRepositories.Impelementation;

public class BaseTreeSettingRepository<TEntity,TContext> 
    : BaseSettingRepository<TEntity,TContext>,
        IBaseTreeSettingRepository<TEntity,TContext> 
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TContext : IdentityDbContext<ApplicationUser>

{
    public BaseTreeSettingRepository(TContext context) : base(context)
    => _dbSet = context.Set<TEntity>();
    
    public async Task<List<TEntity>> GetLevel(int level = 0)
    {
        List<TEntity> entities = new List<TEntity>();
        entities = await _dbSet.Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;
        else
            return await GetChildren(entities, level - 1);
    }

    public async Task<List<TEntity>> GetChildren(List<TEntity> parents, int level = 0)
    {

        foreach (TEntity parent in parents)
            parent.Children = await GetChildren(parent.Id, level);

        return parents;
    }

    public async Task<List<TEntity>> GetChildren(Guid id, int level = 1)
    {
        List<TEntity> children = new List<TEntity>();

        children = await _dbSet.Where(e => e.ParentId.Equals(id)).ToListAsync();
        if (level == 0)
            return children;

        else
            return await GetChildren(children, level - 1);
    }

    public async Task<bool> HasChildren(Guid id)
    => await _dbSet.AnyAsync(e => e.ParentId == id);
}