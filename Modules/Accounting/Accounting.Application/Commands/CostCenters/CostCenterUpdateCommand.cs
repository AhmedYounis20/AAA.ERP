using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace Accounting.Application.Commands.CostCenters;

public class CostCenterUpdateCommand : BaseTreeSettingUpdateCommand<CostCenter>
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
    public List<Guid>? ChartOfAccounts { get; set; }
}
