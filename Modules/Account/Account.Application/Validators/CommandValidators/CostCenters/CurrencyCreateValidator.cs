using Account.Domain.Commands.CostCenters;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;
using Account.Domain.Models.Entities.CostCenters;
using FluentValidation;
using Shared.Domain.BaseEntities;

namespace Account.Application.Validators.CommandValidators.CostCenters;

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