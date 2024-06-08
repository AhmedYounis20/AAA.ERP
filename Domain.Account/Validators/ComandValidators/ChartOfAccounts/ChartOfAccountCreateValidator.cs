using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.ChartOfAccounts;

public class ChartOfAccountCreateValidator : BaseTreeSettingCreateValidator<ChartOfAccountCreateCommand, ChartOfAccount>
{
    public ChartOfAccountCreateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}