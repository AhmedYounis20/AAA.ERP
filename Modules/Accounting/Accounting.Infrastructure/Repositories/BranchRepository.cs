using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class BranchRepository : BaseSubLeadgerRepository<Branch>, IBranchRepository
{
    public BranchRepository(IApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Branch>> Get()
    {
        return await _dbSet.Include(e => e.Attachment).Include(e => e.ChartOfAccount).ToListAsync();
    }

    public override async Task<Branch?> Get(Guid? id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).Include(e => e.Attachment).Include(e => e.ChartOfAccount).FirstOrDefaultAsync();
    }

    public override async Task<List<Branch>> GetLevel(int level = 0)
    {
        List<Branch> entities = await _dbSet.Include(e => e.ChartOfAccount).Include(e => e.Attachment).Where(e => e.ParentId == null).ToListAsync();
        if (level == 0)
            return entities;

        return await GetChildren(entities, level - 1);
    }

    public override async Task<List<Branch>> GetChildren(Guid id, int level = 1)
    {
        List<Branch> children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e => e.Attachment).Include(e => e.ChartOfAccount).ToListAsync();
        if (level == 0)
            return children;

        return await GetChildren(children, level - 1);
    }
}
