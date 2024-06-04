using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.SubLeadgers;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.Interfaces.SubLeadgers;

namespace AAA.ERP.Repositories.Impelementation.SubLeadgers;

public class CashInBoxRepository : BaseTreeSettingRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(ApplicationDbContext context) : base(context){}
}