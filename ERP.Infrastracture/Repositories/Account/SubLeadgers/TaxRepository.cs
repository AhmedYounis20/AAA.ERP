using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class TaxRepository : BaseSubLeadgerRepository<Tax>, ITaxRepository
{
    public TaxRepository(IApplicationDbContext context) : base(context) { }
}
