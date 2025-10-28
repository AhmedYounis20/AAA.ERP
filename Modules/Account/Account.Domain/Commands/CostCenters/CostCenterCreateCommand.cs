using Account.Domain.Models.Entities.CostCenters;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Domain.Commands.CostCenters;

public class CostCenterCreateCommand : BaseTreeSettingCreateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}