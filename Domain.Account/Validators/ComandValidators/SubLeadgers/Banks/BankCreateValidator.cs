using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.Banks;

public class BankCreateValidator : BaseSubLeadgerCreateValidator<BankCreateCommand, Bank>
{
    public BankCreateValidator() : base()
    {
        _ = RuleFor(e => e.BankAddress).MaximumLength(300);
        _ = RuleFor(e => e.BankAccount).MaximumLength(300);
        _ = RuleFor(e => e.Email).EmailAddress().MaximumLength(300);
        _ = RuleFor(e => e.Phone).MaximumLength(300);
    }
}