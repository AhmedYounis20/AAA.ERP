using ERP.Application.Repositories.Account;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(ApplicationDbContext context) : base(context) { }
}
