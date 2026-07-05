using Accounting.Application.Commands.Customers;
using Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.BaseEntities;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.Customers;

public class CustomerUpdateValidator : BaseSubLeadgerUpdateValidator<CustomerUpdateCommand, Customer>
{
    public CustomerUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Phone).MaximumLength(300).WithMessage(ValidationMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Mobile).MaximumLength(300).WithMessage(ValidationMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e => !string.IsNullOrEmpty(e.Email)).MaximumLength(300).WithMessage(ValidationMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.TaxNumber).MaximumLength(300).WithMessage(ValidationMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.CustomerType).IsInEnum().WithMessage(ValidationMessages.NotValidCustomerType).When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
