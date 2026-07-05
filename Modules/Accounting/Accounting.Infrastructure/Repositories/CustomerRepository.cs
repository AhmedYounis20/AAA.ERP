using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class CustomerRepository : BaseSubLeadgerRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IApplicationDbContext context) : base(context)
    {
    }
}
