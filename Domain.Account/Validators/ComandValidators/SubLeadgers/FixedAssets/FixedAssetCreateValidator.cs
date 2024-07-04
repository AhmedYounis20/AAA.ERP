using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.ChartOfAccounts;
using FluentValidation;
using Shared.BaseEntities;

namespace Domain.Account.Validators.ComandValidators.SubLeadgers.Banks;

public class FixedAssetCreateValidator : BaseSubLeadgerCreateValidator<FixedAssetCreateCommand, FixedAsset>
{
    public FixedAssetCreateValidator() : base()
    {
        _ = RuleFor(e => e.Version).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Serial).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Model).EmailAddress().MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ManufactureCompany).MaximumLength(300).When(e=>e.NodeType.Equals(NodeType.Domain));
    }
}