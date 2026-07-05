using Accounting.Application.Commands.ChartOfAccounts;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using FluentValidation;

namespace Accounting.Application.Validators.ChartOfAccounts;

public class ChartOfAccountCreateValidator : BaseTreeSettingCreateValidator<ChartOfAccountCreateCommand, ChartOfAccount>
{
    public ChartOfAccountCreateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}
