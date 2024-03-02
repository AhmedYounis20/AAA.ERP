using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Data.AccountGuide;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;

namespace AAA.ERP.Repositories.Impelementation;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(ApplicationDbContext context) : base(context) {}
}
