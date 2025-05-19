using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Models.Entities.ChartOfAccounts;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.ChartOfAccounts;

public class ChartOfAccountUpdateValidator : BaseTreeSettingUpdateValidator<ChartOfAccountUpdateCommand, ChartOfAccount>
{
    public ChartOfAccountUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage("ChartOfAccountCodeMaxLengthValidation").NotEmpty().WithMessage("ChartOfAccountCodeRequired");
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage("ChartOfAccountAccountGuideRequired");
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage("ChartOfAccountNotValidAccountNature");
    }
}