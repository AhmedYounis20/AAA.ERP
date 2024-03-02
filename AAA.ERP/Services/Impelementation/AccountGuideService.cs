using AAA.ERP.Services.BaseServices.impelemtation;

namespace AAA.ERP.Services.Impelementation;

using AAA.ERP.Models.Data.AccountGuide;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;

public class AccountGuideService : BaseSettingService<AccountGuide>, IAccountGuideService
{
    public AccountGuideService(IAccountGuideRepository repository, IAccountGuideBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    { }
}
