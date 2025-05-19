using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class CustomerRepository : BaseSubLeadgerRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) { }
}