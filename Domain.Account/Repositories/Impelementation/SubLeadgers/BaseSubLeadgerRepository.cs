using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Shared.BaseEntities;
using Shared.BaseRepositories.Impelementation;

namespace Domain.Account.Repositories.Impelementation.SubLeadgers;

public class BaseSubLeadgerRepository<TEntity> : BaseTreeSettingRepository<TEntity>, IBaseSubLeadgerRepository<TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
{
    private ApplicationDbContext _context;
    public BaseSubLeadgerRepository(ApplicationDbContext context) : base(context)
        => _context= context;

    public override async Task<IEnumerable<TEntity>> Get()
    {
        return await _dbSet.Include(e => e.ChartOfAccount).ToListAsync();
    }

    public override async Task<TEntity?> Get(Guid id)
    {
        return await _dbSet.Where(e=>e.Id.Equals(id)).Include(e => e.ChartOfAccount).FirstOrDefaultAsync();
    }

    public override async Task<List<TEntity>> GetLevel(int level = 0)
    {
        List<TEntity> entities = new List<TEntity>();
        entities = await _dbSet.Include(e=>e.ChartOfAccount).Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;
        else
            return await GetChildren(entities, level - 1);
    }
    
    public override async Task<List<TEntity>> GetChildren(Guid id, int level = 1)
    {
        List<TEntity> children = new List<TEntity>();

        children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e=>e.ChartOfAccount).ToListAsync();
        if (level == 0)
            return children;

        else
            return await GetChildren(children, level - 1);
    }
}