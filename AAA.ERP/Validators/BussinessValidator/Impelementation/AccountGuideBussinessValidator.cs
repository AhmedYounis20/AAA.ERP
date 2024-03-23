using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Resources;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Validators.BussinessValidator.Impelementation;

public class AccountGuideBussinessValidator : BaseSettingBussinessValidator<AccountGuide>, IAccountGuideBussinessValidator
{
    private IAccountGuideRepository _repository;
    public AccountGuideBussinessValidator(IAccountGuideRepository repository, IStringLocalizer<Resource> localizer) : base(repository, localizer)
    {
        _repository = repository;
    }
}