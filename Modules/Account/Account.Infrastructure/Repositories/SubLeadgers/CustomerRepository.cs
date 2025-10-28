namespace Account.Infrastructure.Repositories.SubLeadgers;

public class CustomerRepository : BaseSubLeadgerRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IAccountDbContext context) : base(context) { }
}
