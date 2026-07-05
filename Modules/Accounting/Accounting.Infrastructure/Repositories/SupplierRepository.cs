using Accounting.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IApplicationDbContext context) : base(context)
    {
    }
}
