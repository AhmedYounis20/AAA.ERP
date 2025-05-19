using Domain.Account.Models.Entities.CostCenters;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class CostCenterRepository : BaseTreeSettingRepository<CostCenter>, ICostCenterRepository
{
    private ApplicationDbContext _context;

    public CostCenterRepository(ApplicationDbContext context) : base(context)
        => _context = context;


    public override async Task<CostCenter?> Get(Guid id)
    {
        return await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts).Where(e => e.Id.Equals(id))
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<CostCenter>> Get()
    {
        return await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts)
            .ToListAsync();
    }

    public override async Task<IEnumerable<CostCenter>> Get(Expression<Func<CostCenter, bool>> predicate)
    {
        return await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts)
            .Where(predicate).ToListAsync();
    }

    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts)
        => _context.Set<CostCenterChartOfAccount>().RemoveRange(chartOfAccounts);
}