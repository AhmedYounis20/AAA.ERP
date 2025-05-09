using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.CostCenters;
using Domain.Account.Models.Entities.Currencies;
using Shared.Responses;

namespace Domain.Account.Commands.Currencies;

public class CostCenterUpdateCommand : BaseTreeSettingUpdateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}