using Domain.Account.InputModels.BaseInputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;

namespace Domain.Account.InputModels;

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