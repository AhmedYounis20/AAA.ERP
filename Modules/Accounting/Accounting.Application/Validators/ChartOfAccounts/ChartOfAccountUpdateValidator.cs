using Accounting.Application.Commands.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.ChartOfAccounts;

public class ChartOfAccountUpdateValidator : BaseTreeSettingUpdateValidator<ChartOfAccountUpdateCommand, ChartOfAccount>
{
    public ChartOfAccountUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Code).MaximumLength(100).WithMessage(ErrorMessages.ChartOfAccountCodeMaxLengthValidation).NotEmpty().WithMessage(ErrorMessages.ChartOfAccountCodeRequired);
        _ = RuleFor(e => e.AccountGuidId).NotEmpty().WithMessage(ErrorMessages.ChartOfAccountAccountGuideRequired);
        _ = RuleFor(e => e.AccountNature).IsInEnum().WithMessage(ErrorMessages.ChartOfAccountNotValidAccountNature);
    }
}
