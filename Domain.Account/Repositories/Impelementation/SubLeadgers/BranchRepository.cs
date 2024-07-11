using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Shared.BaseRepositories.Impelementation;

namespace Domain.Account.Repositories.Impelementation.SubLeadgers;

public class BranchRepository : BaseSubLeadgerRepository<Branch>, IBranchRepository
{
    private ApplicationDbContext _context;

    public BranchRepository(ApplicationDbContext context) : base(context){}
        public override async Task<IEnumerable<Branch>> Get()
        {
            return await _dbSet.Include(e=>e.Attachment).Include(e => e.ChartOfAccount).ToListAsync();
        }
    
        public override async Task<Branch?> Get(Guid id)
        {
            return await _dbSet.Where(e=>e.Id.Equals(id)).Include(e=>e.Attachment).Include(e => e.ChartOfAccount).FirstOrDefaultAsync();
        }
    
        public override async Task<List<Branch>> GetLevel(int level = 0)
        {
            List<Branch> entities = new List<Branch>();
            entities = await _dbSet.Include(e=>e.ChartOfAccount).Include(e=>e.Attachment).Where(e => e.ParentId == null).ToListAsync();
            if (level == 0)
                return entities;
            else
                return await GetChildren(entities, level - 1);
        }
        
        public override async Task<List<Branch>> GetChildren(Guid id, int level = 1)
        {
            List<Branch> children = new List<Branch>();
    
            children = await _dbSet.Where(e => e.ParentId.Equals(id)).Include(e=>e.Attachment).Include(e=>e.ChartOfAccount).ToListAsync();
            if (level == 0)
                return children;
    
            else
                return await GetChildren(children, level - 1);
        }
}