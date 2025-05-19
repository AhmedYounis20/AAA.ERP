using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using ERP.Application.Services.Account;

namespace ERP.Infrastracture.Services.Account;
public class AccountGuideService :
    BaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository) : base(repository)
    { }
}
