using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;
using FluentValidation;
using Shared.BaseEntities;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.Branches;
public class BranchUpdateValidator : BaseSubLeadgerUpdateValidator<BranchUpdateCommand, Branch>
{
    public BranchUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Address).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Phone).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Logo.Length).LessThanOrEqualTo(10*1024*1024).When(e=>e.NodeType.Equals(NodeType.Domain) && e.Logo != null);
        _ = RuleFor(e => e.Logo.ContentType).Must(IsImage).When(e=>e.NodeType.Equals(NodeType.Domain) && e.Logo != null);
    }

    private bool IsImage(string contentType)
    => contentType.StartsWith("image/");
    
}