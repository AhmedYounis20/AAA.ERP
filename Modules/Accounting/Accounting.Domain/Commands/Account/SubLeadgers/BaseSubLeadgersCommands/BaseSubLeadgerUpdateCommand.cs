using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;

namespace Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<TEntity> where TEntity : BaseSettingEntity
{
    public string? Code { get; set; }
}