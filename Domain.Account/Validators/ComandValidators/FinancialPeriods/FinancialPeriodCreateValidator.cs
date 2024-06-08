using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.FinancialPeriods;

public class FinancialPeriodCreateValidator : BaseCreateValidator<FinancialPeriodCreateCommand, FinancialPeriod>
{
    public FinancialPeriodCreateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
        _ = RuleFor(e => e.StartDate).NotEmpty().WithMessage("FinancialPeriodStartDateRequired");
        _ = RuleFor(e => e.PeriodTypeByMonth).Must(IsValidPeriodType).WithMessage("NotValidPeriodType");
    }

    public bool IsValidPeriodType(byte periodType)
    {
        return periodType == FinancialPeriodType.OneMonth
               || periodType == FinancialPeriodType.ThreeMonths
               || periodType == FinancialPeriodType.SixMonths
               || periodType == FinancialPeriodType.OneYear;
    }
}