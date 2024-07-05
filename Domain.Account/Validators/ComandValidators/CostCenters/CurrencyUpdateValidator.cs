using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.Currencies;

public class CostCenterUpdateValidator : BaseTreeSettingUpdateValidator<CostCenterUpdateCommand, CostCenter>
{
    public CostCenterUpdateValidator() : base()
    {

        _ = RuleFor(e => e.Percent)
            .GreaterThan(0).When(e => e.NodeType.Equals(NodeType.Domain))
            .LessThanOrEqualTo(100).When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ChartOfAccounts).NotEmpty().When(e =>
            e.NodeType.Equals(NodeType.Domain) && e.CostCenterType == CostCenterType.RelatedToAccount);
    }
}