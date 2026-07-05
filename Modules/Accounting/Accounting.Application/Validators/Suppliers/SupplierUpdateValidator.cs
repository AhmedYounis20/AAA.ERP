using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.BaseEntities;
using Shared.Constants;

namespace Accounting.Application.Validators.Suppliers;

public class SupplierUpdateValidator : BaseSubLeadgerUpdateValidator<SupplierUpdateCommand, Supplier>
{
    public SupplierUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Phone).MaximumLength(300).WithMessage(ErrorMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Mobile).MaximumLength(300).WithMessage(ErrorMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e => !string.IsNullOrEmpty(e.Email)).MaximumLength(300).WithMessage(ErrorMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.TaxNumber).MaximumLength(300).WithMessage(ErrorMessages.MaxLength300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.CustomerType).IsInEnum().WithMessage(ErrorMessages.NotValidCustomerType).When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
