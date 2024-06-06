using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using Domain.Account.Validators.BussinessValidator.Interfaces;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace Domain.Account.Validators.BussinessValidator.Impelementation;

public class AccountGuideBussinessValidator : BaseSettingBussinessValidator<AccountGuide>, IAccountGuideBussinessValidator
{
    private IAccountGuideRepository _repository;
    public AccountGuideBussinessValidator(IAccountGuideRepository repository, IStringLocalizer<Resource> localizer) : base(repository, localizer)
    {
        _repository = repository;
    }
}