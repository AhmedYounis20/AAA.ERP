using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(ApplicationDbContext context) : base(context) {}
}
