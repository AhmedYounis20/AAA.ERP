using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.FixedAssets;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.FixedAssets;

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