using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
namespace Accounting.Infrastructure.Repositories;

public class FixedAssetRepository : BaseSubLeadgerRepository<FixedAsset>, IFixedAssetRepository
{
    public FixedAssetRepository(IApplicationDbContext context) : base(context)
    {
    }

    public override async Task<FixedAsset?> Get(Guid? id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).Include(e => e.ChartOfAccount).Include(e => e.ExpensesAccount).Include(e => e.AccumlatedAccount).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<FixedAsset?> GetAsNoTracking(Guid id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();
    }
}
