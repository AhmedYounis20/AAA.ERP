using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Domain.Commands.ChartOfAccounts;

public class ChartOfAccountCreateCommand : BaseTreeSettingCreateCommand<ChartOfAccount>
{
    public string? Code { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsDepreciable { get; set; }
    public AccountNature AccountNature { get; set; }
}