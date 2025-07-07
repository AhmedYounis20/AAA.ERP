using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class BaseSubLeadgerRepository<TEntity> : BaseTreeSettingRepository<TEntity>, IBaseSubLeadgerRepository<TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
{
    private ApplicationDbContext _context;
    public BaseSubLeadgerRepository(ApplicationDbContext context) : base(context)
        => _context = context;

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
        List<TEntity> entities = new List<TEntity>();
        entities = await _dbSet.Include(e => e.ChartOfAccount).Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;
        else
            return await GetChildren(entities, level - 1);
    }

    public override async Task<List<TEntity>> GetChildren(Guid id, int level = 1)
    {
        List<TEntity> children = new List<TEntity>();

        children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e => e.ChartOfAccount).ToListAsync();
        if (level == 0)
            return children;

        else
            return await GetChildren(children, level - 1);
    }
}