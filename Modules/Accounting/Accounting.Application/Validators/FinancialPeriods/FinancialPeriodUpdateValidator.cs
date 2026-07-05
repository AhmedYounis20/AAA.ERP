using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseUpdateValidator<FinancialPeriodUpdateCommand, FinancialPeriod>
{
    public FinancialPeriodUpdateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodRequiredYearNumber).MaximumLength(50).WithMessage(ValidationMessages.FinancialPeriodMaximumLength);
    }
}
