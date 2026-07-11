using Accounting.Application.Commands.CostCenters;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.CostCenters;
using FluentValidation;
using ERP.BuildingBlocks.Domain.Entities;

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
