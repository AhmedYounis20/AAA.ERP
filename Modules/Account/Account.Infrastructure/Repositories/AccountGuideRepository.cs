using Account.Application.Repositories;
using Account.Domain.Models.Entities.AccountGuides;

namespace Account.Infrastructure.Repositories;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(AccountDbContext context) : base(context) { }
}
