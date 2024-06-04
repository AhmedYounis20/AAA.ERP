using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.BussinessValidator.Interfaces;

namespace AAA.ERP.Services.Impelementation;

using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;

public class AccountGuideService : BaseSettingService<AccountGuide>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository, IAccountGuideBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    { }
}
