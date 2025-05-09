using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Models.Entities.CostCenters;
using Domain.Account.Models.Entities.Currencies;
using Shared.Responses;

namespace Domain.Account.Commands.Currencies;

public class CostCenterCreateCommand : BaseTreeSettingCreateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}