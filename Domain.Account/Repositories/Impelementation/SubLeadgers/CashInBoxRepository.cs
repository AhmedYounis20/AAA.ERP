using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Shared.BaseRepositories.Impelementation;

namespace Domain.Account.Repositories.Impelementation.SubLeadgers;

public class CashInBoxRepository : BaseTreeSettingRepository<CashInBox>, ICashInBoxRepository
{
    public CashInBoxRepository(ApplicationDbContext context) : base(context){}
}