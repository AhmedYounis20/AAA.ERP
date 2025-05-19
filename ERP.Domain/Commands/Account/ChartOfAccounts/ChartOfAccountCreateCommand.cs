using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.ChartOfAccounts;

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