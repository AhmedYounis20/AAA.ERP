using Domain.Account.InputModels.BaseInputModels;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;

namespace ERP.Domain.Commands.Account;

public class ChartOfAccountInputModel : BaseTreeSettingInputModel
{
    public string? Code { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsDepreciable { get; set; }
    public AccountNature AccountNature { get; set; }
}