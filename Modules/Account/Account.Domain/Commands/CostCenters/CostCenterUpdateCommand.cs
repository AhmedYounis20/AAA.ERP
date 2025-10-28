using Account.Domain.Models.Entities.CostCenters;
using Shared.Application.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.CostCenters;

public class CostCenterUpdateCommand : BaseTreeSettingUpdateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}