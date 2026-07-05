using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class BankRepository : BaseSubLeadgerRepository<Bank>, IBankRepository
{
    public BankRepository(IApplicationDbContext context) : base(context)
    {
    }
}
