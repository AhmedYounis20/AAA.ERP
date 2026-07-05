using Accounting.Application.Commands.CostCenters;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.CostCenters;
using FluentValidation;
using Shared.BaseEntities;

namespace Accounting.Application.Validators.CostCenters;

public class CostCenterCreateValidator : BaseTreeSettingCreateValidator<CostCenterCreateCommand, CostCenter>
{
    public CostCenterCreateValidator() : base()
    {
        _ = RuleFor(e => e.Percent)
            .GreaterThan(0).When(e => e.NodeType.Equals(NodeType.Domain))
            .LessThanOrEqualTo(100).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ChartOfAccounts).NotEmpty().When(e =>
            e.NodeType.Equals(NodeType.Domain) && e.CostCenterType == CostCenterType.RelatedToAccount);
    }
}
