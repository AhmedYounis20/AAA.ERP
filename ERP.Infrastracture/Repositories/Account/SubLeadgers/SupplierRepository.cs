using ERP.Application.Repositories.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IApplicationDbContext context) : base(context) { }
}