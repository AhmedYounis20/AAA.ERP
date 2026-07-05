using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.FinancialPeriods;

public class FinancialPeriodUpdateInputValidator : BaseInputValidator<FinancialPeriodUpdateInputModel>
{
    public FinancialPeriodUpdateInputValidator()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage(ErrorMessages.FinancialPeriodRequiredYearNumber).MaximumLength(50).WithMessage(ErrorMessages.FinancialPeriodMaximumLength);
    }
}
