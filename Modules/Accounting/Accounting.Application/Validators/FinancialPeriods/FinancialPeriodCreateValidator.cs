using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.FinancialPeriods;

public class FinancialPeriodCreateValidator : BaseCreateValidator<FinancialPeriodCreateCommand, FinancialPeriod>
{
    public FinancialPeriodCreateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage(ErrorMessages.FinancialPeriodRequiredYearNumber).MaximumLength(50).WithMessage(ErrorMessages.FinancialPeriodMaximumLength);
        _ = RuleFor(e => e.StartDate).NotEmpty().WithMessage(ErrorMessages.FinancialPeriodStartDateRequired);
        _ = RuleFor(e => e.PeriodTypeByMonth).Must(IsValidPeriodType).WithMessage(ErrorMessages.NotValidPeriodType);
    }

    public bool IsValidPeriodType(byte periodType)
    {
        return periodType == FinancialPeriodType.OneMonth
               || periodType == FinancialPeriodType.ThreeMonths
               || periodType == FinancialPeriodType.SixMonths
               || periodType == FinancialPeriodType.OneYear;
    }
}
