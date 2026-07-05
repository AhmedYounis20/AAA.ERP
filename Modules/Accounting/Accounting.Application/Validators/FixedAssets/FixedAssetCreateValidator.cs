using Accounting.Application.Commands.FixedAssets;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace Accounting.Application.Validators.FixedAssets;

public class FixedAssetCreateValidator : BaseSubLeadgerCreateValidator<FixedAssetCreateCommand, FixedAsset>
{
    public FixedAssetCreateValidator() : base()
    {
        _ = RuleFor(e => e.Version).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Serial).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Model).EmailAddress().MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ManufactureCompany).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.DepreciationRate).GreaterThan(0).When(e => e.IsDepreciable)
            .LessThanOrEqualTo(100);
        _ = RuleFor(e => e.AssetLifeSpanByYears).GreaterThan(0).When(e => e.IsDepreciable);
    }
}
