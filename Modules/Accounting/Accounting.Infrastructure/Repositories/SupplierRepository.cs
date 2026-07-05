using ERP.Application.Repositories.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;

namespace Accounting.Infrastructure.Repositories;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IApplicationDbContext context) : base(context)
    {
    }
}
