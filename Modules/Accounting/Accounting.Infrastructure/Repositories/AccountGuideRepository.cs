using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(IApplicationDbContext context) : base(context) { }
}
