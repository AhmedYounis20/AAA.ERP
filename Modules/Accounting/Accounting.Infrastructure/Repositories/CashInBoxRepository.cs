using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class CashInBoxRepository : BaseSubLeadgerRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(IApplicationDbContext context) : base(context)
    {
    }
}
