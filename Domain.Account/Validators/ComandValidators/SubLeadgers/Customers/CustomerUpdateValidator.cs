using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.Customers;
public class CustomerUpdateValidator : BaseSubLeadgerUpdateValidator<CustomerUpdateCommand, Customer>
{
    public CustomerUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Phone).MaximumLength(300).WithMessage("MaxLength300");
        _ = RuleFor(e => e.Mobile).MaximumLength(300).WithMessage("MaxLength300");
        _ = RuleFor(e => e.Email).EmailAddress().MaximumLength(300).WithMessage("MaxLength300");
        _ = RuleFor(e => e.TaxNumber).MaximumLength(300).WithMessage("MaxLength300");
        _ = RuleFor(e => e.CustomerType).IsInEnum().WithMessage("NotValidCustomerType");
    }
}