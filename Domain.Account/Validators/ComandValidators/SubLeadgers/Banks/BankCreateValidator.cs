using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;
using FluentValidation;
using Shared.BaseEntities;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.Banks;

public class BankCreateValidator : BaseSubLeadgerCreateValidator<BankCreateCommand, Bank>
{
    public BankCreateValidator() : base()
    {
        _ = RuleFor(e => e.BankAddress).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.BankAccount).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e=>!string.IsNullOrEmpty(e.Email)).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Phone).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
    }
}