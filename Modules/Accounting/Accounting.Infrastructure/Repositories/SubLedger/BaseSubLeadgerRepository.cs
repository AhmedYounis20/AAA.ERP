using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class BaseSubLeadgerRepository<TEntity> : BaseTreeSettingRepository<TEntity>, IBaseSubLeadgerRepository<TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
{
    public BaseSubLeadgerRepository(IApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<TEntity>> Get()
    {
        return await _dbSet.Include(e => e.ChartOfAccount).ToListAsync();
    }

    public override async Task<TEntity?> Get(Guid? id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).Include(e => e.ChartOfAccount).FirstOrDefaultAsync();
    }

    public override async Task<List<TEntity>> GetLevel(int level = 0)
    {
        var entities = await _dbSet.Include(e => e.ChartOfAccount).Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;

        return await GetChildren(entities, level - 1);
    }

    public override async Task<List<TEntity>> GetChildren(Guid id, int level = 1)
    {
        var children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e => e.ChartOfAccount).ToListAsync();
        if (level == 0)
            return children;

        return await GetChildren(children, level - 1);
    }
}
