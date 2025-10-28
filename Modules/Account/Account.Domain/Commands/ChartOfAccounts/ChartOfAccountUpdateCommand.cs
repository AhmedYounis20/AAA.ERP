using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.ChartOfAccounts;

public class ChartOfAccountUpdateCommand : BaseTreeSettingUpdateCommand<ChartOfAccount>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameSecondLanguage { get; set; } = string.Empty;
    public AccountNature AccountNature { get; set; }
    public SubLeadgerType SubLeadgerType { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
}