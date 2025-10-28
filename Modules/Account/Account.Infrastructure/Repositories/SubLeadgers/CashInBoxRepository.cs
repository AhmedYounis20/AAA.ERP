namespace Account.Infrastructure.Repositories.SubLeadgers;

public class CashInBoxRepository : BaseSubLeadgerRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(IAccountDbContext context) : base(context) { }
}
