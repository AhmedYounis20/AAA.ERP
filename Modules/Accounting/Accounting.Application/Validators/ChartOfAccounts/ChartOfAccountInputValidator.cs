using Accounting.Application.Commands.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.ChartOfAccounts;

public class ChartOfAccountInputValidator : BaseTreeSettingInputValidator<ChartOfAccountInputModel>
{
    public ChartOfAccountInputValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage(ValidationMessages.ChartOfAccountCodeMaxLengthValidation).NotEmpty().WithMessage(ValidationMessages.ChartOfAccountCodeRequired);
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage(ValidationMessages.ChartOfAccountAccountGuideRequired);
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage(ValidationMessages.ChartOfAccountNotValidAccountNature);
    }
}
