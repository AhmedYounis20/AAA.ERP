using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class CashInBoxRepository : BaseSubLeadgerRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(ApplicationDbContext context) : base(context) { }
}