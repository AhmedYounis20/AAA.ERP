using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class CustomerRepository : BaseSubLeadgerRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IApplicationDbContext context) : base(context)
    {
    }
}
