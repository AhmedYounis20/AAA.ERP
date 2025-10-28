using Account.Domain.Commands.ChartOfAccounts;
using Account.Domain.Models.Entities.ChartOfAccounts;
using FluentValidation;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Account.Application.Validators.CommandValidators.ChartOfAccounts;

public class ChartOfAccountUpdateValidator : BaseTreeSettingUpdateValidator<ChartOfAccountUpdateCommand, ChartOfAccount>
{
    public ChartOfAccountUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}