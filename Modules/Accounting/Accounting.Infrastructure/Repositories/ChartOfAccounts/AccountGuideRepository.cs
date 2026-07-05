using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(IApplicationDbContext context) : base(context) { }
}
