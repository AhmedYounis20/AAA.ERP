using ERP.Application.Validators.Account.InputValidators.BaseValidators;
using ERP.Domain.Commands.Account;
using FluentValidation;

namespace ERP.Application.Validators.Account.InputValidators;

public class ChartOfAccountInputValidator : BaseTreeSettingInputValidator<ChartOfAccountInputModel>
{
    public ChartOfAccountInputValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}