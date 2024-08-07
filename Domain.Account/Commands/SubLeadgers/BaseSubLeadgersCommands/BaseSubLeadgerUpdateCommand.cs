using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;

public class BaseSubLeadgerUpdateCommand<TEntity> : BaseTreeSettingUpdateCommand<TEntity> where TEntity :BaseSettingEntity
{
    public string? Code { get; set; }  
}