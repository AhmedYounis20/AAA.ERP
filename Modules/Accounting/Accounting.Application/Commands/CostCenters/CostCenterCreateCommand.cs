using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Application.Commands.CostCenters;

public class CostCenterCreateCommand : BaseTreeSettingCreateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}
