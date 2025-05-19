using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Shared.BaseEntities;

namespace ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerCreateCommand<TEntity> : BaseTreeSettingCreateCommand<TEntity> where TEntity : BaseSettingEntity
{
    public string? Code { get; set; }
}