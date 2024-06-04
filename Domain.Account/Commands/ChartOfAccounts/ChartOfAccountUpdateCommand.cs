using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.ChartOfAccounts;

namespace Domain.Account.Commands.ChartOfAccounts;

public class ChartOfAccountUpdateCommand : BaseSettingUpdateCommand<ChartOfAccountUpdateCommand>
{
    public string? Code { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsDepreciable { get; set; }
    public AccountNature AccountNature { get; set; }
}