using Shared.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Domain.BaseEntities;

namespace Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;

public abstract class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
{
    public Guid ChartOfAccountId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}