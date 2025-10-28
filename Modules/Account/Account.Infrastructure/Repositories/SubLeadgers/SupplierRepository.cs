namespace Account.Infrastructure.Repositories.SubLeadgers;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IAccountDbContext context) : base(context) { }
}
