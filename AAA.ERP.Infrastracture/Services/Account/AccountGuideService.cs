using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace ERP.Infrastracture.Services.Account;
public class AccountGuideService :
    BaseSettingService<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository) : base(repository)
    { }
}
