using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class CustomerRepository : BaseSubLeadgerRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) { }
}