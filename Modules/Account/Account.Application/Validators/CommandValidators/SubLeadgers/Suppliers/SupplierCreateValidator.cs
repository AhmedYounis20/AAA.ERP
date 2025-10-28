using Account.Domain.Commands.SubLeadgers.Suppliers;
using Account.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.Suppliers;

public class SupplierCreateValidator : BaseSubLeadgerCreateValidator<SupplierCreateCommand, Supplier>
{
    public SupplierCreateValidator() : base()
    {
        _ = RuleFor(e => e.Phone).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Mobile).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e => !string.IsNullOrEmpty(e.Email)).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.TaxNumber).MaximumLength(300).WithMessage("MaxLength300").When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.CustomerType).IsInEnum().WithMessage("NotValidCustomerType").When(e => e.NodeType.Equals(NodeType.Domain));
    }
}