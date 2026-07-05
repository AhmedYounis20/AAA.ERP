using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IApplicationDbContext context) : base(context)
    {
    }
}
