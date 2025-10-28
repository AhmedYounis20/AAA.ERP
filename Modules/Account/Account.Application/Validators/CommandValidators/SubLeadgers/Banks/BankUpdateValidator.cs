using Account.Domain.Commands.SubLeadgers.Banks;
using Account.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;
using Account.Application.Validators.CommandValidators.SubLeadgers.BaseSubLeadgers;

namespace Account.Application.Validators.ComandValidators.SubLeadgers.Banks;
public class BankUpdateValidator : BaseSubLeadgerUpdateValidator<BankUpdateCommand, Bank>
{
    public BankUpdateValidator() : base()
    {
        _ = RuleFor(e => e.BankAddress).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.BankAccount).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Email).EmailAddress().When(e => !string.IsNullOrEmpty(e.Email)).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Phone).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
    }
}