using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class TaxRepository : BaseSubLeadgerRepository<Tax>, ITaxRepository
{
    public TaxRepository(IApplicationDbContext context) : base(context)
    {
    }
}
