using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;

namespace AAA.ERP.Services.Impelementation;

using AAA.ERP.Services.Interfaces;

public class AccountGuideService : 
    BaseSettingService<AccountGuide,AccountGuideCreateCommand,AccountGuideUpdateCommand>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository) : base(repository)
    { }
}
