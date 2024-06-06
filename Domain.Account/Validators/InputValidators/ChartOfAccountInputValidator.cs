using AAA.ERP.Validators.InputValidators.BaseValidators;
using Domain.Account.InputModels;
using FluentValidation;

namespace Domain.Account.Validators.InputValidators;

public class ChartOfAccountInputValidator : BaseTreeSettingInputValidator<ChartOfAccountInputModel>
{
    public ChartOfAccountInputValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}