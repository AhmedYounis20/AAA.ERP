using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
namespace Accounting.Infrastructure.Repositories;

public class BankRepository : BaseSubLeadgerRepository<Bank>, IBankRepository
{
    public BankRepository(IApplicationDbContext context) : base(context)
    {
    }
}
