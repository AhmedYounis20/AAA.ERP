using Account.Application.Repositories;
using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;

namespace Account.Infrastructure.Services;
public class AccountGuideService :
    BaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository) : base(repository)
    { }
}
