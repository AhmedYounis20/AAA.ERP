using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Application.Commands.ChartOfAccounts;

public class ChartOfAccountUpdateCommand : BaseTreeSettingUpdateCommand<ChartOfAccount>
{
    public string? Code { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsDepreciable { get; set; }
    public AccountNature AccountNature { get; set; }
}
