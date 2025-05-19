using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.ChartOfAccounts;

public class ChartOfAccountCreateValidator : BaseTreeSettingCreateValidator<ChartOfAccountCreateCommand, ChartOfAccount>
{
    public ChartOfAccountCreateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}