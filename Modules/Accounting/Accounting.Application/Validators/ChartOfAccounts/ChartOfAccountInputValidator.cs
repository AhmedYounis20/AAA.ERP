using Accounting.Application.Commands.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.ChartOfAccounts;

public class ChartOfAccountInputValidator : BaseTreeSettingInputValidator<ChartOfAccountInputModel>
{
    public ChartOfAccountInputValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage(ErrorMessages.ChartOfAccountCodeMaxLengthValidation).NotEmpty().WithMessage(ErrorMessages.ChartOfAccountCodeRequired);
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage(ErrorMessages.ChartOfAccountAccountGuideRequired);
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage(ErrorMessages.ChartOfAccountNotValidAccountNature);
    }
}
