using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    private readonly IApplicationDbContext _context;

    public AccountGuideRepository(IApplicationDbContext context) : base(context)
        => _context = context;

    public Task<bool> IsUsedInChartOfAccounts(Guid id)
        => _context.Set<ChartOfAccount>().AnyAsync(e => e.AccountGuidId == id);
}
