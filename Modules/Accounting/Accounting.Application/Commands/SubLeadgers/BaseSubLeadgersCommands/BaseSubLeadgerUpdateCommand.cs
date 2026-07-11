using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<TEntity> where TEntity : BaseSettingEntity
{
    public string? Code { get; set; }
}