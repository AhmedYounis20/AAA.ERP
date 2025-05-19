using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context) { }
}