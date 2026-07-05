using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.CostCenters;
using ERP.Infrastructure.Repositories.BaseRepositories;
using System.Linq.Expressions;

namespace Accounting.Infrastructure.Repositories;

public class CostCenterRepository : BaseTreeSettingRepository<CostCenter>, ICostCenterRepository
{
    private readonly IApplicationDbContext _context;

    public CostCenterRepository(IApplicationDbContext context) : base(context)
        => _context = context;

    public override async Task<CostCenter?> Get(Guid? id)
        => await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts).Where(e => e.Id.Equals(id))
            .FirstOrDefaultAsync();

    public override async Task<IEnumerable<CostCenter>> Get()
        => await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts).ToListAsync();

    public override async Task<IEnumerable<CostCenter>> Get(Expression<Func<CostCenter, bool>> predicate)
        => await _context.Set<CostCenter>().Include(e => e.ChartOfAccounts).Where(predicate).ToListAsync();

    public void RemoveChartOfAccounts(List<CostCenterChartOfAccount> chartOfAccounts)
        => _context.Set<CostCenterChartOfAccount>().RemoveRange(chartOfAccounts);
}
