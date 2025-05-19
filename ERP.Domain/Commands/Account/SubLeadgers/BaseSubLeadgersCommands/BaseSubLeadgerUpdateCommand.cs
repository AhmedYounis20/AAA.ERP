using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;

namespace ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<TEntity> where TEntity : BaseSettingEntity
{
    public string? Code { get; set; }
}