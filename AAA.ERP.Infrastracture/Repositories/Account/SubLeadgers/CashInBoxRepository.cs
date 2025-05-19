using Domain.Account.DBConfiguration.DbContext;
using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class CashInBoxRepository : BaseSubLeadgerRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(ApplicationDbContext context) : base(context) { }
}