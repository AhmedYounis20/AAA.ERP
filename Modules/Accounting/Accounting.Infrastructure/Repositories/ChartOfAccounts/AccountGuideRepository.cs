using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class AccountGuideRepository : BaseSettingRepository<AccountGuide>, IAccountGuideRepository
{
    public AccountGuideRepository(IApplicationDbContext context) : base(context) { }
}
