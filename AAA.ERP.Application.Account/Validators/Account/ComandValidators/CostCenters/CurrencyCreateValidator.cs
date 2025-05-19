using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;
using FluentValidation;
using Shared.BaseEntities;

namespace ERP.Application.Validators.Account.ComandValidators.CostCenters;

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