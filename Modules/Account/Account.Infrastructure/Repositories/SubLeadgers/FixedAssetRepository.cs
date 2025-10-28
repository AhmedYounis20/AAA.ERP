namespace Account.Infrastructure.Repositories.SubLeadgers;

public class FixedAssetRepository : BaseSubLeadgerRepository<FixedAsset>, IFixedAssetRepository
{
    public FixedAssetRepository(IAccountDbContext context) : base(context) { }


    public override async Task<FixedAsset?> Get(Guid? id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).Include(e => e.ChartOfAccount).Include(e => e.ExpensesAccount).Include(e => e.AccumlatedAccount).AsNoTracking().FirstOrDefaultAsync();
    }
    public virtual async Task<FixedAsset?> GetAsNoTracking(Guid id)
    {
        return await _dbSet.Where(e => e.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();
    }
}
