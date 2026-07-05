using Accounting.Application.Commands.Banks;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace Accounting.Application.Validators.Banks;

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
