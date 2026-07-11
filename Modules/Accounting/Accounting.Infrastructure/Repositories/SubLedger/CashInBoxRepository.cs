using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;
namespace Accounting.Infrastructure.Repositories;

public class CashInBoxRepository : BaseSubLeadgerRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(IApplicationDbContext context) : base(context)
    {
    }
}
