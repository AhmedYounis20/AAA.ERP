using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.FinancialPeriods;

public class FinancialPeriodCreateValidator : BaseCreateValidator<FinancialPeriodCreateCommand, FinancialPeriod>
{
    public FinancialPeriodCreateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodRequiredYearNumber).MaximumLength(50).WithMessage(ValidationMessages.FinancialPeriodMaximumLength);
        _ = RuleFor(e => e.StartDate).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodStartDateRequired);
        _ = RuleFor(e => e.PeriodTypeByMonth).Must(IsValidPeriodType).WithMessage(ValidationMessages.NotValidPeriodType);
    }

    public bool IsValidPeriodType(byte periodType)
    {
        return periodType == FinancialPeriodType.OneMonth
               || periodType == FinancialPeriodType.ThreeMonths
               || periodType == FinancialPeriodType.SixMonths
               || periodType == FinancialPeriodType.OneYear;
    }
}
