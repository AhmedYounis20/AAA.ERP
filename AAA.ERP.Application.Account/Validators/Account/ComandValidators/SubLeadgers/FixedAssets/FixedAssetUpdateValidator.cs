using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.FixedAssets;
public class FixedAssetUpdateValidator : BaseSubLeadgerUpdateValidator<FixedAssetUpdateCommand, FixedAsset>
{
    public FixedAssetUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Version).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Serial).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.Model).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ManufactureCompany).MaximumLength(300).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.DepreciationRate).GreaterThan(0).When(e => e.IsDepreciable)
            .LessThanOrEqualTo(100);
        _ = RuleFor(e => e.AssetLifeSpanByYears).GreaterThan(0).When(e => e.IsDepreciable);
    }
}