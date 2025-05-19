using Domain.Account.Models.Entities.AccountGuide;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(ApplicationDbContext context) : base(context) { }
}
