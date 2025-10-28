namespace Account.Infrastructure.Repositories.SubLeadgers;

public class BankRepository : BaseSubLeadgerRepository<Bank>, IBankRepository
{
    public BankRepository(IAccountDbContext context) : base(context) { }
}
