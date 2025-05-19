using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.Suppliers;

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