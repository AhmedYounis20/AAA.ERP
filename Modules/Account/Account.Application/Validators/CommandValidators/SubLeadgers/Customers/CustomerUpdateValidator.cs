using Account.Domain.Commands.SubLeadgers.Customers;
using Account.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.Customers;
public class CustomerUpdateValidator : BaseSubLeadgerUpdateValidator<CustomerUpdateCommand, Customer>
{
    public CustomerUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Phone).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Mobile).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e => !string.IsNullOrEmpty(e.Email)).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.TaxNumber).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.CustomerType).IsInEnum().WithMessage("NotValidCustomerType").When(e => e.NodeType.Equals(NodeType.Domain));
    }
}