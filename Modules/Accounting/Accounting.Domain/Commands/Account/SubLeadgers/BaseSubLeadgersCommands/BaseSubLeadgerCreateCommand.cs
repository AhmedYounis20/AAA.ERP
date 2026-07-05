using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Shared.BaseEntities;

namespace Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerCreateCommand<TEntity> : BaseTreeSettingCreateCommand<TEntity> where TEntity : BaseSettingEntity
{
    public string? Code { get; set; }
}